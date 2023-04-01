using DirectumMeetings.Controllers;

namespace DirectumMeetings.Utils
{
    public class NotificationManager
    {
        private MeetingsController _meetingsController;
        private Action<string> _notificate;
        private List<string> _titleNotificated;

        public NotificationManager(MeetingsController meetingsController, Action<string> notificate)
        {
            _meetingsController = meetingsController;
            _notificate = notificate;
            _titleNotificated = new List<string>();
        }
        /// <summary>
        /// Запустить бесконечный цикл проверки уведомлений
        /// </summary>
        /// <param name="millisecondsTimeout">задержка в миллисекундах</param>
        /// <returns></returns>
        public Task RunCheckNotification(int millisecondsTimeout)
        {    
            while (true)
            {
                _meetingsController.GetAllMeetings().Where(m => m.TimeNotification != TimeSpan.Zero).ToList()
                    .ForEach(m =>
                    {
                        if(!_titleNotificated.Contains(m.Title) && DateTime.Now > m.DateStart.Subtract(m.TimeNotification))
                        {
                            _notificate?.Invoke($"{m.Title} начнётся через {m.TimeNotification.Minutes} мин");
                            _titleNotificated.Add(m.Title);
                        }
                    });
                Thread.Sleep(millisecondsTimeout);
            }
        }
        /// <summary>
        /// Добавить действие к уведомлению
        /// </summary>
        /// <param name="action"></param>
        public void SubscribeNotificAction(Action<string> action)
        {
            this._notificate += action;
        }
        /// <summary>
        /// Отписать действие
        /// </summary>
        /// <param name="action"></param>
        public void UnsubscribeNotificAction(Action<string> action)
        {
            if(action != null)
                this._notificate -= action;
        }
    }
}
