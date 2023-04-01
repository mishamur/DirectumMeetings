using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumMeetings.Models
{
    public class Meeting
    {
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Title { get; set; }
        public string Place { get; set; }
        public TimeSpan Duration { get; set; }

        public Meeting(DateTime dateStart, string title, string place, TimeSpan duration)
        {
            DateStart = dateStart;
            Title = title;
            Place = place;
            Duration = duration;

            DateEnd = DateStart.AddMinutes(duration.TotalMinutes); // DateEnd.AddTicks(duration.Ticks);
        }

        public static Meeting? CreateWithConsole()
        {
            Meeting meeting = null; 
            try
            {
                meeting = TryToCreateWithConsole();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return meeting;
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static Meeting TryToCreateWithConsole()
        {
            Console.WriteLine("\nВведите название встречи:");
            string title = Console.ReadLine();

            Console.WriteLine("Введите дату и время встречи: (в формате ГГГГ-ММ-ДД ЧЧ:ММ):");
            DateTime dateStart = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Введите продолжительность встречи (в минутах):");
            TimeSpan duration = TimeSpan.FromMinutes(double.Parse(Console.ReadLine()));

            Console.WriteLine("Введите место проведения встречи:");
            string place = Console.ReadLine();

            return new Meeting(dateStart, title, place, duration);
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine($"Название встречи: {Title}");
            result.AppendLine($"Дата начала встречи: {DateStart.ToString("g")}");
            result.AppendLine($"Продолжительность встречи(в минутах): {Duration.TotalMinutes}");
            result.AppendLine($"Место проведения встречи: {Place}");
            result.AppendLine($"Дата окончания встречи: {DateEnd.ToString("g")}");
            return result.ToString();
        }
    }
}
