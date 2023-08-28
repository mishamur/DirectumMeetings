using DirectumMeetings.Models;
using DirectumMeetings.FileLoad;

namespace DirectumMeetings.Controllers
{
    public class MeetingsController
    {
        private List<Meeting> _meetings;
        private Action<string> logger;

        public MeetingsController(ICollection<Meeting> meetings, Action<string> logger = null)
        {
            this.logger = logger;
            _meetings = meetings.ToList();
        }
        public MeetingsController(Action<string> logger = null)
        {
            this.logger = logger;
            this._meetings = new List<Meeting>();
        }
        /// <summary>
        /// Получить все встречи
        /// </summary>
        /// <returns></returns>
        public ICollection<Meeting> GetAllMeetings()
        {
            return _meetings.Select(x => x).OrderBy(m => m.DateStart).ToList();
        }
        /// <summary>
        /// Добавить встречу
        /// </summary>
        /// <param name="meeting"></param>
        public void AddMeeting(Meeting meeting)
        {
            if (meeting.DateStart < DateTime.Now)
            {
                logger?.Invoke("Встречи всегда планируются только на будущее время");
                return;
            }
            //проверка на пересечение!!!
            foreach (var value in _meetings)
            {
                if (!(value.DateStart < meeting.DateStart && value.DateEnd < meeting.DateStart ||
                value.DateStart > meeting.DateEnd))
                {
                    logger?.Invoke("встреча пересекается");
                    return;
                }
            }
            //если пересечений нет, то добавляем
            _meetings.Add(meeting);
        }
        /// <summary>
        /// Изменить встречу
        /// </summary>
        public void UpdateMeeting(string title, Meeting newMeeting)
        {
            int updateIndex = _meetings.FindIndex(m => m.Title.Equals(title));
            if (updateIndex == -1)
            {
                logger?.Invoke("Встреча не найдена");
                return;
            }
            else
            {
                _meetings.RemoveAt(updateIndex);
                AddMeeting(newMeeting);
            }
        }
        /// <summary>
        /// Удалить встречу
        /// </summary>
        /// <param name="title">Название встречи</param>
        public void RemoveMeeting(string title)
        {
            int deleteIndex = _meetings.FindIndex(m => m.Title.Equals(title));
            if (deleteIndex != -1)
                _meetings.RemoveAt(deleteIndex);
        }
        /// <summary>
        /// Удалить встречу
        /// </summary>
        /// <param name="meeting">Модель встречи</param>
        public void RemoveMeeting(Meeting meeting)
        {
            _meetings.Remove(meeting);
        }
        /// <summary>
        /// Сохранить встречи за конкретную дату в файл
        /// </summary>
        /// <param name="dateTime">Конкретная дата</param>
        public void SaveMeetingsToFile(DateTime dateTime)
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "meetings");
            string pathToFile = Path.Combine(folderPath, dateTime.Date.ToString("dddd-MMMM-yyyy") + ".txt");
           FileLoader fileLoader = new FileLoader(pathToFile);

            fileLoader.Load(string.Concat(_meetings
                .Where(x => x.DateStart.Date.Equals(dateTime))
                .Select(x => x.ToString() + "\n")
                .ToList()));

            logger?.Invoke($"Записи успешно сохранены в файл {pathToFile}");
        }
        public bool IsContainsWithTitle(string title)
        {
            return _meetings.Find(x => x.Title.Equals(title)) != null;
        }
    }
}
