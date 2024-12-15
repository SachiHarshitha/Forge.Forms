# Forge.Forms.AvaloniaUI

## 📖 About

[Forge.Forms ](https://github.com/WPF-Forge/Forge.Forms)you can create dynamic forms in WPF from classes or XML.
Using this library is straightforward. DynamicForm is a control that will render controls bound to an associated model.
A model can be an object, a type, a primitive, or a custom IFormDefinition.

Given the flexibility it provides especially in MVVM based
applications, [Sachith Liyanagama](https://github.com/SachiHarshitha), is currently porting the project into the *
*[Avalonia](https://avaloniaui.net/) framework**.

To get started, check out the original [guide](https://wpf-forge.github.io/Forge.Forms/guides/getting-started) for now.
Avalonia-Specific guide will be documented later.
or follow the [installation](#installation) instructions below.

## Demo Project in Avalonia

| **Desktop**                                                                                                                           | **WASM**                                                                                                                                         |
|---------------------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------|
| <img src="https://github.com/user-attachments/assets/c658e2ec-230d-4292-9e14-b50e9e686d24" title="Avalonia Desktop App" width="400"/> | <img src="https://github.com/user-attachments/assets/c773e661-8458-4755-9d65-17a10c738609" title="Avalonia Webassembly App (WASM)" width="400"/> |

## ⚠️ Warning

Due to the original WPF-Based library ships with dependencies to UI toolkits such as Material Design, there can be
certain bottlenecks.
Please consider this project/repository to be experimental until a better and cleaner approach has been developed.

## 🚀 Avalonia PORTING Log

Due to the complex usage of deferred bindings in the original project, porting to avalonia is done incermentally.
Similarly based on the functionality the components are classified as follows.

### Control Components

First iteration of the controls are ported using basic Fluent Theme styling and later need to be ported to CONTROL
THEMES.

|    Control Type    | Status                 | Comment                                                              |
|:------------------:|------------------------|----------------------------------------------------------------------|
|   Action Element   | :construction:         |                                                                      |
|   Boolean Field    | :construction:         |                                                                      |
|   Break Element    | :crossed_flags: Ported |                                                                      |
|    Card Element    | :crossed_flags: Ported | Dependency to Card Component of Material Design                      |
|  Converted Field   | :construction:         |                                                                      |
|     Date Field     | :crossed_flags: Ported | Must use DateTimeOffset for the bound properties instead of DateTime |
|     Time Field     | :zzz: Incomplete       | Avalonia Returns DateTime offset. Need to change the Resource type   |
|  Divider Element   | :crossed_flags: Ported |                                                                      |
| Error Text Element | :zzz: Incomplete       | Need to rewrite the validation concept.                              |
|  Heading Element   | :crossed_flags: Ported |                                                                      |
|   Image Element    | :zzz: Incomplete       |                                                                      |
|  Selection Field   | :zzz: Ported           | Dynamic Binding not available due to Binding Incompatibilities       |
|    Slider Field    | :construction:         |                                                                      |
|    Text Element    | :zzz:  Ported          | WASM seems to not handle the MultiBinding Strings.                   |
|   Title Element    | :crossed_flags: Ported |                                                                      |

### Logical Components

|          Logic          | Status           | Comment                                                                                                                                                                    |
|:-----------------------:|------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
|      Form Builder       | :zzz: Ported     | Need more testing                                                                                                                                                          |
|    XML Form Builder     | :zzz: Ported     | Need more testing. Bindings seems not to work.                                                                                                                             |
|       Attributes        | :zzz: Ported     | Mostly working, other than dynamic property bindings for Selection.                                                                                                        |
| Form Binding Extension  | :zzz: Incomplete | Given that the DataContext setting is not set mostly during initialization, had to optimize logic and seperate certain Bindings to use a converter (FormBindingConverter). |
| Freezable Proxy Objects | :construction:   | Given Avalonia do not provide Freezable objects, need to rewrite persisted value extraction logic.                                                                         |
|         Dialogs         | :construction:   | Need to reconstruct using Avalonia.DialogHost.                                                                                                                             |
|      File Binding       | :zzz: Incomplete | FileWatcher only works on Desktop. Need to find an alternative.                                                                                                            |
|     Data Validation     | :construction:   | Need to rewrite the whole Validation logic. Because there is no **ValidationRule** in Avalonia.                                                                            |

## Installation

```
Import Project Forge.Forms.AvaloniaUI
```

For default style sheet, add this to App.axaml

```xaml
<StyleInclude Source="avares://Forge.Forms.AvaloniaUI/Themes/Base.axaml" />
```

### DynamicForm control

If you want to use `DynamicForm`, import this namespace in XAML:

```xml
xmlns:forms="clr-namespace:Forge.Forms.AvaloniaUI.Controls;assembly=Forge.Forms"
```

And use the control:

```xml
<forms:DynamicForm Model="{Binding Model}" />
```

### Displaying Windows

If you only need to show windows and dialogs, use the `Show` helper:

```csharp
using Forge.Forms;

await Show.Window().For(new Alert("Hello world!"));
```

### Displaying Dialogs 🚧

````csharp
using Forge.Forms;

await Show.Dialog().For<Login>(); 
````

~~Note: if you are using `Show.Dialog()` without specifying a dialog identifier, it expects you to have a `DialogHost`
in your XAML tree.~~
