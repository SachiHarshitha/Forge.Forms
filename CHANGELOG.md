# Changelog

## 1.2.0 (beta)

- Upgrade UI framework dependencies. Thanks to @BenjaminSieg and @ChezzPlaya.
- Fix window focus/keyboard focus when showing windows. Thanks to @aalex675.

## 1.1.4

- Added support for `MultiBinding` syntax in dynamic resources.

## 1.1.3

- Added `initialFocus` property for XML fields.

## 1.1.2

- Added property `IgnoreNullOrEmpty` to validation attributes. If this property is set to `true` then validation will pass if the value is null or an empty string. This is useful when a field is optional and validation needs to occur only when a value is given.
- Added DatePicker to metro theme.
- Fixed behavior of `WindowOptions.TopMost` and `WindowOptions.BringToFront`.
- Dropdown selections are now virtualized for better performance.

## 1.1.1

- Added `minheight` and `maxheight` to xml `<layout>` element. If content overflows `maxheight` a vertical scroll bar appears.

## 1.1.0

- `Prompt<T>` now validates input on positive action.
- Metro dialog window no longer forces controls theme.
- Metro dialog window now passes initial focus to the controls.
- Added ComboBox control to WPF theme.
- Added `IsDefault` and `IsCancel` bindings in WPF and Metro actions.
- Added `[DialogAction]` action that is visible only when the form is hosted in a dialog. This action has `ClosesDialog = true` by default.
- Dialogs and dialog windows have environment flag `DialogHostContext` added by default.
- Fixed getter only properties throwing two-way binding errors.
- Added `SelectionType.RadioButtonsInlineRightAligned` for right-aligned radio buttons.
- Added `[DirectContent]` attribute that allows passing element rendering directly to WPF.
- Changed resource reference from `MaterialDesignTextAreaTextBox` to `MaterialDesignOutlinedTextFieldTextBox`

## 1.0.19

- Added support for `IsEnabled` in attributes or `enabled` in XML for input elements.

## 1.0.18

- Fixed bug with bound expressions not escaping curly braces properly.

## 1.0.17

- Added FormBuilder.TypeConstructors that allows registering custom factories for custom XML input `type` attributes.
- Added TimePicker control
    - You can add it by decorating `DateTime` class properties with the `[Time]` attribute. Property `Is24Hours` can be assigned to a boolean or a dynamic resource.
    - You can add it via `<input type="time" ... />` in XML. Attribute `is24hours` can be assigned to a boolean or a dynamic resource.
- Fixed `MaterialColoredIconStyle` validation brush fill for invalid controls.

## 1.0.16

- Added error text element.
    - You can add it via `[ErrorText]` attribute in classes.
    - You can add it via `<error>` tag in XML.

## 1.0.15

- Added parameterized value converters which are created from factories registered in `Resource` class.
- Non-input XML elements accept `visible` property.
- Added `Invalidate`, `IsValid`, `ValidateWithoutUpdate`, `GetValidationErrors` utilities in ModelState class.
    - `Invalidate` temporarily fails validation for a property. If you modify the field, call `Validate`, or call `ValidateWithoutUpdate`, the marked error will disappear. To check the state of this validation error use `IsValid`/`GetValidationErrors`.
    - `IsValid` is useful if you want to check current validation state without triggering side-effects like `Validate`, which sometimes produces unexpected results.
    - `ValidateWithoutUpdate` should generally be avoided unless you are dealing with some strange edge cases.
    - `GetValidationErrors` returns the list of error strings from specified properties/whole model.
- Added `Must.BeInvalid` (and `Must.Fail` alias) validator for external validation. This validator always fails, so you must combine it with the `When` condition.
- Added `ValueAttribute.OnActivation` and `ValueAttribute.OnDeactivation` validation actions which specify what happens when the validator is enabled/disabled from the `When` condition.
- Added `SelectionType.RadioButtonsInline` for horizontal layout of radio buttons in selections.

## 1.0.14

- Stable NuGet release.
