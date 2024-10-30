using System.Collections;
using UnityEngine;

namespace UnlockSystem
{
    public class US_LightsSwitcher : MonoBehaviour
    {
        #region Attributes

        [Header("LIGHT")]
        [SerializeField] private GameObject Red;
        [SerializeField] private GameObject Green;
        [SerializeField] private GameObject Blue;
        [SerializeField] private GameObject White;

        [Header("ATTRIBUTES")]
        [SerializeField] private float fps = 1.0f;

        private bool isActive;
        private float timer = 0.0f;
        private int activeLightIndex = 0;

        #endregion

        private void OnEnable()
        {
            isActive = true;
            StartCoroutine(InitLightSwitcher());
        }

        private void OnDisable()
        {
            isActive = false;
            timer = 0.0f;
            activeLightIndex = 0;
        }

        #region PRIVATE

        private IEnumerator InitLightSwitcher()
        {
            yield return null;

            while(isActive)
            {
                if (timer >= fps)
                {
                    SetActiveLight();

                    timer = 0.0f;
                }

                timer += Time.deltaTime;

                yield return null;
            }
        }

        private void SetActiveLight()
        {
            if (activeLightIndex == 0)
            {
                Red.SetActive(true);
                Green.SetActive(false);
                Blue.SetActive(false);
                White.SetActive(false);
                activeLightIndex = 1;
            }
            else if (activeLightIndex == 1)
            {
                Red.SetActive(false);
                Green.SetActive(true);
                Blue.SetActive(false);
                White.SetActive(false);
                activeLightIndex = 2;
            }
            else if (activeLightIndex == 2)
            {
                Red.SetActive(false);
                Green.SetActive(false);
                Blue.SetActive(true);
                White.SetActive(false);
                activeLightIndex = 3;
            }
            else if (activeLightIndex == 3)
            {
                Red.SetActive(false);
                Green.SetActive(false);
                Blue.SetActive(false);
                White.SetActive(true);
                activeLightIndex = 0;
            }
        }

        #endregion
    }
}