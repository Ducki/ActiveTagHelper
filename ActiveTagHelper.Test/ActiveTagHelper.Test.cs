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