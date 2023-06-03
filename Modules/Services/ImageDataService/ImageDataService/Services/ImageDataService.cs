using ClientInterfaces;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using NPU.Protocols;
using NPU.Utils.ImageDataRepository;
using static NPU.Protocols.ImageDataService;

namespace NPU.ImageDataService.Services
{
    public class ImageDataService : ImageDataServiceBase
    {
        private ImageDataRepository _repository;
        private IAuthenticatorClient _authenticatorClient;

        public ImageDataService(IAuthenticatorClient authenticatorClient)
        {
            _authenticatorClient = authenticatorClient;
            _repository = new ImageDataRepository();
        }

        public override async Task<ImageData> GetFirstImageData(ImageSessionData request, ServerCallContext context)
        {
            if (await _authenticatorClient.ValidateSessionAsync(request.UserName, request.SessionToken, context.CancellationToken))
            {
                var imageData = await _repository.GetFirstImage(request.UserName, context.CancellationToken);
                var description = await _repository.GetFirstImageDescription(request.UserName, context.CancellationToken);
                var imageID = await _repository.GetImageId(imageData, context.CancellationToken);
                return new ImageData() { SerializedImage = ByteString.CopyFrom(imageData), Description = description, ImageID = imageID ,IsSessionValid=true};
            }
            return new ImageData() { IsSessionValid = false };
        }

        public override async Task<ImageData> GetNextImageData(ImageIdentiferData request, ServerCallContext context)
        {
            if (await _authenticatorClient.ValidateSessionAsync(request.SessionData.UserName, request.SessionData.SessionToken, context.CancellationToken))
            {
                var imageData = await _repository.GetNextImageFromID(request.SessionData.UserName, request.ImageID, context.CancellationToken);
                var imageID = await _repository.GetImageId(imageData, context.CancellationToken);
                var description = await _repository.GetImageDescription(request.SessionData.UserName, imageID, context.CancellationToken);
                return new ImageData() { SerializedImage = ByteString.CopyFrom(imageData), Description = description ,ImageID=imageID, IsSessionValid = true };
            }
            return new ImageData() { IsSessionValid = false };
        }

        public override async Task<ImageData> GetImageData(ImageIdentiferData request, ServerCallContext context)
        {
            if (await _authenticatorClient.ValidateSessionAsync(request.SessionData.UserName, request.SessionData.SessionToken, context.CancellationToken))
            {
                var imageData = await _repository.GetImageFromID(request.SessionData.UserName, request.ImageID, context.CancellationToken);
                var description = await _repository.GetImageDescription(request.SessionData.UserName,await  _repository.GetImageId(imageData,context.CancellationToken), context.CancellationToken);
                var imageID = await _repository.GetImageId(imageData, context.CancellationToken);
                return new ImageData() { SerializedImage = ByteString.CopyFrom(imageData), Description = description,ImageID = imageID, IsSessionValid = true };
            }
            return new ImageData() { IsSessionValid = false };
        }

        public override async Task<ImageSaveResult> SaveImageData(ImageUploadData request, ServerCallContext context)
        {
            if (await _authenticatorClient.ValidateSessionAsync(request.SessionData.UserName, request.SessionData.SessionToken, context.CancellationToken))
            {
                var imageID = await _repository.SaveImage(request.SessionData.UserName, request.SerializedImage.ToByteArray(), context.CancellationToken);
                await _repository.SaveImageDescription(request.SessionData.UserName, imageID, request.Description, context.CancellationToken);
                return new ImageSaveResult() { ImageID = imageID, IsSessionValid=true};
            }
            return new ImageSaveResult() { IsSessionValid = false };
        }

        public override async Task<SessionValidity> RemoveImageData(ImageIdentiferData request, ServerCallContext context)
        {
            if (await _authenticatorClient.ValidateSessionAsync(request.SessionData.UserName, request.SessionData.SessionToken, context.CancellationToken))
            {
                await _repository.RemoveImage(request.SessionData.UserName, request.ImageID, context.CancellationToken);
                return new SessionValidity() { IsSessionValid = true };
            }
            return new SessionValidity() { IsSessionValid = false } ;
        }
    }
}