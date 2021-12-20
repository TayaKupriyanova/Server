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
        public string login;
        public string hashPassw;
        public string privateKeyString;
        public string publicKeyString;
        public RSAParameters rsaPublicKeyInfo;
        public RSAParameters rsaPrivateKeyInfo;
        public string pathFolder;
        
        StringBuilder builder;
        public User()
        {
            builder = new StringBuilder();
        }

        public void Clear()
        {
            login = "";
            hashPassw = "";
        }

        public void setKeys() // генерируем новую пару ключей 
        {
            RSA rsa = RSA.Create(); // генерирует пару ключей
            // Вычисляем значения ключей и в формате RSAParamerts, и в формате string
            
            // RSAParamerts
            rsaPublicKeyInfo = rsa.ExportParameters(false);
            rsaPrivateKeyInfo = rsa.ExportParameters(true);

            // string
            publicKeyString =  Convert.ToBase64String(rsa.ExportRSAPublicKey());
            privateKeyString = Convert.ToBase64String(rsa.ExportRSAPrivateKey());

        }

        // при авторизации мы переписываем значения из базы данных в usera
        public void setData( string log, string pass, string [] keys) 
        {
            login = log;
            hashPassw = pass;
            privateKeyString =keys[0];
            publicKeyString = keys[1];

            setRSAkeys(); 
        }

        public void setData(string log, string pass) //при регистрации добавляем к пользователю только логин и пароль,
                                                     //тк значения ключей уже записаны функцией setKeys
        {
            login = log;
            hashPassw = pass;
        }

        public void setRSAkeys() // при авторизации мы получаем ключи в строковом формате, нужно преобразовать в RSAParametrs
        {
            // спосите
        }
        public void setFolder() //при регистрации
        {
            pathFolder = Path.Join(@"D:\SecurityServer", login);
            System.IO.Directory.CreateDirectory(pathFolder);
        }

        public void getFolder() // при авторизации
        {
            pathFolder = Path.Join(@"D:\SecurityServer", login);
        }

        public string[] getFiles() // возвращает список всех файлов
        {
            return Directory.GetFiles(pathFolder);
        }
    }
}
