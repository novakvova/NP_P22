using MimeKit;
using MailKit.Net.Smtp;

namespace _7.SMTP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            EmailConfiguration config = new EmailConfiguration();
            Message message = new Message
            {
                Body = "Треба іти гуляти",
                Subject = "Виходь на вулицю",
                To = "novakvova@gmail.com"
            };

            // Створення повідомлення
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(config.From));
            emailMessage.To.Add(new MailboxAddress(message.To));
            emailMessage.Subject = message.Subject;

            // Тіло повідомлення
            emailMessage.Body = new TextPart("plain")
            {
                Text = message.Body
            };

            using var client = new SmtpClient();
            try
            {
                client.Connect(config.SmtpServer, config.Port, true);
                client.Authenticate(config.UserName, config.Password);
                client.Send(emailMessage);
                client.Disconnect(true);
               
                Console.WriteLine("Лист успішно відправлено!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Hello, World!");
        }
    }
}
