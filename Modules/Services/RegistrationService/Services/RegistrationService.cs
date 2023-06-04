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
        private ILogger<RegistrationService> _logger;
        private ICredentialManager _credentialManager;

        public RegistrationService(ICredentialManager credentialManager, ILogger<RegistrationService> logger)
        {
            _logger = logger;
            _credentialManager = credentialManager;
        }

        public override Task<RegistrationValidationResult> ValidateRegistrationData(RegistrationData request, ServerCallContext context)
        {
            return Task.Run(() =>
            {
                _logger.LogDebug($"Request for validate registration data with username: {request.UserName}, taskid {Task.CurrentId}");
                try
                {
                    if (_credentialManager.IsCredentialValid(request.UserName, request.Password))
                        return new RegistrationValidationResult() { IsValid = true };
                    return new RegistrationValidationResult() { IsValid = false, InValidReason = "Username already taken" };
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Error during registration data validation for user: {request.UserName}, taskid {Task.CurrentId}");
                    return new RegistrationValidationResult() { IsValid = false, InValidReason = "Error during validation" };
                }
                finally
                {
                    _logger.LogDebug($"Registration data validated for user: {request.UserName}, taskid {Task.CurrentId}");
                }
            });
        }

        public override Task<RegistrationResult> Register(RegistrationData request, ServerCallContext context)
        {
            return _credentialManager.RegisterUserAsync(request.UserName, request.Password, context.CancellationToken)
                                     .ContinueWith((t) =>
                                     {
                                         _logger.LogDebug($"Request for registration with username: {request.UserName}, taskid {Task.CurrentId}");
                                         try
                                         {
                                             if (t.Result)
                                             {
                                                 _logger.LogInformation($"Successfull registration with username: {request.UserName}, taskid {Task.CurrentId}");
                                             }
                                             else
                                             {
                                                 _logger.LogInformation($"Unsuccessfull registration with username: {request.UserName}, taskid {Task.CurrentId}");
                                             }
                                             return new RegistrationResult() { IsSucceeded = t.Result };
                                         }
                                         catch (Exception e)
                                         {
                                             _logger.LogError(e, $"Error during registration for user: {request.UserName}, taskid {Task.CurrentId}");
                                             return new RegistrationResult() { IsSucceeded = false };
                                         }
                                         finally
                                         {
                                             _logger.LogDebug($"Registration request finished for user: {request.UserName}, taskid {Task.CurrentId}");
                                         }
                                     });
        }
    }
}