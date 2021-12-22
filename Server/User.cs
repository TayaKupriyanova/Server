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
            publicKeyString = rsa.ToXmlString(false); // public
            privateKeyString = rsa.ToXmlString(true); // private
        }

        // при авторизации мы переписываем значения из базы данных в usera
        public void setData( string log, string pass, string [] keys) 
        {
            login = log;
            hashPassw = pass;
            privateKeyString =keys[0];
            publicKeyString = keys[1];
        }

        public void setData(string log, string pass) //при регистрации добавляем к пользователю только логин и пароль,
                                                     //тк значения ключей уже записаны функцией setKeys
        {
            login = log;
            hashPassw = pass;
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
