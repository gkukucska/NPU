using NPU.Services;
using NPU.Protocols;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using static NPU.Protocols.AuthenticationService;
using NPU.Utils.FileIOHelpers;
using NPU.Interfaces;

namespace NPU.Services.AuthenticationService
{
    public class AuthenticationService : AuthenticationServiceBase
    {
        private ISessionTokenManager _sessionTokenManager;

        public AuthenticationService(ISessionTokenManager sessionTokenManager)
        {
            _sessionTokenManager=sessionTokenManager;
        }
        public override Task<SessionData> OpenSession(LoginCredentialData request, ServerCallContext context)
        {
            return Task.Run(() =>
            {
                try
                {
                    return new SessionData() { UserName = request.UserName, SessionToken = _sessionTokenManager.GetSessionToken(request.UserName, request.Password) };
                }
                catch (Exception e)
                {
                    return new SessionData();
                }
            });
        }

        public override Task<SessionValidationData> ValidateSession(SessionData request, ServerCallContext context)
        {
            return Task.Run(() =>
            {
                if (_sessionTokenManager.ValidateSession(request.UserName, request.SessionToken))
                {
                    return new SessionValidationData() { IsValid = true };
                }
                return new SessionValidationData() { IsValid = false, InValidReason = "Invalid session token" };
            });
        }

        public override Task<Empty> CloseSession(SessionData request, ServerCallContext context)
        {
            return Task.Run(() =>
            {
                _sessionTokenManager.CloseSession(request.UserName, request.SessionToken);
                return new Empty();
            });
        }
    }
}