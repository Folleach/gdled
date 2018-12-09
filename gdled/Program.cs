using System;

namespace gdled
{
    class Program
    {
        static ProgresReader Reader;
        static Arduino Ard;

        static void Main(string[] args)
        {
            Console.WriteLine("Доступные порты:");
            foreach (string element in Arduino.GetPortNames())
                Console.WriteLine(element);
            Console.Write("Выберите порт > ");
            Ard = new Arduino(Console.ReadLine());
            Reader = new ProgresReader(35);
            Reader.ProgresChanged += Reader_ProgresChanged;
        }

        private static void Reader_ProgresChanged(int procentage)
        {
            Ard.SendProgres(procentage);
        }
    }
}
