using System;
using System.Dynamic;
using System.Text;
using System.IO;

namespace TravelHelper
{ 
    class Program : Traveler
    {
        const double TicketPrice = 30000.0;
        TextWriter originalConsole = Console.Out;

        static void Main(string[] args)
        {
            int maxDays = 14;
            int minDays = 1;

            // File path for output
            string filePath = "route.txt";

            // Save the current console output
            TextWriter originalConsole = Console.Out;

            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Console.WriteLine("*** ПОМОЩНИК ПУТЕШЕСТВЕННИКА ***");
            Console.WriteLine("Программа поможет вам спланировать ваше путешествие.");
            Countries();
            
            Traveler traveler = new Traveler();
            traveler.GetTouristName();
            traveler.GetCountry();

            if (GetPricePerNight(traveler.Country) == -1)
            {
                Console.WriteLine("Ошибка: такой страны нет в списке. Перезапустите программу и выберите из предложенных стран.");
                return;
            }

            traveler.GetDay();
            traveler.GetBudget();

            if(traveler.Day > maxDays || traveler.Day < minDays)
            {
                Console.WriteLine("Количество дней должно быть от 1 до 14");
                return;
            }else
            {
                // Create a StreamWriter to write to file
                using (StreamWriter writer = new StreamWriter(filePath, false, new UTF8Encoding(true)))
                {
                    // Redirect console output to the file
                    Console.SetOut(writer);

                    double dailyCost = CalculateDailyCost(traveler.Budget, traveler.Day, TicketPrice, traveler.Country);
                    PrintResult(traveler, dailyCost);

                }
                Console.SetOut(originalConsole);
                Console.OutputEncoding = Encoding.UTF8;

                //Console.WriteLine("Маршрут сохранен в файл route.txt");
                Console.WriteLine("Travel plan saved to route.txt");
            }
        }

        static void PrintResult(Traveler traveler, double dailyCost)
        {
            if (dailyCost == -1)
            {
                return;
            }

            traveler.ShowInfo();

            Console.WriteLine($"Средний дневной расход: {GetPricePerNight(traveler.Country)}");
            Console.WriteLine($"Остаток бюджета после билетов и отеля: {Math.Round(dailyCost, 2)}");
            Console.WriteLine("");

            string[] weatherForecast = GenerateWeatherForecast(traveler.Day);
            ShowWeatherForecast(traveler.Day, traveler.Country, weatherForecast);
            Console.WriteLine("");
            TravelPlan(traveler.Day, traveler.Country);
            Console.WriteLine("");
            PackingSuggestions(traveler.Day, weatherForecast);
            Console.WriteLine("");
            //SaveResultToFile(traveler, dailyCost);
        }

        static void Countries()
        {
            Console.WriteLine("Доступные страны: Япония, Франция. Турция");
        }
        static double GetPricePerNight(string country)
        {
            switch (country)
            {
                case "Япония": return 7000.0;
                case "Франция": return 9000.0;
                case "Турция": return 5000.0;
                default: return -1;
            }
        }

        static double CalculateDailyCost(double budget, int days, double ticketPrice, string country)
        {
            double pricePerNight = GetPricePerNight(country);


            // Calculate remaining budget after ticket
            double remainingBudget = budget - ticketPrice;

            // Ensure budget is sufficient
            if (remainingBudget < 0)
            {
                Console.WriteLine("Бюджет слишком мал для оплаты билета.");
                return -1;
            }
            double totalAccommodationCost = pricePerNight * days;

            // Check if the budget covers the accommodation
            if (remainingBudget < totalAccommodationCost)
            {
                Console.WriteLine("Бюджета недостаточно для проживания на указанный срок.");
                return -1;
            }
            double dailyBudget = (remainingBudget - totalAccommodationCost) / days;
            
            return dailyBudget;
        }

        static string[] GenerateWeatherForecast(int days)
        {
            Random random = new Random();
            string[] forecast = new string[] { "Солнечно", "Облачно", "Дождливо" };
            string[] result = new string[days];

            for (int i = 0; i < days; i++)
            {
                int temperature = random.Next(10, 30);
                result[i] = $"{forecast[random.Next(0, forecast.Length)]}, {temperature}°C";
            }

            return result;
        }

        static void ShowWeatherForecast(int days, string country, string[] weatherForecast)
        {
            country = country switch
            {
                "Япония" => "Японии",
                "Турция" => "Турции",
                "Франция" => "Франции",
                _ => country
            };
            Console.WriteLine($"Прогноз погоды в {country} на {days} дней:");
            for (int i = 0; i < days; i++)
            {
                Console.WriteLine(weatherForecast[i]);
            }
        }

        static void PackingSuggestions(int days, string[] weatherForecast)
        {
            string[] advice = new string[3];
            Console.WriteLine("Рекомендации по упаковке вещей:");

            for (int i = 0; i < days; i++)
            {
                string[] weatherParts = weatherForecast[i].Split(',');
                string weather = weatherParts[0];
                int temperature = int.Parse(weatherParts[1].Replace("\u00B0C", "").Trim());
                

                if (weather == "Дождливо")
                {
                    advice[0] = "зонт";
                }

                if (temperature > 25)
                {
                    advice[1] = "солнцезащитный крем";
                }
                else if (temperature < 15)
                {
                    advice[2] = "теплую одежду";
                }
            }
            Console.WriteLine("Возьмите: " + string.Join(", ", advice.Where(a => a != null)) + ".");
        }

        static void TravelPlan(int days, string country)
        {
            Random random = new Random();
            string[] activities = new string[] { "Шопинг", "Экскурсия", "День отдыха", "Прогулка по городу"};
            country = country switch
            {
                "Япония" => "Японию",
                "Турция" => "Турцию",
                "Франция" => "Францию",
                _ => country
            };

            Console.WriteLine("***");
            Console.WriteLine($"Маршрут для поездки в {country} на {days} дней:");

            Console.WriteLine("1 день: Заселение в отель, прогулка по окрестностям.");
            Console.WriteLine("2 день: Посещение музея.");
            Console.WriteLine("3 день: Экскурсия на природу.");
            
            for (int i = 3; i < days; i++)
            {
                Console.WriteLine($"{i + 1} день: {activities[random.Next(0, activities.Length)]}.");
            }
            Console.WriteLine("***");
        }

    }

}

