using UnityEngine;

namespace UnlockSystem
{
    public class US_LootBox : MonoBehaviour
    {
        #region Attributes

        public enum LevelOfDifficulty
        {
            EASY,
            MEDIUM,
            HARD
        }

        [Header("ATTRIBUTES")]
        [SerializeField] private LevelOfDifficulty levelOfDifficulty = LevelOfDifficulty.EASY;
        [SerializeField] private US_UnlockSystem.USType usType = US_UnlockSystem.USType.TYPE_MECHANICAL_V1;

        [Header("CODE LOCK ATTRIBUTES")]
        [SerializeField] private US_CodeLockGenerator.LockType lockType = US_CodeLockGenerator.LockType.NUMBERS;
        [SerializeField] private US_Door doorReference;

        public Vector2 lockField { get; private set; } = Vector2.zero;
        public bool isOpen { get; set; } = false;

        private bool hasBeenGenerated = false; // indicates that the field of the lock has been generated

        #endregion

        #region FUNCTIONS

        public US_Door DoorReference
        {
            get => doorReference;
        }

        public US_CodeLockGenerator.LockType LockType
        {
            get => lockType;
        }

        public US_UnlockSystem.USType USType
        {
            get => usType;
        }

        public LevelOfDifficulty Level
        {
            get => levelOfDifficulty;
        }

        #endregion

        #region PUBLIC

        /// <summary>
        /// Random generate field of the lock in degrees
        /// </summary>
        public void GenerateLockField()
        {
            if (!hasBeenGenerated)
            {
                float firstSide = Random.Range(US_UnlockAction.instance.limitRotZ.x - 10.0f, US_UnlockAction.instance.limitRotZ.y + 10.0f);
                float secondSide = 0.0f;
                float direction = Random.Range(-1.0f, 1.0f);

                if (direction < 0.0f)
                {
                    secondSide = Random.Range(US_UnlockAction.instance.limitRotZ.x, firstSide);

                    if (secondSide < firstSide)
                        lockField = new Vector2(secondSide, firstSide);
                    else
                        lockField = new Vector2(firstSide, secondSide);
                }
                else if (direction > 0.0f)
                {
                    secondSide = Random.Range(firstSide, US_UnlockAction.instance.limitRotZ.y);

                    if (secondSide < firstSide)
                        lockField = new Vector2(secondSide, firstSide);
                    else
                        lockField = new Vector2(firstSide, secondSide);
                }
                else
                {
                    GenerateLockField();
                }

                Vector2 field = US_UnlockSystem.instance.GetLevelField(levelOfDifficulty);
                if (Mathf.Abs(firstSide - secondSide) < field.x || Mathf.Abs(firstSide - secondSide) > field.y)
                    GenerateLockField();

                hasBeenGenerated = true;
            }
        }

        #endregion
    }
}