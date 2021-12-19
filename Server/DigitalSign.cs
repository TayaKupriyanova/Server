using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography; // для RSA алгоритма

namespace Server
{
    internal class DigitalSign
    {
        public string msgFileName; // имя файла клиента
        public string signedFileName; // имя зашифрованного файла
        public User user;
        public string sign;
        //public RSA dsrsa;

        public DigitalSign(User u, string msg) // привязываем конкретного юзера к объекту конкретной подписи
        {
            user = new User();
            user = u;
            signedFileName = "";
            msgFileName = msg;
            sign = "";
        }

        // функция формирования нового имени
        public void setSignedFileName()
        {
            signedFileName = Path.Join(user.pathFolder, Path.GetFileName(@msgFileName) ); //"D:\Security_server\логин юзера\" + "имя файла юзера";
        }

        // функция чтения файла юзера
        public string readCLientFile()
        {
            string result = "";
            using (StreamReader sr = new StreamReader(msgFileName, System.Text.Encoding.Default))
            {
                string line;
               
                while ((line = sr.ReadLine()) != null)
                {
                    result+=line;
                }
                sr.Close();
            }
            return result;

        }
        // функция записи в новый файл шифра
        public void createSign()
        {
                StreamWriter sw = new StreamWriter(signedFileName, false, System.Text.Encoding.UTF8);
                sw.WriteLine(sign);
            sw.Close();
        }

        // функция шифрования (здесь sign получает своё значение)
        public void Rsa()
        {
            StringBuilder builder = new StringBuilder();
            byte[] encryptedData;
            byte[] msg = Encoding.Unicode.GetBytes(readCLientFile()); // преобразовали в байты клиентский файл
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider(); // создали объект шифровальщика
            provider.ImportParameters(user.rsaPrivateKeyInfo); // установили закрытый ключ 
            encryptedData = provider.Encrypt(msg, false); // зашифровали закрытым ключом и получили шифр в байтах

            sign = BitConverter.ToString(encryptedData).Replace("-", " ");
            //sign = builder.Append(Encoding.Unicode.GetString(encryptedData)).ToString(); // преобразовали в строку
            //sign = encryptedData.ToString();
        }


        // функция work которая собирает все функции класса воедино
        public void Work()
        {
            setSignedFileName(); // создать имя файла с подписью
           // string userfile = readCLientFile();// прочитать файл юзера
            Rsa(); // вызвать функцию рсы

            createSign();// записать полученное в файл
        }
    }
}

