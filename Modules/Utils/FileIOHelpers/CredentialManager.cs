using NPU.Utils.EncriptionServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPU.Utils.FileIOHelpers
{
    public static class CredentialManager
    {
        private static string _credentialLocation = "Users.dat";
        private static object _registrationlock=new object();   
        private static IEnumerable<KeyValuePair< string,string>> _credentialList;

        static CredentialManager()
        {
            _credentialList = FileIOHelpers.Load(_credentialLocation).Result.Select(x => new KeyValuePair<string, string>(x.Split(";")[0], x.Split(";")[1]));
        }

        public static bool IsUserTaken(string username)
        {
            return _credentialList.Any(x=>x.Key.Equals(username));
        }

        public async static Task<bool> RegisterUserAsync(string username, string password)
        {
            lock (_registrationlock)
            {
                if (IsUserTaken(username))
                {
                    return false;
                }
                _credentialList=_credentialList.Append(new KeyValuePair<string, string>(username, password.EncryptToStoredString()));
            }
            await FileIOHelpers.Save(username + ";" + password.EncryptToStoredString(), _credentialLocation);
            return true;
        }

        public static bool IsCredentialValid(string username,string password)
        {
            var userCredentials=_credentialList.First(x => x.Key.Equals(username));
            return userCredentials.Value.Equals(password.EncryptToStoredString());
        }
    }
}
