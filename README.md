# ActiveTagHelper

Adds `active` class to `<a>` element when it mirrors the current route.

## Installing (doesn't work yet, because it hasn't been published to nuget :)
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