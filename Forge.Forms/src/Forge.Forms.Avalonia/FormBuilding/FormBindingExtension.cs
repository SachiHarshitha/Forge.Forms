using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using Avalonia;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Styling;
using Avalonia.Xaml.Interactions.Core;
using Forge.Forms.AvaloniaUI.DynamicExpressions;
using MultiBinding = Avalonia.Data.MultiBinding;

namespace Forge.Forms.AvaloniaUI.FormBuilding;

/// <summary>
///     Markup extension for creating deferred bindings.
/// </summary>
public class FormBindingExtension : MarkupExtension
{
    #region Fields

    private bool _dataContextSet; // Flag to track if DataContext has been set
    private IDisposable _dataContextSubscription;

    private bool _isUpdating;
    private string _targetPropertyName; // Store the target property name

    #endregion

    #region Constructor

    /// <summary>
    ///     empty constructor
    /// </summary>
    public FormBindingExtension()
    {
    }

    /// <summary>
    ///     Name constructor
    /// </summary>
    /// <param name="name"></param>
    public FormBindingExtension(string name)
    {
        Name = name;
    }

    #endregion

    #region Properties

    [ConstructorArgument("name")] public string Name { get; set; }
    public string Converter { get; set; }

    #endregion

    #region Methods

    /// <summary>
    ///     Provide the value for the markup extension.
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var pvt = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
        if (pvt?.TargetObject == null) return null; // Return null if no target object is found.

        if (pvt.TargetObject is StyledElement styledElement)
        {
            // Store the target property name the first time it's accessed
            if (_targetPropertyName == null)
                _targetPropertyName = pvt.TargetProperty?.ToString(); // Store the name of the target property

            // Ensure TargetProperty is initialized before proceeding
            if (styledElement.DataContext != null)
            {
                _dataContextSet = true;
                return ApplyBinding(styledElement); // Apply the binding immediately if DataContext is available
            }

            // DataContext is null, subscribe to changes.
            ObserveDataContextChange(styledElement);
            var targetType = pvt.TargetProperty?.GetType();
            if (targetType?.Name == nameof(AvaloniaProperty) ||
                (targetType?.IsSubclassOf(typeof(AvaloniaProperty)) ?? false))
            {
                var targetProperty = pvt.TargetProperty as AvaloniaProperty;
                if (targetProperty != null)
                {
                    if (Nullable.GetUnderlyingType(targetProperty?.PropertyType) != null)
                    {
                        return null;
                    }

                    if (targetProperty?.PropertyType.IsEnum ?? false)
                        return Enum.GetValues(targetProperty?.PropertyType).GetValue(0);
                    if (targetProperty?.PropertyType == typeof(bool))
                        return false;
                    if (targetProperty?.PropertyType == typeof(string))
                        return string.Empty;
                    if (targetProperty?.PropertyType == typeof(double))
                        return double.NaN;

                    // if (targetProperty?.PropertyType is IBinding)
                    //return new FormBindingExtension(Name);
                }
                //return AvaloniaProperty.UnsetValue;
            }

            return null;
        }

        if (pvt.TargetObject is SetterBase setter)
        {
            if ((pvt.TargetProperty is AvaloniaProperty dp2 && dp2.PropertyType == typeof(BindingBase))
                || (pvt.TargetProperty is PropertyInfo p2 && p2.PropertyType == typeof(BindingBase)))
                return new Binding($"[{Name}].Value")
                {
                    Converter = GetConverter()
                };

            return new Binding($"[{Name}].Value")
            {
                Converter = GetConverter()
            };
        }

        return this; // Fallback behavior for non-StyledElement
    }

    /// <summary>
    ///     Apply the binding to the target property.
    /// </summary>
    /// <param name="styledElement"></param>
    /// <returns></returns>
    private object ApplyBinding(StyledElement styledElement)
    {
        if (_isUpdating)
            return null;

        _isUpdating = true;

        if (styledElement.DataContext is IBindingProvider field)
        {
            var value = field.ProvideValue(Name);
            //Console.WriteLine($"Provided value for {Name}: {value}");

            // If the value is a BindingBase (e.g., Binding), apply it
            if (value is Binding binding)
            {
                var avaProp = GetDependencyProperty(styledElement.GetType(), _targetPropertyName);
                if (avaProp?.PropertyType == typeof(BindingBase)) return value;
                if (avaProp != null)
                {
                    var expression = styledElement.Bind(avaProp, binding);
                    if (expression != null) field.BindingCreated(expression, Name);
                    return expression;
                }

                var prop = GetPropInfoProperty(styledElement.GetType(), _targetPropertyName);
                if (prop != null)
                    if (prop?.PropertyType == typeof(BindingBase))
                        return value;

                return value;
            }

            if (value is MultiBinding multiBinding)
            {
                var avaProp = GetDependencyProperty(styledElement.GetType(), _targetPropertyName);
                if (avaProp?.PropertyType == typeof(BindingBase)) return value;

                if (avaProp != null)
                {
                    var expression = styledElement.Bind(avaProp, multiBinding);
                    if (expression != null) field.BindingCreated(expression, Name);
                    return expression;
                }

                var prop = GetPropInfoProperty(styledElement.GetType(), _targetPropertyName);
                if (prop != null)
                    if (prop?.PropertyType == typeof(BindingBase))
                        return value;

                return value;
            }

            // Apply the literal value directly
            ApplyLiteralValueViaReflection(styledElement, value);
            _isUpdating = false;
        }
        else
        {
            Debug.WriteLine($"DataContext does not implement IBindingProvider for {Name}.");
            _isUpdating = false;
        }

        return this;
    }


    /// <summary>
    ///     Apply the literal value to the target property via reflection.
    /// </summary>
    /// <param name="styledElement"></param>
    /// <param name="value"></param>
    private void ApplyLiteralValueViaReflection(StyledElement styledElement, object value)
    {
        // Use reflection to set the literal value
        var targetPropertyInfo = GetAllProperties(styledElement).FirstOrDefault(x => x.Name == _targetPropertyName);
        if (targetPropertyInfo != null)
            try
            {
                if (styledElement is DataTriggerBehavior dataTrigger && _targetPropertyName == "Binding")
                    dataTrigger.SetValue(DataTriggerBehavior.BindingProperty, value);

                if (targetPropertyInfo.PropertyType.FullName.Contains(nameof(IBinding)) && value is string expr)
                    targetPropertyInfo.SetValue(styledElement, new Binding(expr));
                else if (targetPropertyInfo.PropertyType.FullName.Contains(nameof(IImage)) && value is string image)
                {
                    var bitmap = new Bitmap(AssetLoader.Open(new Uri(image)));
                    targetPropertyInfo.SetValue(styledElement, bitmap);
                }
                else
                    targetPropertyInfo.SetValue(styledElement, value);

                //Console.WriteLine($"Literal value set via reflection to {_targetPropertyName}");
                return;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Fail to apply property via reflection due to: {e}");
            }
        else
        {
            var avaProp = GetDependencyProperty(styledElement.GetType(), _targetPropertyName);
            if (avaProp != null)
            {
                if (avaProp.PropertyType == typeof(BindingBase))
                {
                    styledElement.SetValue(avaProp, new Binding(value.ToString()));
                    return;
                }
                else
                {
                    styledElement.SetValue(avaProp, value);
                    return;
                }
            }
        }

        Debug.WriteLine($"Target property '{_targetPropertyName}' not found on target.");
    }

    /// <summary>
    /// Retrieves all properties from an object, including those from base classes.
    /// </summary>
    /// <param name="obj">The object to get properties from.</param>
    /// <returns>A list of PropertyInfo objects representing the properties of the object.</returns>
    public static List<PropertyInfo> GetAllProperties(object obj)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        var type = obj.GetType();
        var properties = new List<PropertyInfo>();

        // Traverse the inheritance hierarchy to collect properties
        while (type != null)
        {
            properties.AddRange(type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                                                   BindingFlags.Instance | BindingFlags.DeclaredOnly));
            type = type.BaseType;
        }

        return properties;
    }

    /// <summary>
    ///     Observe datatcontext chagnes and apply binding when it's set.
    /// </summary>
    /// <param name="styledElement"></param>
    private void ObserveDataContextChange(StyledElement styledElement)
    {
        // If we've already subscribed, return early to prevent multiple subscriptions
        if (_dataContextSubscription != null) return;

        _dataContextSubscription = styledElement.GetObservable(StyledElement.DataContextProperty)
            .Where(dataContext => dataContext != null)
            .Subscribe(dataContext =>
            {
                // Once DataContext is set, apply the binding
                if (!_isUpdating)
                {
                    //_isUpdating = true;
                    // Console.WriteLine("DataContext set, applying binding...");
                    ApplyBinding(styledElement);
                    _isUpdating = false;
                }
            });
    }

    /// <summary>
    ///     Get converter from the resource dictionary.
    /// </summary>
    /// <returns></returns>
    private IValueConverter GetConverter()
    {
        if (string.IsNullOrEmpty(Converter)) return null;

        //Console.WriteLine($"Getting converter: {Converter}");
        return Resource.GetValueConverter(null, Converter);
    }

    /// <summary>
    ///     Dispose Logic
    /// </summary>
    public void Dispose()
    {
        _dataContextSubscription?.Dispose();
        _dataContextSubscription = null;
    }


    /// <summary>
    ///     Get the dependency property from the type and property name.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    private static AvaloniaProperty? GetDependencyProperty(Type type, string propertyName)
    {
        AvaloniaProperty avaloniaProperty = null;
        var avaloniaProps =
            GetFieldInfosIncludingBaseClasses(type, BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.FieldType.IsSubclassOf(typeof(AvaloniaProperty)));
        var property = avaloniaProps.FirstOrDefault(p => p.Name.Equals($"{propertyName}Property"));
        if (property != null) avaloniaProperty = property.GetValue(null) as AvaloniaProperty;
        return avaloniaProperty;
    }

    /// <summary>
    ///     Get the property info from the type and property name.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    private static PropertyInfo? GetPropInfoProperty(Type type, string propertyName)
    {
        var props = type.GetProperties(BindingFlags.Static | BindingFlags.Public);
        return props.FirstOrDefault(p => p.Name.Equals($"{propertyName}"));
    }

    /// <summary>
    ///     Get all field infos including base classes.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="bindingFlags"></param>
    /// <returns></returns>
    public static FieldInfo[] GetFieldInfosIncludingBaseClasses(Type type, BindingFlags bindingFlags)
    {
        FieldInfo[] fieldInfos = type.GetFields(bindingFlags);

        // If this class doesn't have a base, don't waste any time
        if (type.BaseType == typeof(object)) return fieldInfos;

        // Otherwise, collect all types up to the furthest base class
        var currentType = type;
        var fieldComparer = new FieldInfoComparer();
        var fieldInfoList = new HashSet<FieldInfo>(fieldInfos, fieldComparer);
        while (currentType != typeof(object))
        {
            fieldInfos = currentType.GetFields(bindingFlags);
            fieldInfoList.UnionWith(fieldInfos);
            currentType = currentType.BaseType;
        }

        return fieldInfoList.ToArray();
    }

    /// <summary>
    ///     FieldInfo comparer
    /// </summary>
    private class FieldInfoComparer : IEqualityComparer<FieldInfo>
    {
        public bool Equals(FieldInfo x, FieldInfo y)
        {
            return x.DeclaringType == y.DeclaringType && x.Name == y.Name;
        }

        public int GetHashCode(FieldInfo obj)
        {
            return obj.Name.GetHashCode() ^ obj.DeclaringType.GetHashCode();
        }
    }

    #endregion
}