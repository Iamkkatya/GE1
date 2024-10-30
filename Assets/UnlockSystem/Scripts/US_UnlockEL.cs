using UnityEngine;

namespace UnlockSystem
{
    public class US_UnlockEL : MonoBehaviour
    {
        #region Attributes

        [Header("ATTRIBUTES")]
        [SerializeField] private US_PhoneScript.LevelOfDifficulty usLockOfDifficulty = US_PhoneScript.LevelOfDifficulty.EASY;
        [SerializeField] private US_UnlockSystem.USType usType = US_UnlockSystem.USType.TYPE_MECHANICAL_V1;

        [Header("REFERENCES")]
        [SerializeField] private US_Door doorReference;

        public bool isOpen { get; set; }

        #endregion

        #region FUNCTIONS

        public US_Door DoorReference
        {
            get => doorReference;
        }

        public US_PhoneScript.LevelOfDifficulty Level
        {
            get => usLockOfDifficulty;
        }

        public US_UnlockSystem.USType USType
        {
            get => usType;
        }

        #endregion
    }
}