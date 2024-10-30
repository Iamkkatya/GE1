using System.Collections;
using UnityEngine;

namespace UnlockSystem
{
    public class US_CodeLockGenerator : MonoBehaviour
    {
        #region Attributes

        public enum LockType
        {
            NONE,
            NUMBERS,
            WORDS
        }

        public enum LockLevel
        {
            Level_3 = 3, // 3 drums
            Level_4 = 4, // 4 drums
            Level_5 = 5, // 5 drums
        }

        [Header("ATTRIBUTES")]
        [SerializeField] private bool randomLevel = false;
        [SerializeField] private float waitForSecondsAfterWin = 3f;

        [Header("ROOTS")]
        [SerializeField] private GameObject pinPivotPoint;
        [SerializeField] private GameObject housingRight;
        [SerializeField] private GameObject drumPivotPoint;

        [Header("PREFABS")]
        [SerializeField] private GameObject pinPrefab;
        [SerializeField] private GameObject drumNumbersPrefab;
        [SerializeField] private GameObject drumWordsPrefab;

        public LockType lockType { get; set; } = LockType.NUMBERS;
        public LockLevel lockLevel { get; set; } = LockLevel.Level_3;
        public bool isWin { get; private set; }

        private float offsetZ = 0.085f;
        private int amountCodes = 0;
        private int[] codes;
        private GameObject[] drums, pins;

        #endregion

        private void Start()
        {
            SetAmountCodes();

            GenerateCode();
            GenerateDrums();
            GeneratePins();

            SetHousingPosition();
        }

        #region FUNCTIONS

        public bool RandomLevel
        {
            get => randomLevel;
            set => randomLevel = value;
        }

        #endregion

        #region PUBLIC

        /// <summary>
        /// Checking input code for correct input
        /// </summary>
        public void CheckCode()
        {
            int[] vspCode = new int[amountCodes];

            for (int i = 0; i < amountCodes; i++)
                vspCode[i] = drums[i].GetComponent<US_DrumAction>().countPress;

            int matches = 0;
            for (int i = 0; i < amountCodes; i++)
            {
                if (codes[i] == vspCode[i])
                    matches++;
            }

            if (matches == amountCodes)
            {
                Debug.Log("WIN");
                isWin = true;
                StartCoroutine(WinAction());
            }
        }

        /// <summary>
        /// Check one code number if it input correctly
        /// </summary>
        /// <param name="code">the code number</param>
        /// <param name="id">the code id</param>
        /// <returns></returns>
        public bool CheckOneShotCode(int code, int id)
        {
            if (codes[id] == code)
                return true;

            return false;
        }

        #endregion

        #region PRIVATE

        private IEnumerator WinAction()
        {
            if (isWin)
                US_UnlockSystem.instance.LootBoxReference.isOpen = true;

            US_Test.instance.SetActiveIconUnlockClose(false);

            yield return new WaitForSeconds(waitForSecondsAfterWin);

            CloseUnlockArea();

            US_Menu.instance.ExitUnlockArea();

            //
            // Actions - place here you actions/events
            //

            if (US_Menu.instance.IsDemoScene) // Open Door
            {
                US_UnlockSystem.instance.DoorReference.GetComponent<US_Door>().hasBeenUnlocked = true;
            }
        }

        #region Generation

        private void GenerateCode()
        {
            codes = new int[amountCodes];

            for (int i = 0; i < amountCodes; i++)
            {
                if (lockType == LockType.NUMBERS)
                    codes[i] = Mathf.FloorToInt(Random.Range(0, 10)); // 0 - 9
                else if (lockType == LockType.WORDS)
                    codes[i] = Mathf.FloorToInt(Random.Range(0, 8)); // A - H (0 - 7)
            }
        }

        private void GenerateDrums()
        {
            drums = new GameObject[amountCodes];

            for (int i = 0; i < amountCodes; i++)
            {
                if (lockType == LockType.NUMBERS)
                    drums[i] = Instantiate(drumNumbersPrefab);
                else if (lockType == LockType.WORDS)
                    drums[i] = Instantiate(drumWordsPrefab);

                drums[i].transform.SetParent(drumPivotPoint.transform);
                drums[i].transform.localScale = Vector3.one;
                drums[i].transform.localRotation = Quaternion.Euler(Vector3.zero);
                drums[i].transform.localPosition = new Vector3(0.0f, 0.0f, -(float)(offsetZ * i));

                drums[i].GetComponent<US_DrumAction>().InitCode(this, codes[i], i, lockType);
            }
        }

        private void GeneratePins()
        {
            pins = new GameObject[amountCodes];

            for (int i = 0; i < amountCodes; i++)
            {
                pins[i] = Instantiate(pinPrefab);

                pins[i].transform.SetParent(pinPivotPoint.transform);
                pins[i].transform.localScale = Vector3.one;
                pins[i].transform.localRotation = Quaternion.Euler(Vector3.zero);
                pins[i].transform.localPosition = new Vector3(0.0f, 0.0f, (float)(offsetZ * i));
            }
        }

        #endregion

        #region Get, Set

        private LockType GetRandomDrumType()
        {
            return (LockType)((int)Mathf.FloorToInt(Random.Range(1f, 2.9f)));
        }

        private LockLevel GetRandomLevel()
        {
            return (LockLevel)((int)Mathf.FloorToInt(Random.Range(3f, 5.9f)));
        }

        private void SetAmountCodes()
        {
            if (!randomLevel)
                amountCodes = lockLevel.GetHashCode();
            else
                amountCodes = GetRandomLevel().GetHashCode();
        }

        private void SetHousingPosition()
        {
            float posZ = housingRight.transform.localPosition.z + (float)offsetZ * (amountCodes - 1);
            housingRight.transform.localPosition = new Vector3(0.0f, 0.0f, posZ);
        }

        #endregion

        #region Close

        private void CloseUnlockArea()
        {
            for (int i = 0; i < amountCodes; i++)
            {
                codes[i] = -1;
                Destroy(pins[i]);
                Destroy(drums[i]);
            }

            pins = null;
            drums = null;
            isWin = false;
            amountCodes = 0;
        }

        #endregion

        #endregion
    }
}