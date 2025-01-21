using System.Diagnostics;
using System.Text;

namespace _6.NovaPoshta
{
    internal class Program
    {
        static async  Task Main(string[] args)
        {
            //int logicalCores = Environment.ProcessorCount;
            //Console.WriteLine($"Кількість логічних ядер (потоків): {logicalCores}");
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            NovaPoshtaService novaPoshtaService = new NovaPoshtaService();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            await novaPoshtaService.SeedAreas();
            await novaPoshtaService.SeedCities();
            await novaPoshtaService.SeedDepartments();
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);

            var list = novaPoshtaService.GetListAreas();

        }
    }
}
