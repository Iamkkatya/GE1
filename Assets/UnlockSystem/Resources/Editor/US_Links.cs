using UnityEngine;
using UnityEditor;

namespace UnlockSystem
{
    public class US_Links : EditorWindow
    {
        GUISkin skin;
        private Texture2D m_Logo = null;
        private Texture2D m_MiniLogo = null;
        Vector2 rect = new Vector2(410, 760); // Размеры окна
        private Vector2 scrollPosition;

        void OnEnable()
        {
            m_Logo = (Texture2D)Resources.Load("US_logo", typeof(Texture2D)); // Загружаем логотип
            m_MiniLogo = (Texture2D)Resources.Load("LogoMini", typeof(Texture2D));
        }

        void OnGUI()
        {
            this.titleContent = new GUIContent("US: Links");
            this.minSize = rect;

            GUILayout.Label(m_Logo, GUILayout.MaxHeight(260)); // Размеры логотипа

            if (!skin) skin = Resources.Load("USskin") as GUISkin;
            GUI.skin = skin;

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            GUILayout.BeginVertical("window");

            //
            //
            //

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("US - Unlock System: " + US_UnlockSystem.VERSION, EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Check Update"))
            {
                UnityEditorInternal.AssetStore.Open("/content/137588");
                this.Close();
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.HelpBox("UPDATE INSTRUCTIONS: \n\n *ALWAYS BACKUP YOUR PROJECT BEFORE UPDATE!* \n\n Delete the US's Folder from the Project before import the new version", MessageType.Info);
            EditorGUILayout.Space();

            //
            //
            //

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Unity Forum (Eng)", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("FORUM"))
            {
                Application.OpenURL("https://forum.unity.com/threads/unlock-system-by-voo-in-progress.611881/");
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.HelpBox("INFORMATION: \n\n Discussion, Update Information, Video, Image, Spoilers and etc", MessageType.None);
            EditorGUILayout.Space();

            //
            //
            //

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Portfolio - MyNameIsVoo", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("PORTFOLIO"))
            {
                Application.OpenURL("https://www.artstation.com/mynameisvoo");
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.HelpBox("INFORMATION: \n\n *This is MY creation!*", MessageType.None);
            EditorGUILayout.Space();

            //
            //
            //

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("YouTube channel", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("YouTube"))
            {
                Application.OpenURL("https://www.youtube.com/channel/UCPwDyeLdS0Am046NhRjjwYg");
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.HelpBox("INFORMATION: \n\n Updates, Tutorials, How to... and etc.", MessageType.None);
            EditorGUILayout.Space();

            //
            //
            //

            GUILayout.Box(m_MiniLogo, GUILayout.MaxHeight(50));
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }
    }
}
