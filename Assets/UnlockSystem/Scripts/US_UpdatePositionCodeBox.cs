using UnityEngine;
using UnityEngine.UI;

namespace UnlockSystem
{
    public class US_UpdatePositionCodeBox : MonoBehaviour // See the CodeBox prefab
    {
        #region Attributes

        [Header("ATTRIBUTES")]
        public Vector2 MaxDistance = new Vector2(-0.001f, 0.001f);

        [Header("COLORS")]
        [SerializeField] private Color baseColor;     // Исходный цвет кнопки
        [SerializeField] private Color pressColor;    // Цвет после нажатия
        [SerializeField] private Color lossColor;     // Цвет после того как время исчтечет все кнопки примут данный цвет

        public bool canStopTwitching { get; set; }
        public int codeNumber { get; set; } // The button code
        public bool isPressed { get; set; }
        public Vector3 defaultPosition { get; set; }

        #endregion

        private void Start()
        {
            GetComponentInChildren<Button>().enabled = false;
        }

        private void Update()
        {
            if (US_PhoneScript.instance.canActiveMoveCodeButtons && !canStopTwitching)
            {
                GetComponentInChildren<Button>().enabled = true;
                transform.localPosition = new Vector3(defaultPosition.x + Random.Range(MaxDistance.x, MaxDistance.y), defaultPosition.y + Random.Range(MaxDistance.x, MaxDistance.y), 0.0f);
            }
        }

        #region PUBLIC

        #region Button

        public void ButtonAction(bool state)
        {
            if (state && !isPressed)
            {
                US_PhoneScript.instance.codeBoxButtonPresses[US_PhoneScript.instance.countButtonPresses] = codeNumber; // сохраняем в массив код - встает в очередь
                US_PhoneScript.instance.countButtonPresses++; // увеличиваем счетчик нажатых кнопок
                GetComponentInChildren<Image>().color = pressColor; // Изменяем цвет
                GetComponentInChildren<Button>().enabled = false; // Отключаем ее
                canStopTwitching = true; // останавливаем дергани
                isPressed = true; // говорим, что нажата
            }
        }

        #endregion

        /// <summary>
        /// Reset the button parameters
        /// </summary>
        public void ResetAction()
        {
            GetComponentInChildren<Image>().color = baseColor;
            GetComponentInChildren<Button>().enabled = true;
            canStopTwitching = false;
            isPressed = false;
        }

        /// <summary>
        /// The time is up - the button is inactive
        /// </summary>
        public void LossAction()
        {
            GetComponentInChildren<Image>().color = lossColor;
            GetComponentInChildren<Button>().enabled = false;
            canStopTwitching = true;
            isPressed = true;
        }

        #endregion
    }
}