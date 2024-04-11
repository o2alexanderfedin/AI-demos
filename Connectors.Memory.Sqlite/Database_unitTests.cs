using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Xunit;

namespace Microsoft.SemanticKernel.Connectors.Sqlite.Tests;

public partial class DatabaseTests
{
    private static SqliteConnection CreateInMemorySqliteConnection()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        return connection;
    }

    [Fact]
    public async Task CreateCollectionAsync_AddsCollection_WhenItDoesNotExist()
    {
        // Arrange
        using var connection = CreateInMemorySqliteConnection();
        var database = new Database();
        string collectionName = "TestCollection";

        await database.CreateTableAsync(connection);

        // Act
        await database.CreateCollectionAsync(connection, collectionName);

        // Assert
        bool collectionExists = await database.DoesCollectionExistsAsync(connection, collectionName);
        Assert.True(collectionExists);
    }

    [Fact]
    public async Task CreateCollectionAsync_DoesNothing_WhenCollectionAlreadyExists()
    {
        // Arrange
        using var connection = CreateInMemorySqliteConnection();
        var database = new Database();
        string collectionName = "TestCollection";

        await database.CreateTableAsync(connection);

        // Act
        await database.CreateCollectionAsync(connection, collectionName);
        await database.CreateCollectionAsync(connection, collectionName); // Call second time

        // Assert
        bool collectionExists = await database.DoesCollectionExistsAsync(connection, collectionName);
        Assert.True(collectionExists); // Expect the collection to exist, no exception thrown
    }

    [Fact]
    public async Task CreateCollectionAsync_Throws_WhenCollectionNameIsNull()
    {
        // Arrange
        using var connection = CreateInMemorySqliteConnection();
        var database = new Database();

        await database.CreateTableAsync(connection);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => database.CreateCollectionAsync(connection, null));
    }

    [Fact]
    public async Task CreateCollectionAsync_RespectsCancellationToken()
    {
        // Arrange
        using var connection = CreateInMemorySqliteConnection();
        var database = new Database();
        string collectionName = "CancelableCollection";

        await database.CreateTableAsync(connection);

        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        // Act & Assert
        await Assert.ThrowsAsync<TaskCanceledException>(() => database.CreateCollectionAsync(connection, collectionName, cancellationTokenSource.Token));
    }

    [Fact]
    public async Task CreateCollectionAsync_Throws_WhenSqlConnectionIsNull()
    {
        // Arrange
        var database = new Database();
        string collectionName = "TestCollection";

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => database.CreateCollectionAsync(null, collectionName));
    }

    [Fact]
    public async Task CreateCollectionAsync_DoesNotThrow_WhenCollectionNameIsEmpty()
    {
        // Arrange
        using var connection = CreateInMemorySqliteConnection();
        var database = new Database();
        string collectionName = string.Empty;

        await database.CreateTableAsync(connection);

        // Act
        await database.CreateCollectionAsync(connection, collectionName); // Should not throw

        // Assert
        bool collectionExists = await database.DoesCollectionExistsAsync(connection, collectionName);
        Assert.True(collectionExists); // Expect empty collection name to be valid and exist
    }
}
