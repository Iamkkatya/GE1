using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UnlockSystem
{
    public class US_Test : MonoBehaviour
    {
        #region Attributes

        [Header("ATTRIBUTES")]
        [SerializeField] private float MaxDistanceRay = 2.0f;
        [SerializeField] private float waitSecondsToClosePanel;

        [Header("KEYS")]
        [SerializeField] private KeyCode keyCodeInteraction = KeyCode.F;

        [Header("ICONS")]
        [SerializeField] private GameObject mIconPoint;

        [SerializeField] private GameObject mIconUnlockClose;
        [SerializeField] private GameObject mIconUnlockOpen;

        [SerializeField] private GameObject mIconNoMultitools;
        [SerializeField] private GameObject mIconAmountMultitools;

        [SerializeField] private GameObject mIconCloseDoor;
        [SerializeField] private GameObject mIconOpenDoor;
        [SerializeField] private GameObject mIconDoorLocked;

        public static US_Test instance { get; private set; }

        #endregion

        private void Start()
        {
            instance = this;
            mIconUnlockClose.SetActive(false);
            mIconUnlockOpen.SetActive(false);
            mIconPoint.SetActive(true);
        }

        private void Update()
        {
            if (Input.GetKeyUp(keyCodeInteraction))
            {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                if (Physics.Raycast(ray, out RaycastHit hit, MaxDistanceRay))
                    CheckHitPoint(hit);
            }

            UpdateIcon();
        }

        #region PUBLIC

        public void SetIconAmountMultitoolsText(string text)
        {
            mIconAmountMultitools.GetComponentInChildren<Text>().text = text;
        }

        public void SetActiveIconAmountMultitools(bool isActive)
        {
            mIconAmountMultitools.SetActive(isActive);
        }

        public void SetActiveIconUnlockClose(bool isActive)
        {
            mIconUnlockClose.SetActive(isActive);
        }

        #endregion

        #region PRIVATE

        private void CheckHitPoint(RaycastHit hit)
        {
            if (hit.transform == null)
                return;

            if (hit.transform.CompareTag("LockBase"))
            {
                US_UnlockSystem.instance.currentActiveLockType = hit.transform.GetComponent<US_LootBox>().USType;
                US_UnlockSystem.instance.LootBoxReference = hit.transform.GetComponent<US_LootBox>();

                if (!hit.transform.GetComponent<US_LootBox>().isOpen && US_PlayerItems.instance.AmountMultools >= 1)
                {
                    US_UnlockSystem.instance.ActivateUnlockArea(true);
                }
                else
                {
                    if (US_PlayerItems.instance.AmountMultools == 0)
                        StartCoroutine(InitPanel());
                }
            }
            else if (hit.transform.CompareTag("LockCode"))
            {
                if (!hit.transform.GetComponent<US_LootBox>().isOpen)
                {
                    US_UnlockSystem.instance.currentActiveLockType = hit.transform.GetComponent<US_LootBox>().USType;
                    US_UnlockSystem.instance.LootBoxReference = hit.transform.GetComponent<US_LootBox>();

                    US_UnlockSystem.instance.ActivateUnlockArea(true);

                    if (US_Menu.instance.IsDemoScene)
                        US_UnlockSystem.instance.DoorReference = hit.transform.GetComponent<US_LootBox>().DoorReference;
                }
                else
                {
                    print("OPEND");
                }
            }
            else if (hit.transform.CompareTag("LockEL"))
            {
                if (!hit.transform.GetComponent<US_UnlockEL>().isOpen)
                {
                    US_UnlockSystem.instance.currentActiveLockType = hit.transform.GetComponent<US_UnlockEL>().USType;
                    US_UnlockSystem.instance.ELBoxReference = hit.transform.GetComponent<US_UnlockEL>();
                    US_UnlockSystem.instance.usLevelOfDifficulty = hit.transform.GetComponent<US_UnlockEL>().Level;

                    US_UnlockSystem.instance.ActivateUnlockArea(true);

                    //
                    // Удали его
                    //

                    if (US_Menu.instance.IsDemoScene)
                    {
                        US_UnlockSystem.instance.DoorReference = hit.transform.GetComponent<US_UnlockEL>().DoorReference;
                    }
                }
                else
                {
                    print("OPEND");
                }
            }
            else if (hit.transform.CompareTag("Door"))
            {
                if (hit.transform.GetComponent<US_Door>().hasBeenUnlocked) // Если взломана
                {
                    if (!hit.transform.GetComponent<US_Door>().Opened)
                        hit.transform.GetComponent<US_Door>().opening = true; // Открываем дверь
                    else if (!hit.transform.GetComponent<US_Door>().Closed)
                        hit.transform.GetComponent<US_Door>().closing = true; // Закрываем дверь
                }
            }
        }

        private IEnumerator InitPanel()
        {
            mIconNoMultitools.SetActive(true);

            yield return new WaitForSeconds(waitSecondsToClosePanel);

            mIconNoMultitools.SetActive(false);
        }

        private void UpdateIcon()
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(ray, out RaycastHit hit, MaxDistanceRay))
            {
                if (hit.transform.CompareTag("LockBase") || hit.transform.CompareTag("LockCode"))
                {
                    if (!hit.transform.GetComponent<US_LootBox>().isOpen)
                    {
                        mIconUnlockClose.SetActive(true);
                        mIconUnlockOpen.SetActive(false);
                    }
                    else
                    {
                        mIconUnlockOpen.SetActive(true);
                        mIconUnlockClose.SetActive(false);
                    }

                    mIconPoint.SetActive(false);
                    mIconDoorLocked.SetActive(false);
                    mIconCloseDoor.SetActive(false);
                    mIconOpenDoor.SetActive(false);
                }
                else if (hit.transform.CompareTag("LockEL"))
                {
                    if (!hit.transform.GetComponent<US_UnlockEL>().isOpen)
                    {
                        mIconUnlockClose.SetActive(true);
                        mIconUnlockOpen.SetActive(false);
                    }
                    else
                    {
                        mIconUnlockOpen.SetActive(true);
                        mIconUnlockClose.SetActive(false);
                    }

                    mIconPoint.SetActive(false);
                    mIconDoorLocked.SetActive(false);
                    mIconCloseDoor.SetActive(false);
                    mIconOpenDoor.SetActive(false);
                }
                else if (hit.transform.CompareTag("Door"))
                {
                    if (hit.transform.GetComponent<US_Door>().hasBeenUnlocked)
                    {
                        if (hit.transform.GetComponent<US_Door>().Closed)
                        {
                            mIconCloseDoor.SetActive(false);
                            mIconOpenDoor.SetActive(true);
                        }
                        else if (hit.transform.GetComponent<US_Door>().Opened)
                        {
                            mIconCloseDoor.SetActive(true);
                            mIconOpenDoor.SetActive(false);
                        }
                        mIconUnlockClose.SetActive(false);
                        mIconDoorLocked.SetActive(false);
                    }
                    else
                    {
                        mIconDoorLocked.SetActive(true);
                        mIconCloseDoor.SetActive(false);
                        mIconOpenDoor.SetActive(false);
                        mIconUnlockClose.SetActive(false);
                    }
                }
                else
                {
                    mIconUnlockClose.SetActive(false);
                    mIconUnlockOpen.SetActive(false);
                    mIconDoorLocked.SetActive(false);
                    mIconCloseDoor.SetActive(false);
                    mIconOpenDoor.SetActive(false);
                    mIconPoint.SetActive(true);
                }
            }
            else
            {
                mIconUnlockClose.SetActive(false);
                mIconUnlockOpen.SetActive(false);
                mIconDoorLocked.SetActive(false);
                mIconCloseDoor.SetActive(false);
                mIconOpenDoor.SetActive(false);
                mIconPoint.SetActive(true);
            }
        }

        #endregion
    }
}