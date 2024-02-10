/**
 * Represents a record within the Semantic Kernel Memory Table that includes embedding information
 * and metadata in JSON format. This class is an extension of {@code DataEntryBase} and is designed
 * to enable semantic processing and machine learning applications by providing data in both contextual
 * and numerical formats.
 */
public class DatabaseEntry extends DataEntryBase {
    // JSON string containing the embedding vector data for machine learning models.
    private final String embedding;
    // JSON string containing descriptive information related to the entry.
    private final String metadata;

    /**
     * Constructs a {@code DatabaseEntry} object with the specified key, metadata, embedding, and timestamp.
     * The key uniquely identifies the entry, while the metadata and embedding provide contexts and
     * numerical representation of the data, respectively. The timestamp indicates when the entry was created or modified.
     *
     * @param key       The unique identifier for the entry.
     * @param metadata  A JSON string containing descriptive information about the entry.
     * @param embedding A JSON string representing the entry in a numerical format for machine learning purposes.
     * @param timestamp The time of the last creation or update of the entry.
     */
    public DatabaseEntry(String key, String metadata, String embedding, ZonedDateTime timestamp) {
        super(key, timestamp);
        this.metadata = metadata;
        this.embedding = embedding;
    }

    /**
     * Retrieves the metadata associated with this entry. The metadata is provided in a JSON format and
     * could include a range of information pertinent to the entry such as titles, descriptions, and other metadata elements.
     *
     * @return The JSON string containing the metadata.
     */
    public String getMetadata() {
        return metadata;
    }

    /**
     * Retrieves the embedding information associated with this entry. The embedding is provided in a JSON format
     * and consists of a numerical representation suitable for processing by machine learning models.
     *
     * @return The JSON string containing the embedding information.
     */
    public String getEmbedding() {
        return embedding;
    }

    // No additional public methods or fields are present that require documentation comments.
    // Private fields embedding and metadata are described adequately by their initial comments and Javadoc for public getters.
}
