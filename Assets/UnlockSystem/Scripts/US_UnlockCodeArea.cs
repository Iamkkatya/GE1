using UnityEngine;

namespace UnlockSystem
{
    public class US_UnlockCodeArea : MonoBehaviour
    {
        [Header("REFERENCES")]
        [SerializeField] private US_CodeLockGenerator lockGenerator;

        public static US_UnlockCodeArea instance { get; private set; }

        private void OnEnable()
        {
            if (US_UnlockSystem.instance.LootBoxReference.Level == US_LootBox.LevelOfDifficulty.EASY)
                lockGenerator.lockLevel = US_CodeLockGenerator.LockLevel.Level_3;
            else if (US_UnlockSystem.instance.LootBoxReference.Level == US_LootBox.LevelOfDifficulty.MEDIUM)
                lockGenerator.lockLevel = US_CodeLockGenerator.LockLevel.Level_4;
            else if (US_UnlockSystem.instance.LootBoxReference.Level == US_LootBox.LevelOfDifficulty.HARD)
                lockGenerator.lockLevel = US_CodeLockGenerator.LockLevel.Level_5;

            lockGenerator.lockType = US_UnlockSystem.instance.LootBoxReference.LockType;
        }

        private void Start()
        {
            instance = this;
        }
    }
}