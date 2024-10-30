using UnityEngine;

namespace UnlockSystem
{
    public class US_SwayCamera : MonoBehaviour
    {
        #region Attributes

        [Header("ATTRIBUTES")]
        [SerializeField] private float amount = 5f;
        [SerializeField] private float maxAmount = 10f;

        private float smooth = 3f; // smooth mouse look
        private Vector3 defaultPosition;

        #endregion

        private void Start()
        {
            defaultPosition = transform.localPosition;
        }

        private void Update()
        {
            float factorX = -Input.GetAxis("Mouse X") * amount;
            float factorY = -Input.GetAxis("Mouse Y") * amount;

            if (factorX > maxAmount)
                factorX = maxAmount;
            else if (factorX < -maxAmount)
                factorX = -maxAmount;
            if (factorY > maxAmount)
                factorY = maxAmount;
            else if (factorY < -maxAmount)
                factorY = -maxAmount;

            Vector3 final = new Vector3(defaultPosition.x + factorX, defaultPosition.y + factorY, defaultPosition.z);
            transform.localPosition = Vector3.Lerp(transform.localPosition, final, Time.deltaTime * smooth);
        }
    }
}