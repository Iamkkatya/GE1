using UnityEngine;

namespace UnlockSystem
{
    public class US_PlayerItems : MonoBehaviour
    {
        #region Attributes

        [Header("ATTRIBUTES")]
        [SerializeField] private int amountMultools = 2;

        public static US_PlayerItems instance { get; private set; }

        #endregion

        private void Start()
        {
            instance = this;
        }

        #region FUNCTIONS

        public int AmountMultools
        {
            get => amountMultools;
            set
            {
                if (value >= 0)
                    amountMultools = value;
                else
                    amountMultools = 0;
            }
        }

        #endregion
    }
}