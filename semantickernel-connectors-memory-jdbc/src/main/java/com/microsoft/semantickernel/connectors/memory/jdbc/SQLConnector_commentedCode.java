// Copyright (c) Microsoft. All rights reserved.
package com.microsoft.semantickernel.connectors.memory.jdbc;

import java.time.ZonedDateTime;
import java.util.Collection;
import java.util.List;
import reactor.core.publisher.Mono;

/**
 * SQLConnector interface defines asynchronous methods for interacting with a SQL database.
 * It leverages Reactor's Mono type to return promises of operations that will be completed in the future.
 */
public interface SQLConnector {

    /**
     * Asynchronously creates a table in the database following a predefined schema if applicable.
     *
     * @return A Mono that completes when the table creation operation has finished.
     */
    Mono<Void> createTableAsync();

    /**
     * Asynchronously creates a new collection (such as a table or a document space) in the database.
     *
     * @param collectionName The name of the collection to be created.
     * @return A Mono that completes when the collection creation operation has finished.
     */
    Mono<Void> createCollectionAsync(String collectionName);

    /**
     * Performs an upsert operation asynchronously, which inserts a new entry or updates an existing entry based on the provided key.
     *
     * @param collection The name of the collection to perform the upsert into.
     * @param key        The unique identifier for the entry.
     * @param metadata   The associated metadata as a JSON string or structured data.
     * @param embedding  The associated embedding data or content.
     * @param timestamp  The ZonedDateTime object representing when the upsert operation occurs.
     * @return A Mono that emits the key of the upserted entry when the operation has completed.
     */
    Mono<String> upsertAsync(
            String collection,
            String key,
            String metadata,
            String embedding,
            ZonedDateTime timestamp);

    /**
     * Inserts or updates a batch of entries into the specified collection asynchronously.
     *
     * @param collection The name of the collection where the entries will be upserted.
     * @param records    A collection of DatabaseEntry objects representing the entries to upsert.
     * @return A Mono emitting a collection of keys for the upserted entries once the operation has completed.
     */
    Mono<Collection<String>> upsertBatchAsync(String collection, Collection<DatabaseEntry> records);

    /**
     * Asynchronously checks if a specified collection exists in the database.
     *
     * @param collectionName The name of the collection to check for existence.
     * @return A Mono emitting true if the collection exists, false otherwise.
     */
    Mono<Boolean> doesCollectionExistsAsync(String collectionName);

    /**
     * Retrieves a list of existing collection names in the database asynchronously.
     *
     * @return A Mono emitting a list of collection names available in the database.
     */
    Mono<List<String>> getCollectionsAsync();

    /**
     * Asynchronously reads all entries from a specified collection.
     *
     * @param collectionName The name of the collection from which to read entries.
     * @return A Mono emitting a list of DatabaseEntry objects representing all entries from the specified collection.
     */
    Mono<List<DatabaseEntry>> readAllAsync(String collectionName);

    /**
     * Asynchronously retrieves a single entry from a specified collection based on the provided key.
     *
     * @param collectionName The name of the collection to read from.
     * @param key            The unique identifier of the entry to retrieve.
     * @return A Mono emitting a DatabaseEntry representing the retrieved entry or null if not found.
     */
    Mono<DatabaseEntry> readAsync(String collectionName, String key);

    /**
     * Asynchronously reads a batch of entries from a specified collection identified by a collection of keys.
     *
     * @param collectionName The name of the collection to read from.
     * @param keys           A collection of keys identifying the entries to be retrieved.
     * @return A Mono emitting a collection of DatabaseEntry objects representing the retrieved entries.
     */
    Mono<Collection<DatabaseEntry>> readBatchAsync(String collectionName, Collection<String> keys);

    /**
     * Asynchronously deletes an entire collection from the database.
     *
     * @param collectionName The name of the collection to delete.
     * @return A Mono that completes when the collection deletion operation has finished.
     */
    Mono<Void> deleteCollectionAsync(String collectionName);

    /**
     * Deletes a specific entry from a collection based on the provided key.
     *
     * @param collectionName The name of the collection from which the entry will be deleted.
     * @param key            The unique identifier for the entry to delete.
     * @return A Mono that completes when the entry deletion operation has finished.
     */
    Mono<Void> deleteAsync(String collectionName, String key);

    /**
     * Deletes a batch of entries from a specified collection identified by a collection of keys.
     *
     * @param collectionName The name of the collection from which the entries will be deleted.
     * @param keys           A collection of keys identifying the entries to delete.
     * @return A Mono that completes when the batch entry deletion operation has finished.
     */
    Mono<Void> deleteBatchAsync(String collectionName, Collection<String> keys);

    /**
     * Deletes all entries from a collection that are considered empty based on a predefined criteria.
     *
     * @param collectionName The name of the collection from which empty entries will be deleted.
     * @return A Mono that completes when the empty entry deletion operation has finished.
     */
    Mono<Void> deleteEmptyAsync(String collectionName);
}
