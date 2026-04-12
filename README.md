# ActiveTagHelper

[![Nuget](https://img.shields.io/nuget/v/ActiveTagHelper?style=flat)](https://www.nuget.org/packages/ActiveTagHelper)

Adds `active` class to your links (like menu items, or `a` tags, but it actually works on every tag) based on whether
its `asp-page`, `asp-controller` or `asp-action` matches the current URL.

It's easy – just add a little attribute to your links. This is how:

## Compatibility

Targets .NET 8 (LTS) and .NET 10 (LTS). Works on any version in between (e.g. .NET 9) via the .NET 8 build.

## Installation

1. Add a reference to the package from the cmd line:
    ```
    MyProject> dotnet add package ActiveTagHelper
    ```
2. Restore:
    ```
    MyProject> dotnet restore
    ```
3. Register the Tag Helpers in your application's `_ViewImports.cshtml` file:
    ```
   @addTagHelper *, ActiveTagHelper
    ```

To use it, you have two possibilities:

## Add `check-active` attribute to your links

Every link that should have its destination and current route compared needs to get the `check-active` attribute.

### Example

```razor
<a check-active asp-page="/Index">Index</a>
<a check-active asp-page="/Privacy">Privacy</a>
```

When the user is on the `Index` page, the code gets automatically changed to:

```razor
<a class="active" href="/Index">Index</a>
<a href="/Privacy">Privacy</a>
```

If there are already classes defined, `active` just gets appended to the existing ones. The `check-active` attribute
always gets removed.

By the way, this works on every HTML element, not just `<a>`.

## Set custom trigger css class instead of `check-active` attribute

You can also use a css class as a trigger. For example, here we use a class `nav-link`
as a trigger class, because we already have it as a distinguishing feature of navigation links.
Thus, we can leave out the `check-active` attribute and leverage that class.
Set it up in your Program.cs where you create your builder:

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

builder.Services.AddActiveTagHelper(o => o.TriggerClass = "nav-link"); // <- this is us!

var app = builder.Build();
// […]
```

### Example

```razor
<a class="nav-link" asp-page="/Index">Index</a>
<a class="nav-link" asp-page="/Privacy">Privacy</a>
```

When the user is on the `Index` page, the code gets automatically changed to:

```razor
<a class="nav-link active" asp-page="/Index">Index</a>
<a class="nav-link" asp-page="/Privacy">Privacy</a>
```

## Optional: set custom active class

You can change the name of the active CSS class by setting
a custom class in your service configuration.

For example:

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

builder.Services.AddActiveTagHelper(o => o.CssClass = "my-custom-class"); // <-- this sets your own class

var app = builder.Build();
// […]
```

## Matching behavior

When using MVC routing (`asp-controller` / `asp-action`):

- **Only `asp-controller` set:** active when the controller matches (any action)
- **Only `asp-action` set:** active when the action matches (any controller)
- **Both set:** active only when **both** controller and action match

When using Razor Pages routing (`asp-page`):

- Active when the page matches. Page routing is independent of controller/action.

### Changelog

#### 1.0.0

Initial

#### 1.0.1

Added support for custom css class

#### 1.1.0

Added possibility to have custom css class as trigger instead of attribute

#### 2.0.0

- **BREAKING:** `builder.MapActiveTagHelperClass(...)` is replaced by `builder.Services.AddActiveTagHelper(...)` (now
  follows ASP.NET `IServiceCollection` convention)
- Fixed: when both `asp-controller` and `asp-action` are set, both must now match (previously either one matching was
  enough)
- Fixed: `Action`, `Controller`, `Page` properties are now correctly nullable
- Fixed: replaced silent catch block with explicit attribute check
- Targets .NET 8 and .NET 10 (dropped .NET 6)