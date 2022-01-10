# ActiveTagHelper
[![Nuget](https://img.shields.io/nuget/v/ActiveTagHelper?style=flat)](https://www.nuget.org/packages/ActiveTagHelper)

Adds `active` class to your links (like menu items, or `a` tags, but it actually works on every tag) based on whether its `asp-page`, `asp-controller` or `asp-action` matches the current URL.

It's easy – just add a little attribute to your links. This is how:

## Compatibility
Currently only tested on .NET 6.

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
## Add `check-active` attribute to your links
Every link that should have its destination and current route compared needs to get the `check-active` attribute.

### Example

```razor
<a check-active asp-page="/Index">Index</a>
<a check-active asp-page="/Privacy">Privacy</a>
```
When the user is on the `Index` page, the code gets automatically changed to:
```razor
<a class="active" asp-page="/Index">Index</a>
<a asp-page="/Privacy">Privacy</a>
```
If there are already classes defined, `active` just gets appended to the existing ones. The `check-active` attribute always gets removed.

By the way, this works on every HTML element, not just `<a>`.

## Optional: set custom class
You can change the name of the CSS class by setting a custom class in your host builder setup. The tag exposes a `MapActiveTagHelperClass` method in the `WebApplicationBuilder` object.

For example:

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

builder.MapActiveTagHelperClass(o => o.CssClass = "my-custom-class"); // <-- this sets your own class

var app = builder.Build();
// […]
```