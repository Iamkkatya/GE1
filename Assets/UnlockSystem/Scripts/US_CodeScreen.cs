using UnityEngine;
using UnityEngine.UI;

namespace UnlockSystem
{
    public class US_CodeScreen : MonoBehaviour
    {
        #region Attributes

        [Header("ATTRIBUTES")]
        [SerializeField] private float speedRoll = 4.0f;

        [Header("TEXTS")]
        [SerializeField] private Text textLevel_0;
        [SerializeField] private Text textLevel_1;

        [Header("REFERENCES")]
        [SerializeField] private GameObject codeUpdateText;
        [SerializeField] private GameObject codeText;

        private float deltaTime = 0.0f; // Timer
        private float deltaUpdateTime = 0.0f; // Timer between the appearance of texts
        private int i = 0;
        private int level = 0;
        private bool isOpen = false;
        private string vspText; // We will store our text here

        private Vector3 defaultCodeTextPosition, defaultUpdateTexPosition;

        #endregion

        private void Start()
        {
            level = 0;
            textLevel_0.text = "Processing...";

            defaultCodeTextPosition = codeText.transform.localPosition;
            defaultUpdateTexPosition = codeUpdateText.transform.localPosition;
        }

        private void Update()
        {
            if (level == 0)
            {
                ActionLevel_0();
            }
            else if (level == 1)
            {
                ActionLevel_1();
            }
            else if (level == 2)
            {
                ActionLevel_2();
            }
            else if (level == 3)
            {
                ActionLevel_3();
            }

            deltaTime += Time.deltaTime;

            if (isOpen)
            {
                if (US_Menu.instance.IsDemoScene)
                    US_UnlockSystem.instance.DoorReference.hasBeenUnlocked = true;

                StartCoroutine(US_Menu.instance.ExitUnlockAreaWaitForSeconds(1.0f));
            }
        }

        #region PUBLIC

        /// <summary>
        /// Reset code screen by default settings
        /// </summary>
        public void ResetCodeScreen()
        {
            level = 0;
            textLevel_0.text = "Processing...";
            isOpen = false;
            i = 0;
            deltaUpdateTime = 0.0f;
            deltaTime = 0.0f;

            codeText.transform.localPosition = defaultCodeTextPosition;
            codeUpdateText.transform.localPosition = defaultUpdateTexPosition;
        }

        #endregion

        #region PRIVATE

        private void ActionLevel_0()
        {
            if (deltaTime >= 6.0f)
            {
                level = 1;
                deltaUpdateTime = 0.0f;

                textLevel_0.text = "Processing...\n.\n.\nLaunch ASPB...\n.\n.\nASPB launched...";
                vspText = textLevel_0.text;
            }
            else
            {
                if (deltaUpdateTime >= 0.25f && deltaTime <= 2.5f)
                {
                    if (i == 0)
                    {
                        textLevel_0.text = "Processing";
                        i = 1;
                    }
                    else if (i == 1)
                    {
                        textLevel_0.text = "Processing.";
                        i = 2;
                    }
                    else if (i == 2)
                    {
                        textLevel_0.text = "Processing..";
                        i = 3;
                    }
                    else if (i == 3)
                    {
                        textLevel_0.text = "Processing...";
                        i = 0;
                    }

                    deltaUpdateTime = 0.0f;
                }

                if (deltaUpdateTime >= 0.25f && deltaTime > 2.5f && deltaTime <= 6.0f)
                {
                    if (i == 0)
                    {
                        textLevel_0.text = "Processing...\n.\n.\nLaunch ASPB";
                        i = 1;
                    }
                    else if (i == 1)
                    {
                        textLevel_0.text = "Processing...\n.\n.\nLaunch ASPB.";
                        i = 2;
                    }
                    else if (i == 2)
                    {
                        textLevel_0.text = "Processing...\n.\n.\nLaunch ASPB..";
                        i = 3;
                    }
                    else if (i == 3)
                    {
                        textLevel_0.text = "Processing...\n.\n.\nLaunch ASPB...";
                        i = 0;
                    }

                    deltaUpdateTime = 0.0f;
                }

                deltaUpdateTime += Time.deltaTime;
            }
        }

        private void ActionLevel_1()
        {
            if (deltaTime >= 9.0f)
            {
                level = 2;
                //deltaTime = 0.0f;
                deltaUpdateTime = 0.0f;

                textLevel_0.text = "\n.\n.\nAnalyzing System Protection Bypass..." + "\n.\n.\nLogin Domain Verification..." + "\n.\n.\nProtection bypass.." + "\n.\n.\nClosing Firewalls..." + "\n.\n.\nCompleted ASPB..." + "\n.\n.\nLaunching Virus Injection...";
                vspText = textLevel_0.text;

                codeText.SetActive(true);
            }
            else
            {
                if (deltaUpdateTime >= 0.25f)
                {
                    if (i == 0)
                    {
                        textLevel_0.text = vspText + "\n.\n.\nAnalyzing System Protection Bypass" + "\n.\n.\nLogin Domain Verification";
                        i++;
                    }
                    else if (i == 1)
                    {
                        textLevel_0.text = vspText + "\n.\n.\nAnalyzing System Protection Bypass." + "\n.\n.\nLogin Domain Verification.";
                        i++;
                    }
                    else if (i == 2)
                    {
                        textLevel_0.text = vspText + "\n.\n.\nAnalyzing System Protection Bypass.." + "\n.\n.\nLogin Domain Verification..";
                        i++;
                    }
                    else if (i == 3)
                    {
                        textLevel_0.text = vspText + "\n.\n.\nAnalyzing System Protection Bypass..." + "\n.\n.\nLogin Domain Verification...";
                        i++;
                    }

                    else if (i == 4)
                    {
                        textLevel_0.text = vspText + "\n.\n.\nAnalyzing System Protection Bypass..." + "\n.\n.\nLogin Domain Verification..." + "\n.\n.\nProtection bypass";
                        i++;
                    }
                    else if (i == 5)
                    {
                        textLevel_0.text = vspText + "\n.\n.\nAnalyzing System Protection Bypass..." + "\n.\n.\nLogin Domain Verification..." + "\n.\n.\nProtection bypass.";
                        i++;
                    }
                    else if (i == 6)
                    {
                        textLevel_0.text = vspText + "\n.\n.\nAnalyzing System Protection Bypass..." + "\n.\n.\nLogin Domain Verification..." + "\n.\n.\nProtection bypass..";
                        i++;
                    }
                    else if (i == 7)
                    {
                        textLevel_0.text = vspText + "\n.\n.\nAnalyzing System Protection Bypass..." + "\n.\n.\nLogin Domain Verification..." + "\n.\n.\nProtection bypass..";
                        i++;
                    }

                    else if (i == 8)
                    {
                        textLevel_0.text = vspText + "\n.\n.\nAnalyzing System Protection Bypass..." + "\n.\n.\nLogin Domain Verification..." + "\n.\n.\nProtection bypass" + "\n.\n.\nClosing Firewalls";
                        i++;
                    }
                    else if (i == 9)
                    {
                        textLevel_0.text = vspText + "\n.\n.\nAnalyzing System Protection Bypass..." + "\n.\n.\nLogin Domain Verification..." + "\n.\n.\nProtection bypass." + "\n.\n.\nClosing Firewalls.";
                        i++;
                    }
                    else if (i == 10)
                    {
                        textLevel_0.text = vspText + "\n.\n.\nAnalyzing System Protection Bypass..." + "\n.\n.\nLogin Domain Verification..." + "\n.\n.\nProtection bypass.." + "\n.\n.\nClosing Firewalls..";
                        i++;
                    }
                    else if (i == 11)
                    {
                        textLevel_0.text = vspText + "\n.\n.\nAnalyzing System Protection Bypass..." + "\n.\n.\nLogin Domain Verification..." + "\n.\n.\nProtection bypass.." + "\n.\n.\nClosing Firewalls...";
                        i++;
                    }

                    deltaUpdateTime = 0.0f;
                }

                deltaUpdateTime += Time.deltaTime;
            }
        }

        private void ActionLevel_2()
        {
            if (deltaTime >= 24.0f)
            {
                level = 3;
                codeText.SetActive(false);
            }
            else
            {
                codeText.transform.localPosition = new Vector3(codeText.transform.localPosition.x, codeText.transform.localPosition.y + speedRoll, codeText.transform.localPosition.z);
                codeUpdateText.transform.localPosition = new Vector3(codeUpdateText.transform.localPosition.x, codeUpdateText.transform.localPosition.y + speedRoll, codeUpdateText.transform.localPosition.z);
            }
        }

        private void ActionLevel_3()
        {
            if (isOpen)
            {
                codeUpdateText.transform.localPosition = new Vector3(10.0f, -10.0f, 0.0f);
                textLevel_0.text = ".\n.\n.\n.OPEN...";

                US_UnlockSystem.instance.ELBoxReference.isOpen = true;
            }
            else
            {
                isOpen = true;
            }
        }

        #endregion
    }
}