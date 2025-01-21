using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7.SMTP
{
    public class Message
    {
        /// <summary>
        /// Тема листа
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Вміст листа
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// Кому на пошту піде лист
        /// </summary>
        public string To { get; set; }
    }
}
