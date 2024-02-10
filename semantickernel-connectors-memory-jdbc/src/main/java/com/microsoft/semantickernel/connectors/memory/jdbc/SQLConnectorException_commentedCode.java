/**
 * Exception class for handling SQL connection related errors.
 * This is a custom exception class that wraps SQL exceptions with specific error codes
 * and messages to provide more context about the error occurred during database operations.
 */
public class SQLConnectorException extends SKException {

    private final ErrorCodes errorCode; // Holds the specific error code for the exception

    /**
     * Constructs a new SQLConnectorException with the specified error code.
     *
     * @param errorCode The error code representing the type of SQL error encountered.
     */
    public SQLConnectorException(@Nonnull ErrorCodes errorCode) {
        this(errorCode, null, null);
    }

    /**
     * Constructs a new SQLConnectorException with the specified error code and descriptive message.
     *
     * @param errorCode The error code representing the type of SQL error encountered.
     * @param message   A message providing additional details about the SQL error.
     */
    public SQLConnectorException(@Nonnull ErrorCodes errorCode, @Nullable String message) {
        this(errorCode, message, null);
    }

    /**
     * Constructs a new SQLConnectorException with the specified error code,
     * descriptive message, and the original cause of the error.
     *
     * @param errorCode The error code representing the type of SQL error encountered.
     * @param message   A message providing additional details about the SQL error.
     * @param cause     The original exception that triggered this SQLConnectorException.
     */
    public SQLConnectorException(
            @Nonnull ErrorCodes errorCode, @Nullable String message, @Nullable Throwable cause) {
        super(message, cause);
        this.errorCode = errorCode;
    }

    /**
     * Retrieves the error code associated with this exception.
     *
     * @return The error code that categorizes the type of SQL error.
     */
    public ErrorCodes getErrorCode() {
        return errorCode;
    }

    /**
     * Enumeration of error codes used to indicate different types of SQL errors.
     */
    public enum ErrorCodes {
        /** Error code for a generic SQL error */
        SQL_ERROR("SQL error");

        private final String message; // A descriptive message for the error code.

        /**
         * Constructs a new ErrorCode enum constant with the specified message.
         *
         * @param message A message describing the type of SQL error represented by the constant.
         */
        ErrorCodes(String message) {
            this.message = message;
        }

        /**
         * Retrieves the message associated with the error code.
         *
         * @return A string containing a description of the SQL error.
         */
        public String getMessage() {
            return message;
        }
    }
}
