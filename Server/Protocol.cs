using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Protocol
    {
        public Request requests;
        public Feedback feedback;

        public Protocol()
        {
            requests = new Request();
            feedback = new Feedback();
        }

    }


    internal class Request
    {
        public string connectionRequest;      // запрос на подключение
        public string disconnectionRequest;   // запрос на отключение
        public string authorizetionRequest;   // запрос на авторизацию юзера
        public string registrationRequest;    // запрос на регистрацию юзера
        public string getSignRequest;         // запрос на получение подписи
        public string logOutRequest;          // запрос на выход из профиля юзера
        public string getFilesRequest;        // запрос на просмотр ранее подписанных файлов
        public string getkeyRequest;        // запрос на получение открытого ключа
        public string getTextRequest;         // запрос на получение текста файла


        public Request()
        {
            connectionRequest = "Запрос на подключение клиента";
            disconnectionRequest = "Запрос на отключение клиента";
            authorizetionRequest = "Запрос на авторизацию пользователя";
            registrationRequest = "Запрос на регистрацию пользователя";
            getSignRequest = "Запрос на получение подписи";
            logOutRequest = "Запрос на выход из профиля юзера";
            getFilesRequest = "Запрос на просмотр файлов";
            getkeyRequest = "Запрос на получение ключа";
            getTextRequest = "Запрос на получение текста файла";
        }

    }


    internal class Feedback
    {
        public string feedbackConnection;       // запрос на подключение принят
        public string feedbackDisconnection;    // запрос на отсоединение принят
        public string feedbackAuthorized;       // пользователь авторизован
        public string feedbackAuthError;        // пользователь не авторизован
        public string feedbackRegistr;          // пользователь зарегистрирован
        public string feedbackRegError;         // пользователь не зарегистрирован
        public string feedbackGetSign;          // запрос на подпись принят
        public string feedbackLogOut;           // выход из профиля юзера прошел успешно
        public string feedbackFiles;            //запрос на просмотр файлов принят
        public string feedbackAu;               // запрос на авторизацию принят
        public string feedbackText;             // запрос на получение содержимого файла принят

        public Feedback()
        {
            feedbackConnection = "Запрос на подключение принят";
            feedbackDisconnection = "Запрос на отсоединение принят";
            feedbackAuthorized = "Пользователь успешно авторизован";
            feedbackAuthError = "Пользователь не может быть авторизован";
            feedbackRegistr = "Пользователь успешно зарегистрирован";
            feedbackRegError = "Пользователь с таким логином уже существует";
            feedbackGetSign = "Запрос на получение подписи принят";
            feedbackLogOut = "Запрос на выход из профиля юзера принят";
            feedbackFiles = "Запрос на просмотр файлов принят";
            feedbackAu = "Запрос на авторизацию принят";
            feedbackText = "Запрос на получение содержимого файла принят";
        }
    }
}
