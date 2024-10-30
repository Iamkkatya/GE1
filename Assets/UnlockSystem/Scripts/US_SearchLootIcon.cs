using UnityEngine;
using UnityEngine.UI;

namespace UnlockSystem
{
    public class US_SearchLootIcon : MonoBehaviour
    {
        #region Attributes 

        [Header("ATTRIBUTES")]
        [SerializeField] private Material m_Material;

        [Header("TYPE")]
        [SerializeField] private bool IsPercente = false;
        [SerializeField] private bool IsArrow = false;

        [Header("TIMERS")]
        [SerializeField] private bool IsTimer = false;
        [SerializeField] private bool IsMiliTimer = false;
        [SerializeField] private int StartSecond = 0;

        [Header("textures ATTRIBUTES")]
        [SerializeField] private int colCount = 4;
        [SerializeField] private int rowCount = 4;

        [Header("animation ATTRIBUTES")]
        [SerializeField] private int rowNumber = 0; //Zero Indexed
        [SerializeField] private int colNumber = 0; //Zero Indexed
        [SerializeField] private int totalCells = 4;
        [SerializeField] private float fps = 10;
        [SerializeField] private float speed = 0.5f;

        public bool isPaused { get; set; }

        private bool isLaunch = false;
        private float time = 0f;

        #endregion

        private void OnEnable()
        {
            if (IsPercente || IsArrow)
            {
                time = 0.0f;
                isPaused = false;
            }
            else
            {
                if (US_UnlockSystem.instance.usLevelOfDifficulty == US_PhoneScript.LevelOfDifficulty.EASY)
                    StartSecond = 10;
                else if (US_UnlockSystem.instance.usLevelOfDifficulty == US_PhoneScript.LevelOfDifficulty.MEDIUM)
                    StartSecond = 5;
                else if (US_UnlockSystem.instance.usLevelOfDifficulty == US_PhoneScript.LevelOfDifficulty.HARD)
                    StartSecond = 0;

                time = IsTimer ? StartSecond : 0.0f;

                SetSpriteAnimation(colCount, rowCount, rowNumber, colNumber, totalCells, fps);
            }
        }

        private void Start()
        {
            GetComponent<Image>().material = Instantiate(m_Material);
            GetComponent<Image>().material.name = gameObject.name;

            ResetMaterial();
        }

        private void Update()
        {
            if (IsTimer && !isLaunch)
            {
                isLaunch = true;
                time = StartSecond;

                SetSpriteAnimation(colCount, rowCount, rowNumber, colNumber, totalCells, fps);
            }

            if (!isPaused)
            {
                SetSpriteAnimation(colCount, rowCount, rowNumber, colNumber, totalCells, fps);
            }

            if (IsTimer)
            {
                if (time >= totalCells - 1)
                    US_PhoneScript.instance.weLost = true;
            }
        }

        #region PUBLIC

        public void ResetMaterial()
        {
            time = 0f;
            isLaunch = false;

            // Size of every cell
            float sizeX = 1.0f / colCount;
            float sizeY = 1.0f / rowCount;
            Vector2 size = new Vector2(sizeX, sizeY);

            // split into horizontal and vertical index
            var uIndex = 0 % colCount;
            var vIndex = 0 / colCount;

            // build offset
            // v coordinate is the bottom of the image in opengl so we need to invert.
            float offsetX = (uIndex + colNumber) * size.x;
            float offsetY = (1.0f - size.y) - (vIndex + rowNumber) * size.y;
            Vector2 offset = new Vector2(offsetX, offsetY);

            GetComponent<Image>().material.SetTextureOffset("_MainTex", offset);
            GetComponent<Image>().material.SetTextureScale("_MainTex", size);
        }

        public void PauseAction()
        {
            isPaused = true;

            if (IsMiliTimer)
                time = 0.0f;
            else if (IsTimer)
                time = 23.0f;

            SetSpriteAnimation(colCount, rowCount, rowNumber, colNumber, totalCells, fps);
        }

        #endregion

        #region PRIVATE

        private void SetSpriteAnimation(int colCount, int rowCount, int rowNumber, int colNumber, int totalCells, float fps)
        {
            // Calculate index
            int index = 0;

            index = (int)(time * fps);
            time += Time.deltaTime * speed;

            // Repeat when exhausting all cells
            index = index % totalCells;

            // Size of every cell
            float sizeX = 1.0f / colCount;
            float sizeY = 1.0f / rowCount;
            Vector2 size = new Vector2(sizeX, sizeY);

            // split into horizontal and vertical index
            var uIndex = index % colCount;
            var vIndex = index / colCount;

            // build offset
            // v coordinate is the bottom of the image in opengl so we need to invert.
            float offsetX = (uIndex + colNumber) * size.x;
            float offsetY = (1.0f - size.y) - (vIndex + rowNumber) * size.y;
            Vector2 offset = new Vector2(offsetX, offsetY);

            GetComponent<Image>().material.SetTextureOffset("_MainTex", offset);
            GetComponent<Image>().material.SetTextureScale("_MainTex", size);

            if (index == (totalCells - 1) && IsPercente)
            {
                isPaused = true;
                US_PhoneScript.instance.canStartInput = true;
            }
        }

        #endregion
    }
}