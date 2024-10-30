using UnityEngine;
using System.Collections;

namespace UnlockSystem
{
    public class US_Menu : MonoBehaviour
    {
        #region Attributes

        [Header("ATTRIBUTES")]
        [SerializeField] private bool lockCursor = true;
        [SerializeField] private bool isDemoScene = false;

        [Header("KEYS")]
        [SerializeField] private KeyCode keyCodeOpenMenu = KeyCode.Escape;

        [Header("REFERENCES")]
        [SerializeField] private GameObject mainMenuRoot;
        [SerializeField] private GameObject helperMenuLevelRoot_0;
        [SerializeField] private GameObject helperMenuLevelRoot_1;

        [Header("SCENE REFERENCES")]
        [SerializeField] private GameObject m_HUD;
        [SerializeField] private GameObject m_Player;

        public bool isPauseMenu { get; private set; }
        public bool isPauseUnlock { get; private set; }
        public bool isActiveOptionsMenu { get; private set; }

        private float timer = 1f;
        private bool isCursorLocked = true;

        public static US_Menu instance { get; private set; }

        #endregion

        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            if (Input.GetKeyDown(keyCodeOpenMenu))
            {
                if (isPauseMenu)
                {
                    ExitUnlockArea();
                }
                else
                {
                    if (isPauseUnlock)
                        PauseGame(false);
                    else
                        isPauseMenu = true;
                }
            }

            if (isPauseMenu == true)
            {
                timer = 0f;
                if (!isActiveOptionsMenu)
                    mainMenuRoot.SetActive(true);
                m_HUD.SetActive(false);
                if (m_Player && m_Player.GetComponent<US_FPSController>())
                    m_Player.GetComponent<US_FPSController>().enabled = false;
            }
            else if (!isPauseUnlock)
            {
                timer = 1f;
                mainMenuRoot.SetActive(false);
                m_HUD.SetActive(true);
                if (m_Player && m_Player.GetComponent<US_FPSController>())
                    m_Player.GetComponent<US_FPSController>().enabled = true;
            }

            Time.timeScale = timer;

            if (!isPauseUnlock)
                UpdateCursorLock();
        }

        #region FUNCTIONS

        public bool IsDemoScene
        {
            get => isDemoScene;
        }

        #endregion

        #region PUBLIC

        public void ExitUnlockArea()
        {
            isPauseMenu = false;
            isActiveOptionsMenu = false;

            if (isPauseUnlock)
                PauseGame(false);
        }

        public IEnumerator ExitUnlockAreaWaitForSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            ExitUnlockArea();
        }

        public void PauseGame(bool isPaused)
        {
            isPauseUnlock = isPaused;

            if (!isPauseUnlock)
                US_UnlockSystem.instance.ActivateUnlockArea(false);

            m_HUD.SetActive(!isPauseUnlock);
            if (m_Player && m_Player.GetComponent<US_FPSController>())
                m_Player.GetComponent<US_FPSController>().enabled = !isPauseUnlock;

            Cursor.lockState = isPauseUnlock ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isPauseUnlock;
        }

        #region Button Events

        public void StartButton(bool state)
        {
            isPauseMenu = state;
        }

        public void QuitGame(bool state)
        {
            Application.Quit();
        }

        public void OptionsButton(bool state)
        {
            if (state)
            {
                isActiveOptionsMenu = true;
                mainMenuRoot.SetActive(false);
            }
        }

        public void BackButton(bool state)
        {
            if (state)
            {
                isActiveOptionsMenu = false;
                mainMenuRoot.SetActive(true);
            }
        }

        #endregion

        #region Set, Get

        public void SetActiveHelperMenuLevelRoot_0(bool isActive)
        {
            helperMenuLevelRoot_0.SetActive(isActive);
        }

        public void SetActiveHelperMenuLevelRoot_1(bool isActive)
        {
            helperMenuLevelRoot_1.SetActive(isActive);
        }

        #endregion

        #region Cursor

        public void UpdateCursorLock()
        {
            if (lockCursor)
                InternalLockUpdate();
        }

        #endregion

        #endregion

        #region PRIVATE

        #region Cursor

        private void InternalLockUpdate()
        {
            if (isCursorLocked == false) // If the cursor is active
            {
                if (!isPauseMenu)
                    isCursorLocked = true;
            }
            else
            {
                if (isPauseMenu)
                    isCursorLocked = false;
            }

            Cursor.lockState = isCursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !isCursorLocked;
        }

        #endregion

        #endregion
    }
}