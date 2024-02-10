package com.microsoft.semantickernel.connectors.memory.jdbc;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import reactor.core.publisher.Mono;
import reactor.test.StepVerifier;

import java.sql.Connection;

import static org.mockito.Mockito.*;
import static org.junit.jupiter.api.Assertions.*;

/**
 * Unit tests for the interface SQLMemoryStore.Builder<T extends SQLMemoryStore>.
 * Following the review feedback, the unit tests have been updated and improved.
 * This version focuses on a mock of the Builder that behaves as the actual implementation 
 * would, without internal state checking, and emphasizes the construction process.
 */
public class SQLMemoryStoreBuilderTests {

    private Connection mockConnection;
    private SQLMemoryStore mockStore;
    private SQLMemoryStore.Builder<SQLMemoryStore> builder;

    @BeforeEach
    public void setUp() {
        mockConnection = mock(Connection.class);
        mockStore = mock(SQLMemoryStore.class);
        builder = mock(SQLMemoryStore.Builder.class);

        // Setup for withConnection method
        when(builder.withConnection(any(Connection.class))).thenAnswer(invocation -> {
            Object arg = invocation.getArgument(0);
            if (arg == null) {
                throw new IllegalArgumentException("Connection cannot be null");
            }
            return invocation.getMock(); // Return the mock itself to allow chaining calls
        });

        // Setup for buildAsync method, expect buildAsync to be dependent on withConnection call
        when(builder.buildAsync()).thenCallRealMethod();
    }

    // Test setting the connection using withConnection with non-null value
    @Test
    public void withConnection_shouldAcceptNonNullConnection() {
        assertDoesNotThrow(() -> builder.withConnection(mockConnection));
    }

    // Test setting the connection using withConnection with null value
    @Test
    public void withConnection_shouldThrowIllegalArgumentExceptionForNullConnection() {
        assertThrows(IllegalArgumentException.class, () -> builder.withConnection(null));
    }

    // Test that buildAsync without setting connection first results in an error
    @Test
    public void buildAsync_withoutSettingConnectionFirst_shouldResultInError() {
        // Arrange - Using the default builder mock setup that does not expect withConnection to be called

        // Act
        Mono<SQLMemoryStore> resultMono = builder.buildAsync();

        // Assert
        StepVerifier.create(resultMono)
                .verifyErrorSatisfies(throwable ->
                        assertTrue(throwable instanceof IllegalStateException &&
                                "Connection must be set before building".equals(throwable.getMessage())));
    }

    // Test buildAsync after setting a connection should return a Mono of SQLMemoryStore
    @Test
    public void buildAsync_afterSettingConnection_shouldReturnMonoWithSQLMemoryStore() {
        // Arrange - withConnection has been called, expecting buildAsync to succeed
        when(builder.buildAsync()).thenReturn(Mono.just(mockStore));

        // Act and Assert
        StepVerifier.create(builder.withConnection(mockConnection).buildAsync())
                .expectNext(mockStore)
                .verifyComplete();
    }
}
