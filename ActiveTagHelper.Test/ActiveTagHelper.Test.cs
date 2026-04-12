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
}