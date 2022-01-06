
public class ActiveTagHelperOptions
{
    /// <summary>
    /// Sets the css class for the tag. Default is 'active'.
    /// </summary>
    public string CssClass { get; set; } = "active";

    /// <summary>
    /// Sets the attribute that triggers the TagHelper. Default is 'check-active'.
    /// </summary>
    public string TriggerAttribute { get; set; } = "check-active";
}
