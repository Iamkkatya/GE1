using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamekit3D
{
    public class ChomperSMBFall : SceneLinkedSMB<ChomperBehavior>
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Перевірка наявності MonoBehaviour та контролера
            if (m_MonoBehaviour == null)
            {
                Debug.LogError("m_MonoBehaviour is null! Cannot add force.");
                return;
            }

            if (m_MonoBehaviour.controller == null)
            {
                Debug.LogError("Controller is null! Cannot add force.");
                return;
            }

            // Додаємо нульову силу, щоб активувати Rigidbody
            m_MonoBehaviour.controller.AddForce(Vector3.zero);
        }

        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Перевірка наявності MonoBehaviour та контролера перед очищенням сили
            if (m_MonoBehaviour != null && m_MonoBehaviour.controller != null)
            {
                m_MonoBehaviour.controller.ClearForce();
            }
            else
            {
                Debug.LogWarning("Cannot clear force: m_MonoBehaviour or controller is null.");
            }
        }
    }
}
