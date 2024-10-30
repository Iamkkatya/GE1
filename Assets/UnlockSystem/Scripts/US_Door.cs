using UnityEngine;

namespace UnlockSystem
{
    public class US_Door : MonoBehaviour
    {
        #region Attributes

        [Header("ATTRIBUTES")]
        [SerializeField] private float speedOpening = 50.0f;
        [SerializeField] private bool opened;
        [SerializeField] private bool closed = true;

        [Header("REFERENCES")]
        [SerializeField] private GameObject doorPivotPoint; // For rotation

        public bool hasBeenUnlocked { get; set; } // The door has been opened (unlocked)
        public bool opening { get; set; } // If we press "F" button when the door is closed
        public bool closing { get; set; } // If we press "F" button when the fdoor is opened

        private Vector3 defaultDoorAngle;

        #endregion

        private void Start()
        {
            defaultDoorAngle = doorPivotPoint.transform.localEulerAngles;
        }

        private void Update()
        {
            if (hasBeenUnlocked)
            {
                if (opening && !opened)
                    Opening();
                else if (closing && !closed)
                    Closing();
            }
        }

        #region FUNCTIONS

        public bool Opened
        {
            get => opened;
        }

        public bool Closed
        {
            get => closed;
        }

        #endregion

        #region PRIVATE

        private void Opening()
        {
            defaultDoorAngle.z += Time.deltaTime * speedOpening;

            doorPivotPoint.transform.localEulerAngles = defaultDoorAngle;

            if (defaultDoorAngle.z >= 120.0f)
            {
                opened = true;
                opening = false;
                closed = false;
            }
        }

        private void Closing()
        {
            if (!closed)
            {
                defaultDoorAngle.z -= Time.deltaTime * speedOpening;

                doorPivotPoint.transform.localEulerAngles = defaultDoorAngle;

                if (defaultDoorAngle.z <= 0.0f)
                {
                    opened = false;
                    closing = false;
                    closed = true;
                }
            }
        }

        #endregion
    }
}