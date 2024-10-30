using UnityEngine;
using UnityEditor;

namespace UnlockSystem
{
    public class US_About : EditorWindow
    {
        GUISkin skin;
        private Texture2D m_Logo = null;
        private Texture2D m_MiniLogo = null;
        Vector2 rect = new Vector2(410, 510); // Размеры окна
        private Vector2 scrollPosition;

        void OnEnable()
        {
            m_Logo = (Texture2D)Resources.Load("US_logo", typeof(Texture2D)); // Загружаем логотип
            m_MiniLogo = (Texture2D)Resources.Load("LogoMini", typeof(Texture2D));
        }

        void OnGUI()
        {
            this.titleContent = new GUIContent("US: About");
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
            if (GUILayout.Button("Asset"))
            {
                UnityEditorInternal.AssetStore.Open("/content/89433");
                this.Close();
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.HelpBox("US: \n\n *ALWAYS BACKUP YOUR PROJECT BEFORE UPDATE!* \n\n US - The Unlock System is present in almost all games, be it a hacking box with valuable loot or a hacking door to the next level. In most cases, these systems are all primitive - “press the button and go” or “get the key and open the door”. This system is interactive, i.e.the player must interact with it, enter a random code or rotate the latchkey or even spin the drums, one way or another the player must be present and turn on the logic. ", MessageType.None);

            //
            //
            //

            GUILayout.Box(m_MiniLogo, GUILayout.MaxHeight(50));
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }
    }
}
