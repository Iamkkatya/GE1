using UnityEngine;

namespace UnlockSystem
{
    public class US_UnlockSystem : MonoBehaviour
    {
        #region Attributes

        public const string VERSION = "1.1.0.0";

        public enum USType
        {
            NONE,
            TYPE_MECHANICAL_V1,
            TYPE_MECHANICAL_V2,
            TYPE_ELECTRONIC,
            TYPE_CODE_LOCK,
        }

        [Header("ATTRIBUTES")]
        [SerializeField] private GameObject rootUS;
        [SerializeField] private UnlockSystemTypeSettings[] unlockSystemTypeSettings;

        [Header("KEYS")]
        [SerializeField] private KeyCode keyCodeMoveLatchkey = KeyCode.W;

        [Header("LEVELs FIELDs")]
        [SerializeField] private LevelFieldSettings[] levelFieldSettings;

        public US_PhoneScript.LevelOfDifficulty usLevelOfDifficulty { get; set; }
        public USType currentActiveLockType { get; set; }
        public bool isActive { get; private set; }

        private US_UnlockEL elBoxReference;
        private US_LootBox lootBoxReference;
        private US_Door doorReference;

        public static US_UnlockSystem instance { get; private set; }

        #endregion

        private void Start()
        {
            instance = this;
        }

        #region FUNCTIONS

        public KeyCode KeyCodeMoveLatchkey
        {
            get => keyCodeMoveLatchkey;
        }

        public US_UnlockEL ELBoxReference
        {
            get => elBoxReference;
            set
            {
                if (value != null)
                    elBoxReference = value;
            }
        }

        public US_LootBox LootBoxReference
        {
            get => lootBoxReference;
            set
            {
                if (value != null)
                    lootBoxReference = value;
            }
        }

        public US_Door DoorReference
        {
            get => doorReference;
            set
            {
                if (value != null)
                    doorReference = value;
            }
        }

        #endregion

        #region PUBLIC

        public void ActivateUnlockArea(bool isActive)
        {
            this.isActive = isActive;

            if (currentActiveLockType == USType.TYPE_MECHANICAL_V1 || currentActiveLockType == USType.TYPE_MECHANICAL_V2)
            {
                if (isActive)
                {
                    US_Test.instance.SetActiveIconAmountMultitools(true);
                    US_Test.instance.SetIconAmountMultitoolsText("Multitools\n" + US_PlayerItems.instance.AmountMultools.ToString());
                }
                else
                    US_UnlockAction.instance.CloseUnlockArea();

                US_Menu.instance.SetActiveHelperMenuLevelRoot_0(isActive);
            }
            else if (currentActiveLockType == USType.TYPE_ELECTRONIC)
            {
                US_Menu.instance.SetActiveHelperMenuLevelRoot_1(isActive);

                if (!isActive)
                    US_PhoneScript.instance.CloseELScript();
            }

            SetActiveRoot(currentActiveLockType, isActive);

            if (isActive)
                US_Menu.instance.PauseGame(true);
            else
                US_Test.instance.SetActiveIconAmountMultitools(false);

            rootUS.SetActive(isActive);
        }

        public Vector2 GetLevelField(US_LootBox.LevelOfDifficulty levelOfDifficulty)
        {
            if (unlockSystemTypeSettings != null && unlockSystemTypeSettings.Length != 0)
            {
                foreach(LevelFieldSettings level in levelFieldSettings)
                {
                    if (level.levelOfDifficulty == levelOfDifficulty)
                        return level.field;
                }
            }

            return Vector2.zero;
        }

        #endregion

        #region PRIVATE

        private void SetActiveRoot(USType usType, bool isActive)
        {
            if (unlockSystemTypeSettings == null || unlockSystemTypeSettings.Length == 0)
                return;

            foreach (UnlockSystemTypeSettings unlock in unlockSystemTypeSettings)
            {
                if (unlock.usType == usType)
                {
                    unlock.usTypeRoot.SetActive(isActive);
                    return;
                }
            }
        }

        #endregion

        [System.Serializable]
        public struct UnlockSystemTypeSettings
        {
            public USType usType;
            public GameObject usTypeRoot;
        }

        [System.Serializable]
        public struct LevelFieldSettings
        {
            public US_LootBox.LevelOfDifficulty levelOfDifficulty;
            public Vector2 field;
        }
    }
}