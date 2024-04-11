using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.SemanticKernel.Connectors.Sqlite.Tests
{
    public class SqliteMemoryStoreCreateCollectionAsyncTests
    {
        [Theory]
        [InlineData("ValidCollection")]
        [InlineData("Another_Valid_Collection")]
        [InlineData("_UnderscoresAllowed_")]
        [InlineData("123456")]
        public async Task CreateCollectionAsync_CreatesValidCollection_When_CollectionNameIsValid(string collectionName)
        {
            // Arrange
            var tempFilePath = Path.GetTempFileName();
            var memoryStore = await SqliteMemoryStore.ConnectAsync(tempFilePath);

            // Act
            await memoryStore.CreateCollectionAsync(collectionName);
            bool doesExist = await memoryStore.DoesCollectionExistAsync(collectionName);

            // Assert
            Assert.True(doesExist);

            // Cleanup
            memoryStore.Dispose();
            File.Delete(tempFilePath);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async Task CreateCollectionAsync_ThrowsArgumentException_When_CollectionNameIsNullOrWhitespace(string collectionName)
        {
            // Arrange
            var tempFilePath = Path.GetTempFileName();
            var memoryStore = await SqliteMemoryStore.ConnectAsync(tempFilePath);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => memoryStore.CreateCollectionAsync(collectionName));
            Assert.Contains("collectionName", exception.Message);

            // Cleanup
            memoryStore.Dispose();
            File.Delete(tempFilePath);
        }

        [Fact]
        public async Task CreateCollectionAsync_DoesNotCreateCollection_When_CollectionNameIsTooLong()
        {
            // Arrange
            const int excessiveLength = 256;
            var longCollectionName = new string('a', excessiveLength);
            var tempFilePath = Path.GetTempFileName();
            var memoryStore = await SqliteMemoryStore.ConnectAsync(tempFilePath);

            // Act & Assert
            await Assert.ThrowsAsync<SqliteException>(() => memoryStore.CreateCollectionAsync(longCollectionName));

            // We expect the above line to throw and not reach below assertions.
            bool doesExist = await memoryStore.DoesCollectionExistAsync(longCollectionName);
            Assert.False(doesExist);

            // Cleanup
            memoryStore.Dispose();
            File.Delete(tempFilePath);
        }

        [Fact]
        public async Task CreateCollectionAsync_Cancels_When_CancellationTokenIsCancelled()
        {
            // Arrange
            var tempFilePath = Path.GetTempFileName();
            var memoryStore = await SqliteMemoryStore.ConnectAsync(tempFilePath);
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act & Assert
            await Assert.ThrowsAsync<TaskCanceledException>(
                () => memoryStore.CreateCollectionAsync("ValidCollection", cts.Token));

            // Cleanup
            memoryStore.Dispose();
            File.Delete(tempFilePath);
        }
    }
}
