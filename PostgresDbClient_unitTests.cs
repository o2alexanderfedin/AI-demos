using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.SemanticKernel.Connectors.Postgres;
using Moq;
using Npgsql;
using Xunit;

public class PostgresDbClientTests
{
    private readonly Mock<NpgsqlDataSource> mockDataSource = new();
    private readonly Mock<NpgsqlConnection> mockConnection = new();
    private readonly Mock<NpgsqlCommand> mockCommand = new();
    private readonly Mock<NpgsqlDataReader> mockDataReader = new();
    private readonly string mockSchema = "test_schema";
    private readonly int mockVectorSize = 3;
    private readonly string tableName = "existing_table";

    private PostgresDbClient CreateClient()
    {
        return new PostgresDbClient(mockDataSource.Object, mockSchema, mockVectorSize);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task DoesTableExistsAsync_ThrowsArgumentException_WhenTableNameIsNullOrWhiteSpace(string tableName)
    {
        // Arrange
        var client = CreateClient();

        // Act & Assert
        // Expect ArgumentException when passing null or empty string as tableName
        var exception = await Assert.ThrowsAnyAsync<ArgumentException>(() => client.DoesTableExistsAsync(tableName));
        Assert.Equal("tableName", exception.ParamName);
    }

    [Fact]
    public async Task DoesTableExistsAsync_ReturnsTrue_WhenTableExists()
    {
        // Arrange
        mockDataSource.Setup(ds => ds.OpenConnectionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockConnection.Object);
        mockConnection.Setup(conn => conn.CreateCommand())
            .Returns(mockCommand.Object);
        mockCommand.Setup(cmd => cmd.ExecuteReaderAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockDataReader.Object);
        mockDataReader.Setup(reader => reader.ReadAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        mockDataReader.Setup(reader => reader.GetString(It.IsAny<int>()))
            .Returns(tableName);

        var client = CreateClient();

        // Act
        var result = await client.DoesTableExistsAsync(tableName);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DoesTableExistsAsync_ReturnsFalse_WhenTableDoesNotExist()
    {
        // Arrange
        mockDataSource.Setup(ds => ds.OpenConnectionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockConnection.Object);
        mockConnection.Setup(conn => conn.CreateCommand())
            .Returns(mockCommand.Object);
        mockCommand.Setup(cmd => cmd.ExecuteReaderAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockDataReader.Object);
        mockDataReader.Setup(reader => reader.ReadAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false); // When the table does not exist, ReadAsync should return false

        var client = CreateClient();

        // Act
        var result = await client.DoesTableExistsAsync(tableName);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DoesTableExistsAsync_ThrowsException_WhenDatabaseOperationFails()
    {
        // Arrange
        mockDataSource.Setup(ds => ds.OpenConnectionAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NpgsqlException()); // Simulate an exception during database operation

        var client = CreateClient();

        // Act & Assert
        await Assert.ThrowsAsync<NpgsqlException>(() => client.DoesTableExistsAsync(tableName));
    }

    [Fact]
    public async Task DoesTableExistsAsync_UsesCorrectSqlCommand_WhenCalled()
    {
        // Arrange
        string actualCommandText = string.Empty;
        mockDataSource.Setup(ds => ds.OpenConnectionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockConnection.Object);
        mockConnection.Setup(conn => conn.CreateCommand())
            .Returns(mockCommand.Object);
        mockCommand.SetupSet(cmd => cmd.CommandText = It.IsAny<string>())
            .Callback<string>(cmdText => actualCommandText = cmdText);
        mockCommand.Setup(cmd => cmd.ExecuteReaderAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockDataReader.Object);
        mockDataReader.Setup(reader => reader.ReadAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false); // The command text is being verified, not the result

        var client = CreateClient();

        // Act
        await client.DoesTableExistsAsync(tableName);

        // Assert
        string expectedCommandText = @"
                SELECT table_name
                FROM information_schema.tables
                WHERE table_schema = @schema
                    AND table_type = 'BASE TABLE'
                    AND table_name = 'existing_table'";
        Assert.Contains("SELECT table_name", actualCommandText);
        Assert.Contains("FROM information_schema.tables", actualCommandText);
        Assert.Contains("WHERE table_schema = @schema", actualCommandText);
        Assert.Contains("AND table_type = 'BASE TABLE'", actualCommandText);
        Assert.Contains("AND table_name = 'existing_table'", actualCommandText); // Validates the constructed command text
    }
}
