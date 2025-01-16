using System.Text;

namespace _6.NovaPoshta
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            NovaPoshtaService novaPoshtaService = new NovaPoshtaService();
            //novaPoshtaService.SeedAreas();
            
            var list = novaPoshtaService.GetListAreas();

        }
    }
}
