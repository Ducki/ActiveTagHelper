using Microsoft.Extensions.DependencyInjection;

namespace ActiveTagHelper.Test;

public class ActiveTagHelperTest
{
    [Fact]
    public void Instantiate_ShouldCreateInstance_WhenOptionsProvided()
    {
        // Arrange
        var options = Options.Create(new ActiveTagHelperOptions());

        // Act
        var tagHelper = new ActiveTagHelper(options);

        // Assert
        Assert.NotNull(tagHelper);
    }

    [Fact]
    public void Process_ShouldNotThrow_WhenPropertiesAreNull()
    {
        // Arrange
        var options = Options.Create(new ActiveTagHelperOptions { TriggerClass = "nav-link" });
        var tagHelper = new ActiveTagHelper(options)
        {
            // Action, Controller, Page bleiben null (nicht gesetzt)
            CheckActive = true
        };

        var routeData = new RouteData
        {
            Values =
            {
                ["controller"] = "Home",
                ["action"] = "Index"
            }
        };

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
        var tagHelper = new ActiveTagHelper(options)
        {
            Controller = "Home",
            Action = "About",
            CheckActive = true
        };

        var routeData = new RouteData
        {
            Values =
            {
                ["controller"] = "Home",
                ["action"] = "Index" // Action stimmt NICHT
            }
        };

        tagHelper.Vc = new ViewContext { RouteData = routeData };

        var context = MakeTagHelperContext();
        var output = MakeTagHelperOutput();

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.DoesNotContain("active",
            output.Attributes
                .FirstOrDefault(a => a.Name == "class")?.Value?.ToString()
            ?? "");
    }

    [Fact]
    public void Process_ShouldBeActive_WhenControllerAndActionBothMatch()
    {
        // Arrange
        var options = Options.Create(new ActiveTagHelperOptions());
        var tagHelper = new ActiveTagHelper(options)
        {
            Controller = "Home",
            Action = "Index",
            CheckActive = true
        };

        var routeData = new RouteData
        {
            Values =
            {
                ["controller"] = "Home",
                ["action"] = "Index"
            }
        };

        tagHelper.Vc = new ViewContext { RouteData = routeData };

        var context = MakeTagHelperContext();
        var output = MakeTagHelperOutput();

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Contains("active",
            output.Attributes
                .FirstOrDefault(a => a.Name == "class")?.Value?.ToString()
            ?? "");
    }

    [Fact]
    public void Process_ShouldBeActive_WhenOnlyControllerMatchesAnyAction()
    {
        // Arrange
        var options = Options.Create(new ActiveTagHelperOptions());
        var tagHelper = new ActiveTagHelper(options)
        {
            Controller = "Home",
            CheckActive = true
        };

        var routeData = new RouteData
        {
            Values =
            {
                ["controller"] = "Home",
                ["action"] = "Whatever"
            }
        };

        tagHelper.Vc = new ViewContext { RouteData = routeData };

        var context = MakeTagHelperContext();
        var output = MakeTagHelperOutput();

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Contains("active",
            output.Attributes
                .FirstOrDefault(a => a.Name == "class")?.Value?.ToString()
            ?? "");
    }

    [Fact]
    public void Process_ShouldBeActive_WhenPageMatches()
    {
        // Arrange
        var options = Options.Create(new ActiveTagHelperOptions());
        var tagHelper = new ActiveTagHelper(options)
        {
            Page = "/Index",
            CheckActive = true
        };

        var routeData = new RouteData
        {
            Values =
            {
                ["page"] = "/Index"
            }
        };

        tagHelper.Vc = new ViewContext { RouteData = routeData };

        var context = MakeTagHelperContext();
        var output = MakeTagHelperOutput();

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Contains("active",
            output.Attributes
                .FirstOrDefault(a => a.Name == "class")?.Value?.ToString()
            ?? "");
    }

    [Fact]
    public void Process_ShouldNotThrow_WhenNoClassAttributeExists()
    {
        // Arrange
        var options = Options.Create(new ActiveTagHelperOptions());
        var tagHelper = new ActiveTagHelper(options)
        {
            Controller = "Home",
            CheckActive = true
        };

        var routeData = new RouteData
        {
            Values =
            {
                ["controller"] = "Home"
            }
        };

        tagHelper.Vc = new ViewContext { RouteData = routeData };

        var context = MakeTagHelperContext(); // Kein class-Attribut
        var output = MakeTagHelperOutput();

        // Act
        var exception = Record.Exception(() => tagHelper.Process(context, output));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Process_ShouldPreserveExistingClasses_WhenClassAttributeExists()
    {
        // Arrange
        var options = Options.Create(new ActiveTagHelperOptions());
        var tagHelper = new ActiveTagHelper(options)
        {
            Controller = "Home",
            Class = "nav-link",
            CheckActive = true
        };

        var routeData = new RouteData
        {
            Values =
            {
                ["controller"] = "Home"
            }
        };

        tagHelper.Vc = new ViewContext { RouteData = routeData };

        var attributes = new TagHelperAttributeList { { "class", "nav-link" } };
        var context = MakeTagHelperContext(attributes);
        var output = MakeTagHelperOutput(new TagHelperAttributeList(attributes));

        // Act
        tagHelper.Process(context, output);

        // Assert
        var classValue = output.Attributes.FirstOrDefault(a => a.Name == "class")?.Value?.ToString() ?? "";
        Assert.Contains("nav-link", classValue);
        Assert.Contains("active", classValue);
    }

    [Fact]
    public void AddActiveTagHelper_ShouldRegisterOptions_WhenConfigured()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddActiveTagHelper(opts =>
        {
            opts.CssClass = "current";
            opts.TriggerClass = "nav-link";
        });

        // Assert
        var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<ActiveTagHelperOptions>>();
        Assert.Equal("current", options.Value.CssClass);
        Assert.Equal("nav-link", options.Value.TriggerClass);
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