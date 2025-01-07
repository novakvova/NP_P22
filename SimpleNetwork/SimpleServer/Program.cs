// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Channels;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;
var hostName = Dns.GetHostName();
Console.WriteLine($"Мій хост {hostName}");
//Визначаю список IP адрес у моїй локальній мережі.
var localhost = await Dns.GetHostEntryAsync(hostName);

int i = 0;
Console.WriteLine("Оберіть IP адресу на якій буде працювати сервер:");
foreach (var item in localhost.AddressList)
{
    Console.WriteLine($"{++i}.{item}");
}
Console.Write("->_");
int numberIP = int.Parse(Console.ReadLine()); //Обирайємо один із IP адрес у себе в мережі
int serverPort = 1095;

var serverIp = localhost.AddressList[numberIP - 1];
Console.Title = serverIp.ToString()+$":{serverPort}";

//Параметри нашого сервера ip адреса і порт
IPEndPoint iPEndPoint = new IPEndPoint(serverIp, serverPort);
//Створюємо сокет - це по суті є сервер, який може на себе приймати запити по мережі.
Socket server = new Socket(AddressFamily.InterNetwork, 
    SocketType.Stream, ProtocolType.Tcp);

try
{
    //Прявзати сокер до нашої кінцевої точки
    server.Bind(iPEndPoint);
    server.Listen(10); //10 - розмір черги підключень
    while (true)
    {
        Console.WriteLine("------Сервер очікує запитів від клієнтів-------");
        //У цьому місці сервер стоїть і чекає поки не надійде якийсь запит від клієнта
        Socket client = server.Accept(); //Очікує запит від клієнта
        Console.WriteLine($"До нас стукає {client.RemoteEndPoint}");
        int bytes = 0; //змінна зберігає кількість байт, якій отримує сервер від клієнта
        byte[] buffer = new byte[1024]; //дані які отримані від клієнта
        do
        {
            bytes = client.Receive(buffer); //читаємо повідомлення, яке прийслав нам клієнт
            Console.WriteLine($"Повідомлення: '{Encoding.Unicode.GetString(buffer)}'");
        } while (client.Available > 0); //Цикл буде працювати до тих пір поки не прочитає повідомлення від клієнта
        //Повідомлення, яке надсилаємо клієнту у відповіднь
        string message = $"Дякую дружок. {DateTime.Now}";
        buffer = Encoding.Unicode.GetBytes(message); //перетворює текст у байти
        client.Send(buffer); //Надсилаю повідомлення назад на клієнт
        client.Shutdown(SocketShutdown.Both);
        client.Close();
    }

}
catch(Exception ex)
{
    Console.WriteLine($"Problem {ex.Message}");
}

Console.ReadKey();

