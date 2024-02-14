// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Pgvector;

namespace Microsoft.SemanticKernel.Connectors.Postgres;

/// <summary>
/// Interface defining operations for managing PostgreSQL database tables and entries.
/// </summary>
public interface IPostgresDbClient
{
    /// <summary>
    /// Asynchronously determines whether a table with the specified name exists in the database.
    /// </summary>
    /// <param name="tableName">The name of the table to check.</param>
    /// <param name="cancellationToken">Token for cancelling the request.</param>
    /// <returns>A task that represents the asynchronous operation and contains the result:
    /// true if the table exists; otherwise, false.</returns>
    Task<bool> DoesTableExistsAsync(string tableName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously creates a new table with the specified name in the database.
    /// </summary>
    /// <param name="tableName">The name for the new table.</param>
    /// <param name="cancellationToken">Token for cancelling the request.</param>
    /// <returns>A task that represents the asynchronous operation of creating the table.</returns>
    Task CreateTableAsync(string tableName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a list of all tables in the database.
    /// </summary>
    /// <param name="cancellationToken">Token for cancelling the request.</param>
    /// <returns>An asynchronous stream of table names present in the database.</returns>
    IAsyncEnumerable<string> GetTablesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously deletes the table with the specified name from the database.
    /// </summary>
    /// <param name="tableName">The name of the table to delete.</param>
    /// <param name="cancellationToken">Token for cancelling the request.</param>
    /// <returns>A task that represents the asynchronous operation of deleting the table.</returns>
    Task DeleteTableAsync(string tableName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously upserts an entry into a table, creating or updating the entry with the provided key.
    /// </summary>
    /// <param name="tableName">The name of the table where the entry will be upserted.</param>
    /// <param name="key">The key of the entry to upsert.</param>
    /// <param name="metadata">The metadata associated with the entry.</param>
    /// <param name="embedding">The vector embedding of the entry.</param>
    /// <param name="timestamp">The timestamp for the entry, which must be in UTC.</param>
    /// <param name="cancellationToken">Token for cancelling the request.</param>
    /// <returns>A task that represents the asynchronous operation of upserting the entry.</returns>
    Task UpsertAsync(string tableName, string key, string? metadata, Vector? embedding, DateTime? timestamp, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves the nearest matches to a given vector embedding from a specified table.
    /// </summary>
    /// <param name="tableName">The name of the table to query for matches.</param>
    /// <param name="embedding">The vector embedding to compare against table's entries.</param>
    /// <param name="limit">The maximum number of matching results to retrieve.</param>
    /// <param name="minRelevanceScore">The minimum score for a match to be considered relevant.</param>
    /// <param name="withEmbeddings">Whether or not to include the embedding in the returned entries.</param>
    /// <param name="cancellationToken">Token for cancelling the request.</param>
    /// <returns>An asynchronous stream of tuples, each containing a matching entry and its relevance score.</returns>
    IAsyncEnumerable<(PostgresMemoryEntry, double)> GetNearestMatchesAsync(string tableName, Vector embedding, int limit, double minRelevanceScore = 0, bool withEmbeddings = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously reads an entry from a table by its key.
    /// </summary>
    /// <param name="tableName">The name of the table where the entry is located.</param>
    /// <param name="key">The key of the entry to retrieve.</param>
    /// <param name="withEmbeddings">Whether or not to include the embedding in the returned entry.</param>
    /// <param name="cancellationToken">Token for cancelling the request.</param>
    /// <returns>A task that represents the asynchronous operation, resulting in the retrieved entry or null if the key is not found.</returns>
    Task<PostgresMemoryEntry?> ReadAsync(string tableName, string key, bool withEmbeddings = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously reads multiple entries from a table by their keys.
    /// </summary>
    /// <param name="tableName">The name of the table where the entries are located.</param>
    /// <param name="keys">The keys of the entries to retrieve.</param>
    /// <param name="withEmbeddings">Whether or not to include the embeddings in the returned entries.</param>
    /// <param name="cancellationToken">Token for cancelling the request.</param>
    /// <returns>An asynchronous stream of entries, each corresponding to one of the provided keys.</returns>
    IAsyncEnumerable<PostgresMemoryEntry> ReadBatchAsync(string tableName, IEnumerable<string> keys, bool withEmbeddings = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously deletes an entry from a table by its key.
    /// </summary>
    /// <param name="tableName">The name of the table from which the entry will be deleted.</param>
    /// <param name="key">The key of the entry to delete.</param>
    /// <param name="cancellationToken">Token for cancelling the request.</param>
    /// <returns>A task that represents the asynchronous operation of deleting the entry.</returns>
    Task DeleteAsync(string tableName, string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously deletes multiple entries from a table by their keys.
    /// </summary>
    /// <param name="tableName">The name of the table from which the entries will be deleted.</param>
    /// <param name="keys">The keys of the entries to delete.</param>
    /// <param name="cancellationToken">Token for cancelling the request.</param>
    /// <returns>A task that represents the asynchronous operation of deleting multiple entries.</returns>
    Task DeleteBatchAsync(string tableName, IEnumerable<string> keys, CancellationToken cancellationToken = default);
}
