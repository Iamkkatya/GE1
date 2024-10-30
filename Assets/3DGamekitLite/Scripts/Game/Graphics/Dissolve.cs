using UnityEngine;

namespace Gamekit3D
{
    public class Dissolve : MonoBehaviour
    {
        public float minStartTime = 2f; // Мінімальний час до початку розчинення
        public float maxStartTime = 6f; // Максимальний час до початку розчинення
        public float dissolveTime = 3f; // Час розчинення
        public AnimationCurve curve; // Крива анімації
        public GameObject parentObjectToDestroy; // Батьківський об'єкт для знищення

        private float m_Timer; // Таймер для розчинення
        private float m_EmissionRate; // Ставка емісії часток
        private Renderer[] m_Renderer; // Масив рендерерів
        private MaterialPropertyBlock m_PropertyBlock; // Блок властивостей матеріалу
        private ParticleSystem m_ParticleSystem; // Часткова система
        private ParticleSystem.EmissionModule m_Emission; // Модуль емісії часток
        private float m_StartTime; // Час початку розчинення
        private float m_EndTime; // Час завершення розчинення

        private const string k_CutoffName = "_Cutoff"; // Ім'я шейдерного параметра

        void Awake()
        {
            m_PropertyBlock = new MaterialPropertyBlock();
            m_Renderer = GetComponentsInChildren<Renderer>();

            m_ParticleSystem = GetComponentInChildren<ParticleSystem>();

            if (m_ParticleSystem != null)
            {
                m_Emission = m_ParticleSystem.emission;
                m_EmissionRate = m_Emission.rateOverTime.constant;
                m_Emission.rateOverTimeMultiplier = 0;
            }
            else
            {
                Debug.LogWarning("No ParticleSystem found in children of " + gameObject.name);
            }

            m_Timer = 0;

            // Визначення часу початку та завершення
            m_StartTime = Time.time + Random.Range(minStartTime, maxStartTime);
            m_EndTime = m_StartTime + dissolveTime + (m_ParticleSystem != null ? m_ParticleSystem.main.startLifetime.constant : 0);
        }

        void Update()
        {
            // Перевірка часу для початку розчинення
            if (Time.time >= m_StartTime)
            {
                float cutoff = 0;

                // Оновлення властивостей матеріалу для всіх рендерерів
                for (int i = 0; i < m_Renderer.Length; i++)
                {
                    m_Renderer[i].GetPropertyBlock(m_PropertyBlock);
                    cutoff = Mathf.Clamp01(m_Timer / dissolveTime);
                    m_PropertyBlock.SetFloat(k_CutoffName, cutoff);
                    m_Renderer[i].SetPropertyBlock(m_PropertyBlock);
                }

                // Оновлення ставки емісії часток
                if (m_ParticleSystem != null)
                {
                    m_Emission.rateOverTimeMultiplier = curve.Evaluate(cutoff) * m_EmissionRate;
                }

                m_Timer += Time.deltaTime; // Оновлення таймера
            }

            // Якщо час завершення розчинення досягнуто
            if (Time.time >= m_EndTime)
            {
                if (parentObjectToDestroy != null) // Перевірка, чи вказано батьківський об'єкт
                {
                    Debug.Log("Destroying parent object: " + parentObjectToDestroy.name);
                    Destroy(parentObjectToDestroy); // Знищення батьківського об'єкта
                }
                else
                {
                    Debug.LogWarning("No parent object assigned for destruction!");
                }
            }
        }
    }
}
