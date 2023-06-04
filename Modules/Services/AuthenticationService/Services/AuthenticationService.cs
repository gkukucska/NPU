using NPU.Services;
using NPU.Protocols;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using static NPU.Protocols.AuthenticationService;
using NPU.Utils;
using NPU.Interfaces;
using Microsoft.Extensions.Logging;

namespace NPU.Services.AuthenticationService
{
    public class AuthenticationService : AuthenticationServiceBase
    {
        private ILogger<AuthenticationService> _logger;
        private ISessionTokenManager _sessionTokenManager;

        public AuthenticationService(ISessionTokenManager sessionTokenManager, ILogger<AuthenticationService> logger)
        {
            _logger = logger;
            _sessionTokenManager = sessionTokenManager;
        }
        public override Task<SessionData> OpenSession(LoginCredentialData request, ServerCallContext context)
        {
            return Task.Run(() =>
            {
                _logger.LogDebug($"Request for open session recieved for user: {request.UserName}, taskid {Task.CurrentId}");
                try
                {
                    var sessionToken = _sessionTokenManager.GetSessionToken(request.UserName, request.Password);
                    return new SessionData() { UserName = request.UserName, SessionToken = sessionToken };
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Error during session opening, taskid {Task.CurrentId}");
                    return new SessionData();
                }
                finally
                {
                    _logger.LogDebug($"Session opened for user: {request.UserName}, taskid {Task.CurrentId}");
                }
            });
        }

        public override Task<SessionValidationData> ValidateSession(SessionData request, ServerCallContext context)
        {
            return Task.Run(() =>
            {
                _logger.LogDebug($"Request for session validation for user: {request.UserName}, taskid {Task.CurrentId}");
                try
                {
                    if (_sessionTokenManager.ValidateSession(request.UserName, request.SessionToken))
                    {
                        _logger.LogDebug($"Valid session for user: {request.UserName}, taskid {Task.CurrentId}");
                        return new SessionValidationData() { IsValid = true };
                    }
                    _logger.LogDebug($"Invalid session for user: {request.UserName}, taskid {Task.CurrentId}");
                    return new SessionValidationData() { IsValid = false, InValidReason = "Invalid session token" };
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Error during session validation, taskid {Task.CurrentId}");
                    return new SessionValidationData() { IsValid = false, InValidReason = "Error during session validation" };
                }
                finally
                {
                    _logger.LogDebug($"Session validation for user: {request.UserName} completed, taskid {Task.CurrentId}");
                }
            });
        }

        public override Task<Empty> CloseSession(SessionData request, ServerCallContext context)
        {
            return Task.Run(() =>
            {
                _logger.LogDebug($"Request for session validation for user: {request.UserName}, taskid {Task.CurrentId}");
                try
                {
                    _sessionTokenManager.CloseSession(request.UserName, request.SessionToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Error during session closing, taskid {Task.CurrentId}");
                }
                return new Empty();
            });
        }
    }
}