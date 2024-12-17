using System.Text;

namespace EncryptedJson.Test;

[TestClass]
public class LoadersTests
{
    [TestMethod]
    public void Load_ExistingFile_ReturnsByteArray()
    {
        // Arrange
        string testFilePath = "test.txt";
        byte[] expectedContent = Encoding.UTF8.GetBytes("test content");
        File.WriteAllBytes(testFilePath, expectedContent);
        var loader = new FileLoader(testFilePath);

        // Act
        byte[] result = loader.Load();

        // Assert
        CollectionAssert.AreEqual(expectedContent, result);

        // Cleanup
        File.Delete(testFilePath);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void Load_NonExistingFile_ThrowsException()
    {
        // Arrange
        string nonExistingFilePath = "non_existing_file.json";
        var loader = new FileLoader(nonExistingFilePath);

        // Act & Assert (exception expected)
        loader.Load();
    }

    [TestMethod]
    public void Load_ValidBase64_ReturnsBytes()
    {
        // Arrange
        string base64Content = "SGVsbG8gV29ybGQh"; // "Hello World!" in Base64
        var loader = new Base64Loader(base64Content);

        // Act
        byte[] result = loader.Load();

        // Assert
        Assert.IsNotNull(result);
        string decodedString = System.Text.Encoding.UTF8.GetString(result);
        Assert.AreEqual("Hello World!", decodedString);
    }

    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void Load_InvalidBase64_ThrowsFormatException()
    {
        // Arrange
        string invalidBase64Content = "ThisIsNotBase64";
        var loader = new Base64Loader(invalidBase64Content);

        // Act & Assert (exception expected)
        loader.Load();
    }
}