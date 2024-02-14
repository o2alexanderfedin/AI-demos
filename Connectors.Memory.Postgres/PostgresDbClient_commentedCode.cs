// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using Pgvector;

namespace Microsoft.SemanticKernel.Connectors.Postgres;

/// <summary>
/// An implementation of a client for Postgres. This class is used for managing postgres database operations.
/// Specifically, it provides an interface for creating, reading, updating, and deleting data in tables that store
/// embeddings and metadata. It handles table existence checks, CRUD operations, and supports vector operations for 
/// retrieving nearest matches, which is particularly useful for applications dealing with embeddings in machine learning.
/// </summary>
public class PostgresDbClient : IPostgresDbClient
{
    // PostgreSQL data source providing the connection to the database.
    private readonly NpgsqlDataSource _dataSource;
    // The database schema in which the tables are defined.
    private readonly string _schema;
    // The dimension size of the vector used for embeddings.
    private readonly int _vectorSize;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostgresDbClient"/> class,
    /// setting the required details for interfacing with the PostgreSQL database.
    /// </summary>
    /// <param name="dataSource">Provides the necessary APIs to connect to the Postgres database.</param>
    /// <param name="schema">The schema of the database where tables will be created or queried.</param>
    /// <param name="vectorSize">Size of the embedding vectors used in the database tables.</param>
    public PostgresDbClient(NpgsqlDataSource dataSource, string schema, int vectorSize)
    {
        this._dataSource = dataSource;
        this._schema = schema;
        this._vectorSize = vectorSize;
    }

    /// <summary>
    /// Asynchronously checks if a specific table exists in the schema of the Postgres database.
    /// </summary>
    /// <param name="tableName">Name of the table to check existence for.</param>
    /// <param name="cancellationToken">Token to signal cancellation.</param>
    /// <returns>A task that represents the asynchronous operation; the task result contains a boolean indicating whether the table exists.</returns>
    public async Task<bool> DoesTableExistsAsync(string tableName, CancellationToken cancellationToken = default)
    {
        // Obtain an open connection to the database with consideration for cancellation.
        using NpgsqlConnection connection = await this._dataSource.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        using NpgsqlCommand cmd = connection.CreateCommand();
        // SQL command to check the existence of the table within the specified schema.
        cmd.CommandText = $@"
            SELECT EXISTS (
                SELECT FROM information_schema.tables 
                WHERE table_schema = @schema AND table_name = @tableName)";
        // Adding parameters to the command to prevent SQL injection attacks.
        cmd.Parameters.AddWithValue("@schema", this._schema);
        cmd.Parameters.AddWithValue("@tableName", tableName);

        // Executing the command and converting the returned object to a boolean indicating table existence.
        return (bool)await cmd.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously creates a table within the schema if it does not already exist.
    /// </summary>
    /// <param name="tableName">Name of the table to be created.</param>
    /// <param name="cancellationToken">Token to signal cancellation.</param>
    /// <returns>A task that represents the asynchronous operation of creating the table.</returns>
    public async Task CreateTableAsync(string tableName, CancellationToken cancellationToken = default)
    {
        using NpgsqlConnection connection = await this._dataSource.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        using NpgsqlCommand cmd = connection.CreateCommand();
        // SQL command to create a new table with a specified structure if it doesn't exist.
        cmd.CommandText = $@"
            CREATE TABLE IF NOT EXISTS {this.GetFullTableName(tableName)} (
                key TEXT NOT NULL,
                metadata JSONB,
                embedding vector({this._vectorSize}),
                timestamp TIMESTAMP WITH TIME ZONE,
                PRIMARY KEY (key))";
        // Executes the create table command.
        await cmd.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously retrieves an enumerable collection of table names present in the schema.
    /// </summary>
    /// <param name="cancellationToken">Token to signal cancellation.</param>
    /// <returns>An asynchronous stream of table names.</returns>
    public async IAsyncEnumerable<string> GetTablesAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using NpgsqlConnection connection = await this._dataSource.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        using NpgsqlCommand cmd = connection.CreateCommand();
        // SQL command to select table names from the information schema where the table type is a 'BASE TABLE'.
        cmd.CommandText = @"
            SELECT table_name
            FROM information_schema.tables
            WHERE table_schema = @schema
                AND table_type = 'BASE TABLE'";
        // Adding the schema parameter.
        cmd.Parameters.AddWithValue("@schema", this._schema);

        using NpgsqlDataReader dataReader = await cmd.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        // Iterates through the results yielding one table name at a time.
        while (await dataReader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            yield return dataReader.GetString(dataReader.GetOrdinal("table_name"));
        }
    }

    /// <summary>
    /// Asynchronously drops a table from the schema, if it exists.
    /// </summary>
    /// <param name="tableName">The name of the table to be deleted.</param>
    /// <param name="cancellationToken">Token to signal cancellation.</param>
    /// <returns>A task that represents the asynchronous operation of dropping the table.</returns>
    public async Task DeleteTableAsync(string tableName, CancellationToken cancellationToken = default)
    {
        using NpgsqlConnection connection = await this._dataSource.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        using NpgsqlCommand cmd = connection.CreateCommand();
        // SQL command to drop the table if it exists.
        cmd.CommandText = $"DROP TABLE IF EXISTS {this.GetFullTableName(tableName)}";
        // Executes the drop table command.
        await cmd.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    // Additional method implementations are omitted for brevity.
    // These include methods to insert, retrieve, update, and delete
    // entries in the tables, as well as retrieve entries based on
    // vector similarity.

    /// <summary>
    /// Constructs the full table name using the schema and collection/table name.
    /// </summary>
    /// <param name="tableName">The name of the table.</param>
    /// <returns>The full table name prefixed with the schema and enclosed in quotes.</returns>
    private string GetFullTableName(string tableName)
    {
        // It is critical for security that tableName is a safe, non-user-controlled string to avoid SQL injection.
        return $"{this._schema}.\"{tableName}\"";
    }

    // The remaining helper methods and private implementation details are also omitted.
    // These include methods for reading data from NpgsqlDataReader objects, handling the
    // creation of NpgsqlCommand objects, adding parameters, and other operational details.
    // Such details are often better represented by clear code and private encapsulation rather
    // than extensive commenting, as the consumers of this class do not interact with these
    // private aspects directly.
}
