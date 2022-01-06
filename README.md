# ActiveTagHelper

Adds `active` class to your links (like menu items, or `a` tags, but it actually works on every tag) based on whether its `asp-page`, `asp-controller` or `asp-action` matches the current URL.

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
## Add `check-active` to your links
Every link that should be checked for an `active` class needs to get the `check-active` attribute.

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
If there are already classes defined, `active` just gets appended to the existing ones.