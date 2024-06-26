﻿using DirectumMeetings.Controllers;
using DirectumMeetings.FileLoad;
using DirectumMeetings.Models;
using DirectumMeetings.Notification;
using DirectumMeetings.Settings;
using DirectumMeetings.Utils;

namespace DirectumMeetings;

public class ProgramManager
{
    public static void RunProcess()
    {
        bool cancellationToken = true;
        Action<string> consoleLogger = Console.WriteLine;

        MeetingsController meetingController = new MeetingsController(consoleLogger);

        NotificationManager notificationManager = new NotificationManager(meetingController, Console.WriteLine);

        new TaskFactory().StartNew(() => { notificationManager.RunCheckNotification(1000); });

        Console.WriteLine("\nДобро пожаловать в приложение для управления личными встречами");
        while (cancellationToken)
        {
            Console.WriteLine("Выберете следующее: ");
            Console.WriteLine("1. Добавить встречу");
            Console.WriteLine("2. Изменить встречу");
            Console.WriteLine("3. Удалить встречу");
            Console.WriteLine("4. Получить все встречи");
            Console.WriteLine("5. Сохранить встречи за конкретное число в файл");
            Console.WriteLine("0. Выход ");

            int choiseNum;
            try
            {
                choiseNum = int.Parse(Console.ReadLine());
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Введите число от 0 до 5");
                continue;
            }

            switch ((UserChoise)choiseNum)
            {
                case UserChoise.AddMeeting:
                    {
                        Console.WriteLine("Заполните:");
                        Meeting meeting = Meeting.CreateWithConsole();
                        if (meeting != null)
                        {
                            meetingController.AddMeeting(meeting);
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
                        if (title != null)
                        {
                            if (meetingController.IsContainsWithTitle(title))
                            {
                                Meeting meeting = Meeting.CreateWithConsole();
                                if (meeting != null)
                                {
                                    meetingController.UpdateMeeting(title, meeting);
                                }
                                else
                                {
                                    Console.WriteLine("Некорректный ввод");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Записи с такими названием не существует");
                            }
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
                        if (title != null)
                        {
                            meetingController.RemoveMeeting(title);
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

                        foreach (var value in meetingController.GetAllMeetings())
                        {
                            Console.WriteLine(value);
                        }
                    }
                    break;
                case UserChoise.SaveMeetingsToFile:
                    {
                        Console.WriteLine("Введите дату, за которую вы хотите получить встречи: (в формате ГГГГ-ММ-ДД)");
                        DateTime dateTime;
                        if (DateTime.TryParse(Console.ReadLine(), out dateTime))
                            meetingController.SaveMeetingsToFile(dateTime,
                                new FileLoader(Path.Combine(ProgramSettings.FolderPathMeetings,
                                dateTime.Date.ToString("dddd-MMMM-yyyy") + ".txt")));
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

