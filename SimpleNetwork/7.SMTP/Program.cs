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

            string pathFile = @"D:\ss.webp";

            var attachment = new MimePart("image", "webp")
            {
                FileName = "Привіт друже",
                Content = new MimeContent(File.OpenRead(pathFile))
            };
            var body = new TextPart("plain")
            {
                Text = message.Body
            };
            var multipart = new Multipart("mixed");
            multipart.Add(body);
            multipart.Add(attachment);


            // Створення повідомлення
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(config.From));
            emailMessage.To.Add(new MailboxAddress(message.To));
            emailMessage.Subject = message.Subject;

            // Тіло повідомлення
            emailMessage.Body = multipart;

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
