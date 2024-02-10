import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

public class SQLConnectorExceptionTest {

    @Test
    public void testConstructorWithErrorCodeOnly() {
        // Arrange
        SQLConnectorException.ErrorCodes expectedErrorCode = SQLConnectorException.ErrorCodes.SQL_ERROR;
        
        // Act
        SQLConnectorException exception = new SQLConnectorException(expectedErrorCode);
        
        // Assert
        assertNotNull(exception, "The exception should not be null");
        assertEquals(expectedErrorCode, exception.getErrorCode(), "The error code should match the one provided");
        assertNull(exception.getMessage(), "The message should be null when not provided");
        assertNull(exception.getCause(), "The cause should be null when not provided");
    }

    @Test
    public void testConstructorWithErrorCodeAndNullMessage() {
        // Arrange
        SQLConnectorException.ErrorCodes expectedErrorCode = SQLConnectorException.ErrorCodes.SQL_ERROR;
        
        // Act
        SQLConnectorException exception = new SQLConnectorException(expectedErrorCode, null);
        
        // Assert
        assertNotNull(exception, "The exception should not be null");
        assertEquals(expectedErrorCode, exception.getErrorCode(), "The error code should match the one provided");
        assertNull(exception.getMessage(), "The message should be null when null is provided");
        assertNull(exception.getCause(), "The cause should be null when not provided");
    }

    @Test
    public void testConstructorWithErrorCodeAndMessage() {
        // Arrange
        SQLConnectorException.ErrorCodes expectedErrorCode = SQLConnectorException.ErrorCodes.SQL_ERROR;
        String expectedMessage = "An SQL error occurred";
        
        // Act
        SQLConnectorException exception = new SQLConnectorException(expectedErrorCode, expectedMessage);
        
        // Assert
        assertNotNull(exception, "The exception should not be null");
        assertEquals(expectedErrorCode, exception.getErrorCode(), "The error code should match the one provided");
        assertEquals(expectedMessage, exception.getMessage(), "The message should match the one provided");
        assertNull(exception.getCause(), "The cause should be null when not provided");
    }
    
    @Test
    public void testConstructorWithErrorCodeMessageAndCause() {
        // Arrange
        SQLConnectorException.ErrorCodes expectedErrorCode = SQLConnectorException.ErrorCodes.SQL_ERROR;
        String expectedMessage = "An SQL error occurred";
        Throwable expectedCause = new RuntimeException("Underlying cause");
        
        // Act
        SQLConnectorException exception = new SQLConnectorException(expectedErrorCode, expectedMessage, expectedCause);
        
        // Assert
        assertNotNull(exception, "The exception should not be null");
        assertEquals(expectedErrorCode, exception.getErrorCode(), "The error code should match the one provided");
        assertEquals(expectedMessage, exception.getMessage(), "The message should match the one provided");
        assertEquals(expectedCause, exception.getCause(), "The cause should match the one provided");
    }

    @Test
    public void testConstructorWithErrorCodeAndCauseAndNullMessage() {
        // Arrange
        SQLConnectorException.ErrorCodes expectedErrorCode = SQLConnectorException.ErrorCodes.SQL_ERROR;
        Throwable expectedCause = new RuntimeException("Underlying cause");
        
        // Act
        SQLConnectorException exception = new SQLConnectorException(expectedErrorCode, null, expectedCause);
        
        // Assert
        assertNotNull(exception, "The exception should not be null");
        assertEquals(expectedErrorCode, exception.getErrorCode(), "The error code should match the one provided");
        assertNull(exception.getMessage(), "The message should be null when null is provided");
        assertEquals(expectedCause, exception.getCause(), "The cause should match the one provided");
    }

    @Test
    public void testErrorCodeCannotBeNull() {
        // Assert that passing a null errorCode to any constructor throws a NullPointerException
        assertThrows(NullPointerException.class, () -> {
            new SQLConnectorException(null);
        }, "Constructor should throw a NullPointerException when errorCode is null");
        
        assertThrows(NullPointerException.class, () -> {
            new SQLConnectorException(null, "Message");
        }, "Constructor should throw a NullPointerException when errorCode is null");
        
        assertThrows(NullPointerException.class, () -> {
            new SQLConnectorException(null, "Message", new Throwable());
        }, "Constructor should throw a NullPointerException when errorCode is null");
    }
}
