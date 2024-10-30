using UnityEngine;
using UnityEditor;

namespace UnlockSystem
{
    public class US_HelpManager : EditorWindow
    {
        [MenuItem("US/US About")]
        public static void AboutMenu()
        {
            EditorWindow.GetWindow(typeof(US_About));
        }

        [MenuItem("US/US Links")]
        public static void HelpMenu()
        {
            EditorWindow.GetWindow(typeof(US_Links));
        }

        [MenuItem("US/Help/FORUM")]
        public static void ForumMenu()
        {
            Application.OpenURL("https://forum.unity.com/threads/unlock-system-by-voo-in-progress.611881/");
        }

        [MenuItem("US/Help/PORTFOLIO")]
        public static void PORTFOLIOMenu()
        {
            Application.OpenURL("https://www.artstation.com/mynameisvoo");
        }

        [MenuItem("US/Help/YouTube")]
        public static void YoutubeMenu()
        {
            Application.OpenURL("https://www.youtube.com/channel/UCPwDyeLdS0Am046NhRjjwYg");
        }
    }
}
