using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global

namespace ActiveTagHelper;

[HtmlTargetElement(Attributes = "check-active")]
public class ActiveTagHelper : TagHelper
{
    [ViewContext] public ViewContext Vc { get; set; } = null!;

    [HtmlAttributeName("asp-action")] public string Action { get; set; } = null!;

    [HtmlAttributeName("asp-controller")] public string Controller { get; set; } = null!;

    [HtmlAttributeName("asp-page")] public string Page { get; set; } = null!;

    private readonly ActiveTagHelperOptions _options;


    public ActiveTagHelper(IOptions<ActiveTagHelperOptions> options)
    {
        _options = options.Value;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.RemoveAll("check-active");

        var currentPage = "";
        var currentController = "";
        var currentAction = "";

        if (Vc.RouteData.Values["page"] is not null)
        {
            currentPage = Vc.RouteData.Values["page"]?.ToString();
        }
        if (Vc.RouteData.Values["controller"] is not null)
        {
            currentController = Vc.RouteData.Values["controller"]?.ToString();
        }
        if (Vc.RouteData.Values["action"] is not null)
        {
            currentAction = Vc.RouteData.Values["action"]?.ToString();
        }

        bool isActive = false;

        if ((currentPage is not null) && currentPage.Equals(Page, StringComparison.OrdinalIgnoreCase))
        {
            isActive = true;
        }

        if ((currentController is not null) && currentController.Equals(Controller, StringComparison.OrdinalIgnoreCase))
        {
            isActive = true;
        }
        if ((currentAction is not null) && currentAction.Equals(Action, StringComparison.OrdinalIgnoreCase))
        {
            isActive = true;
        }

        if (isActive)
        {
            output.AddClass(_options.CssClass, System.Text.Encodings.Web.HtmlEncoder.Default);
        }

    }
}

public static class UseExtension
{
    public static void MapActiveTagHelperClass(this WebApplicationBuilder app, Action<ActiveTagHelperOptions> options)
    {
        app.Services.AddOptions<ActiveTagHelperOptions>().Configure(options);
    }
}