// Copyright (c) Microsoft. All rights reserved.

using Microsoft.SemanticKernel.Memory;
using Npgsql;

namespace Microsoft.SemanticKernel.Connectors.Postgres;

/// <summary>
/// Provides extension methods to augment <see cref="MemoryBuilder"/> with Postgres database connectivity,
/// enabling the storage and retrieval of semantic data in a Postgres database.
/// </summary>
public static class PostgresMemoryBuilderExtensions
{
    /// <summary>
    /// Adds a Postgres database as a memory store to the memory builder, allowing for
    /// the persistence and retrieval of semantic vectors within a Postgres database.
    /// </summary>
    /// <param name="builder">The memory builder to which the Postgres memory store will be added.</param>
    /// <param name="connectionString">The connection string to the Postgres database.</param>
    /// <param name="vectorSize">The size of the semantic vectors to be stored in the database.</param>
    /// <param name="schema">The database schema that contains the collection tables. Uses a default schema if not specified.</param>
    /// <returns>The memory builder with the Postgres memory store included for chainable configuration.</returns>
    public static MemoryBuilder WithPostgresMemoryStore(
        this MemoryBuilder builder,
        string connectionString,
        int vectorSize,
        string schema = PostgresMemoryStore.DefaultSchema)
    {
        builder.WithMemoryStore((_) =>
        {
            return new PostgresMemoryStore(connectionString, vectorSize, schema);
        });

        return builder;
    }

    /// <summary>
    /// Adds a Postgres data source as a memory store to the memory builder, supporting
    /// the persistence and retrieval of semantic vectors using an existing Npgsql data source.
    /// </summary>
    /// <param name="builder">The memory builder to which the Postgres memory store will be added.</param>
    /// <param name="dataSource">The Npgsql data source representing a connection to the Postgres database.</param>
    /// <param name="vectorSize">The size of the semantic vectors to be stored with the data source.</param>
    /// <param name="schema">The database schema that contains the collection tables. Uses a default schema if not specified.</param>
    /// <returns>The memory builder with the Postgres data source based memory store included for chainable configuration.</returns>
    public static MemoryBuilder WithPostgresMemoryStore(
        this MemoryBuilder builder,
        NpgsqlDataSource dataSource,
        int vectorSize,
        string schema = PostgresMemoryStore.DefaultSchema)
    {
        builder.WithMemoryStore((_) =>
        {
            return new PostgresMemoryStore(dataSource, vectorSize, schema);
        });

        return builder;
    }

    /// <summary>
    /// Integrates an existing Postgres database client as a memory store into the memory builder,
    /// facilitating the management of semantic vectors through a custom Postgres client interface.
    /// </summary>
    /// <param name="builder">The memory builder to which the Postgres memory store will be added.</param>
    /// <param name="postgresDbClient">An interface to a Postgres database client that handles the interactions with the database.</param>
    /// <returns>The memory builder with the custom Postgres database client based memory store included for chainable configuration.</returns>
    public static MemoryBuilder WithPostgresMemoryStore(
        this MemoryBuilder builder,
        IPostgresDbClient postgresDbClient)
    {
        builder.WithMemoryStore((_) =>
        {
            return new PostgresMemoryStore(postgresDbClient);
        });

        return builder;
    }
}
