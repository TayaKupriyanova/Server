using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO; // для работы с файлами
using System.Net;
using System.Net.Sockets; // для сокетов
using MySql.Data.MySqlClient;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        { 
            string passw, login;
            string request;

            try
            {
                Protocol protocol = new Protocol();         // задаем протокол общения сервера и клиента
                Connection connection = new Connection();   // задаем подключение
                connection.bind();                          // связываем сокет с локальной точкой, по которой будем принимать данные

                User user = new User();                     // задали юзера с которым будем работать

                Database data = new Database();             // подключили БД
                MySqlConnection con = data.conn;
                Console.WriteLine("Подключение к БД установлено");

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
                        else if (request == protocol.requests.authorizetionRequest)
                        {
                            Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + request);
                            string result;
                            login = connection.getFromClient(client);
                            passw = connection.getFromClient(client);

                            if (data.IsDataRight(login, passw, con)) // нашли пользователя с такими данными в БД
                            {
                                string keyPr = "", keyPub = "";
                                data.GetKeys(login, passw, keyPr, keyPub, con);

                                user.setData(login, passw,keyPr, keyPub);// заполняем объект юзера информацией про него
                                user.getFolder();
                                result = protocol.feedback.feedbackAuthorized; // формируем ответ об успехе
                            }
                            else // не нашли в БД
                            {
                                result = protocol.feedback.feedbackAuthError; // формируем ответ об ошибке
                            }
                            connection.sendToClient(result, client); // отправляем ответ
                        }

                        // запрос на регистрацию юзера 
                        else if (request == protocol.requests.registrationRequest)
                        {
                            Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + request);
                            string result;
                             login = connection.getFromClient(client);
                            connection.sendToClient("Логин получен", client);
                             passw = connection.getFromClient(client);

                             if ( data.isUnique(login, con)) // если пользователя с таким именем не сушествует
                             {
                                user.setKeys();             // сгенерили ключи
                                user.setData(login, passw);             // запоминаем юзера
                                user.setFolder();           // создали папку, в которой будут храниться все подписанные файлы этим юзером
                                data.InsertInto(user, con);      // внесли информацию про него в БД
                                result = protocol.feedback.feedbackRegistr; // формируем ответ об успехе
                             }
                            else
                            {
                                result = protocol.feedback.feedbackRegError; // формируем ответ об ошибке
                            }
                            connection.sendToClient(result, client); // отправляем ответ
                        }

                        // запрос на отключение клиента
                        else if (request == protocol.requests.disconnectionRequest)
                        {
                            Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + request);
                            data.Close();
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

                            DigitalSign ds = new DigitalSign(user, msg);
                            ds.Work(); // шифруем, в поле ds.sign вычислится шифр
                            
                            connection.sendToClient(user.publicKeyString, client); // отправляем открытый ключ
                            string f = connection.getFromClient(client);
                            connection.sendToClient(ds.signedFileName, client); // отправляем имя зашифрованного файла
                             

                        }
                        else Console.WriteLine("Что-то на татарском");

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
