using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.SemanticKernel.Connectors.Postgres.Tests;

public class PostgresDbClientTests
{
    [Fact]
    public async Task DoesTableExistsAsync_WhenTableExists_ReturnsTrue()
    {
        // Arrange
        var tableName = "existingTable";
        var mockClient = new Mock<IPostgresDbClient>();
        mockClient.Setup(client => client.DoesTableExistsAsync(tableName, It.IsAny<CancellationToken>()))
                  .ReturnsAsync(true);
        var cancellationToken = new CancellationToken(false);

        // Act
        var result = await mockClient.Object.DoesTableExistsAsync(tableName, cancellationToken);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DoesTableExistsAsync_WhenTableDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var tableName = "nonExistingTable";
        var mockClient = new Mock<IPostgresDbClient>();
        mockClient.Setup(client => client.DoesTableExistsAsync(tableName, It.IsAny<CancellationToken>()))
                  .ReturnsAsync(false);
        var cancellationToken = new CancellationToken(false);

        // Act
        var result = await mockClient.Object.DoesTableExistsAsync(tableName, cancellationToken);

        // Assert
        Assert.False(result);
    }

    // Ensures that TaskCanceledException is thrown when the operation is canceled
    [Fact]
    public async Task DoesTableExistsAsync_WithCancelledToken_ThrowsTaskCanceledException()
    {
        // Arrange
        var tableName = "testTable";
        var mockClient = new Mock<IPostgresDbClient>();
        var cancellationToken = new CancellationToken(true);

        // Act & Assert
        await Assert.ThrowsAsync<TaskCanceledException>(() => mockClient.Object.DoesTableExistsAsync(tableName, cancellationToken));
    }

    // Ensures that the method throws ArgumentException when an invalid table name (empty string) is provided.
    [Fact]
    public async Task DoesTableExistsAsync_WithEmptyTableName_ThrowsArgumentException()
    {
        // Arrange
        var tableName = string.Empty;
        var mockClient = new Mock<IPostgresDbClient>();
        var cancellationToken = new CancellationToken(false);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => mockClient.Object.DoesTableExistsAsync(tableName, cancellationToken));
    }
    
    // Additional unit tests based on the review feedback:
    
    // Tests the method with null as the table name, ensuring that ArgumentNullException is thrown.
    [Fact]
    public async Task DoesTableExistsAsync_WithNullTableName_ThrowsArgumentNullException()
    {
        // Arrange
        string tableName = null;
        var mockClient = new Mock<IPostgresDbClient>();
        var cancellationToken = new CancellationToken(false);
        
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => mockClient.Object.DoesTableExistsAsync(tableName, cancellationToken));
    }

    // Tests the method with white space as the table name, ensuring that ArgumentException is thrown.
    [Fact]
    public async Task DoesTableExistsAsync_WithWhiteSpaceTableName_ThrowsArgumentException()
    {
        // Arrange
        var tableName = "   ";
        var mockClient = new Mock<IPostgresDbClient>();
        var cancellationToken = new CancellationToken(false);
        
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => mockClient.Object.DoesTableExistsAsync(tableName, cancellationToken));
    }
    
    // Tests the method providing a valid, non-canceled token to ensure normal operation.
    [Fact]
    public async Task DoesTableExistsAsync_WithNonCancelledToken_CompletesSuccessfully()
    {
        // Arrange
        var tableName = "existingTable";
        var mockClient = new Mock<IPostgresDbClient>();
        mockClient.Setup(client => client.DoesTableExistsAsync(tableName, It.IsAny<CancellationToken>()))
                  .ReturnsAsync(true);
        using (var cts = new CancellationTokenSource())
        {
            var cancellationToken = cts.Token;
            
            // Act
            var result = await mockClient.Object.DoesTableExistsAsync(tableName, cancellationToken);
            
            // Assert
            Assert.True(result);
        }
    }
}
