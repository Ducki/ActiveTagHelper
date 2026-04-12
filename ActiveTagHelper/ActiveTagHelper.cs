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
    [HtmlAttributeName("asp-action")] public string? Action { get; set; }
    [HtmlAttributeName("asp-controller")] public string? Controller { get; set; }
    [HtmlAttributeName("asp-page")] public string? Page { get; set; }
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
        var currentPage = Vc.RouteData.Values["page"]?.ToString();
        var currentController = Vc.RouteData.Values["controller"]?.ToString();
        var currentAction = Vc.RouteData.Values["action"]?.ToString();

        // Page-based routing (Razor Pages) — independent of Controller/Action
        if (Page is not null)
        {
            return string.Equals(currentPage, Page, StringComparison.OrdinalIgnoreCase);
        }

        // Controller/Action-based routing (MVC)
        var controllerMatches = Controller is null
            || string.Equals(currentController, Controller, StringComparison.OrdinalIgnoreCase);

        var actionMatches = Action is null
            || string.Equals(currentAction, Action, StringComparison.OrdinalIgnoreCase);

        // Both must be true. If only Controller is set, Action is null → actionMatches = true.
        // If only Action is set, Controller is null → controllerMatches = true.
        // If both are set, both must match. If neither is set, returns true (no filter).
        if (Controller is not null || Action is not null)
        {
            return controllerMatches && actionMatches;
        }

        return false;
    }
}

public static class UseExtension
{
    public static void MapActiveTagHelperClass(this WebApplicationBuilder app, Action<ActiveTagHelperOptions> options)
    {
        app.Services.AddOptions<ActiveTagHelperOptions>().Configure(options);
    }
}