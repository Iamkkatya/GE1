using UnityEngine;
using UnityEngine.UI;

namespace UnlockSystem
{
    public class US_ButtonScan : MonoBehaviour
    {
        #region Attributes

        [Header("ATTRIBUTES")]
        [SerializeField] private float speedPulse = 0.6f;
        [SerializeField] private Vector2 pulsingField = new Vector2(0.6f, 1.0f);

        [Header("REFERENCES")]
        [SerializeField] private Image iconReference;
        [SerializeField] private Button buttonReference;

        public bool isActivePulsing { get; set; }

        private Color defaultColor;

        #endregion

        private void Start()
        {
            buttonReference.enabled = false;

            defaultColor = iconReference.color;
            defaultColor.a = 1.0f;
        }

        private void Update()
        {
            if (isActivePulsing)
            {
                int pulseDirection = 1;

                if (defaultColor.a <= pulsingField.x)
                    pulseDirection = -1;
                else if (defaultColor.a >= pulsingField.y)
                    pulseDirection = 1;

                defaultColor.a -= Time.deltaTime * speedPulse * pulseDirection;

                iconReference.color = defaultColor;
            }
        }

        #region PUBLIC

        /// <summary>
        /// Deactivate button pulsing and reset button color
        /// </summary>
        public void StopPulse()
        {
            isActivePulsing = false;

            defaultColor.a = pulsingField.y;
            iconReference.color = defaultColor;
        }

        #endregion
    }
}