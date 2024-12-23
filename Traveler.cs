
using System.Diagnostics.Metrics;

namespace TravelHelper
{
    class Traveler
    {

        public string TouristName { get; set; }
        public string Country { get; set; }
        public int Day { get; set; }
        public double Budget { get; set; }

        public void ShowInfo()
        {
            Console.WriteLine("-----------------------------Информация о путешественнике--------------------------");
            Console.WriteLine($"Имя путешественника: {TouristName}");
            Console.WriteLine($"Страна: {Country}");
            Console.WriteLine($"Количество дней: {Day}");
            Console.WriteLine($"Бюджет: {Budget}");
        }

        public void GetTouristName()
        {
            Console.WriteLine("Привет! Как тебя зовут? ");
            TouristName = Console.ReadLine();
        }

        public void GetCountry()
        {
            Console.WriteLine("В какую страну планируешь путешествие? ");
            Country = Console.ReadLine();
        }

        public void GetDay()
        {
            Console.WriteLine("Сколько дней ты собираешься там провести? (1-14): ");
            Day = Convert.ToInt32(Console.ReadLine());
        }

        public void GetBudget()
        {
            Console.WriteLine("Какой у тебя бюджет на поездку ");
            Budget = Convert.ToDouble(Console.ReadLine());
        }

    }
}