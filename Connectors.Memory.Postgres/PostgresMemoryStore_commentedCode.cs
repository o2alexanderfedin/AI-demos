// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.SemanticKernel.Memory;
using Npgsql;
using Pgvector;

namespace Microsoft.SemanticKernel.Connectors.Postgres;

/// <summary>
/// Implements <see cref="IMemoryStore"/> to provide a vector-based memory store using a PostgreSQL database with the pgvector extension.
/// This allows for efficient similarity search and vector operations on large datasets.
/// </summary>
public class PostgresMemoryStore : IMemoryStore, IDisposable
{
    // Default database schema to be used if none is specified during initialization.
    internal const string DefaultSchema = "public";

    // Data source providing connections to the PostgreSQL database.
    private readonly NpgsqlDataSource? _dataSource;

    // Database client responsible for performing operations such as creating tables, upserting, and querying data.
    private readonly IPostgresDbClient _postgresDbClient;

    /// <summary>
    /// Creates a new instance of the <see cref="PostgresMemoryStore"/> which connects to a PostgreSQL database.
    /// </summary>
    /// <param name="connectionString">The connection string to the PostgreSQL database.</param>
    /// <param name="vectorSize">The size of the vectors stored in the memory store, used for similarity comparisons.</param>
    /// <param name="schema">The schema within the PostgreSQL database in which the tables will be created.</param>
    public PostgresMemoryStore(string connectionString, int vectorSize, string schema = DefaultSchema)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.UseVector();
        _dataSource = dataSourceBuilder.Build();
        _postgresDbClient = new PostgresDbClient(_dataSource, schema, vectorSize);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PostgresMemoryStore"/> with a specified Postgres data source.
    /// </summary>
    /// <param name="dataSource">The data source for the PostgreSQL database.</param>
    /// <param name="vectorSize">The size of the vectors to be handled.</param>
    /// <param name="schema">The database schema to be used.</param>
    public PostgresMemoryStore(NpgsqlDataSource dataSource, int vectorSize, string schema = DefaultSchema)
        : this(new PostgresDbClient(dataSource, schema, vectorSize))
    {
    }

    /// <summary>
    /// Constructs a PostgresMemoryStore with a specific Postgres database client, offering flexibility in its instantiation.
    /// </summary>
    /// <param name="postgresDbClient">The database client to interact with the PostgreSQL database.</param>
    public PostgresMemoryStore(IPostgresDbClient postgresDbClient)
    {
        _postgresDbClient = postgresDbClient;
    }

    /// <inheritdoc/>
    public async Task CreateCollectionAsync(string collectionName, CancellationToken cancellationToken = default)
    {
        Verify.NotNullOrWhiteSpace(collectionName);
        await _postgresDbClient.CreateTableAsync(collectionName, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> DoesCollectionExistAsync(string collectionName, CancellationToken cancellationToken = default)
    {
        Verify.NotNullOrWhiteSpace(collectionName);
        return await _postgresDbClient.DoesTableExistsAsync(collectionName, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<string> GetCollectionsAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var collection in _postgresDbClient.GetTablesAsync(cancellationToken).ConfigureAwait(false))
        {
            yield return collection;
        }
    }

    /// <inheritdoc/>
    public async Task DeleteCollectionAsync(string collectionName, CancellationToken cancellationToken = default)
    {
        Verify.NotNullOrWhiteSpace(collectionName);
        await _postgresDbClient.DeleteTableAsync(collectionName, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<string> UpsertAsync(string collectionName, MemoryRecord record, CancellationToken cancellationToken = default)
    {
        Verify.NotNullOrWhiteSpace(collectionName);
        return await InternalUpsertAsync(collectionName, record, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<string> UpsertBatchAsync(string collectionName, IEnumerable<MemoryRecord> records, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        Verify.NotNullOrWhiteSpace(collectionName);
        foreach (var record in records)
        {
            yield return await InternalUpsertAsync(collectionName, record, cancellationToken).ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public async Task<MemoryRecord?> GetAsync(string collectionName, string key, bool withEmbedding = false, CancellationToken cancellationToken = default)
    {
        Verify.NotNullOrWhiteSpace(collectionName);
        var entry = await _postgresDbClient.ReadAsync(collectionName, key, withEmbedding, cancellationToken).ConfigureAwait(false);
        return entry.HasValue ? GetMemoryRecordFromEntry(entry.Value) : null;
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<MemoryRecord> GetBatchAsync(string collectionName, IEnumerable<string> keys, bool withEmbeddings = false, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        Verify.NotNullOrWhiteSpace(collectionName);
        await foreach (var entry in _postgresDbClient.ReadBatchAsync(collectionName, keys, withEmbeddings, cancellationToken).ConfigureAwait(false))
        {
            yield return GetMemoryRecordFromEntry(entry);
        }
    }

    /// <inheritdoc/>
    public async Task RemoveAsync(string collectionName, string key, CancellationToken cancellationToken = default)
    {
        Verify.NotNullOrWhiteSpace(collectionName);
        await _postgresDbClient.DeleteAsync(collectionName, key, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task RemoveBatchAsync(string collectionName, IEnumerable<string> keys, CancellationToken cancellationToken = default)
    {
        Verify.NotNullOrWhiteSpace(collectionName);
        await _postgresDbClient.DeleteBatchAsync(collectionName, keys, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<(MemoryRecord, double)> GetNearestMatchesAsync(
        string collectionName,
        ReadOnlyMemory<float> embedding,
        int limit,
        double minRelevanceScore = 0,
        bool withEmbeddings = false,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        Verify.NotNullOrWhiteSpace(collectionName);
        if (limit <= 0) yield break;
        
        var results = _postgresDbClient.GetNearestMatchesAsync(
            tableName: collectionName,
            embedding: new Vector(GetOrCreateArray(embedding)),
            limit: limit,
            minRelevanceScore: minRelevanceScore,
            withEmbeddings: withEmbeddings,
            cancellationToken: cancellationToken);
        
        await foreach (var (entry, cosineSimilarity) in results.ConfigureAwait(false))
        {
            yield return (GetMemoryRecordFromEntry(entry), cosineSimilarity);
        }
    }

    /// <inheritdoc/>
    public async Task<(MemoryRecord, double)?> GetNearestMatchAsync(string collectionName, ReadOnlyMemory<float> embedding, double minRelevanceScore = 0, bool withEmbedding = false,
        CancellationToken cancellationToken = default)
    {
        return await GetNearestMatchesAsync(
            collectionName: collectionName,
            embedding: embedding,
            limit: 1,
            minRelevanceScore: minRelevanceScore,
            withEmbeddings: withEmbedding,
            cancellationToken: cancellationToken).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
    }

    // Ensures the disposal of the object and its resources, like a database connection pool.
    public void Dispose()
    {
        Dispose(true);
        // Avoid finalization overhead since we're manually disposing of resources.
        GC.SuppressFinalize(this);
    }

    // A helper method for properly disposing managed objects like NpgsqlDataSource.
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            // Casts DataSource to IDisposable and disposes it if it's not null.
            (_dataSource as IDisposable)?.Dispose();
        }
    }

    // Inserts or updates a memory record in the given collection.
    private async Task<string> InternalUpsertAsync(string collectionName, MemoryRecord record, CancellationToken cancellationToken)
    {
        record.Key = record.Metadata.Id;
        await _postgresDbClient.UpsertAsync(
            tableName: collectionName,
            key: record.Key,
            metadata: record.GetSerializedMetadata(),
            embedding: new Vector(GetOrCreateArray(record.Embedding)),
            timestamp: record.Timestamp?.UtcDateTime,
            cancellationToken: cancellationToken).ConfigureAwait(false);
        return record.Key;
    }

    // Converts an internal representation of a memory entry to a memory record.
    private MemoryRecord GetMemoryRecordFromEntry(PostgresMemoryEntry entry)
    {
        return MemoryRecord.FromJsonMetadata(
            json: entry.MetadataString,
            embedding: entry.Embedding?.ToArray() ?? ReadOnlyMemory<float>.Empty,
            key: entry.Key,
            timestamp: entry.Timestamp?.ToLocalTime());
    }

    // Converts a ReadOnlyMemory instance to a corresponding array, ensures optimal memory usage.
    private static float[] GetOrCreateArray(ReadOnlyMemory<float> memory)
    {
        return MemoryMarshal.TryGetArray(memory, out ArraySegment<float> array) && array.Count == array.Array!.Length ? array.Array : memory.ToArray();
    }
}
