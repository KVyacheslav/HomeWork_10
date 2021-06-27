using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Telegram.Bot;

namespace HomeWork_10
{
    /// <summary>
    /// Класс, описывающий бота.
    /// </summary>
    public class MessageBotClient
    {
        /// <summary>
        /// Телеграм бот.
        /// </summary>
        private TelegramBotClient bot;

        /// <summary>
        /// Главное окно.
        /// </summary>
        private MainWindow window;

        /// <summary>
        /// Страртовая информация.
        /// </summary>
        private string startInfo;

        /// <summary>
        /// Список пользователей.
        /// </summary>
        private HashSet<long> listUsers;
        
        /// <summary>
        /// Список для хранения логов.
        /// </summary>
        public ObservableCollection<MessageLog> logs { get; set; }

        /// <summary>
        /// Создать экземпляр бота.
        /// </summary>
        /// <param name="token">Токен бота.</param>
        /// <param name="window">Главное окно.</param>
        [Obsolete]
        public MessageBotClient(string token, MainWindow window)
        {
            this.logs = new ObservableCollection<MessageLog>();
            this.window = window;
            this.startInfo = "Хотите узнать погоду в городе? Введите название города!";
            listUsers = new HashSet<long>();

            this.bot = new TelegramBotClient(token);
            this.bot.OnMessage += Bot_OnMessage;
            this.bot.StartReceiving();
        }

        /// <summary>
        /// Обработчик события при колучении сообщения.
        /// </summary>
        /// <param name="sender">Отправитель.</param>
        /// <param name="e">Аргументы сообщения</param>
        [Obsolete]
        private void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            long id = e.Message.Chat.Id;
            string firstName = e.Message.Chat.FirstName;
            string message = e.Message.Text;
            string time = DateTime.Now.ToLongTimeString();

            // Если пользователь написал впервые.
            if (!listUsers.Contains(id))
            {
                // То отправляем ему информацию о боте.
                bot.SendTextMessageAsync(id, startInfo);
            }

            this.listUsers.Add(id);
            this.window.Dispatcher.Invoke(() => logs.Add(
                new MessageLog(id, time, message, firstName)
                ));

            string weather = Weather.GetWeather(message);

            if (!(weather == null))
            {
                bot.SendTextMessageAsync(id, weather);
            }

        }

        /// <summary>
        /// Отправка сообщения
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="id">ID пользователя.</param>
        public void Send(string message, long id)
        {
            bot.SendTextMessageAsync(id, message);
        }

        /// <summary>
        /// Очистить список логов.
        /// </summary>
        public void Clear()
        {
            logs.Clear();
        }

        /// <summary>
        /// Сохранить логи.
        /// </summary>
        public void SaveLogs()
        {
            JObject oLogs = new JObject();
            JArray arrLogs = new JArray();
            var path = "logs.json";

            foreach (var log in logs)
            {
                JObject oLog = new JObject();
                oLog["id"] = log.Id;
                oLog["first_name"] = log.FirstName;
                oLog["time"] = log.Time;
                oLog["message"] = log.Message;
                arrLogs.Add(oLog);
            }

            oLogs["logs"] = arrLogs;

            File.WriteAllText(path, oLogs.ToString());

            MessageBox.Show("Логи сохранены!");
        }

        /// <summary>
        /// Загрузить логи.
        /// </summary>
        public void LoadLogs()
        {
            var path = "logs.json";
            if (!File.Exists(path))
            {
                MessageBox.Show("Файл с логами не создан.");
                return;
            }

            logs.Clear();

            JObject oLogs = JObject.Parse(File.ReadAllText(path));
            var arrLogs = oLogs["logs"].ToArray();

            foreach (var oLog in arrLogs)
            {
                long id = Convert.ToInt64(oLog["id"]);
                string firstName = oLog["first_name"].ToString();
                string time = oLog["time"].ToString();
                string message = oLog["message"].ToString();
                window.Dispatcher.Invoke(() =>
                    logs.Add(new MessageLog(id, time, message, firstName)));
            }

            MessageBox.Show("Логи загружены!");
        }
    }
}
