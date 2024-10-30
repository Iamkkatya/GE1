using System.Collections;
using UnityEngine;

namespace UnlockSystem
{
    public class US_UnlockAction : MonoBehaviour
    {
        #region Attributes

        [Header("AUDIO")]
        public AudioClip AudioLock;
        public AudioClip AudioMultitool;
        public AudioClip AudioWin;
        private AudioSource a_AudioSource;

        [Header("ATTRIBUTES")]
        public GameObject mRootLockRotate;
        public GameObject mRootMultitool;
        public GameObject mRootLatchkey;
        public GameObject mLatchkeyRotate;
        public GameObject Multitool;
        public GameObject MultitoolPrefab;

        [Header("ATTRIBUTES lock V2")]
        public GameObject cilinder;

        [Header("ATTRIBUTES")]
        public Vector2 sensitivityLatchkeyRot = new Vector2(4, 4);
        public float speedCoefficient = 0.02f;

        [Header("LIMITATION")]
        public Vector2 limitRotX = new Vector2(0, 0);
        public Vector2 limitRotY = new Vector2(0, 0);
        public Vector2 limitRotZ = new Vector2(-80, 80);

        [Header("ATTRIBUTES")]
        public float lockTwitch = 2.0f;
        public float timeToLatchkeyBreak = 1.0f;

        private Animation anim;
        private Vector3 currentRot;
        private Vector2 lockField = Vector2.zero;
        private Vector2 lockFieldTwitching = Vector2.zero;
        private Vector2 limitLatchkeyRotZ = new Vector2(-180, 180);
        private Quaternion rootLockTargetRot, lockTargetRot, latchkeyTargetRot;   // To update the rotation of the model
        private Quaternion vspRootMultitool, vspRootLatchkey, vspRootLockRotate;

        private int vsp = 1; // For changinh sign
        private float zRot0 = 0.01f;
        private float zRot1 = 0.01f;
        private float zRot2 = 0.01f;
        private float rot = 0.0f;
        private float rot1 = 0.0f;
        private float xRotLast = 0.0f;
        private float deltaTime = 0.0f;
        private float deltaAudioTime = 0.0f;
        private float deltaRewind = 0.0f;
        private bool isActivePressW = false;
        private bool canToolMoving = false;
        private bool canLatchkeyTwitching = false;
        private bool isLockTwitching = false;
        private bool isToolReturned = false;
        private bool isWin = false;
        private bool isLost = false;
        private bool isBroken = false;
        private bool isFirstOpening = true;
        private bool isToolTwitching = false;
        private bool isToolMoving = false;
        private bool isLockAudio = false;

        public static US_UnlockAction instance { get; private set; }

        #endregion

        private void Start()
        {
            instance = this;

            a_AudioSource = GetComponent<AudioSource>();

            US_UnlockSystem.instance.LootBoxReference.GenerateLockField(); // Рандомно задаем поле, в которое нужно попасть отмычкой для победы

            lockField = US_UnlockSystem.instance.LootBoxReference.lockField; // Задаем поля (на краях поля выигрыша), если мы немного не дотянули до поля выигрышы мы все равно будем двигать мультитул и, когда он дойдет до упора он будет дергаться
            lockFieldTwitching.x = lockField.x - 5.0f;
            lockFieldTwitching.y = lockField.y + 5.0f;

            canToolMoving = false;
            isActivePressW = false;

            vspRootMultitool = mRootMultitool.GetComponent<Transform>().localRotation;   // Сохраняем исходное положение предмета
            vspRootLatchkey = mRootLatchkey.GetComponent<Transform>().localRotation;   // Сохраняем исходное положение предмета
            vspRootLockRotate = mRootLockRotate.GetComponent<Transform>().localRotation;   // Сохраняем исходное положение предмета

            anim = mLatchkeyRotate.GetComponentInChildren<Animation>();
        }

        private void Update()
        {
            if (isFirstOpening)
            {
                Update3DView();
            }
            else // We need to reset parameters and then we can rotate latchkey
            {
                US_UnlockSystem.instance.LootBoxReference.GenerateLockField();
                lockField = US_UnlockSystem.instance.LootBoxReference.lockField;

                lockFieldTwitching.x = lockField.x - 5.0f;
                lockFieldTwitching.y = lockField.y + 5.0f;

                canToolMoving = false;
                isActivePressW = false;

                mRootMultitool.GetComponent<Transform>().localRotation = vspRootMultitool;
                mRootLatchkey.GetComponent<Transform>().localRotation = vspRootLatchkey;
                mRootLockRotate.GetComponent<Transform>().localRotation = vspRootLockRotate;

                anim = mLatchkeyRotate.GetComponentInChildren<Animation>();

                isFirstOpening = true;
            }
        }

        #region PUBLIC

        public void CloseUnlockArea()
        {
            isActivePressW = false;
            canToolMoving = false;
            canLatchkeyTwitching = false;
            isLockTwitching = false;
            isToolReturned = false;
            isBroken = false;
            isWin = false;
            isLost = false;
            isFirstOpening = false;
            lockField = Vector2.zero;
            isToolMoving = false;
            isLockAudio = false;

            zRot0 = 0.01f;
            zRot1 = 0.01f;
            zRot2 = 0.01f;
            rot = 0.0f;

            deltaAudioTime = a_AudioSource.clip.length;

            mRootMultitool.GetComponent<Transform>().localRotation = vspRootMultitool;
            mRootLatchkey.GetComponent<Transform>().localRotation = vspRootLatchkey;
            mRootLockRotate.GetComponent<Transform>().localRotation = vspRootLockRotate;

            mLatchkeyRotate.GetComponent<Transform>().eulerAngles = Vector3.zero;
        }

        #endregion

        #region PRIVATE

        private void Update3DView()
        {
            if (!isWin && !isLost)
            {
                SeachField();
                RotateLock();
                RotateKey();

                if (isBroken) // If the latchkey is broken
                {
                    if (deltaRewind < anim.clip.length) // Wait until the animation play
                    {
                        anim.Play("Break");

                        deltaRewind += Time.deltaTime;
                    }
                    else // Then destroy the latchkey
                    {
                        isBroken = false;
                        canLatchkeyTwitching = false;
                        canToolMoving = false;

                        Transform GO = Multitool.transform;

                        Destroy(Multitool);

                        if (US_PlayerItems.instance.AmountMultools != 0) // If we have another one
                        {
                            Multitool = Instantiate(MultitoolPrefab);
                            Multitool.transform.SetParent(GO.transform.parent);
                            Multitool.transform.localScale = GO.transform.localScale;
                            Multitool.transform.localRotation = GO.transform.localRotation;
                            Multitool.transform.localPosition = GO.transform.localPosition;

                            anim = Multitool.GetComponent<Animation>();
                            US_PlayerItems.instance.AmountMultools -= 1;
                            US_Test.instance.SetIconAmountMultitoolsText("Multitools\n" + US_PlayerItems.instance.AmountMultools.ToString());
                        }
                        else
                        {
                            isLost = true;
                        }
                    }

                    isToolReturned = false;
                }
            }
            else
            {
                StartCoroutine(WinAction());
            }
        }

        private void SeachField()
        {
            if (!canToolMoving && !canLatchkeyTwitching && !isToolTwitching && !isLockTwitching && isToolReturned)
            {
                currentRot = mLatchkeyRotate.GetComponent<Transform>().eulerAngles; // Save current latchkey rotation
                rot = currentRot.z;
                if (currentRot.z <= 360.0f && currentRot.z >= 270.0f) // since the right side of the coordinate axis starts from 360 to 270, we need to convert it to negative values (from 0 to -90)
                    rot = -(360.0f - currentRot.z);

                if (rot > lockField.x && rot < lockField.y) // Did we hit the field?
                {
                    canToolMoving = true;
                    canLatchkeyTwitching = false;
                    isToolTwitching = false;
                }
                else
                {
                    canToolMoving = false;

                    if ((rot < lockField.x && rot > lockFieldTwitching.x) || (rot > lockField.y && rot < lockFieldTwitching.y))   // Did we hit the field?
                    {
                        canLatchkeyTwitching = true;
                        isToolTwitching = false;
                    }
                }
            }
        }

        private IEnumerator WinAction()
        {
            if (isWin)
                US_UnlockSystem.instance.LootBoxReference.isOpen = true;
            US_Test.instance.SetActiveIconAmountMultitools(false);

            //
            // Audio
            //
            if (deltaAudioTime > AudioWin.length)
            {
                a_AudioSource.volume = 0.7f;
                a_AudioSource.clip = AudioWin;
                a_AudioSource.PlayOneShot(a_AudioSource.clip);
                deltaAudioTime = 0.0f;
            }

            if (US_UnlockSystem.instance.currentActiveLockType == US_UnlockSystem.USType.TYPE_MECHANICAL_V2)
                StartCoroutine(cilinder.GetComponent<US_CilinderAction>().InitMoving());

            if (US_UnlockSystem.instance.currentActiveLockType == US_UnlockSystem.USType.TYPE_MECHANICAL_V1)
                yield return new WaitForSeconds(1.25f);
            else if (US_UnlockSystem.instance.currentActiveLockType == US_UnlockSystem.USType.TYPE_MECHANICAL_V2)
                yield return new WaitForSeconds(2f);

            CloseUnlockArea();
            US_Menu.instance.ExitUnlockArea();

            //
            // Actions
            //
        }

        #region Rotation

        private void RotateKey()
        {
            if (!Input.GetKey(US_UnlockSystem.instance.KeyCodeMoveLatchkey) && !isBroken && isToolReturned)
            {
                float xRot = Input.GetAxis("Mouse X") * sensitivityLatchkeyRot.x;

                // Audio
                if (xRotLast != xRot && !isLockAudio)
                {
                    deltaAudioTime += Time.deltaTime;
                    if (deltaAudioTime > a_AudioSource.clip.length)
                    {
                        a_AudioSource.volume = 1.0f;
                        a_AudioSource.clip = AudioLock;
                        a_AudioSource.PlayOneShot(a_AudioSource.clip);
                        deltaAudioTime = 0.0f;
                    }
                }

                latchkeyTargetRot = mLatchkeyRotate.GetComponent<Transform>().localRotation;
                latchkeyTargetRot *= Quaternion.Euler(0.0f, 0.0f, -xRot);
                latchkeyTargetRot = ClampRotationAroundAxis(latchkeyTargetRot, limitRotX, 0);
                latchkeyTargetRot = ClampRotationAroundAxis(latchkeyTargetRot, limitRotY, 1);
                latchkeyTargetRot = ClampRotationAroundAxis(latchkeyTargetRot, limitRotZ, 2);

                mLatchkeyRotate.GetComponent<Transform>().localRotation = latchkeyTargetRot;

                Vector3 CurrentRotMouse = mLatchkeyRotate.GetComponent<Transform>().eulerAngles; // Save current latchkey rotation
                float RotMouse = CurrentRotMouse.z;
                if (CurrentRotMouse.z <= 360.0f && CurrentRotMouse.z >= 270.0f)   // since the right side of the coordinate axis starts from 360 to 270, we need to convert it to negative values (from 0 to -90)
                    RotMouse = -(360.0f - CurrentRotMouse.z);

                if (RotMouse > (limitRotZ.y - 2.0f) || RotMouse < (limitRotZ.x + 2.0f))
                    isLockAudio = true;
                else
                    isLockAudio = false;

                xRotLast = xRot;
            }
        }

        private void RotateLock()
        {
            if (Input.GetKey(US_UnlockSystem.instance.KeyCodeMoveLatchkey) && isToolReturned)   // Did we press key? Did the latchkey returned back?
            {
                if (!isActivePressW) // At the first launch (after releasing the key), we reset some parameters
                {
                    zRot0 = 0.01f;
                    zRot1 = 0.01f;
                    zRot2 = 0.01f;
                    deltaTime = 0.0f;
                    deltaRewind = 0.0f;
                    isActivePressW = true;
                    isToolTwitching = false;
                    vsp = 1;
                }

                if (canToolMoving)    // We are in the field and we can move the tool
                {
                    isToolMoving = true;

                    zRot0 += speedCoefficient * sensitivityLatchkeyRot.x;
                    lockTargetRot = mRootMultitool.GetComponent<Transform>().localRotation;
                    lockTargetRot *= Quaternion.Euler(0.0f, 0.0f, -zRot0);
                    lockTargetRot = ClampRotationAroundAxis(lockTargetRot, limitRotX, 0);
                    lockTargetRot = ClampRotationAroundAxis(lockTargetRot, limitRotY, 1);
                    lockTargetRot = ClampRotationAroundAxis(lockTargetRot, limitRotZ, 2);

                    mRootMultitool.GetComponent<Transform>().localRotation = lockTargetRot;
                    mRootLockRotate.GetComponent<Transform>().localRotation = lockTargetRot;
                    mRootLatchkey.GetComponent<Transform>().localRotation = lockTargetRot;

                    latchkeyTargetRot = mLatchkeyRotate.GetComponent<Transform>().localRotation;
                    latchkeyTargetRot *= Quaternion.Euler(0.0f, 0.0f, zRot0);
                    latchkeyTargetRot = ClampRotationAroundAxis(latchkeyTargetRot, limitRotX, 0);
                    latchkeyTargetRot = ClampRotationAroundAxis(latchkeyTargetRot, limitRotY, 1);
                    latchkeyTargetRot = ClampRotationAroundAxis(latchkeyTargetRot, limitLatchkeyRotZ, 2);

                    mLatchkeyRotate.GetComponent<Transform>().localRotation = latchkeyTargetRot;

                    currentRot = mRootMultitool.GetComponent<Transform>().eulerAngles;
                    rot1 = currentRot.z;
                    if (currentRot.z <= 360.0f && currentRot.z >= 270.0f)
                        rot1 = -(360.0f - currentRot.z);

                    deltaAudioTime += Time.deltaTime;
                    if (deltaAudioTime > a_AudioSource.clip.length)
                    {
                        a_AudioSource.volume = 0.7f;
                        a_AudioSource.clip = AudioMultitool;
                        a_AudioSource.PlayOneShot(a_AudioSource.clip);
                        deltaAudioTime = 0.0f;
                    }

                    if (Mathf.Abs(rot1) >= (Mathf.Abs(limitRotZ.y) - 10.0f))
                    {
                        Debug.Log("WIN");
                        zRot0 = 0.01f;
                        zRot1 = 0.01f;
                        zRot2 = 0.01f;
                        isWin = true;
                        deltaAudioTime = 1.0f;
                    }
                }
                else if (canLatchkeyTwitching) // Otherwise the tool will twitch
                {
                    isToolMoving = true;
                    if (deltaTime <= timeToLatchkeyBreak + Random.Range(-0.5f, 0.5f))
                    {
                        if (isToolTwitching)
                        {
                            zRot2 = lockTwitch * vsp; // Essentially the same value, but each frame changes its sign
                            deltaTime += Time.deltaTime;
                            vsp = -vsp; // Change sign

                            deltaAudioTime += Time.deltaTime;
                            if (deltaAudioTime > (a_AudioSource.clip.length - a_AudioSource.clip.length / 2.5f))
                            {
                                a_AudioSource.volume = 0.3f;
                                a_AudioSource.clip = AudioMultitool;
                                a_AudioSource.PlayOneShot(a_AudioSource.clip);
                                deltaAudioTime = 0.0f;
                            }
                        }
                        else
                        {
                            if (zRot2 < 0)
                                zRot2 = zRot2 * (-1.0f);
                            zRot2 += speedCoefficient * sensitivityLatchkeyRot.x;
                            isToolTwitching = false;
                        }

                        lockTargetRot = mRootMultitool.GetComponent<Transform>().localRotation;
                        lockTargetRot *= Quaternion.Euler(0.0f, 0.0f, -zRot2);
                        lockTargetRot = ClampRotationAroundAxis(lockTargetRot, limitRotX, 0);
                        lockTargetRot = ClampRotationAroundAxis(lockTargetRot, limitRotY, 1);
                        lockTargetRot = ClampRotationAroundAxis(lockTargetRot, limitRotZ, 2);

                        mRootMultitool.GetComponent<Transform>().localRotation = lockTargetRot;
                        mRootLockRotate.GetComponent<Transform>().localRotation = lockTargetRot;
                        mRootLatchkey.GetComponent<Transform>().localRotation = lockTargetRot;

                        latchkeyTargetRot = mLatchkeyRotate.GetComponent<Transform>().localRotation;
                        latchkeyTargetRot *= Quaternion.Euler(0.0f, 0.0f, zRot2);
                        latchkeyTargetRot = ClampRotationAroundAxis(latchkeyTargetRot, limitRotX, 0);
                        latchkeyTargetRot = ClampRotationAroundAxis(latchkeyTargetRot, limitRotY, 1);
                        latchkeyTargetRot = ClampRotationAroundAxis(latchkeyTargetRot, limitLatchkeyRotZ, 2);

                        mLatchkeyRotate.GetComponent<Transform>().localRotation = latchkeyTargetRot;

                        Vector3 CurrentRot1 = mRootMultitool.GetComponent<Transform>().eulerAngles;
                        float rot1 = CurrentRot1.z;
                        if (CurrentRot1.z < 360.0f && CurrentRot1.z > 270.0f)
                            rot1 = -(360.0f - CurrentRot1.z);
                        if (Mathf.Abs(rot1) > (Mathf.Abs(limitRotZ.y) - 10.0f))
                            isToolTwitching = true;
                    }
                    else    // Broken
                    {
                        isBroken = true;
                    }
                }
                else
                {
                    if (!isToolMoving)
                    {
                        if (deltaTime <= timeToLatchkeyBreak + Random.Range(-0.5f, 0.5f))
                        {
                            isLockTwitching = true;

                            zRot2 = lockTwitch * vsp;

                            lockTargetRot = mRootMultitool.GetComponent<Transform>().localRotation;
                            lockTargetRot *= Quaternion.Euler(0.0f, 0.0f, -zRot2);
                            lockTargetRot = ClampRotationAroundAxis(lockTargetRot, limitRotX, 0);
                            lockTargetRot = ClampRotationAroundAxis(lockTargetRot, limitRotY, 1);
                            lockTargetRot = ClampRotationAroundAxis(lockTargetRot, limitRotZ, 2);

                            mRootMultitool.GetComponent<Transform>().localRotation = lockTargetRot;
                            mRootLockRotate.GetComponent<Transform>().localRotation = lockTargetRot;
                            mRootLatchkey.GetComponent<Transform>().localRotation = lockTargetRot;

                            deltaTime += Time.deltaTime;
                            vsp = -vsp;

                            deltaAudioTime += Time.deltaTime;
                            if (deltaAudioTime > (a_AudioSource.clip.length - a_AudioSource.clip.length / 2.5f))
                            {
                                a_AudioSource.volume = 0.3f;
                                a_AudioSource.clip = AudioMultitool;
                                a_AudioSource.PlayOneShot(a_AudioSource.clip);
                                deltaAudioTime = 0.0f;
                            }
                        }
                        else
                        {
                            isBroken = true;
                        }
                    }
                }
            }
            else // The key is not pressed, we return to the original position
            {
                if (isActivePressW) // We enter here after the key was released - we need to reset some parameters
                {
                    zRot0 = 0.01f;
                    zRot1 = 0.01f;
                    zRot2 = 0.01f;
                    isActivePressW = false;
                    isToolMoving = false;
                    vsp = 1;
                }

                if (!isActivePressW)
                {
                    // As a result of twitching, an error accumulates and must be reset after the key has been released
                    if (isLockTwitching)
                    {
                        mRootMultitool.GetComponent<Transform>().localRotation = vspRootMultitool;
                        mRootLockRotate.GetComponent<Transform>().localRotation = vspRootLockRotate;
                        isLockTwitching = false;
                    }

                    if (mRootMultitool.GetComponent<Transform>().localRotation.z <= 0 && !isLockTwitching)  // We need to check whether we have gone abroad, i.e. whether our tool has dropped below its original position (it is also worth checking - if we enter here after twitching, it is necessary to return the tool to its original floor)
                    {
                        isToolReturned = false;

                        zRot1 += speedCoefficient * sensitivityLatchkeyRot.x;

                        lockTargetRot = mRootMultitool.GetComponent<Transform>().localRotation;
                        lockTargetRot *= Quaternion.Euler(0.0f, 0.0f, zRot1);
                        lockTargetRot = ClampRotationAroundAxis(lockTargetRot, limitRotX, 0);
                        lockTargetRot = ClampRotationAroundAxis(lockTargetRot, limitRotY, 1);
                        lockTargetRot = ClampRotationAroundAxis(lockTargetRot, limitRotZ, 2);

                        mRootMultitool.GetComponent<Transform>().localRotation = lockTargetRot;
                        mRootLockRotate.GetComponent<Transform>().localRotation = lockTargetRot;
                        mRootLatchkey.GetComponent<Transform>().localRotation = lockTargetRot;

                        latchkeyTargetRot = mLatchkeyRotate.GetComponent<Transform>().localRotation;
                        latchkeyTargetRot *= Quaternion.Euler(0.0f, 0.0f, -zRot1);
                        latchkeyTargetRot = ClampRotationAroundAxis(latchkeyTargetRot, limitRotX, 0);
                        latchkeyTargetRot = ClampRotationAroundAxis(latchkeyTargetRot, limitRotY, 1);
                        latchkeyTargetRot = ClampRotationAroundAxis(latchkeyTargetRot, limitLatchkeyRotZ, 2);

                        mLatchkeyRotate.GetComponent<Transform>().localRotation = latchkeyTargetRot;

                        deltaAudioTime += Time.deltaTime;
                        if (deltaAudioTime > a_AudioSource.clip.length)
                        {
                            a_AudioSource.volume = 0.5f;
                            a_AudioSource.clip = AudioMultitool;
                            a_AudioSource.PlayOneShot(a_AudioSource.clip);
                            deltaAudioTime = 0.0f;
                        }
                    }
                    else
                    {
                        isToolReturned = true;
                        canToolMoving = false;
                        canLatchkeyTwitching = false;
                    }
                }
            }
        }

        #endregion

        #region Helper

        private Quaternion ClampRotationAroundAxis(Quaternion q, Vector2 Limit, int AxisIndex)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angle = 0.0f;

            if (AxisIndex == 0) // X
            {
                angle = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
                angle = Mathf.Clamp(angle, Limit.x, Limit.y);
                q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angle);
            }
            else if (AxisIndex == 1) // Y
            {
                angle = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.y);
                angle = Mathf.Clamp(angle, Limit.x, Limit.y);
                q.y = Mathf.Tan(0.5f * Mathf.Deg2Rad * angle);
            }
            else if (AxisIndex == 2) // Z
            {
                angle = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.z);
                angle = Mathf.Clamp(angle, Limit.x, Limit.y);
                q.z = Mathf.Tan(0.5f * Mathf.Deg2Rad * angle);
            }

            return q;
        }

        #endregion

        #endregion
    }
}