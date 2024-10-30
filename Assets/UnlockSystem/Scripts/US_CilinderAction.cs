using System.Collections;
using UnityEngine;

namespace UnlockSystem
{
    public class US_CilinderAction : MonoBehaviour
    {
        #region Attributes

        [Header("ATTRIBUTES")]
        [SerializeField] private float speedMoving = 0.04f;
        [SerializeField] private float defaultPositionX;

        [SerializeField] private float leftPosition = -0.11f;

        private Vector3 defaultPosition;

        #endregion

        private void Start()
        {
            defaultPosition = transform.localPosition;

            ResetPosition();
        }

        private void OnDisable()
        {
            ResetPosition();
        }

        #region PUBLIC

        /// <summary>
        /// Initialize lock cylinders moving (closing)
        /// </summary>
        /// <returns></returns>
        public IEnumerator InitMoving()
        {
            bool canStopMoving = false;
            while (!canStopMoving)
            {
                Vector3 vsp = transform.localPosition;
                vsp.x -= Time.deltaTime * speedMoving;
                transform.localPosition = vsp;

                if (Mathf.Abs(vsp.x) >= leftPosition)
                    canStopMoving = true;

                yield return null;
            };
        }

        #endregion

        #region PRIVATE

        private void ResetPosition()
        {
            Vector3 vsp = transform.localPosition;
            vsp.x = defaultPositionX;
            transform.localPosition = vsp;
        }

        #endregion
    }
}