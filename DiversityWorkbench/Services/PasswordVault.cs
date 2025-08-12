using System;
using System.Linq;

namespace DiversityWorkbench.Services
{
    public interface IPasswordVault
    {
        string[] GetUsernamePassword(string host);
        void SetUsernamePassword(string host, string username, string pwd);
    }

    public class PasswordVault : IPasswordVault
    {
        public virtual string[] GetUsernamePassword(string host)
        {

            string[] cred = new string[2];
            try
            {
                var vault = new Windows.Security.Credentials.PasswordVault();
                var credential = vault.FindAllByResource(host).FirstOrDefault();
                if (credential != null)
                {
                    Windows.Security.Credentials.PasswordCredential pwCred = vault.Retrieve(credential.Resource, credential.UserName);
                    //string decryptedPWD = Decrypt(test.Password);

                    cred[0] = credential.UserName;
                    cred[1] = pwCred.Password;

                    return cred;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in PasswordVault.GetUsernamePassword", ex);
                return null;
            }
        }
        public virtual void SetUsernamePassword(string host, string username, string pwd)
        {
            var vault = new Windows.Security.Credentials.PasswordVault();
            // string encryptedPWD = Encrypt("test");
            var credential = new Windows.Security.Credentials.PasswordCredential(
                resource: host,
                userName: username,
                password: pwd);
            if (IsSingleHostResource(host, username))
                vault.Add(credential);
        }

        public virtual bool IsSingleHostResource(string host, string username)
        {
            var vault = new Windows.Security.Credentials.PasswordVault();
            var credentials = vault.FindAllByResource(host);
            if (credentials == null)
                return true;
            foreach(var cred in credentials)
            {
                if (cred.UserName == username)
                    return false;
            }
            return true;
        }

    }


}
