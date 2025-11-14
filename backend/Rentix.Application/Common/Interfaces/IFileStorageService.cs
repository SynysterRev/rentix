namespace Rentix.Application.Common.Interfaces
{
    public interface IFileStorageService
    {
        /// <summary>
        /// Saves the provided file bytes to the configured storage using the specified file name.
        /// Returns the storage path or URL where the file was saved.
        /// </summary>
        /// <param name="fileStream">Stream content of the file to save.</param>
        /// <param name="fileName">Desired file name (may be altered to avoid collisions).</param>
        /// <returns>A task that resolves to the stored file path or URL as a string.</returns>
        public Task<string> SaveFileAsync(Stream fileStream, string fileName);

        /// <summary>
        /// Retrieves the file content from storage for the given file path or URL.
        /// </summary>
        /// <param name="filePath">Path or URL of the file in the storage system.</param>
        /// <returns>A task that resolves to the file content as a byte array.</returns>
        public Task<byte[]> GetFileAsync(string filePath);
    }
}
