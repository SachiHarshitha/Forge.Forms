# Forge.Forms.AvaloniaUI
![NuGet](https://img.shields.io/nuget/v/Forge.Forms.AvaloniaUI.svg)
## 📖 About

With [Forge.Forms ](https://github.com/WPF-Forge/Forge.Forms)you can create dynamic forms in WPF from classes or XML.
Using this library is straightforward. DynamicForm is a control that will render controls bound to an associated model.
A model can be an object, a type, a primitive, or a custom IFormDefinition.

Given the flexibility it provides especially in MVVM based
applications, [Sachith Liyanagama](https://github.com/SachiHarshitha), is currently porting the project into the *
*[Avalonia](https://avaloniaui.net/) framework**.

To get started, check out the original [guide](https://wpf-forge.github.io/Forge.Forms/guides/getting-started) for now.
Avalonia-Specific guide will be documented later.
or follow the [installation](#installation) instructions below.

## 📷 Screenshots

### Cross Platform User Interface ([Demo Project](https://github.com/SachiHarshitha/Forge.Forms/tree/avalonia/Forge.Forms.Avalonia.Demo))
| **Desktop**                                                                                                                           | **WASM**                                                                                                                                         |
|---------------------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------|
| <img src="https://github.com/user-attachments/assets/b65363fd-c91a-4a28-9518-2a73703f5b36" title="Avalonia Desktop App" width="400"/> | <img src="https://github.com/user-attachments/assets/c773e661-8458-4755-9d65-17a10c738609" title="Avalonia Webassembly App (WASM)" width="400"/> |

### Data Types & Control Components

| Type      | Sub Types                   | Screenshot                                                                                                                                                                                                   |
|-----------|-----------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| String    | String, Password, Multiline | ![image](https://github.com/user-attachments/assets/a4ce5b22-f9e2-4ae9-8abe-1be45d8e4ea9)                                                                                                                    |
| Numerical |                             | ![Numeric Up Down](https://github.com/user-attachments/assets/b964f927-94b3-4fa1-8415-b72f9fe9747f)<br/>![Slider](https://github.com/user-attachments/assets/a9594ca2-2bfb-4223-b141-b3aefc44004a)           |
| Date      |                             | ![image](https://github.com/user-attachments/assets/1351c12a-ff99-425e-ae83-d38264cab4c7)                                                                                                                    |
| Time      |                             | ![image](https://github.com/user-attachments/assets/92e17280-31d9-4535-a9e0-6121e529de37)                                                                                                                    |
| Selection |                             | ![Radio Buttons](https://github.com/user-attachments/assets/49b84594-8229-4265-9a3b-4c5acfd3ea82)<br/> ![Editable Combobox](https://github.com/user-attachments/assets/7e2dd256-fdb4-42e8-abed-7120cef359d6) |
| Boolean   |                             | ![Toggle Switch](https://github.com/user-attachments/assets/040c6eef-90ac-4ba5-a51b-c4a2ed6352e3)<br/> ![Checkbox](https://github.com/user-attachments/assets/0b6b6cea-848f-4e09-aa53-847008ea8e72)          |

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

|    Control Type    | Status                 | Comment                                                                                                                                |
|:------------------:|------------------------|----------------------------------------------------------------------------------------------------------------------------------------|
|   Action Element   | :crossed_flags: Ported |                                                                                                                                        |
|   Boolean Field    | :crossed_flags: Ported |                                                                                                                                        |
|   Break Element    | :crossed_flags: Ported |                                                                                                                                        |
|    Card Element    | :crossed_flags: Ported | Dependency to Card Component of Material Design, need to work on sizing.                                                               |
|  Converted Field   | :crossed_flags: Ported | Need to test more                                                                                                                      |
|     Date Field     | :crossed_flags: Ported | Must use DateTimeOffset for the bound properties instead of DateTime                                                                   |
|     Time Field     | :crossed_flags: Ported | Avalonia Returns DateTime offset. Need to change the Resource type                                                                     |
|  Divider Element   | :crossed_flags: Ported |                                                                                                                                        |
| Error Text Element | :zzz: Will be removed  | Do not needed.                                                                                                                         |
|  Heading Element   | :crossed_flags: Ported |                                                                                                                                        |
|   Image Element    | :crossed_flags: Ported | Need to copy image to the "Assets" folder and write path in the format of: ``` [Image("avares://(AssemblyName)/Assets/(filename)")]``` |
|  Selection Field   | :crossed_flags: Ported | Dynamic Binding not available due to Binding Incompatibilities                                                                         |
|    Slider Field    | :crossed_flags: Ported |                                                                                                                                        |
|    Text Element    | :crossed_flags: Ported | WASM seems to not handle the MultiBinding Strings.                                                                                     |
|   Title Element    | :crossed_flags: Ported |                                                                                                                                        |
|    Toggle Field    | :crossed_flags: Ported |                                                                                                                                        |

### Logical Components

|          Logic          | Status                    | Comment                                                                                                                                                                                                                                                |
|:-----------------------:|---------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
|      Form Builder       | :crossed_flags: Ported    | Need more testing                                                                                                                                                                                                                                      |
|    XML Form Builder     | :crossed_flags: Ported    | Need more testing. Bindings seems not to work.                                                                                                                                                                                                         |
|       Attributes        | :crossed_flags: Ported    | Mostly working, other than dynamic property bindings for Selection.                                                                                                                                                                                    |
| Form Binding Extension  | :crossed_flags: Recreated | Given that the DataContext setting is not set mostly during initialization, had to optimize logic and seperate certain Bindings to use a converter (FormBindingConverter).                                                                             |
| Freezable Proxy Objects | :crossed_flags: Ported    | Replaced Ifreezable with avalonia object and pass binding from control.                                                                                                                                                                                |
|         Dialogs         | :crossed_flags: Ported    | Need more testing and style optimization. Window needs to be optimized.                                                                                                                                                                                |
|      File Binding       | :zzz: Incomplete          | FileWatcher only works on Desktop. Need to find an alternative.                                                                                                                                                                                        |
|     Data Validation     | :crossed_flags: Recreated | Rewrote the validation logic in a simple manner using a model wrapper, Validation event and [DataValidationErrors](https://github.com/AvaloniaUI/Avalonia/blob/07f3ad23e49da9ced46b7a68392e78a150622c35/src/Avalonia.Controls/DataValidationErrors.cs) |

### Themes

| Framework         | Method                 | Status                    | How to Use                                                                                  |
|-------------------|------------------------|---------------------------|---------------------------------------------------------------------------------------------|
| Avalonia - Fluent | Styles & Control Theme | Currently being optimized | ```xaml <StyleInclude Source="avares://Forge.Forms.AvaloniaUI/Themes/Fluent.axaml" />```    |
| Material          | Styles & Control Theme | Currently being optimized | ```xaml <StyleInclude Source="avares://Forge.Forms.AvaloniaUI/Themes/Material.axaml" /> ``` |

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

<xmlns:forms="clr-namespace:Forge.Forms.AvaloniaUI.Controls;assembly=Forge.Forms"/>
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

Note: if you are using `Show.Dialog()` without specifying a dialog identifier, it expects you to have a `DialogHost`
in your XAML tree.
