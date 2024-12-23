using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using Forge.Forms.AvaloniaUI.Annotations;
using Humanizer;

namespace Forge.Forms.AvaloniaUI.Validator;

public class ModelValidator
{
    #region Fields

    // Regex pattern to match variable placeholders in error messages
    private static readonly Regex VariablePattern = new Regex(@"\{(?<ObjectName>\w+)(\|(?<Property>\w+))?(:(?<Format>.+?))?\}", RegexOptions.Compiled);
    private readonly Must _validationType;
    private readonly object _comparisonValue;
    private readonly Type _modelType;
    private readonly string _methodName;
    
    #endregion

    #region Constructors
    /// <summary>
    /// Constructor to initialize the ModelValidator with validation type, error message, comparison value, model type, and method name
    /// </summary>
    /// <param name="validationType"></param>
    /// <param name="errorMessage"></param>
    /// <param name="comparisonValue"></param>
    /// <param name="modelType"></param>
    /// <param name="methodName"></param>
    public ModelValidator(Must validationType,string errorMessage, object comparisonValue = null, Type modelType = null,
        string methodName = null)
    {
        ErrorMessage = errorMessage;
        _validationType = validationType;
        _comparisonValue = comparisonValue;
        _modelType = modelType;
        _methodName = methodName;
    }


    #endregion

    #region Properties
    public bool IgnoreNullOrEmpty { get; set; }
    public string ErrorMessage { get; set; }
    
    #endregion

    /// <summary>
    /// Method to validate a value based on the specified validation type
    /// </summary>
    /// <param name="value"></param>
    /// <param name="propertyName"></param>
    /// <param name="propertyType"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public bool Validate(object value, string propertyName = null, Type propertyType = null)
    {
        if (IgnoreNullOrEmpty && (value == null || (value is string s && string.IsNullOrWhiteSpace(s))))
        {
            return false; // Skip validation for null or empty values.
        }

        return _validationType switch
        {
            Must.BeEqualTo => Equals(value, _comparisonValue),
            Must.NotBeEqualTo => !Equals(value, _comparisonValue),
            Must.BeGreaterThan => Compare(value, _comparisonValue,propertyType) > 0,
            Must.BeGreaterThanOrEqualTo => Compare(value, _comparisonValue,propertyType) >= 0,
            Must.BeLessThan => Compare(value, _comparisonValue,propertyType) < 0,
            Must.BeLessThanOrEqualTo => Compare(value, _comparisonValue,propertyType) <= 0,
            Must.BeEmpty => IsEmpty(value),
            Must.NotBeEmpty => !IsEmpty(value),
            Must.BeTrue => value is bool b && b,
            Must.BeFalse => value is bool b && !b,
            Must.BeNull => value == null,
            Must.NotBeNull => value != null,
            Must.ExistIn => ExistsInCollection(value, _comparisonValue),
            Must.NotExistIn => !ExistsInCollection(value, _comparisonValue),
            Must.MatchPattern => value is string str && MatchesPattern(str, _comparisonValue as string),
            Must.NotMatchPattern => value is string strNot && !MatchesPattern(strNot, _comparisonValue as string),
            Must.SatisfyMethod => InvokeSatisfyMethod(value, propertyName),
            Must.SatisfyContextMethod => InvokeContextMethod(value, propertyName),
            Must.BeInvalid => false,
            _ => throw new InvalidOperationException($"Unsupported validation type: {_validationType}")
        };
    }

    /// <summary>
    /// Method to compare two values and return an integer indicating their relative order
    /// </summary>
    /// <param name="value">Value to be compared</param>
    /// <param name="comparisonValue">Value to be compared with</param>
    /// <param name="propertyType">Target Type</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private static int Compare(object value, object comparisonValue, Type propertyType = null)
    {
        // Handle null values explicitly
        if (value == null && comparisonValue == null)
        {
            return 0; // Both are null, so they are considered equal
        }
        if (value == null)
        {
            return -1; // Null is considered less than any non-null value
        }
        if (comparisonValue == null)
        {
            return 1; // Any non-null value is considered greater than null
        }

        // Attempt type conversion if propertyType is provided
        if (propertyType != null)
        {
            try
            {
                value = ConvertToType(value, propertyType);
                comparisonValue = ConvertToType(comparisonValue, propertyType); }
            catch
            {
                throw new InvalidOperationException($"Cannot cast values to type {propertyType.Name} for comparison.");
            }
        }

        // Both values are non-null, attempt comparison
        if (value is IComparable comparable)
        { 
            return comparable.CompareTo(comparisonValue);
        }

        throw new InvalidOperationException("Values are not comparable.");
    }

    /// <summary>
    /// Method to convert a value to a specified target type
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private static object ConvertToType(object value, Type targetType)
{
    // Determine the underlying (non-nullable) type
    var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

    if (value == null)
    {
        // Return null if the target type is nullable
        if (Nullable.GetUnderlyingType(targetType) != null)
        {
            return null;
        }

        throw new InvalidOperationException($"Cannot convert null to non-nullable type {underlyingType.Name}.");
    }

    // If the value is already of the target type, return it
    if (underlyingType.IsInstanceOfType(value))
    {
        return value;
    }

    try
    {
        if (value is string stringValue)
        {
            // Look for the Parse or TryParse method
            var parseMethod = underlyingType.GetMethod("Parse", new[] { typeof(string), typeof(IFormatProvider) }) ??
                              underlyingType.GetMethod("Parse", new[] { typeof(string) });

            if (parseMethod != null)
            {
                // Invoke Parse(string) or Parse(string, IFormatProvider)
                return parseMethod.GetParameters().Length == 1
                    ? parseMethod.Invoke(null, new object[] { stringValue })
                    : parseMethod.Invoke(null, new object[] { stringValue, CultureInfo.InvariantCulture });
            }

            // Look for TryParse if Parse is unavailable
            var tryParseMethod = underlyingType.GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static, null,
                new[] { typeof(string), underlyingType.MakeByRefType() }, null);

            if (tryParseMethod != null)
            {
                var parameters = new object[] { stringValue, Activator.CreateInstance(underlyingType) };
                var success = (bool)tryParseMethod.Invoke(null, parameters);
                if (success)
                {
                    return parameters[1];
                }
            }

            throw new InvalidOperationException(
                $"Type {underlyingType.Name} does not have a suitable Parse or TryParse method.");
        }

        // Fallback to Convert.ChangeType
        return Convert.ChangeType(value, underlyingType, CultureInfo.InvariantCulture);
    }
    catch (Exception ex)
    {
        throw new InvalidOperationException(
            $"Failed to convert value '{value}' to type {underlyingType.Name}. Details: {ex.Message}");
    }
}
    
    /// <summary>
    /// Method to check if a value is empty
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private static bool IsEmpty(object value)
    {
        return value switch
        {
            null => true,
            string s => string.IsNullOrEmpty(s),
            ICollection collection => collection.Count == 0,
            _ => throw new InvalidOperationException("Unsupported type for empty check.")
        };
    }

    /// <summary>
    /// Method to check if a value exists in a collection
    /// </summary>
    /// <param name="value"></param>
    /// <param name="collection"></param>
    /// <returns></returns>
    private static bool ExistsInCollection(object value, object collection)
    {
        if (collection is IEnumerable enumerable)
        {
            foreach (var item in enumerable)
            {
                if (Equals(item, value))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Method to check if a string value matches a specified pattern
    /// </summary>
    /// <param name="value"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private static bool MatchesPattern(string value, string pattern)
    {
        if (string.IsNullOrEmpty(pattern))
        {
            throw new InvalidOperationException("Pattern cannot be null or empty.");
        }

        return Regex.IsMatch(value, pattern);
    }

    /// <summary>
    /// Method to invoke a custom validation method on the model type
    /// </summary>
    /// <param name="value"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private bool InvokeSatisfyMethod(object value, string propertyName)
    {
        if (_modelType == null || string.IsNullOrEmpty(_methodName))
        {
            throw new InvalidOperationException("Model type and method name must be provided for SatisfyMethod validation.");
        }

        var method = _modelType.GetMethod(_methodName, BindingFlags.Public | BindingFlags.Static);
        if (method == null || method.ReturnType != typeof(bool) || method.GetParameters().Length != 1)
        {
            throw new InvalidOperationException($"Method {_methodName} not found or invalid in type {_modelType.Name}.");
        }

        // Determine the expected parameter type
        var parameterType = method.GetParameters()[0].ParameterType;
        object context;

        if (parameterType == typeof(System.ComponentModel.DataAnnotations.ValidationContext))
        {
            // Use a dummy instance if value is null
            var dummyInstance = new object();
            context = new System.ComponentModel.DataAnnotations.ValidationContext(
                value ?? dummyInstance, serviceProvider: null, items: null
            )
            {
                DisplayName = propertyName
            };
        }
        else if (parameterType == typeof(ValidationContext))
        {
            // Use a custom context
            context = new ValidationContext(value, propertyName);
        }
        else
        {
            throw new InvalidOperationException($"Unsupported parameter type for method {_methodName}: {parameterType.Name}");
        }

        // Invoke the validation method
        return (bool)method.Invoke(null, new object[] { context });
    }

    /// <summary>
    ///  Method to invoke a context-based validation method on the model type
    /// </summary>
    /// <param name="value"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    private bool InvokeContextMethod(object value, string propertyName)
    {
        if (_modelType == null || string.IsNullOrEmpty(_methodName))
        {
            return true; // No validation if context method is missing.
        }

        var method = _modelType.GetMethod(_methodName, BindingFlags.Public | BindingFlags.Static);
        if (method == null || method.ReturnType != typeof(bool) || method.GetParameters().Length != 1)
        {
            return true; // Ignore if method is not found or invalid.
        }

        var context = new ValidationContext(value, propertyName);
        return (bool)method.Invoke(null, new object[] { context });
    }

    /// <summary>
    ///  Method to get the error message for a validation failure
    /// </summary>
    /// <param name="value"></param>
    /// <param name="propertyName"></param>
    /// <param name="propertyType"></param>
    /// <returns></returns>
    public string GetErrorMessage(object value, string propertyName, Type propertyType)
    {
        if (string.IsNullOrEmpty(ErrorMessage))
        {
            ErrorMessage = _validationType switch
            {
                Must.BeGreaterThan => "Value must be greater than {Argument}.",
                Must.BeGreaterThanOrEqualTo => "Value must be greater than or equal to {Argument}.",
                Must.BeLessThan => "Value must be less than {Argument}.",
                Must.BeLessThanOrEqualTo => "Value must be less than or equal to {Argument}.",
                Must.BeEmpty => "@Field must be empty.",
                Must.NotBeEmpty => "@Field cannot be empty.",
                _ => "@Invalid value."
            };
        }

        // Format the message with placeholders
        ErrorMessage = ErrorMessage
            .Replace("@Field", propertyName)
            .Replace("{Argument}", _comparisonValue?.ToString())
            .Replace("{Value}", value?.ToString());

        if (ErrorMessage.Contains("{"))
            ErrorMessage = ResolvePlaceholders(value, ErrorMessage, propertyType);

        return ErrorMessage.Humanize() ?? $"Validation failed for property '{propertyName}' with condition '{_validationType}'.";
    }
    
    /// <summary>
    /// Method to resolve placeholders in the error message template
    /// </summary>
    /// <param name="value"></param>
    /// <param name="template"></param>
    /// <param name="propertyType"></param>
    /// <returns></returns>
    private string ResolvePlaceholders(object value, string template, Type propertyType)
    {
        return VariablePattern.Replace(template, match =>
        {
            string objectName = match.Groups["ObjectName"].Value;
            string property = match.Groups["Property"].Value;
            string format = match.Groups["Format"].Value;

            // Resolve property value if specified
            if (!string.IsNullOrEmpty(property))
            {
                var propertyInfo = propertyType?.GetProperty(property);
                if (propertyInfo == null)
                {
                    return "null"; // Return null if property metadata is unavailable
                }

                try
                {
                    // Attempt to cast the value to the property's type
                    value = Convert.ChangeType(value, propertyInfo.PropertyType, CultureInfo.InvariantCulture);
                }
                catch
                {
                    return $"{{Invalid cast to {propertyInfo.PropertyType.Name}}}"; // Return error for invalid casts
                }

                // Retrieve the property's value
                value = propertyInfo.GetValue(value);
            }

            // Apply formatting if specified
            if (!string.IsNullOrEmpty(format))
            {
                if (value is IFormattable formattable)
                {
                    return formattable.ToString(format, CultureInfo.InvariantCulture);
                }
                return $"{{{objectName}:{format}}}"; // Keep unresolved for non-formattable types
            }

            // Return the resolved value as a string
            return value?.ToString() ?? string.Empty;
        });
    }
}