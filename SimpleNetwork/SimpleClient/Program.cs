// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;
using System.Text;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("Вкажіть IP адресу сервера");
var ip = IPAddress.Parse(Console.ReadLine());
Console.WriteLine("Вкажіть порт сервера");
var port = int.Parse(Console.ReadLine());
try
{
    var serverEndPoint = new IPEndPoint(ip, port);
    //Сокет, який буде відправляти запити на сервер
    Socket server = new Socket(AddressFamily.InterNetwork,
        SocketType.Stream, ProtocolType.Tcp);
    server.Connect(serverEndPoint); //клієнт пробує підключити до сервера
    Console.WriteLine("Напишіть текст, який потрібно відправити");
    string text = Console.ReadLine();
    var data = Encoding.Unicode.GetBytes(text); //текст повідомлення кодуємо в байти
    server.Send(data); //відправляємо дані на сервер
    //тепер очікуємо відповіді від сервера
    data = new byte[1024]; //дані куди буде зберігати відповідь
    int bytes = 0; //розмір байтів у відповіді сервера
    do
    {
        bytes = server.Receive(data); //отримали відповідь від сервера
        Console.WriteLine($"Сервер відповів {Encoding.Unicode.GetString(data)}");

    } while (server.Available > 0);
}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}
