using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HomeWork_10
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Бот.
        /// </summary>
        private MessageBotClient bot;

        /// <summary>
        /// Токен бота.
        /// </summary>
        private string token;

        [Obsolete]
        public MainWindow()
        {
            InitializeComponent();
            token = @"token.txt";
            if (File.Exists(token))
            {
                bot = new MessageBotClient(File.ReadAllText(token), this);
                listLog.ItemsSource = bot.logs;
            }
            else
            {
                MessageBox.Show("Файл с токеном не найден!");
                this.Close();
            }
        }

        /// <summary>
        /// Закрыть приложение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Перемещение рабочего окна.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        /// <summary>
        /// Перемещение курсора в текстбоксе при нажатии Enter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSendMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int start = txtSendMessage.SelectionStart + 1;
                txtSendMessage.Text = txtSendMessage.Text.Insert(txtSendMessage.SelectionStart, "\n");
                txtSendMessage.SelectionStart = start;

            }
        }

       /// <summary>
       /// Отправить сообщение.
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void SendMessage(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbId.Text))
            {
                bot.Send(txtSendMessage.Text, Convert.ToInt64(tbId.Text));
                txtSendMessage.Clear();
            }
            else
            {
                MessageBox.Show("Не выбран пользователь.");
            }
        }

        /// <summary>
        /// Очистить окно логов.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearLogs(object sender, RoutedEventArgs e)
        {
            bot.Clear();
        }

        /// <summary>
        /// Сохранить логи.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveLogs(object sender, RoutedEventArgs e)
        {
            bot.SaveLogs();
        }

        /// <summary>
        /// Загрузить логи.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadLogs(object sender, RoutedEventArgs e)
        {
            bot.LoadLogs();
        }
    }
}
