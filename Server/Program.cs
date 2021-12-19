using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO; // для работы с файлами
using System.Net;
using System.Net.Sockets; // для сокетов
//using MySqlConnector;
//using 


namespace Server
{
    class Program
    {

        static void Main(string[] args)
        { 
            string passw, login;
            string request;

            /*MySqlConnectionStringBuilder db;    // для работы
            MySqlConnection conn;               // с базой
            MySqlCommand cmd;                   // данных
            string sql;                         // сюда будем писать запросы*/


            try
            {
                Protocol protocol = new Protocol(); // задаем протокол общения сервера и клиента
                Connection connection = new Connection(); // задаем подключение
                connection.bind(); // связываем сокет с локальной точкой, по которой будем принимать данные

                User user = new User(); // задали юзера с которым будем работать

                // ЗДЕСЬ БУДЕМ ПОДКЛЮЧАТЬ БД

               /* db = new MySqlConnectionStringBuilder();    //инициализируем БД
                db.Server = "mdbdigitalsign.database.windows.net"; // хостинг БД
                db.Database = "dbDigitalSign"; // Имя БД
                db.Port = 3306;
                db.UserID = "admin"; // Имя пользователя БД
                db.Password = "1111"; // Пароль пользователя БД
                db.CharacterSet = "utf8"; // Кодировка Базы Данных
                conn = new MySqlConnection(db.ConnectionString);    // поключаемся к БД
                conn.Open(); // открыли БД
                Console.WriteLine("Подключение к БД установлено");*/

                // ПРИМЕР ЗАПРОСА К БД И ВЫГРУЗКЕ ОТТУДА
                /*sql = "SELECT name_group FROM groups"; 
                cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                textBox1.Clear();
                while (reader.Read())
                    textBox1.Text += reader[0].ToString()*/

                // ПРИМЕР ЗАПИСИ В БД НОВОГО ЭЛЕМЕНТА
                /*sql = "INSERT INTO students (id_stud, name1, name2, name3, group_num) ";
                sql += "VALUES(NULL, " +
                "'" + textBox2.Text + "', " + // Фамилия
                "'" + textBox3.Text + "', " + // Имя
                "'" + textBox4.Text + "', " + // Отчество
                getNumGroup().ToString() + ")"; // номер группы
                cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();*/



                // начинаем прослушивание
                connection.listen();

                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    Socket client = connection.listenSocket.Accept(); // сокет для подключившегося клиента

                    while(true) { // цикл который работает с текущим клиентом

                        // получаем сообщение
                        request = connection.getFromClient(client);
                        Console.WriteLine("Получен новый запрос");


                        // запрос на подключение клиента
                        if (request == protocol.requests.connectionRequest)
                        {
                            Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + request);
                            connection.sendToClient(protocol.feedback.feedbackConnection, client);
                        }

                        // запрос на logout
                        else if (request == protocol.requests.logOutRequest)
                        {
                            Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + request);
                            // очистить данные юзера
                            user.Clear();

                            connection.sendToClient(protocol.feedback.feedbackLogOut, client); // формируем ответ об успехе
                        }

                        //запрос на авторизацию юзера
                        /*else if (request == protocol.requests.authorizetionRequest)
                        {
                            Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + request);
                            string result;
                            login = connection.getFromClient(client);
                            passw = connection.getFromClient(client);

                            // ищем его в БД


                            if (есть в БД) // нашли в БД
                            {
                                user.setDataFromDatabase();// запоминаем айдишник  юзера
                                result = protocol.feedback.feedbackAuthorized; // формируем ответ об успехе
                            }
                            else // не нашли в БД
                            {
                                result = protocol.feedback.feedbackAuthError; // формируем ответ об ошибке
                            }
                            connection.sendToClient(result, client); // отправляем ответ
                        }*/



                        // запрос на регистрацию юзера 
                        else if (request == protocol.requests.registrationRequest)
                        {
                            Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + request);
                            string result;
                             login = connection.getFromClient(client);
                            connection.sendToClient("Логин получен", client);
                             passw = connection.getFromClient(client);

                             //if (/*логин не совпадает с уже существующим*/)
                             //{
                             user.setDataFromDatabase(1, login, passw, user.privateKey, user.publicKey);// запоминаем юзера
                             user.setKeys(); // сгенерили ключи
                             user.setFolder(); // создали папку, в которой будут храниться все подписанные файлы этим юзером

                                // записываем в БД

                                
                                result = protocol.feedback.feedbackRegistr; // формируем ответ об успехе
                             //}
                            //else
                            //{
                                //result = protocol.feedback.feedbackRegError; // формируем ответ об ошибке
                            //}
                            connection.sendToClient(result, client); // отправляем ответ
                        }

                        // запрос на отключение клиента
                        else if (request == protocol.requests.disconnectionRequest)
                        {
                            Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + request);
                            client.Shutdown(SocketShutdown.Both);
                            client.Close(); // закрываем сокет

                            break;// выходим из цикла работы с текущим клиентом
                        }

                        // запрос на получение подписи
                        else if (request == protocol.requests.getSignRequest)
                        {
                            Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + request);
                            connection.sendToClient(protocol.feedback.feedbackGetSign, client); // отправляем ответ о том что мы получили запрос на подпись

                            // принять файл, который мы хотим подписать
                            string msg = connection.getFromClient(client);
                            //Console.WriteLine("Сообщение получено");
                            DigitalSign ds = new DigitalSign(user, msg);
                            ds.Work(); // шифруем, в поле ds.sign вычислится шифр
                            
                             connection.sendToClient(user.publicKeyString, client); // отправляем открытый ключ
                             string f = connection.getFromClient(client);
                             connection.sendToClient(ds.signedFileName, client); // отправляем имя зашифрованного файла
                             

                        }
                        else Console.WriteLine("Что-то на татарском");

                    } //ЦИКЛ ДО СЮДА БУДЕТ
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

}
