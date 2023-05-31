using Grpc.Core;
using NPU.Protocols;
using NPU.Services;
using NPU.Utils.FileIOHelpers;
using static NPU.Protocols.RegistrationService;

namespace NPU.Services.RegistrationService
{
    public class RegistrationService : RegistrationServiceBase
    {
        public override Task<RegistrationValidationResult> ValidateRegistrationData(RegistrationData request, ServerCallContext context)
        {
            return Task.Run(() =>
            {
                if (CredentialManager.IsCredentialValid(request.UserName, request.Password))
                    return new RegistrationValidationResult() { IsValid = true };
                return new RegistrationValidationResult() { IsValid = false, InValidReason = "Username already taken" };
            });
        }

        public override Task<RegistrationResult> Register(RegistrationData request, ServerCallContext context)
        {
            return CredentialManager.RegisterUserAsync(request.UserName, request.Password, context.CancellationToken).ContinueWith((t) =>
            {
                return new RegistrationResult() { IsSucceeded = t.Result };
            });
        }
    }
}