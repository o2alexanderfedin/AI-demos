using Microsoft.Data.Sqlite;
using Xunit;
using Moq;
using System;
using Microsoft.SemanticKernel.Connectors.Sqlite;

public class SqliteExtensionsTests
{
    [Fact]
    public void GetFieldValue_WithValidFieldName_ReturnsValue()
    {
        // Arrange
        var mockReader = new Mock<SqliteDataReader>();
        mockReader.Setup(r => r.GetOrdinal("testField")).Returns(0);
        mockReader.Setup(r => r.GetFieldValue<int>(0)).Returns(123);

        // Act
        int result = SqliteExtensions.GetFieldValue<int>(mockReader.Object, "testField");

        // Assert
        Assert.Equal(123, result);
    }

    [Fact]
    public void GetFieldValue_WithInvalidFieldName_ThrowsException()
    {
        // Arrange
        var mockReader = new Mock<SqliteDataReader>();
        mockReader.Setup(r => r.GetOrdinal("invalidField")).Throws(new IndexOutOfRangeException("Invalid field name"));

        // Act & Assert
        Assert.Throws<IndexOutOfRangeException>(() => 
            SqliteExtensions.GetFieldValue<int>(mockReader.Object, "invalidField"));
    }

    [Fact]
    public void GetFieldValue_WithNullFieldName_ThrowsArgumentNullException()
    {
        // Arrange
        var mockReader = new Mock<SqliteDataReader>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            SqliteExtensions.GetFieldValue<int>(mockReader.Object, null));
    }

    [Fact]
    public void GetString_WithValidFieldName_ReturnsString()
    {
        // Arrange
        var mockReader = new Mock<SqliteDataReader>();
        string fieldName = "stringField";
        mockReader.Setup(r => r.GetOrdinal(fieldName)).Returns(1);
        mockReader.Setup(r => r.GetString(1)).Returns("some value");

        // Act
        string result = SqliteExtensions.GetString(mockReader.Object, fieldName);

        // Assert
        Assert.Equal("some value", result);
    }

    [Fact]
    public void GetString_WithInvalidFieldName_ThrowsException()
    {
        // Arrange
        var mockReader = new Mock<SqliteDataReader>();
        mockReader.Setup(r => r.GetOrdinal("invalidField")).Throws(new IndexOutOfRangeException("Invalid field name"));

        // Act & Assert
        Assert.Throws<IndexOutOfRangeException>(() => 
            SqliteExtensions.GetString(mockReader.Object, "invalidField"));
    }

    [Fact]
    public void GetString_WithNullFieldName_ThrowsArgumentNullException()
    {
        // Arrange
        var mockReader = new Mock<SqliteDataReader>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            SqliteExtensions.GetString(mockReader.Object, null));
    }
}
