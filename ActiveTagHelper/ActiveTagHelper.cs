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

[HtmlTargetElement(TagStructure = TagStructure.WithoutEndTag)]
public class ActiveTagHelper : TagHelper
{
    [ViewContext] public ViewContext Vc { get; set; } = null!;
    [HtmlAttributeName("asp-action")] public string Action { get; set; } = null!;
    [HtmlAttributeName("asp-controller")] public string Controller { get; set; } = null!;
    [HtmlAttributeName("asp-page")] public string Page { get; set; } = null!;
    [HtmlAttributeName("class")] public string Class { get; set; } = null!;
    [HtmlAttributeName("check-active")] public bool CheckActive { get; set; }

    private readonly ActiveTagHelperOptions _options;

    public ActiveTagHelper(IOptions<ActiveTagHelperOptions> options)
    {
        _options = options.Value;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var hasCssTrigger = HasCssTrigger();
        var hasCheckActiveAttribute = CheckActive;

        // Retain the previous CSS classes
        try
        {
            output.CopyHtmlAttribute("class", context);
        }
        catch (Exception)
        {
            // Fail silently
        }

        if (!hasCssTrigger && !hasCheckActiveAttribute) return;

        if (IsActive())
        {
            output.AddClass(_options.CssClass, System.Text.Encodings.Web.HtmlEncoder.Default);
        }
    }

    private bool HasCssTrigger()
    {
        var classes = Class?.Split(' ');
        return classes?.Any(c => c.Equals(_options.TriggerClass)) ?? false;
    }

    private bool IsActive()
    {
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

        return isActive;
    }
}

public static class UseExtension
{
    public static void MapActiveTagHelperClass(this WebApplicationBuilder app, Action<ActiveTagHelperOptions> options)
    {
        app.Services.AddOptions<ActiveTagHelperOptions>().Configure(options);
    }
}