using UnityEngine;
using UnityEditor;

namespace UnlockSystem
{
    //[CustomEditor(typeof(US_UnlockSystem))]
    //[CanEditMultipleObjects]
    public class ICWM_InventorBase_Editor
    {
        /*
        private GUISkin skin;
        private Texture2D m_Logo = null;
        private Texture2D m_MiniLogo = null;

        private SerializedProperty DEBUG_MODE;
        private SerializedProperty hideStash;

        private SerializedProperty IsPauseWorld;
        private SerializedProperty DeleteAllSavedFiles;
        private SerializedProperty ActivateDynamicAttachingMods;
        private SerializedProperty activateModdingFromEnemySlots;
        private SerializedProperty NoPlaceMode;
        private SerializedProperty LimitedWindows;

        private SerializedProperty a_AudioSelected;
        private SerializedProperty StartColor;
        private SerializedProperty NoPlaceColor;

        private SerializedProperty miniMenuStyle;
        private SerializedProperty WaitForDestroy;

        private SerializedProperty Is1stPerson;
        private SerializedProperty m_Player;
        private SerializedProperty m_Player3rdCharacter;

        private SerializedProperty RE4_amountOfBullet;

        private SerializedProperty PlayerStatistics;
        private SerializedProperty toolTip;
        private SerializedProperty m_Hotbar;
        private SerializedProperty inventoryTags;

        private bool attributes, selection, minimenu, player, RE4, statistics, tooltip, hotbar, tags;

        void OnEnable()
        {
            m_Logo = (Texture2D)Resources.Load("Logo2x1", typeof(Texture2D)); // Загружаем логотип
            m_MiniLogo = (Texture2D)Resources.Load("LogoMini4x", typeof(Texture2D));

            DEBUG_MODE = serializedObject.FindProperty("DEBUG_MODE");
            hideStash = serializedObject.FindProperty("hideStash");

            IsPauseWorld = serializedObject.FindProperty("IsPauseWorld");
            DeleteAllSavedFiles = serializedObject.FindProperty("DeleteAllSavedFiles");
            ActivateDynamicAttachingMods = serializedObject.FindProperty("ActivateDynamicAttachingMods");
            activateModdingFromEnemySlots = serializedObject.FindProperty("activateModdingFromEnemySlots");
            NoPlaceMode = serializedObject.FindProperty("NoPlaceMode");
            LimitedWindows = serializedObject.FindProperty("LimitedWindows");

            a_AudioSelected = serializedObject.FindProperty("a_AudioSelected");
            StartColor = serializedObject.FindProperty("StartColor");
            NoPlaceColor = serializedObject.FindProperty("NoPlaceColor");

            miniMenuStyle = serializedObject.FindProperty("miniMenuStyle");
            WaitForDestroy = serializedObject.FindProperty("WaitForDestroy");

            Is1stPerson = serializedObject.FindProperty("Is1stPerson");
            m_Player = serializedObject.FindProperty("m_Player");
            m_Player3rdCharacter = serializedObject.FindProperty("m_Player3rdCharacter");

            RE4_amountOfBullet = serializedObject.FindProperty("RE4_amountOfBullet");

            PlayerStatistics = serializedObject.FindProperty("PlayerStatistics");
            toolTip = serializedObject.FindProperty("toolTip");
            m_Hotbar = serializedObject.FindProperty("m_Hotbar");
            inventoryTags = serializedObject.FindProperty("inventoryTags");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (!skin) skin = Resources.Load("ICWMskin") as GUISkin;
            GUI.skin = skin;

            GUILayout.BeginVertical("*INVENTORY DATABASE*", "window");
            GUILayout.Label(m_Logo, GUILayout.MaxWidth(360), GUILayout.MaxHeight(170));

            EditorGUILayout.HelpBox("Main script", MessageType.Info);

            GUILayout.BeginVertical("highlightBox");
            EditorGUILayout.LabelField("DEBUG for developer", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(DEBUG_MODE, new GUIContent("Activate DEBUG MODE"));
            if (hideStash.boolValue)
                EditorGUILayout.PropertyField(hideStash, new GUIContent("Unhide Stash"));
            else
                EditorGUILayout.PropertyField(hideStash, new GUIContent("Hide Stash"));
            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("highlightBox");
            EditorGUILayout.LabelField("ATTRIBUTES", EditorStyles.boldLabel);
            attributes = GUILayout.Toggle(attributes, attributes ? "Close attributes" : "Open attributes", EditorStyles.miniPullDown);
            if (attributes)
            {
                if (IsPauseWorld.boolValue)
                    EditorGUILayout.PropertyField(IsPauseWorld, new GUIContent("Activate Pause World"));
                else
                    EditorGUILayout.PropertyField(IsPauseWorld, new GUIContent("Deactivate Pause World"));
                EditorGUILayout.PropertyField(DeleteAllSavedFiles, new GUIContent("Delete All Saved Files"));
                if (ActivateDynamicAttachingMods.boolValue)
                    EditorGUILayout.PropertyField(ActivateDynamicAttachingMods, new GUIContent("Activate Dynamic Attaching Mods"));
                else
                    EditorGUILayout.PropertyField(ActivateDynamicAttachingMods, new GUIContent("Deactivate Dynamic Attaching Mods"));
                if (activateModdingFromEnemySlots.boolValue)
                    EditorGUILayout.PropertyField(activateModdingFromEnemySlots, new GUIContent("Activate Modding From Enemy Slots"));
                else
                    EditorGUILayout.PropertyField(activateModdingFromEnemySlots, new GUIContent("Deactivate Modding From Enemy Slots"));

                EditorGUILayout.PropertyField(NoPlaceMode, new GUIContent("No Place Mode"));
                EditorGUILayout.PropertyField(LimitedWindows, new GUIContent("Limit Opened Windows"));
            }
            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("highlightBox");
            EditorGUILayout.LabelField("SELECTION ICON", EditorStyles.boldLabel);
            selection = GUILayout.Toggle(selection, selection ? "Close selection parameters" : "Open selection parameters", EditorStyles.miniPullDown);
            if (selection)
            {
                EditorGUILayout.PropertyField(a_AudioSelected, new GUIContent("Audio Selected"));
                EditorGUILayout.PropertyField(StartColor, new GUIContent("Pointing Color"));
                EditorGUILayout.PropertyField(NoPlaceColor, new GUIContent("No Place Color"));
            }
            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("highlightBox");
            EditorGUILayout.LabelField("MINI-MENU", EditorStyles.boldLabel);
            minimenu = GUILayout.Toggle(minimenu, minimenu ? "Close mini-menu parameters" : "Open mini-menu parameters", EditorStyles.miniPullDown);
            if (minimenu)
            {
                EditorGUILayout.PropertyField(miniMenuStyle, new GUIContent("Mini-Menu Style"));
                EditorGUILayout.PropertyField(WaitForDestroy, new GUIContent("Wait For Destroy"));
            }
            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("highlightBox");
            EditorGUILayout.LabelField("PLAYER", EditorStyles.boldLabel);
            player = GUILayout.Toggle(player, player ? "Close player parameters" : "Open player parameters", EditorStyles.miniPullDown);
            if (player)
            {
                EditorGUILayout.PropertyField(Is1stPerson, new GUIContent("Is FPS Player"));
                if (Is1stPerson.boolValue)
                    EditorGUILayout.PropertyField(m_Player, new GUIContent("FPS Player"));
                else
                    EditorGUILayout.PropertyField(m_Player3rdCharacter, new GUIContent("TPC Player"));
            }
            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("highlightBox");
            EditorGUILayout.LabelField("RE4", EditorStyles.boldLabel);
            RE4 = GUILayout.Toggle(RE4, RE4 ? "Close RE4 parameters" : "Open RE4 parameters", EditorStyles.miniPullDown);
            if (RE4)
            {
                EditorGUILayout.PropertyField(RE4_amountOfBullet, new GUIContent("RE4 Amount Of Bullet"));
            }
            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("highlightBox");
            EditorGUILayout.LabelField("STATISTICS", EditorStyles.boldLabel);
            statistics = GUILayout.Toggle(statistics, statistics ? "Close statistics" : "Open statistics", EditorStyles.miniPullDown);
            if (statistics)
            {
                EditorGUILayout.PropertyField(PlayerStatistics, true);
            }
            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("highlightBox");
            EditorGUILayout.LabelField("TOOLTIP", EditorStyles.boldLabel);
            tooltip = GUILayout.Toggle(tooltip, tooltip ? "Close tooltip parameters" : "Open tooltip parameters", EditorStyles.miniPullDown);
            if (tooltip)
            {
                EditorGUILayout.PropertyField(toolTip, true);
            }
            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("highlightBox");
            EditorGUILayout.LabelField("HOTBAR", EditorStyles.boldLabel);
            hotbar = GUILayout.Toggle(hotbar, hotbar ? "Close hotbar parameters" : "Open hotbar parameters", EditorStyles.miniPullDown);
            if (hotbar)
            {
                EditorGUILayout.PropertyField(m_Hotbar, true);
            }
            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical("highlightBox");
            EditorGUILayout.LabelField("TAGS", EditorStyles.boldLabel);
            tags = GUILayout.Toggle(tags, tags ? "Close tags parameters" : "Open tags parameters", EditorStyles.miniPullDown);
            if (tags)
            {
                EditorGUILayout.PropertyField(inventoryTags, true);
            }
            EditorGUILayout.EndVertical();

            //base.OnInspectorGUI();

            EditorGUILayout.BeginVertical();

            GUILayout.EndVertical();
            GUILayout.Label(m_MiniLogo, GUILayout.MaxWidth(360), GUILayout.MaxHeight(50));
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            serializedObject.ApplyModifiedProperties();
        }
        */
    }
}