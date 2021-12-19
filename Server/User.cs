using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography; // для RSA алгоритма

namespace Server
{
    internal class User
    {
        public int id;
        public string login;
        public string hashPassw;
        public string privateKeyString;
        public string publicKeyString;
        public byte[] publicKey; // ?? зачем они мне нужны вообще
        public byte[] privateKey;// ?? зачем они мне нужны вообще
        public RSAParameters rsaPublicKeyInfo;
        public RSAParameters rsaPrivateKeyInfo;
        StringBuilder builder;
        public string pathFolder;

        public User()
        {
            builder = new StringBuilder();
        }

        public void setDataFromDatabase(int idd, string l, string p, byte[] prk, byte[] pubk)
        {
            publicKey = prk;
            id = idd;
            publicKey = pubk;
            login = l;
            hashPassw = p;

            //publicKeyString = builder.Append(Encoding.Unicode.GetString(publicKey)).ToString();
            //privateKeyString = builder.Append(Encoding.Unicode.GetString(privateKey)).ToString();


        }

        /*public string getLogin()
        {
            return login;
        }

        public int getId()
        {
            return id;
        }

        public string getPrivate()
        {
            return privateKeyString;
        }

        public string getPublic()
        {
            return publicKeyString;
        }

        public string getPassword()
        {
            return hashPassw;
        }

        public void setPrivateKeyString()
        {
            privateKeyString = privateKey.ToString();
        }

        public void setPublicKeyString()
        {
            publicKeyString = publicKey.ToString();
        }*/

        public void Clear()
        {
            id = -1;
            login = "";
            hashPassw = "";
        }

        public void setKeys()
        {
            RSA rsa = RSA.Create(); // генерирует пару ключей
            //Save the public key information to an RSAParameters structure.  
            rsaPublicKeyInfo = rsa.ExportParameters(false);
            rsaPrivateKeyInfo = rsa.ExportParameters(true);

            publicKey = rsa.ExportRSAPublicKey();
            privateKey = rsa.ExportRSAPrivateKey();

            publicKeyString =  Convert.ToBase64String(publicKey);
            privateKeyString = Convert.ToBase64String(privateKey);

            //publicKeyString = builder.Append(Encoding.UTF8.GetString(publicKey)).ToString();
            //privateKeyString = builder.Append(Encoding.UTF8.GetString(privateKey)).ToString();

        }

        public void setFolder()
        {
            pathFolder = Path.Join(@"D:\SecurityServer", login);
            System.IO.Directory.CreateDirectory(pathFolder);
        }
    }
}
