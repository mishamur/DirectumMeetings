using System.Text;

namespace DirectumMeetings.Models
{
    public class Meeting
    {
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Title { get; set; }
        public string Place { get; set; }
        public TimeSpan TimeDuration { get; set; }
        public TimeSpan TimeNotification { get; set; }

        public Meeting(DateTime dateStart, string title, string place, TimeSpan duration, double notification = 0)
        {
            DateStart = dateStart;
            Title = title;
            Place = place;
            TimeDuration = duration;

            DateEnd = DateStart.AddMinutes(duration.TotalMinutes); // DateEnd.AddTicks(duration.Ticks);
            TimeNotification = TimeSpan.FromMinutes(notification);
        }
        /// <summary>
        /// Создать объект meeting с помощью консоли
        /// </summary>
        /// <returns></returns>
        public static Meeting? CreateWithConsole()
        {
            Meeting meeting = null; 
            try
            {
                meeting = TryCreateWithConsole();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return meeting;
        }
        
        private static Meeting TryCreateWithConsole()
        {
            Console.WriteLine("\nВведите название встречи:");
            string title = Console.ReadLine();

            Console.WriteLine("Введите дату и время встречи (в формате ГГГГ-ММ-ДД ЧЧ:ММ):");
            DateTime dateStart = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Введите продолжительность встречи (в минутах):");
            TimeSpan duration = TimeSpan.FromMinutes(double.Parse(Console.ReadLine()));

            Console.WriteLine("Введите место проведения встречи:");
            string place = Console.ReadLine();

            Console.WriteLine("Введите время за которое нужно уведомить до начала встречи (в минутах)" +
                "\nВведите 0 чтобы не уведомлять");
            double notification = double.Parse(Console.ReadLine()); 
            if(DateTime.Now >  dateStart.Subtract(TimeSpan.FromMinutes(notification)))
            {
                Console.WriteLine("Уведомить можно только в будущем!\nВремя уведомления установлено на 0");
                notification = 0;
            }
            return new Meeting(dateStart, title, place, duration, notification);
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine($"Название встречи: {Title}");
            result.AppendLine($"Дата начала встречи: {DateStart.ToString("g")}");
            result.AppendLine($"Продолжительность встречи(в минутах): {TimeDuration.TotalMinutes}");
            result.AppendLine($"Место проведения встречи: {Place}");
            result.AppendLine($"Дата окончания встречи: {DateEnd.ToString("g")}");
            if(TimeNotification != TimeSpan.Zero)
            {
                result.AppendLine($"Напомнить за {TimeNotification.Minutes} мин.");
            }
            return result.ToString();
        }
    }
}
