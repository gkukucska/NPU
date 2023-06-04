namespace NPU.Interfaces
{
    public interface IImageDataRepository
    {
        /// <summary>
        /// Gets an image data by its ID
        /// </summary>
        /// <param name="imageID">If null, the first image is retrieved for the user</param>
        /// <returns></returns>
        Task<byte[]> GetImageByID(string username, CancellationToken cancellationToken, string? imageID = null);


        /// <summary>
        /// Gets the next image from the user
        /// </summary>
        /// <param name="imageID">If null, the first image is retrieved for the user</param>
        /// <returns></returns>
        Task<byte[]> GetNextImageByID(string username, CancellationToken cancellationToken, string? imageID = null);


        /// <summary>
        /// Gets the image description from the user
        /// </summary>
        /// <param name="imageID">If null, the description of the first image is retrieved for the user</param>
        /// <returns></returns>
        Task<string> GetImageDescription(string userName, CancellationToken cancellationToken, string? imageID = null);


        /// <summary>
        /// Saves image in data repository
        /// </summary>
        Task<string> SaveImage(string userName, byte[] imagedata, CancellationToken cancellationToken);


        /// <summary>
        /// Saves image description in data repository
        /// </summary>
        Task SaveImageDescription(string userName, string imageID, string description, CancellationToken cancellationToken);


        /// <summary>
        /// Gets the unique ID of the image based only on its data
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> GetImageId(byte[] imageData, CancellationToken cancellationToken);


        /// <summary>
        /// Removes the image based on its ID
        /// </summary>
        Task RemoveImage(string userName, string imageID, CancellationToken cancellationToken);
    }
}