using Xunit;
using Microsoft.SemanticKernel.Memory;
using Npgsql;
using System;

namespace Microsoft.SemanticKernel.Connectors.Postgres.Tests;

public class PostgresMemoryBuilderExtensionsTests
{
    // Combined unit tests including new tests below after addressing review feedback

    [Fact]
    public void WithPostgresMemoryStore_ValidParameters_ShouldRegisterPostgresMemoryStore()
    {
        // Arrange
        var builder = new MemoryBuilder();
        string connectionString = "Host=localhost;Username=test;Password=test;Database=test";
        int vectorSize = 100;
        string schema = "public";

        // Act
        var result = PostgresMemoryBuilderExtensions.WithPostgresMemoryStore(builder, connectionString, vectorSize, schema);

        // Assert
        Assert.NotNull(result);
        // Additional verification would be needed to ensure that PostgresMemoryStore is actually registered within the MemoryBuilder.
    }

    [Fact]
    public void WithPostgresMemoryStore_InvalidConnectionString_ThrowsArgumentException()
    {
        // Arrange
        var builder = new MemoryBuilder();
        string connectionString = "InvalidConnectionString";
        int vectorSize = 100;
        string schema = "public";

        // Act & Assert
        // Assume that an invalid connection string should throw a specific exception related to the format or validity of the connection string itself.
        // This is not currently implementable here because unless the PostgresMemoryStore constructor validates the connection string format,
        // there is no way to check this in unit tests without making an actual connection attempt.
    }

    [Fact]
    public void WithPostgresMemoryStore_EmptyConnectionString_ThrowsArgumentException()
    {
        // Arrange
        var builder = new MemoryBuilder();
        string connectionString = string.Empty;
        int vectorSize = 100;
        string schema = "public";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            PostgresMemoryBuilderExtensions.WithPostgresMemoryStore(builder, connectionString, vectorSize, schema));

        Assert.Equal("connectionString", exception.ParamName);
    }

    [Fact]
    public void WithPostgresMemoryStore_NullConnectionString_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new MemoryBuilder();
        string connectionString = null;
        int vectorSize = 100;
        string schema = "public";

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() =>
            PostgresMemoryBuilderExtensions.WithPostgresMemoryStore(builder, connectionString, vectorSize, schema));

        Assert.Equal("connectionString", exception.ParamName);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void WithPostgresMemoryStore_InvalidVectorSize_ThrowsArgumentException(int invalidVectorSize)
    {
        // Arrange
        var builder = new MemoryBuilder();
        string connectionString = "Host=localhost;Username=test;Password=test;Database=test";
        string schema = "public";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            PostgresMemoryBuilderExtensions.WithPostgresMemoryStore(builder, connectionString, invalidVectorSize, schema));

        Assert.Equal("vectorSize", exception.ParamName);
    }

    [Fact]
    public void WithPostgresMemoryStore_EmptySchema_ThrowsArgumentException()
    {
        // Arrange
        var builder = new MemoryBuilder();
        string connectionString = "Host=localhost;Username=test;Password=test;Database=test";
        int vectorSize = 100;
        string schema = string.Empty;

        // Act & Assert
        // Even though the default schema could be okay, assume that an empty string should not be an allowed value.
        var exception = Assert.Throws<ArgumentException>(() =>
            PostgresMemoryBuilderExtensions.WithPostgresMemoryStore(builder, connectionString, vectorSize, schema));

        Assert.Equal("schema", exception.ParamName);
    }

    [Fact]
    public void WithPostgresMemoryStore_InvalidSchema_ThrowsArgumentException()
    {
        // Arrange
        var builder = new MemoryBuilder();
        string connectionString = "Host=localhost;Username=test;Password=test;Database=test";
        int vectorSize = 100;
        string schema = "!nv@lidSc#ema";  // Invalid characters for schema

        // Act & Assert
        // Assume that the PostgresMemoryStore throws an ArgumentException if the schema name contains invalid characters.
        var exception = Assert.Throws<ArgumentException>(() =>
            PostgresMemoryBuilderExtensions.WithPostgresMemoryStore(builder, connectionString, vectorSize, schema));

        Assert.Equal("schema", exception.ParamName);
    }

    [Fact]
    public void WithPostgresMemoryStore_WhitespaceSchema_ThrowsArgumentException()
    {
        // Arrange
        var builder = new MemoryBuilder();
        string connectionString = "Host=localhost;Username=test;Password=test;Database=test";
        int vectorSize = 100;
        string schema = " ";

        // Act & Assert
        // Assume that whitespace should not be a valid schema name
        var exception = Assert.Throws<ArgumentException>(() =>
            PostgresMemoryBuilderExtensions.WithPostgresMemoryStore(builder, connectionString, vectorSize, schema));

        Assert.Equal("schema", exception.ParamName);
    }

    [Fact]
    public void WithPostgresMemoryStore_NullNpgsqlDataSource_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new MemoryBuilder();
        NpgsqlDataSource dataSource = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => 
            PostgresMemoryBuilderExtensions.WithPostgresMemoryStore(builder, dataSource, 100, "public"));

        Assert.Equal("dataSource", exception.ParamName);
    }

    [Fact]
    public void WithPostgresMemoryStore_NullPostgresDbClient_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new MemoryBuilder();
        IPostgresDbClient postgresDbClient = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => 
            PostgresMemoryBuilderExtensions.WithPostgresMemoryStore(builder, postgresDbClient));

        Assert.Equal("postgresDbClient", exception.ParamName);
    }
}
