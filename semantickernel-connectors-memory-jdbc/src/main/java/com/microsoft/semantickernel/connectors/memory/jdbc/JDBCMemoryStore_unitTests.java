package com.microsoft.semantickernel.connectors.memory.jdbc;

import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;
import reactor.core.publisher.Mono;
import reactor.test.StepVerifier;

import static org.mockito.Mockito.*;

// Addressed review feedback for the unit tests
@ExtendWith(MockitoExtension.class)
public class JDBCMemoryStoreTests {

    @Mock
    private SQLConnector mockConnector;

    @Test
    @DisplayName("createCollectionAsync with non-null collection name should invoke DBConnector")
    public void createCollectionAsync_WithNonNullCollectionName_ShouldInvokeDBConnector() {
        // Arrange
        String collectionName = "testCollection";
        JDBCMemoryStore memoryStore = new JDBCMemoryStore(mockConnector);
        when(mockConnector.createCollectionAsync(collectionName)).thenReturn(Mono.empty());

        // Act
        Mono<Void> result = memoryStore.createCollectionAsync(collectionName);

        // Assert
        StepVerifier.create(result)
                .verifyComplete();
        
        // Verification step to ensure that createCollectionAsync method of the mocked SQLConnector is called
        verify(mockConnector).createCollectionAsync(collectionName);
    }

    @Test
    @DisplayName("createCollectionAsync with null collection name should throw NullPointerException")
    public void createCollectionAsync_WithNullCollectionName_ShouldThrowNullPointerException() {
        // Arrange
        JDBCMemoryStore memoryStore = new JDBCMemoryStore(mockConnector);

        // Act & Assert
        StepVerifier.create(memoryStore.createCollectionAsync(null))
                .verifyError(NullPointerException.class);
    }

    @Test
    @DisplayName("createCollectionAsync with empty collection name should throw IllegalArgumentException")
    public void createCollectionAsync_WithEmptyCollectionName_ShouldThrowIllegalArgumentException() {
        // Arrange
        JDBCMemoryStore memoryStore = new JDBCMemoryStore(mockConnector);
        String collectionName = "";

        // Act & Assert
        StepVerifier.create(memoryStore.createCollectionAsync(collectionName))
                .verifyError(IllegalArgumentException.class); // Assuming empty collection name is not allowed
    }
    
    // Add these tests if we assume that a non-empty collection name must be provided
    @Test
    @DisplayName("createCollectionAsync with whitespace-only collection name should throw IllegalArgumentException")
    public void createCollectionAsync_WithWhitespaceOnlyCollectionName_ShouldThrowIllegalArgumentException() {
        // Arrange
        JDBCMemoryStore memoryStore = new JDBCMemoryStore(mockConnector);
        String collectionName = "   ";

        // Act & Assert
        StepVerifier.create(memoryStore.createCollectionAsync(collectionName))
                .verifyError(IllegalArgumentException.class); // Assuming whitespace-only names are considered invalid
    }
    
    // Additional test to handle creating an existing collection
    @Test
    @DisplayName("createCollectionAsync with existing collection name should complete without error")
    public void createCollectionAsync_WithExistingCollectionName_ShouldCompleteWithoutError() {
        // Arrange
        String collectionName = "existingCollection";
        JDBCMemoryStore memoryStore = new JDBCMemoryStore(mockConnector);
        when(mockConnector.createCollectionAsync(collectionName)).thenReturn(Mono.empty());
        when(mockConnector.doesCollectionExistsAsync(collectionName)).thenReturn(Mono.just(true));

        // Act
        Mono<Void> result = memoryStore.createCollectionAsync(collectionName);

        // Assert
        StepVerifier.create(result)
                .verifyComplete();
        
        // Verify that interaction with mockConnector for checking existence and creating the collection
        verify(mockConnector).doesCollectionExistsAsync(collectionName); // Mocking the existence check
        verify(mockConnector).createCollectionAsync(collectionName); // Assuming it's okay to create an existing collection
    }
}
