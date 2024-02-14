// Copyright (c) Microsoft. All rights reserved.

using System;
using Pgvector;

namespace Microsoft.SemanticKernel.Connectors.Postgres;

/// <summary>
/// Represents a single unit of memory storage in a Postgres database used for storing embeddings and associated metadata.
/// This structure allows for efficient memory retrieval and operations on vectorized data within a semantic analysis system.
/// </summary>
public record struct PostgresMemoryEntry
{
    /// <summary>
    /// Gets or sets the unique identifier for the memory entry used to retrieve and associate the stored data.
    /// </summary>
    /// <value>The key as a string.</value>
    public string Key { get; set; }

    /// <summary>
    /// Gets or sets a string representing additional data related to the entry, such as JSON-formatted information
    /// that provides context for the embeddings.
    /// </summary>
    /// <value>The metadata string associated with the memory entry.</value>
    public string MetadataString { get; set; }

    /// <summary>
    /// Gets or sets the vector representation of the associated data used for machine learning applications and semantic
    /// analysis. The embedding can be used for similarity searches and other vector-related database operations.
    /// It is nullable to accommodate entries that may not have associated embeddings.
    /// </summary>
    /// <value>The embedding as a <see cref="Vector"/>, or null if not set.</value>
    public Vector? Embedding { get; set; }

    /// <summary>
    /// Gets or sets an optional timestamp indicating when the memory entry was created or last updated.
    /// This timestamp is always in UTC to guarantee consistency across different time zones and systems.
    /// </summary>
    /// <value>The UTC timestamp for the memory entry, or null if not set.</value>
    /// <remarks>
    /// Storing timestamps in UTC is a common practice to avoid issues with local time zones and daylight saving time changes.
    /// </remarks>
    public DateTime? Timestamp { get; set; }
}
