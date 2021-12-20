using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Server
{
    internal class Database
    {
        MySqlConnectionStringBuilder db;    // для работы
        public MySqlConnection conn;               // с базой
        MySqlCommand cmd;                   // данных
        string sql;                         // сюда будем писать запросы

        public Database()
        {
            db = new MySqlConnectionStringBuilder();    //инициализируем БД
            db.Server = "127.0.0.1"; // хостинг БД
            db.Database = "DigitalSign"; // Имя БД
            db.Port = 3306;
            db.UserID = "admin"; // Имя пользователя БД
            db.Password = "1111"; // Пароль пользователя БД
            db.CharacterSet = "utf8mb4"; // Кодировка Базы Данных
            conn = new MySqlConnection(db.ConnectionString);    // поключаемся к БД
            sql = "";
        }

        public void Open()
        {
            conn.Open(); // открыли БД
        }

        //запрос на запись строки в БД - регистрация пользователя
        public void InsertInto (User u, MySqlConnection con)
        {
            //con.Open();
            sql = "INSERT INTO Clients (id, login, passwordhash, privatekey, publickey) ";
            sql += "VALUES(NULL, " +
            "'" + u.login + "', " +             // Логин
            "'" + u.hashPassw + "', " +         // Хэш пароля
            "'" + u.privateKeyString + "', " +  // Закрытый ключ
            "'" + u.publicKeyString + "')";      // Открытый ключ

            cmd = new MySqlCommand(sql, con);
            cmd.ExecuteNonQuery();
        }

        //запрос на уникальность логина - перед тем как зарегать пользователя
        public bool isUnique(string s, MySqlConnection con) // найти в столбце логинов строку s                           
        {
            con.Open();
            sql = "SELECT login FROM Clients WHERE login = " + "'" + s + "'";
            cmd = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            bool flag = !reader.HasRows;
            reader.Dispose();
            cmd.Dispose();
            return flag; // вернуть true, если не нашли
        }

        //запрос на совпадение логина и пароля - при авторизации
        public bool IsDataRight(string login, string hash, MySqlConnection con)
        {
            con.Open();
            sql = "SELECT * FROM Clients WHERE login = " + "'" + login + "' AND passwordhash = " + "'" + hash + "'";
            cmd = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            bool flag = reader.HasRows;
            reader.Dispose();
            cmd.Dispose();
            return flag; // вернуть true, если нашли строку
        }

        //запрос на получение строки по логину и паролю - для получения ключей
        public string[] GetKeys(string login, string hash, MySqlConnection con)
        {
            string[] keys = new string[2];
            sql = "SELECT privatekey, publickey FROM Clients WHERE login = " + "'" + login + "' AND passwordhash = " + "'" + hash + "'";
            cmd = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                keys[0] = reader.GetValue(0).ToString();
                keys[1] = reader.GetValue(1).ToString();
            }
            reader.Dispose();
            cmd.Dispose();
            return keys;
        }

        public void Close()
        {
            conn.Close();
        }
    }
}
