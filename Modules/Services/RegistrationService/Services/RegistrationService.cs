using Grpc.Core;
using NPU.Interfaces;
using NPU.Protocols;
using NPU.Services;
using NPU.Utils.FileIOHelpers;
using static NPU.Protocols.RegistrationService;

namespace NPU.Services.RegistrationService
{
    public class RegistrationService : RegistrationServiceBase
    {
        private ICredentialManager _credentialManager;

        public RegistrationService(ICredentialManager credentialManager)
        {
            _credentialManager=credentialManager;
        }
        public override Task<RegistrationValidationResult> ValidateRegistrationData(RegistrationData request, ServerCallContext context)
        {
            return Task.Run(() =>
            {
                if (_credentialManager.IsCredentialValid(request.UserName, request.Password))
                    return new RegistrationValidationResult() { IsValid = true };
                return new RegistrationValidationResult() { IsValid = false, InValidReason = "Username already taken" };
            });
        }

        public override Task<RegistrationResult> Register(RegistrationData request, ServerCallContext context)
        {
            return _credentialManager.RegisterUserAsync(request.UserName, request.Password, context.CancellationToken).ContinueWith((t) =>
            {
                return new RegistrationResult() { IsSucceeded = t.Result };
            });
        }
    }
}