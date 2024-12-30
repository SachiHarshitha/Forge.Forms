using System;
using System.Collections.Generic;
using Forge.Forms.AvaloniaUI.DynamicExpressions;

namespace Forge.Forms.AvaloniaUI.FormBuilding.Defaults;

/// <summary>
///     Represents a numeric input field.
/// </summary>
public class NumericField : DataFormField
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="NumericField" /> class.
    /// </summary>
    /// <param name="key">The unique key for the field.</param>
    /// <param name="propertyType">The numeric type of the field (e.g., int, double).</param>
    public NumericField(string key, Type propertyType)
        : base(key, propertyType)
    {
        if (!IsNumericType(propertyType))
            throw new ArgumentException("NumericField only supports numeric types.", nameof(propertyType));
    }

    #region Properties

    /// <summary>
    ///     Gets or sets the minimum value for the numeric field.
    /// </summary>
    public IValueProvider Minimum { get; set; } = new LiteralValue(0);

    /// <summary>
    ///     Gets or sets the maximum value for the numeric field.
    /// </summary>
    public IValueProvider Maximum { get; set; } = new LiteralValue(100);

    /// <summary>
    ///     Gets or sets the increment value for the numeric field.
    /// </summary>
    public IValueProvider Increment { get; set; } = new LiteralValue(1);

    #endregion Properties

    #region Methods

    /// <summary>
    ///     Freezes the state of the numeric field and adds all relevant resources.
    /// </summary>
    protected internal override void Freeze()
    {
        base.Freeze();
        Resources.Add(nameof(Minimum), Minimum);
        Resources.Add(nameof(Maximum), Maximum);
        Resources.Add(nameof(Increment), Increment);
    }

    /// <summary>
    ///     Creates the binding provider for the numeric field.
    /// </summary>
    /// <param name="context">The resource context.</param>
    /// <param name="formResources">The form resources.</param>
    /// <returns>The binding provider.</returns>
    protected internal override IBindingProvider CreateBindingProvider(
        IResourceContext context,
        IDictionary<string, IValueProvider> formResources)
    {
        return new NumericPresenter(context, Resources, formResources);
    }

    /// <summary>
    ///     Determines if a type is numeric.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the type is numeric; otherwise, false.</returns>
    private static bool IsNumericType(Type type)
    {
        return type == typeof(byte) || type == typeof(sbyte) ||
               type == typeof(short) || type == typeof(ushort) ||
               type == typeof(int) || type == typeof(uint) ||
               type == typeof(long) || type == typeof(ulong) ||
               type == typeof(float) || type == typeof(double) ||
               type == typeof(decimal);
    }

    #endregion Methods
}

/// <summary>
///     The presenter for numeric fields.
/// </summary>
public class NumericPresenter : ValueBindingProvider
{
    public NumericPresenter(
        IResourceContext context,
        IDictionary<string, IValueProvider> fieldResources,
        IDictionary<string, IValueProvider> formResources)
        : base(context, fieldResources, formResources, true)
    {
    }

    protected override Type StyleKeyOverride => typeof(NumericPresenter);
}