using ClientInterfaces;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using NPU.Interfaces;
using NPU.Protocols;
using NPU.Utils.ImageDataRepository;
using static NPU.Protocols.ImageDataService;
using static System.Net.Mime.MediaTypeNames;

namespace NPU.ImageDataService.Services
{
    public class ImageDataService : ImageDataServiceBase
    {
        private IImageDataRepository _repository;
        private IAuthenticatorClient _authenticatorClient;
        private ILogger _logger;

        public ImageDataService(IAuthenticatorClient authenticatorClient, IImageDataRepository imageDataRepository, ILogger<ImageDataService> logger)
        {
            _authenticatorClient = authenticatorClient;
            _repository = imageDataRepository;
            _logger = logger;
        }

        public override async Task<ImageData> GetFirstImageData(ImageSessionData request, ServerCallContext context)
        {
            try
            {
                _logger.LogDebug($"First image requested for user {request.UserName}, taskid {Task.CurrentId}");
                if (await _authenticatorClient.ValidateSessionAsync(request.UserName, request.SessionToken, context.CancellationToken))
                {
                    var imageData = await _repository.GetNextImageByID(request.UserName, context.CancellationToken);
                    var description = await _repository.GetImageDescription(request.UserName, context.CancellationToken);
                    var imageID = await _repository.GetImageId(imageData, context.CancellationToken);
                    var serializedImage = ByteString.CopyFrom(imageData);
                    _logger.LogDebug($"Found and returning image data with ID {imageID}, taskid {Task.CurrentId}");
                    return new ImageData() { SerializedImage = serializedImage, Description = description, ImageID = imageID, IsSessionValid = true };
                }
                _logger.LogDebug($"Credentials invalid for user {request.UserName}, taskid {Task.CurrentId}");
                return new ImageData() { IsSessionValid = false };
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error during first image request for user {request.UserName}, taskid {Task.CurrentId}");
                return new ImageData() { IsSessionValid = true };
            }
            finally
            {
                _logger.LogDebug($"First image request for user {request.UserName} completed, taskid {Task.CurrentId}");
            }
        }

        public override async Task<ImageData> GetNextImageData(ImageIdentiferData request, ServerCallContext context)
        {
            try
            {
                _logger.LogDebug($"Next image requested for user {request.SessionData.UserName}, taskid {Task.CurrentId}");
                if (await _authenticatorClient.ValidateSessionAsync(request.SessionData.UserName, request.SessionData.SessionToken, context.CancellationToken))
                {
                    var imageData = await _repository.GetNextImageByID(request.SessionData.UserName, context.CancellationToken, request.ImageID);
                    var imageID = await _repository.GetImageId(imageData, context.CancellationToken);
                    var description = await _repository.GetImageDescription(request.SessionData.UserName, context.CancellationToken, imageID);
                    var serializedImage = ByteString.CopyFrom(imageData);
                    _logger.LogDebug($"Found and returning image data with ID {imageID}, taskid {Task.CurrentId}");
                    return new ImageData() { SerializedImage = serializedImage, Description = description, ImageID = imageID, IsSessionValid = true };
                }
                _logger.LogDebug($"Credentials invalid for user {request.SessionData.UserName}, taskid {Task.CurrentId}");
                return new ImageData() { IsSessionValid = false };
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error during next image request for user {request.SessionData.UserName}, taskid {Task.CurrentId}");
                return new ImageData() { IsSessionValid = true };
            }
            finally
            {
                _logger.LogDebug($"Next image request for user {request.SessionData.UserName} completed, taskid {Task.CurrentId}");
            }
        }

        public override async Task<ImageData> GetImageData(ImageIdentiferData request, ServerCallContext context)
        {
            try
            {
                _logger.LogDebug($"Image requested for user {request.SessionData.UserName}, taskid {Task.CurrentId}");
                if (await _authenticatorClient.ValidateSessionAsync(request.SessionData.UserName, request.SessionData.SessionToken, context.CancellationToken))
                {
                    var imageData = await _repository.GetImageByID(request.SessionData.UserName, context.CancellationToken, request.ImageID);
                    var imageID = await _repository.GetImageId(imageData, context.CancellationToken);
                    var description = await _repository.GetImageDescription(request.SessionData.UserName, context.CancellationToken, imageID);
                    var serializedImage = ByteString.CopyFrom(imageData);
                    return new ImageData() { SerializedImage = serializedImage, Description = description, ImageID = imageID, IsSessionValid = true };
                }
                _logger.LogDebug($"Credentials invalid for user {request.SessionData.UserName}, taskid {Task.CurrentId}");
                return new ImageData() { IsSessionValid = false };
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error during image request for user {request.SessionData.UserName}, taskid {Task.CurrentId}");
                return new ImageData() { IsSessionValid = true };
            }
            finally
            {
                _logger.LogDebug($"Image request for user {request.SessionData.UserName} completed, taskid {Task.CurrentId}");
            }
        }

        public override async Task<ImageSaveResult> SaveImageData(ImageUploadData request, ServerCallContext context)
        {
            try
            {
                _logger.LogDebug($"Image save requested for user {request.SessionData.UserName}, taskid {Task.CurrentId}");
                if (await _authenticatorClient.ValidateSessionAsync(request.SessionData.UserName, request.SessionData.SessionToken, context.CancellationToken))
                {
                    var imageData = request.SerializedImage.ToByteArray();
                    var imageID = await _repository.SaveImage(request.SessionData.UserName, imageData, context.CancellationToken);
                    await _repository.SaveImageDescription(request.SessionData.UserName, imageID, request.Description, context.CancellationToken);
                    return new ImageSaveResult() { ImageID = imageID, IsSessionValid = true };
                }
                _logger.LogDebug($"Credentials invalid for user {request.SessionData.UserName}, taskid {Task.CurrentId}");
                return new ImageSaveResult() { IsSessionValid = false };
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error during save image request for user {request.SessionData.UserName}, taskid {Task.CurrentId}");
                return new ImageSaveResult() { IsSessionValid = true };
            }
            finally
            {
                _logger.LogDebug($"Image save request for user {request.SessionData.UserName} completed, taskid {Task.CurrentId}");
            }
        }

        public override async Task<SessionValidity> RemoveImageData(ImageIdentiferData request, ServerCallContext context)
        {
            try
            {
                _logger.LogDebug($"Image remove requested for user {request.SessionData.UserName}, taskid {Task.CurrentId}");
                if (await _authenticatorClient.ValidateSessionAsync(request.SessionData.UserName, request.SessionData.SessionToken, context.CancellationToken))
                {
                    await _repository.RemoveImage(request.SessionData.UserName, request.ImageID, context.CancellationToken);
                    _logger.LogDebug($"Removed image with ID {request.ImageID} for user {request.SessionData.UserName}, taskid {Task.CurrentId}");
                    return new SessionValidity() { IsSessionValid = true };
                }
                _logger.LogDebug($"Credentials invalid for user {request.SessionData.UserName}, taskid {Task.CurrentId}");
                return new SessionValidity() { IsSessionValid = false };
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error during remove image request for user {request.SessionData.UserName}, taskid {Task.CurrentId}");
                return new SessionValidity() { IsSessionValid = true };
            }
            finally
            {
                _logger.LogDebug($"Image remove request for user {request.SessionData.UserName} completed, taskid {Task.CurrentId}");
            }
        }
    }
}