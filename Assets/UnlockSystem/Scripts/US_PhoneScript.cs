using UnityEngine;
using UnityEngine.UI;

namespace UnlockSystem
{
    public class US_PhoneScript : MonoBehaviour
    {
        #region Attributes

        public const int MAX_CODE_NUMBERS = 10;

        public enum LevelOfDifficulty
        {
            EASY,
            MEDIUM,
            HARD
        }

        [Header("ATTRIBUTES")]
        [SerializeField] private GameObject mRenderScenePP; // The code render scene
        [SerializeField] private GameObject textArea;
        [SerializeField] private GameObject loadingIcons;
        [SerializeField] private GameObject buttonScan; // Activation scan button

        [Header("ICONS")]
        [SerializeField] private GameObject timerMiliSeconds;
        [SerializeField] private GameObject timerSeconds;
        [SerializeField] private GameObject loadingArrow;
        [SerializeField] private GameObject loadingPersent;

        [Header("CODE BUTTON")]
        [SerializeField] private GameObject codeBoxRoot;
        [SerializeField] private Sprite[] codeBoxes = new Sprite[MAX_CODE_NUMBERS];
        [SerializeField] private GameObject[] codeBoxLevels = new GameObject[3];
        [SerializeField] private Vector2[] PositionCodeBoxInScreen = new Vector2[MAX_CODE_NUMBERS]; // Starting position (not random)   

        [Header("CODE BUTTON PREFABS")]
        [SerializeField] private GameObject codeBoxPrefab;

        public bool canStartScan { get; set; }
        public bool canStartInput { get; set; }
        public bool canActiveMoveCodeButtons { get; set; }
        public bool weLost { get; set; }
        public bool hasBeenOpened { get; private set; } = true; // We opened the window
        public int[] codeBoxButtonPresses { get; private set; } // After press a button we will save it for checking
        public int countButtonPresses { get; set; } = 0;

        private GameObject[] codeBoxesReferences;
        private Vector2[] vspPositionCodeBoxInScreen = new Vector2[MAX_CODE_NUMBERS];

        private bool codeHasBeenGenerated = false;

        private int levelCode = 0; // Level of dificulty
        private LevelOfDifficulty level = 0;  // The level - number
        private int[] code;
        private int[] vspCodeBoxes;

        private float timer = 0.0f;

        public static US_PhoneScript instance { get; private set; }

        #endregion

        private void Start()
        {
            instance = this;

            timerSeconds.GetComponent<US_SearchLootIcon>().isPaused = true;
            timerMiliSeconds.GetComponent<US_SearchLootIcon>().isPaused = true;

            // Copy button positions
            for (int i = 0; i < MAX_CODE_NUMBERS; i++)
                vspPositionCodeBoxInScreen[i] = PositionCodeBoxInScreen[i];

            SetCodeLevel();
            GenerateCode();

            codeBoxesReferences = new GameObject[levelCode];
            codeBoxButtonPresses = new int[levelCode];

            SpawnCodeBox(); // Create code boxes (buttons) on the screen

            hasBeenOpened = false;
        }

        private void Update()
        {
            if (!weLost) // If we did not lose - we continue play
            {
                if (hasBeenOpened) // If we opened
                {
                    timerSeconds.GetComponent<US_SearchLootIcon>().isPaused = true;
                    timerMiliSeconds.GetComponent<US_SearchLootIcon>().isPaused = true;

                    for (int i = 0; i < 10; i++)
                        vspPositionCodeBoxInScreen[i] = PositionCodeBoxInScreen[i];

                    SetCodeLevel();
                    GenerateCode();

                    codeBoxesReferences = new GameObject[levelCode];
                    codeBoxButtonPresses = new int[levelCode];
                    //LineInScreen = new GameObject[LevelCode];

                    SpawnCodeBox();

                    hasBeenOpened = false;
                }
                else
                {
                    if (!canStartInput) // Можно вводить код? нет нельзя загрузка системы
                    {
                        if (timer >= 1.0f) // Ждем 1 секунду и после включаем "загрузку" системы - включаем иконку
                        {
                            loadingIcons.SetActive(true);
                        }
                        timer += Time.deltaTime;
                    }
                    else // We loaded the scene - inpit code
                    {
                        if (!codeHasBeenGenerated) //If we did not generate code - generate code
                        {
                            codeBoxRoot.SetActive(true);
                            RandomCodeSpawn();

                            timerSeconds.GetComponent<US_SearchLootIcon>().isPaused = false;
                            timerMiliSeconds.GetComponent<US_SearchLootIcon>().isPaused = false;
                        }

                        canActiveMoveCodeButtons = true; // разрешаем дергание кнопок

                        if (countButtonPresses == levelCode) // Проверка кода - после того, как все кнопки нажаты мы сверяем получившееся число и кодом
                        {
                            CheckCode(); // Checking code
                        }
                    }
                }
            }
            else // Мы проиграли
            {
                timerSeconds.GetComponent<US_SearchLootIcon>().PauseAction();
                timerMiliSeconds.GetComponent<US_SearchLootIcon>().PauseAction();

                for (int i = 0; i < levelCode; i++)
                {
                    codeBoxesReferences[i].GetComponent<US_UpdatePositionCodeBox>().LossAction();
                }

                StartCoroutine(US_Menu.instance.ExitUnlockAreaWaitForSeconds(3.0f)); // Closing window after 3 seconds
            }
        }

        #region PUBLIC

        #region Button

        public void ActionStart(bool state)
        {
            if (state && canStartScan)
            {
                textArea.SetActive(true);
                mRenderScenePP.SetActive(true);
                buttonScan.GetComponent<US_ButtonScan>().StopPulse();
                canStartScan = false;
            }
        }

        #endregion

        public void CloseELScript()
        {
            hasBeenOpened = true;
            timer = 0.0f;
            codeHasBeenGenerated = false;

            ResetButtons();

            weLost = false;
            canStartScan = false;
            canStartInput = false;
            canActiveMoveCodeButtons = false;

            codeBoxRoot.SetActive(false);
            loadingIcons.SetActive(false);

            buttonScan.GetComponent<US_ButtonScan>().StopPulse();
            buttonScan.GetComponent<Button>().enabled = false;

            mRenderScenePP.GetComponent<US_CodeScreen>().ResetCodeScreen();
            mRenderScenePP.SetActive(false);
            textArea.SetActive(false);

            for (int i = 0; i < levelCode; i++)
                Destroy(codeBoxesReferences[i]);
        }

        #endregion

        #region PRIVATE

        private void CheckCode()
        {
            int matches = 0;
            for (int i = 0; i < levelCode; i++)
            {
                if (codeBoxButtonPresses[i] == code[i])
                    matches++;
            }

            if (matches == levelCode) // WIN
                InputAction();
            else
                ResetButtons();
        }

        private void ResetButtons()
        {
            for (int i = 0; i < levelCode; i++)
            {
                codeBoxesReferences[i].GetComponent<US_UpdatePositionCodeBox>().ResetAction();
                codeBoxButtonPresses[i] = 0;
            }

            countButtonPresses = 0;
        }

        private void GenerateCode()
        {
            code = new int[levelCode];
            for (int i = 0; i < levelCode; i++)
            {
                int codeNumber = Mathf.RoundToInt(Random.Range(0, MAX_CODE_NUMBERS));
                code[i] = codeNumber;
            }
        }

        private void SpawnCodeBox()
        {
            for (int i = 0; i < levelCode; i++) 
            {
                codeBoxesReferences[i] = Instantiate(codeBoxPrefab);
                codeBoxesReferences[i].transform.SetParent(codeBoxLevels[level.GetHashCode()].transform);

                int k = 0; // Для рандомного определения позиции
                bool finish = false; // флаг выхода из цикла

                // Цикл необходим, чтобы кнопки не вставали друг на друга (т.е. не получили одну и туже позицию) они должны быть разные
                while (!finish)
                {
                    k = Mathf.RoundToInt(Random.Range(0, MAX_CODE_NUMBERS));

                    if (vspPositionCodeBoxInScreen[k].x != -1.0f) // -1 означает, что позиция занята
                        finish = true;
                }

                codeBoxesReferences[i].transform.localScale = Vector3.one;
                codeBoxesReferences[i].transform.localEulerAngles = Vector3.zero;
                codeBoxesReferences[i].transform.localPosition = new Vector3(vspPositionCodeBoxInScreen[k].x, vspPositionCodeBoxInScreen[k].y, 0.0f);

                vspPositionCodeBoxInScreen[k].x = vspPositionCodeBoxInScreen[k].y = -1.0f; // Position is closed

                codeBoxesReferences[i].GetComponentInChildren<Image>().sprite = codeBoxes[code[i]];

                codeBoxesReferences[i].GetComponent<US_UpdatePositionCodeBox>().codeNumber = code[i]; // Присваиваем кнопке код (цифру)
                codeBoxesReferences[i].GetComponent<US_UpdatePositionCodeBox>().defaultPosition = codeBoxesReferences[i].transform.localPosition;
            }
        }

        private void RandomCodeSpawn()
        {
            int codeIndex = 0;
            bool completed = false;
            vspCodeBoxes = new int[levelCode];

            int i = 0;
            while (!completed)
            {
                if (i == levelCode)
                {
                    completed = true;
                    break;
                }

                codeIndex = Mathf.RoundToInt(Random.Range(0, MAX_CODE_NUMBERS));
                vspCodeBoxes[i] = codeIndex;
                i++;
            }

            codeHasBeenGenerated = true;
        }

        private void InputAction()
        {
            canStartScan = true; // Разрешить сканирование - большая красная кнопка активируется

            // Активируем ее
            buttonScan.GetComponent<Button>().enabled = true;
            buttonScan.GetComponent<US_ButtonScan>().isActivePulsing = true;

            timerSeconds.GetComponent<US_SearchLootIcon>().isPaused = true;
            timerMiliSeconds.GetComponent<US_SearchLootIcon>().isPaused = true;
        }

        private void SetCodeLevel()
        {
            if (US_UnlockSystem.instance.usLevelOfDifficulty == LevelOfDifficulty.EASY)
                levelCode = 3; // 3 numberns in the code
            else if (US_UnlockSystem.instance.usLevelOfDifficulty == LevelOfDifficulty.MEDIUM)
                levelCode = 4;
            else if (US_UnlockSystem.instance.usLevelOfDifficulty == LevelOfDifficulty.HARD)
                levelCode = 5;

            level = LevelOfDifficulty.EASY;
        }

        #endregion
    }
}