using UnityEngine;

namespace UnlockSystem
{
    public class US_DrumAction : MonoBehaviour
    {
        #region Attributes

        [Header("AUDIO")]
        [SerializeField] private AudioClip AudioLock;
        [SerializeField] private AudioClip AudioUnLock;

        [Header("REFERENCES")]
        [SerializeField] private GameObject drum;

        [Header("BUTTONS")]
        [SerializeField] private Transform arrowButtonUp;
        [SerializeField] private Transform arrowButtonDown;

        private bool rotateUp = false;
        private bool rotateDown = false;

        public int countPress { get; private set; } = 0;
        private int additionCountPress = 0;
        private int lastCountPress = 0;

        public int drumID { get; private set; } = -1;
        public bool isNumbersDrum { get; private set; }

        private float[] offsetDrumsMassive;
        private int[] codes;

        private int direction = 1;
        private float rotate = 0.0f;
        private float deltaRotate = 0.0f;

        private US_CodeLockGenerator codeLockGenerator;
        private AudioSource a_AudioSource;

        #endregion

        private void Start()
        {
            a_AudioSource = GetComponent<AudioSource>();

            InitOffsetMassive();
        }

        private void Update()
        {
            if (rotateUp)
            {
                RotateDrumUp();
            }
            else if (rotateDown)
            {
                RotateDrumDown();
            }
            else
            {
                rotate = 0.0f;
                deltaRotate = 0.0f;
            }

            UpdatePositionArrows();
        }

        #region PUBLIC

        public void InitCode(US_CodeLockGenerator codeLockGenerator, int code, int drumID, US_CodeLockGenerator.LockType lockType)
        {
            this.codeLockGenerator = codeLockGenerator;
            this.drumID = drumID;
            isNumbersDrum = lockType == US_CodeLockGenerator.LockType.NUMBERS;

            if (isNumbersDrum)
                codes = new int[10];
            else
                codes = new int[8];

            for (int i = 0; i < codes.Length; i++)
            {
                if (i == code)
                    codes[i] = code;
                else
                    codes[i] = -1;
            }
        }

        #region Buttons

        public void RotateUp()
        {
            rotateUp = true;

            lastCountPress = countPress;
            countPress++;

            deltaRotate = drum.transform.localEulerAngles.z;

            if (isNumbersDrum)
            {
                if (countPress >= 10)
                    countPress = 0;
            }
            else
            {
                if (countPress >= 8)
                    countPress = 0;
            }

            additionCountPress = countPress;

            PlayRotationLockAudio();
        }

        public void RotateDown()
        {
            rotateDown = true;

            if (drum.transform.localEulerAngles.z == 0.0f)
                drum.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 359.999f);

            deltaRotate = drum.transform.localEulerAngles.z;

            additionCountPress--;

            if (isNumbersDrum)
            {
                if (additionCountPress < 0)
                    additionCountPress = 9;
            }
            else
            {
                if (additionCountPress < 0)
                    additionCountPress = 7;
            }

            lastCountPress = countPress;
            countPress = Mathf.Abs(additionCountPress);

            PlayRotationLockAudio();
        }

        #endregion

        #endregion

        #region PRIVATE

        private void UpdatePositionArrows()
        {
            if (arrowButtonUp == null || arrowButtonDown == null)
                return;

            if (arrowButtonUp.localPosition.y > 0.09f)
                direction = -1;
            else if (arrowButtonUp.localPosition.y < 0.075f)
                direction = 1;

            float deltaY_up = arrowButtonUp.localPosition.y + Time.deltaTime * 0.015f * direction;
            arrowButtonUp.localPosition = new Vector3(arrowButtonUp.localPosition.x, deltaY_up, arrowButtonUp.localPosition.z);

            if (arrowButtonDown.localPosition.y < -0.09f)
                direction = -1;
            else if (arrowButtonDown.localPosition.y > -0.075f)
                direction = 1;

            float deltaY_down = arrowButtonDown.localPosition.y - Time.deltaTime * 0.015f * direction;
            arrowButtonDown.localPosition = new Vector3(arrowButtonDown.localPosition.x, deltaY_down, arrowButtonDown.localPosition.z);
        }

        private void InitOffsetMassive()
        {
            if (isNumbersDrum)
            {
                offsetDrumsMassive = new float[11];

                offsetDrumsMassive[0] = 0.0f;       // 0
                offsetDrumsMassive[1] = 34.934f;    // 1
                offsetDrumsMassive[2] = 69.551f;    // 2
                offsetDrumsMassive[3] = 105.06f;    // 3
                offsetDrumsMassive[4] = 138.716f;   // 4
                offsetDrumsMassive[5] = 173.777f;   // 5
                offsetDrumsMassive[6] = 208.54f;    // 6
                offsetDrumsMassive[7] = 246.286f;   // 7
                offsetDrumsMassive[8] = 290.079f;   // 8
                offsetDrumsMassive[9] = 323.877f;   // 9
                offsetDrumsMassive[10] = 360.0f;    // 0
            }
            else
            {
                offsetDrumsMassive = new float[9];

                offsetDrumsMassive[0] = 0.0f;       // 0
                offsetDrumsMassive[1] = 45.0f;      // 1
                offsetDrumsMassive[2] = 90.0f;      // 2
                offsetDrumsMassive[3] = 135.0f;     // 3
                offsetDrumsMassive[4] = 180.0f;     // 4
                offsetDrumsMassive[5] = 225.0f;     // 5
                offsetDrumsMassive[6] = 270.0f;     // 6
                offsetDrumsMassive[7] = 315.0f;     // 7
                offsetDrumsMassive[8] = 360.0f;     // 8
            }

        }

        #region Audio

        private void PlayRotationLockAudio()
        {
            a_AudioSource.volume = 1.0f;
            a_AudioSource.clip = AudioLock;
            a_AudioSource.PlayOneShot(a_AudioSource.clip);
        }

        private void PlayRotationUnLockAudio()
        {
            a_AudioSource.volume = 1.0f;
            a_AudioSource.clip = AudioUnLock;
            a_AudioSource.PlayOneShot(a_AudioSource.clip);
        }

        #endregion

        #region Rotation

        private void RotateDrumUp()
        {
            RotateDrum(1);

            if ((isNumbersDrum && lastCountPress == 9 && countPress == 0) || (!isNumbersDrum && lastCountPress == 7 && countPress == 0))
            {
                if (deltaRotate >= 360.0f)
                {
                    drum.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, offsetDrumsMassive[offsetDrumsMassive.Length - 1]));
                    codeLockGenerator.CheckCode();
                    if (codeLockGenerator.CheckOneShotCode(countPress, drumID))
                        PlayRotationUnLockAudio();
                    rotateUp = false;
                }
            }
            else
            {
                if (Mathf.Abs(drum.transform.localEulerAngles.z) >= offsetDrumsMassive[countPress])
                {
                    drum.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, offsetDrumsMassive[countPress]));
                    codeLockGenerator.CheckCode();
                    if (codeLockGenerator.CheckOneShotCode(countPress, drumID))
                        PlayRotationUnLockAudio();
                    rotateUp = false;
                }
            }
        }

        private void RotateDrumDown()
        {
            RotateDrum(-1);

            if ((isNumbersDrum && lastCountPress == 9 && countPress == 0) || (!isNumbersDrum && lastCountPress == 7 && countPress == 0))
            {
                if (deltaRotate >= 360.0f)
                {
                    drum.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, offsetDrumsMassive[offsetDrumsMassive.Length - 1]));
                    codeLockGenerator.CheckCode();
                    if (codeLockGenerator.CheckOneShotCode(countPress, drumID))
                        PlayRotationUnLockAudio();
                    rotateDown = false;
                }
            }
            if (lastCountPress == 1 && countPress == 0)
            {
                if (rotate <= 0.0f)
                {
                    drum.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, offsetDrumsMassive[0]));
                    codeLockGenerator.CheckCode();
                    if (codeLockGenerator.CheckOneShotCode(countPress, drumID))
                        PlayRotationUnLockAudio();
                    rotateDown = false;
                }
            }
            else
            {
                if (Mathf.Abs(drum.transform.localEulerAngles.z) <= offsetDrumsMassive[countPress])
                {
                    drum.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, offsetDrumsMassive[countPress]));
                    codeLockGenerator.CheckCode();
                    if (codeLockGenerator.CheckOneShotCode(countPress, drumID))
                        PlayRotationUnLockAudio();
                    rotateDown = false;
                }
            }
        }

        private void RotateDrum(int direction)
        {
            rotate = drum.transform.localEulerAngles.z + Time.deltaTime * 80.0f * direction;
            drum.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, rotate));
            deltaRotate += Time.deltaTime * 80.0f;
        }

        #endregion

        #endregion
    }
}