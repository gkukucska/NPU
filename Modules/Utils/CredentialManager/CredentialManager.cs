using NPU.Interfaces;
using NPU.Utils.EncriptionServices;
using NPU.Utils.FileIOHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPU.Utils.CredentialManager
{
    public class CredentialManager:ICredentialManager
    {
        private static string _credentialLocation = "Users.dat";
        private static object _registrationlock=new object();   
        private IEnumerable<KeyValuePair< string,string>> _credentialList;

        public CredentialManager()
        {
            _credentialList = FileIOHelpers.FileIOHelpers.LoadLines(_credentialLocation,CancellationToken.None).Result.Select(x => new KeyValuePair<string, string>(x.Split(";")[0], x.Split(";")[1]));
        }

        public bool IsUserTaken(string username)
        {
            return _credentialList.Any(x=>x.Key.Equals(username));
        }

        public async Task<bool> RegisterUserAsync(string username, string password,CancellationToken token)
        {
            lock (_registrationlock)
            {
                if (IsUserTaken(username))
                {
                    return false;
                }
                _credentialList=_credentialList.Append(new KeyValuePair<string, string>(username, password.EncryptToStoredString()));
            }
            await FileIOHelpers.FileIOHelpers.Append(username + ";" + password.EncryptToStoredString(), _credentialLocation,token);
            return true;
        }

        public bool IsCredentialValid(string username,string password)
        {
            var userCredentials=_credentialList.First(x => x.Key.Equals(username));
            return userCredentials.Value.Equals(password.EncryptToStoredString());
        }
    }
}
