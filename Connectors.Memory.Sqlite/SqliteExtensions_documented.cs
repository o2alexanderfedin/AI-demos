using Microsoft.Data.Sqlite;

namespace Microsoft.SemanticKernel.Connectors.Sqlite;

/// <summary>
/// Provides extension methods for the SqliteDataReader to simplify data access by using field names instead of ordinals.
/// </summary>
internal static class SqliteExtensions
{
    /// <summary>
    /// Retrieves the value of a specified field by name and returns it as the requested type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object to be returned.</typeparam>
    /// <param name="reader">The SqliteDataReader instance from which the field value is to be retrieved.</param>
    /// <param name="fieldName">The name of the field to retrieve the value from.</param>
    /// <returns>The value of the specified field converted to the required type <typeparamref name="T"/>.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown if the field name does not match any column in the result set.</exception>
    /// <exception cref="InvalidCastException">Thrown if the value of the field cannot be cast to the specified type <typeparamref name="T"/>.</exception>
    /// <remarks>
    /// Before calling this method, the <see cref="SqliteDataReader.Read"/> method should be called to advance the reader
    /// to the next record. It must return true, indicating that data is available for retrieval.
    /// </remarks>
    public static T GetFieldValue<T>(this SqliteDataReader reader, string fieldName)
    {
        // Use the field name to obtain the field's ordinal position
        // and then retrieves the value as the specified type.
        int ordinal = reader.GetOrdinal(fieldName);
        return reader.GetFieldValue<T>(ordinal);
    }

    /// <summary>
    /// Retrieves the string value of a specified field by name from the data reader.
    /// </summary>
    /// <param name="reader">The SqliteDataReader instance from which the field value is to be retrieved.</param>
    /// <param name="fieldName">The name of the field to retrieve the string value from.</param>
    /// <returns>The string representation of the specified field's value.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown if the field name is not found in the current result set.</exception>
    /// <exception cref="InvalidCastException">Thrown if the value of the field is not a string or cannot be cast to a string.</exception>
    /// <remarks>
    /// It is the caller’s responsibility to first call the <see cref="SqliteDataReader.Read"/> method to advance the reader
    /// to the next record, which must return true to indicate that there is valid data to read.
    /// </remarks>
    public static string GetString(this SqliteDataReader reader, string fieldName)
    {
        // Retrieve the ordinal and then the string value of the specified field.
        int ordinal = reader.GetOrdinal(fieldName);
        return reader.GetString(ordinal);
    }
}
