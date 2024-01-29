// Unit tests for the PostgresMemoryStore class.
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.SemanticKernel.Connectors.Postgres;
using Microsoft.SemanticKernel.Memory;
using Xunit;
using Moq;

namespace Microsoft.SemanticKernel.UnitTests.Connectors.Postgres;

public class PostgresMemoryStoreTests
{
    private const string connectionString = "Host=localhost;Username=myuser;Password=mypass;Database=mydatabase";
    private const int vectorSize = 128;
    private const string testCollectionName = "test_collection";
    private const string testKey = "test_key";
    private readonly MemoryRecord testRecord = new MemoryRecord("test_metadata", new float[vectorSize], testKey);

    private readonly Mock<IPostgresDbClient> mockDbClient = new Mock<IPostgresDbClient>();

    public PostgresMemoryStoreTests()
    {
        mockDbClient.Setup(m => m.CreateTableAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        // Stub other methods as needed for the specific tests.
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("    ")]
    public async Task CreateCollectionAsync_WithInvalidCollectionName_ThrowsArgumentException(string invalidCollectionName)
    {
        // Arrange
        var memoryStore = new PostgresMemoryStore(mockDbClient.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => memoryStore.CreateCollectionAsync(invalidCollectionName));
        Assert.Contains("collectionName", exception.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("    ")]
    public async Task DoesCollectionExistAsync_WithInvalidCollectionName_ThrowsArgumentException(string invalidCollectionName)
    {
        // Arrange
        var memoryStore = new PostgresMemoryStore(mockDbClient.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => memoryStore.DoesCollectionExistAsync(invalidCollectionName));
        Assert.Contains("collectionName", exception.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("    ")]
    public async Task DeleteCollectionAsync_WithInvalidCollectionName_ThrowsArgumentException(string invalidCollectionName)
    {
        // Arrange
        var memoryStore = new PostgresMemoryStore(mockDbClient.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => memoryStore.DeleteCollectionAsync(invalidCollectionName));
        Assert.Contains("collectionName", exception.Message);
    }

    // Additional test for the constructor argument validations
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("    ")]
    public void Constructor_WithInvalidConnectionString_ThrowsArgumentException(string invalidConnectionString)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new PostgresMemoryStore(invalidConnectionString, vectorSize));
        Assert.Contains("connectionString", exception.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Constructor_WithInvalidVectorSize_ThrowsArgumentException(int invalidVectorSize)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new PostgresMemoryStore(connectionString, invalidVectorSize));
        Assert.Contains("vectorSize must be positive", exception.Message);
    }

    // Tests for UpsertAsync argument validation
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("    ")]
    public async Task UpsertAsync_WithInvalidCollectionName_ThrowsArgumentException(string invalidCollectionName)
    {
        // Arrange
        var memoryStore = new PostgresMemoryStore(mockDbClient.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => memoryStore.UpsertAsync(invalidCollectionName, testRecord));
        Assert.Contains("collectionName", exception.Message);
    }

    [Fact]
    public async Task UpsertAsync_WithNullMemoryRecord_ThrowsArgumentNullException()
    {
        // Arrange
        var memoryStore = new PostgresMemoryStore(mockDbClient.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => memoryStore.UpsertAsync(testCollectionName, null));
    }

    // Additional methods to test argument validation for other methods
    // ...
}

// Please note, the unit test class should contain tests that address all review feedback points
// and follow the Arrange-Act-Assert pattern.
