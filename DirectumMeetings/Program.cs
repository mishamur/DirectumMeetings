using DirectumMeetings.Controllers;
using DirectumMeetings.Models;
using DirectumMeetings.Utils;
using System.Security.Cryptography.X509Certificates;

namespace DirectumMeetings
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool cancellationToken = true;
            Action<string> consoleLogger = Console.WriteLine;

            MeetingsController controller = new MeetingsController(consoleLogger);
            controller.AddMeeting(new Meeting(DateTime.Now, "jaba1", "Udgu", TimeSpan.FromMinutes(60)));
            controller.AddMeeting(new Meeting(DateTime.Now.AddMinutes(120), "jaba2", "Udgu", TimeSpan.FromMinutes(60)));
            
            while (cancellationToken)
            {
                Console.WriteLine("\nДобро пожаловать в приложение для управление личными встречами");
                Console.WriteLine("Выберете следующее: ");
                Console.WriteLine("1. Добавить встречу");
                Console.WriteLine("2. Изменить встречу");
                Console.WriteLine("3. Удалить встречу");
                Console.WriteLine("4. Получить все встречи");
                Console.WriteLine("5. Сохранить встречи за конкретное число в файл");
                Console.WriteLine("0. Выход ");

                int choiseNum = int.Parse(Console.ReadLine());

                switch ((UserChoise)choiseNum)
                {
                    case UserChoise.AddMeeting:
                        { 
                            Console.WriteLine("Заполните:");
                            Meeting meeting = Meeting.CreateWithConsole();
                            if(meeting != null)
                            {
                                controller.AddMeeting(meeting);
                            }
                            else
                            {
                                Console.WriteLine("Некорректный ввод");
                            }
                        }
                        break;
                    case UserChoise.UpdateMeeting:
                        {
                            Console.WriteLine("Введите название встречи которую хотите поменять: ");
                            string title = Console.ReadLine();
                            Meeting meeting = Meeting.CreateWithConsole();
                            if(title != null && meeting != null)
                            {
                                controller.UpdateMeeting(title, meeting);
                            }
                            else
                            {
                                Console.WriteLine("Некорректный ввод");
                            }
                            
                        }
                        break;
                    case UserChoise.RemoveMeeting:
                        { 
                            Console.WriteLine("Введите название встречи которую вы хотите удалить: ");
                            string title = Console.ReadLine();
                            if(title != null)
                            {
                                controller.RemoveMeeting(title);
                            }
                            else
                            {
                                Console.WriteLine("Некорректный ввод");
                            }
                            
                        }
                        break;
                    case UserChoise.GetMeetings:
                        {
                            Console.WriteLine("Получить все свои встречи");
                            
                            foreach(var value in controller.GetAllMeetings())
                            {
                                Console.WriteLine(value);
                            }
                        }
                        break;
                    case UserChoise.SaveMeetingsToFile:
                        {
                            Console.WriteLine("Введите дату, за которую вы хотите получить встречи: (в формате ГГГГ-ММ-ДД)");
                            DateTime dateTime;
                            if(DateTime.TryParse(Console.ReadLine(), out dateTime))
                                controller.SaveMeetingsToFile(dateTime);
                            else
                                Console.WriteLine("Некорректный ввод даты");
                        }
                        break;
                    case UserChoise.Exit:
                        {
                            cancellationToken = false;
                        }
                        break;
                    default:
                        Console.WriteLine("Введите число от 0 до 5");
                        break;
                }

            }
        }

    }
}