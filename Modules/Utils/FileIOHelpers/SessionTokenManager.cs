namespace NPU.Utils.FileIOHelpers
{
    public static class SessionTokenManager
    {
        private static List<KeyValuePair<string, SessionToken>> _sessionTokenList;
        private static readonly TimeSpan _validitySpan = TimeSpan.FromMinutes(5);
        private static object _lock = new object();

        static SessionTokenManager()
        {
            _sessionTokenList = new List<KeyValuePair<string, SessionToken>>();
        }

        public static void CloseSession(string username, string token)
        {
            lock (_lock)
            {
                var entry = _sessionTokenList.FirstOrDefault(x => x.Key.Equals(username) && x.Value.Token.Equals(token));
                try
                {
                    _sessionTokenList.Remove(entry);
                }
                catch (Exception)
                {
                }
            }
        }

        public static string GetSessionToken(string username, string password)
        {
            lock (_lock)
            {
                if (!CredentialManager.IsCredentialValid(username, password))
                {
                    throw new Exception("Invalid credentials");
                }
                Random random;
                if (_sessionTokenList.Any(x => x.Key.Equals(username)))
                {
                    random = new Random(_sessionTokenList.Last(x => x.Key.Equals(username)).Value.GetHashCode());
                    _sessionTokenList.Add(new KeyValuePair<string, SessionToken>(username, new SessionToken(random.Next().ToString(), InvalidateToken)));
                    return _sessionTokenList.Last(x => x.Key.Equals(username)).Value.Token;
                }
                random = new Random(username.GetHashCode());
                _sessionTokenList.Add(new KeyValuePair<string, SessionToken>(username, new SessionToken(random.Next().ToString(), InvalidateToken)));
                return _sessionTokenList.Last(x => x.Key.Equals(username)).Value.Token;
            }
        }

        public static bool ValidateSession(string username, string token)
        {
            lock (_lock)
            {
                if (!_sessionTokenList.Any(x => x.Key.Equals(username) && x.Value.Token.Equals(token)))
                {
                    return false;
                }
                var currenttoken = _sessionTokenList.First(x => x.Key.Equals(username) && x.Value.Token.Equals(token));
                currenttoken.Value.RefreshValidationLimit();
                return true;
            }
        }

        private static void InvalidateToken(SessionToken token)
        {
            _sessionTokenList.RemoveAll(x => x.Value == token);
        }

        private class SessionToken
        {
            public SessionToken(string token, Action<SessionToken> invalidationAction)
            {
                Token = token;
                _invalidationAction = invalidationAction;
                _cts = new CancellationTokenSource();
                _invalidationTask = Task.Delay(_validitySpan, _cts.Token).ContinueWith((t) => _invalidationAction(this));
            }

            private CancellationTokenSource _cts;
            private Action<SessionToken> _invalidationAction;
            private Task _invalidationTask;
            public string Token { get; set; }

            internal void RefreshValidationLimit()
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = new CancellationTokenSource();
                _invalidationTask = Task.Delay(_validitySpan, _cts.Token).ContinueWith((t) => _invalidationAction(this));
            }
        }
    }

}
