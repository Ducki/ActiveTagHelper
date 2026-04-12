namespace ActiveTagHelper.Test;

public class ActiveTagHelperTest
{
    [Fact]
    public void Instantiate_ShouldCreateInstance_WhenOptionsProvided()
    {
        // Arrange
        var options = Options.Create(new ActiveTagHelperOptions());

        // Act
        var tagHelper = new global::ActiveTagHelper.ActiveTagHelper(options);

        // Assert
        Assert.NotNull(tagHelper);
    }

    [Fact]
    public void Process_ShouldNotThrow_WhenPropertiesAreNull()
    {
        // Arrange
        var options = Options.Create(new ActiveTagHelperOptions { TriggerClass = "nav-link" });
        var tagHelper = new global::ActiveTagHelper.ActiveTagHelper(options)
        {
            // Action, Controller, Page bleiben null (nicht gesetzt)
            CheckActive = true
        };

        var routeData = new RouteData();
        routeData.Values["controller"] = "Home";
        routeData.Values["action"] = "Index";

        tagHelper.Vc = new ViewContext { RouteData = routeData };

        var context = MakeTagHelperContext();
        var output = MakeTagHelperOutput();

        // Act
        var exception = Record.Exception(() => tagHelper.Process(context, output));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Process_ShouldNotBeActive_WhenActionDoesNotMatch()
    {
        // Arrange
        var options = Options.Create(new ActiveTagHelperOptions());
        var tagHelper = new global::ActiveTagHelper.ActiveTagHelper(options)
        {
            Controller = "Home",
            Action = "About",
            CheckActive = true
        };

        var routeData = new RouteData();
        routeData.Values["controller"] = "Home";
        routeData.Values["action"] = "Index"; // Action stimmt NICHT

        tagHelper.Vc = new ViewContext { RouteData = routeData };

        var context = MakeTagHelperContext();
        var output = MakeTagHelperOutput();

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.DoesNotContain("active", output.Attributes
            .FirstOrDefault(a => a.Name == "class")?.Value?.ToString() ?? "");
    }

    [Fact]
    public void Process_ShouldBeActive_WhenControllerAndActionBothMatch()
    {
        // Arrange
        var options = Options.Create(new ActiveTagHelperOptions());
        var tagHelper = new global::ActiveTagHelper.ActiveTagHelper(options)
        {
            Controller = "Home",
            Action = "Index",
            CheckActive = true
        };

        var routeData = new RouteData();
        routeData.Values["controller"] = "Home";
        routeData.Values["action"] = "Index";

        tagHelper.Vc = new ViewContext { RouteData = routeData };

        var context = MakeTagHelperContext();
        var output = MakeTagHelperOutput();

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Contains("active", output.Attributes
            .FirstOrDefault(a => a.Name == "class")?.Value?.ToString() ?? "");
    }

    [Fact]
    public void Process_ShouldBeActive_WhenOnlyControllerMatchesAnyAction()
    {
        // Arrange
        var options = Options.Create(new ActiveTagHelperOptions());
        var tagHelper = new global::ActiveTagHelper.ActiveTagHelper(options)
        {
            Controller = "Home",
            CheckActive = true
        };

        var routeData = new RouteData();
        routeData.Values["controller"] = "Home";
        routeData.Values["action"] = "Whatever";

        tagHelper.Vc = new ViewContext { RouteData = routeData };

        var context = MakeTagHelperContext();
        var output = MakeTagHelperOutput();

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Contains("active", output.Attributes
            .FirstOrDefault(a => a.Name == "class")?.Value?.ToString() ?? "");
    }

    [Fact]
    public void Process_ShouldBeActive_WhenPageMatches()
    {
        // Arrange
        var options = Options.Create(new ActiveTagHelperOptions());
        var tagHelper = new global::ActiveTagHelper.ActiveTagHelper(options)
        {
            Page = "/Index",
            CheckActive = true
        };

        var routeData = new RouteData();
        routeData.Values["page"] = "/Index";

        tagHelper.Vc = new ViewContext { RouteData = routeData };

        var context = MakeTagHelperContext();
        var output = MakeTagHelperOutput();

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Contains("active", output.Attributes
            .FirstOrDefault(a => a.Name == "class")?.Value?.ToString() ?? "");
    }

    private static TagHelperContext MakeTagHelperContext(TagHelperAttributeList? attributes = null)
    {
        attributes ??= new TagHelperAttributeList();
        return new TagHelperContext(
            tagName: "a",
            allAttributes: attributes,
            items: new Dictionary<object, object>(),
            uniqueId: "test-id");
    }

    private static TagHelperOutput MakeTagHelperOutput(TagHelperAttributeList? attributes = null)
    {
        attributes ??= new TagHelperAttributeList();
        return new TagHelperOutput(
            tagName: "a",
            attributes: attributes,
            getChildContentAsync: (useCachedResult, encoder) =>
                Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));
    }
}