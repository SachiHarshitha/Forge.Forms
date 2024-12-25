using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Forge.Forms.AvaloniaUI.Annotations;

namespace Forge.Forms.AvaloniaUI.Validator;

public class ModelWrapper<T> where T : class
{
    private readonly Dictionary<string, List<ModelValidator>> _validators = new();

    public ModelWrapper(T model)
    {
        Model = model ?? throw new ArgumentNullException(nameof(model));
        InitializeValidators();
    }

    public T Model { get; }

    public event EventHandler<ValidationErrorEventArgs> ValidationErrorsChanged;

    /// <summary>
    /// Initialize the Validators for the model.
    /// </summary>
    private void InitializeValidators()
    {
        var properties = Model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var property in properties)
        {
            var attributes = property.GetCustomAttributes<ValueAttribute>().ToList();
            if (!attributes.Any()) continue;

            var validators = new List<ModelValidator>();
            foreach (var attribute in attributes)
            {
                // Initialize the 'When' condition if provided
                InitializeWhenBinding(attribute, Model, property.Name);

                // Create CustomValidator based on ValueAttribute
                var validator = new ModelValidator(
                    attribute.Condition,
                    attribute.Message,
                    attribute.Argument,
                    Model.GetType(), // Model type (for SatisfyMethod)
                    attribute.Condition == Must.SatisfyMethod ? attribute.Argument as string : null
                )
                {
                    IgnoreNullOrEmpty = attribute.IgnoreNullOrEmpty
                };

                validators.Add(validator);
            }

            _validators[property.Name] = validators;
        }
    }

    /// <summary>
    /// Initialize When condition for the property. 
    /// </summary>
    /// <param name="attribute"></param>
    /// <param name="model"></param>
    /// <param name="propertyName"></param>
    /// <exception cref="InvalidOperationException"></exception>
    private void InitializeWhenBinding(ValueAttribute attribute, object model, string propertyName)
    {
        if (attribute.When is string whenExpression && whenExpression.StartsWith("{Binding "))
        {
            // Extract the property name from the binding expression
            var match = Regex.Match(whenExpression, @"\{Binding (?<PropertyName>\w+)\}");
            if (!match.Success)
            {
                throw new InvalidOperationException($"Invalid binding expression: {whenExpression}");
            }

            var boundPropertyName = match.Groups["PropertyName"].Value;

            // Subscribe to PropertyChanged event of the model
            if (model is INotifyPropertyChanged notifyPropertyChanged)
            {
                notifyPropertyChanged.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == boundPropertyName)
                    {
                        // Re-evaluate the 'When' condition
                        var boundPropertyValue = model.GetType().GetProperty(boundPropertyName)?.GetValue(model);
                        bool isActive = boundPropertyValue is bool b && b;

                        // Trigger validation based on the updated 'When' condition
                        if (isActive)
                        {
                            ValidateProperty(propertyName);
                        }
                    }
                };
            }
        }
        else if (attribute.When == null)
        {
            // Default behavior if no 'When' condition is provided
            attribute.When = true;
        }
    }

    /// <summary>
    /// Validates a specific property.
    /// </summary>
    /// <param name="propertyName">The name of the property to validate.</param>
    /// <returns>A list of validation errors (empty if valid).</returns>
    public List<string> ValidateProperty(string propertyName)
    {
        if (!_validators.ContainsKey(propertyName))
        {
            throw new ArgumentException($"No validators found for property: {propertyName}");
        }

        var property = Model.GetType().GetProperty(propertyName);
        if (property == null)
        {
            throw new ArgumentException($"Property not found: {propertyName}");
        }

        var value = property.GetValue(Model);
        var errors = new List<string>();

        foreach (var validator in _validators[propertyName])
        {
            var isValid = validator.Validate(value, propertyName, property.PropertyType);
            if (!isValid)
            {
                errors.Add(validator.GetErrorMessage(value, propertyName, property.PropertyType));
            }
        }

        // Trigger the event
        ValidationErrorsChanged?.Invoke(this, new ValidationErrorEventArgs(propertyName, errors));

        return errors;
    }

    /// <summary>
    /// Invalidate Property By force
    /// </summary>
    /// <param name="propertyName"></param>
    /// <param name="errorMessage"></param>
    /// <exception cref="ArgumentException"></exception>
    public void InvalidateProperty(string propertyName, string errorMessage)
    {
        var errors = new List<string> { errorMessage };

        // Trigger the event
        ValidationErrorsChanged?.Invoke(this, new ValidationErrorEventArgs(propertyName, errors));
    }

    /// <summary>
    /// Validates the entire model.
    /// </summary>
    /// <returns>A dictionary of property names and their validation errors.</returns>
    public Dictionary<string, List<string>> ValidateModel()
    {
        var result = new Dictionary<string, List<string>>();

        foreach (var propertyName in _validators.Keys)
        {
            try
            {
                var errors = ValidateProperty(propertyName);
                if (errors.Any())
                {
                    result[propertyName] = errors;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Failed to validate property {propertyName}, due to:{e}");
            }
        }

        return result;
    }

    /// <summary>
    /// Invalidate Model by Force
    /// </summary>
    public void InvalidateModel()
    {
        foreach (var propertyName in _validators.Keys)
        {
            try
            {
                InvalidateProperty(propertyName, "Invalid property value.");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Failed to Invalidate property {propertyName}, due to:{e}");
            }
        }
    }
}