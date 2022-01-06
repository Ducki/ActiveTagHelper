using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ActiveTagHelper;

[HtmlTargetElement(Attributes = "check-active")]
public class ActiveTagHelper : TagHelper
{
    [ViewContext] public ViewContext Vc { get; set; } = null!;

    [HtmlAttributeName("asp-action")] public string Action { get; set; } = null!;

    [HtmlAttributeName("asp-controller")] public string Controller { get; set; } = null!;

    [HtmlAttributeName("asp-page")] public string Page { get; set; } = null!;

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