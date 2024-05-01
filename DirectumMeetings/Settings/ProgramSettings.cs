using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumMeetings.Settings
{
    public static class ProgramSettings
    {
        public static string FolderPathMeetings { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "meetings");
    }
}
