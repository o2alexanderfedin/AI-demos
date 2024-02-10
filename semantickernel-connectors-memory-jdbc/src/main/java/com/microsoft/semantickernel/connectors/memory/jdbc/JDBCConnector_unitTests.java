import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.AfterEach;
import java.sql.Connection;
import java.sql.SQLException;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

/**
 * Unit tests for the JDBCConnector class constructor feature.
 */
class JDBCConnectorTest {

    private Connection mockConnection;
    private JDBCConnector jdbcConnector;

    @BeforeEach
    void setUp() {
        // Arrange
        mockConnection = mock(Connection.class);
    }

    @AfterEach
    void tearDown() throws SQLException {
        if (jdbcConnector != null) {
            jdbcConnector.close();
        }
    }

    @Test
    void constructor_WhenGivenValidConnection_ShouldInitializeJDBCConnector() {
        // Arrange (already done in setUp())

        // Act
        jdbcConnector = new JDBCConnector(mockConnection);

        // Assert
        assertNotNull(jdbcConnector, "JDBCConnector should be instantiated.");
        assertNotNull(jdbcConnector.connection, "JDBCConnector's connection should be set.");
        assertSame(mockConnection, jdbcConnector.connection, "The connection stored in JDBCConnector should be the one that was passed to the constructor.");
    }

    @Test
    void constructor_WhenGivenNullConnection_ShouldThrowNullPointerException() {
        // Arrange - nothing additional

        // Act & Assert
        assertThrows(NullPointerException.class, () -> {
            new JDBCConnector(null);
        }, "Constructing JDBCConnector with a null connection should throw NullPointerException.");
    }

    @Test
    void close_ShouldCloseDatabaseConnection_WhenCalled() throws SQLException {
        // Arrange
        jdbcConnector = new JDBCConnector(mockConnection);

        // Act
        jdbcConnector.close();

        // Assert
        verify(mockConnection).close();
    }

    @Test
    void close_ShouldThrowSQLConnectorException_WhenSQLExceptionOccurs() throws SQLException {
        // Arrange
        jdbcConnector = new JDBCConnector(mockConnection);
        doThrow(new SQLException("Simulated SQL error")).when(mockConnection).close();

        // Act & Assert
        assertThrows(SQLConnectorException.class, () -> jdbcConnector.close(), "Closing the JDBCConnector should throw an SQLConnectorException when an SQLException occurs.");
    }
}
