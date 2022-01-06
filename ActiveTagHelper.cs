using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ActiveTagHelper;

[HtmlTargetElement(Attributes = "check-active")]
public class ActiveTagHelper : TagHelper
{
    [ViewContext] private ViewContext Vc { get; set; } = null!;

    [HtmlAttributeName("asp-action")] private string Action { get; set; } = null!;

    [HtmlAttributeName("asp-controller")] private string Controller { get; set; } = null!;

    [HtmlAttributeName("asp-page")] private string Page { get; set; } = null!;

    private ActiveTagHelperOptions Options { get; set; }


    public ActiveTagHelper(IOptions<ActiveTagHelperOptions> options)
    {
        this.Options = options.Value;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        
        var routeValue = Vc.RouteData.Values["page"].ToString();

        if (routeValue.Equals(Page, StringComparison.OrdinalIgnoreCase))
        {
            output.AddClass("active", System.Text.Encodings.Web.HtmlEncoder.Default);
        }

        output.Attributes.RemoveAll("check-active");
    }
}

public static class UseExtension
{
    public static void MapActiveTagHelperOptions(this WebApplicationBuilder app, Action<ActiveTagHelperOptions> conf)
    {
        app.Services.AddOptions<ActiveTagHelperOptions>().Configure(conf);
    }
}