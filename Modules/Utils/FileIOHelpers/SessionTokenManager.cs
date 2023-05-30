using NPU.Utils.EncriptionServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static void CloseSession(string username,string token)
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
                    _sessionTokenList.Add(new KeyValuePair<string, SessionToken>(username, new SessionToken(random.Next().ToString())));
                    return _sessionTokenList.Last(x => x.Key.Equals(username)).Value.Token;
                }
                random = new Random(username.GetHashCode());
                _sessionTokenList.Add(new KeyValuePair<string, SessionToken>(username, new SessionToken(random.Next().ToString())));
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
                if (currenttoken.Value.ValidationLimit.Subtract(DateTime.Now).TotalMilliseconds < 0)
                {
                    _sessionTokenList.Remove(currenttoken);
                    return false;
                }
                currenttoken.Value.RefreshValidationLimit();
                return true;
            }

        }

        private class SessionToken
        {
            public SessionToken(string token)
            {
                Token = token;
                ValidationLimit = DateTime.Now.Add(_validitySpan);
            }
            public string Token { get; set; }
            public DateTime ValidationLimit { get; private set; }

            internal void RefreshValidationLimit()
            {
                ValidationLimit = DateTime.Now.Add(_validitySpan);
            }
        }
    }

}
