using UnityEngine;

namespace UnlockSystem
{
    public class US_RotateCapOfBox : MonoBehaviour
    {
        #region Attributes

        [Header("ATTRIBUTES")]
        [SerializeField] private float amount = 5f;
        [SerializeField] private float maxAmount = 10f;

        private float smooth = 3f; // smooth mouse look
        private Vector3 defaultAngle;

        #endregion

        private void Start()
        {
            defaultAngle = transform.localEulerAngles;
        }

        private void Update()
        {
            float factorX = -Input.GetAxis("Mouse X") * amount;

            if (factorX > maxAmount)
                factorX = maxAmount;
            else if (factorX < -maxAmount)
                factorX = -maxAmount;

            Vector3 final = new Vector3(defaultAngle.x + factorX, -90.0f, 90.0f);
            transform.localEulerAngles = Vector3.Lerp(new Vector3(transform.localEulerAngles.x, -90.0f, 90.0f), final, Time.deltaTime * smooth);
        }
    }
}