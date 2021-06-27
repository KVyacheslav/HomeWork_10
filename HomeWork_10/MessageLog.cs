using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_10
{
    /// <summary>
    /// Лог сообщения
    /// </summary>
    public class MessageLog
    {
        /// <summary>
        /// ID пользователя.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Время сообщения.
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Текст сообщения.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Создать лог сообщения.
        /// </summary>
        /// <param name="id">ID пользователя.</param>
        /// <param name="time">Время сообщения.</param>
        /// <param name="message">Текст сообщения.</param>
        /// <param name="firstName">Имя пользователя.</param>
        public MessageLog(long id, string time, string message, string firstName)
        {
            this.Id = id;
            this.Time = time;
            this.Message = message;
            this.FirstName = firstName;
        }
    }
}
