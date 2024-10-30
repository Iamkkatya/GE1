using UnityEngine;

namespace UnlockSystem
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(AudioSource))]
    public class US_FPSController : MonoBehaviour
    {
        #region Attributes

        [SerializeField] private bool m_IsWalking = false;
        [SerializeField] private float m_JumpSpeed = 10f;
        [SerializeField] private float m_WalkSpeed = 5f;
        [SerializeField] private float m_RunSpeed = 10f;
        [SerializeField] private float m_StickToGroundForce = 9.81f;
        [SerializeField] private float m_GravityMultiplier = 2f;
        [SerializeField] private usFPSMouseLook m_FPSMouseLook;

        private bool previouslyGrounded;
        private Vector3 moveDir = Vector3.zero;
        private Vector2 input;
        private bool jump;
        private bool jumping;
        private Camera mainCamera;
        private CharacterController characterController;
        private CollisionFlags collisionFlags;

        #endregion

        private void Start()
        {
            characterController = GetComponent<CharacterController>();
            mainCamera = Camera.main;
            jumping = false;
            m_FPSMouseLook.InitMouseLook(transform, mainCamera.transform);
        }

        private void FixedUpdate()
        {
            float speed;
            GetInput(out speed);
            Vector3 desiredMove = transform.forward * input.y + transform.right * input.x;
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out hitInfo, characterController.height / 2f, ~0, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            moveDir.x = desiredMove.x * speed;
            moveDir.z = desiredMove.z * speed;

            if (characterController.isGrounded)
            {
                moveDir.y = -m_StickToGroundForce;

                if (jump)
                {
                    moveDir.y = m_JumpSpeed;
                    jump = false;
                    jumping = true;
                }
            }
            else
            {
                moveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
            }
            collisionFlags = characterController.Move(moveDir * Time.fixedDeltaTime);
        }

        private void Update()
        {
            m_FPSMouseLook.LookRotationMouse(transform, mainCamera.transform);
            if (!jump)
            {
                jump = Input.GetButtonDown("Jump");
            }
            if (!previouslyGrounded && characterController.isGrounded)
            {
                moveDir.y = 0f;
                jumping = false;
            }
            if (!characterController.isGrounded && !jumping && previouslyGrounded)
            {
                moveDir.y = 0f;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            previouslyGrounded = characterController.isGrounded;
        }

        #region PRIVATE

        private void GetInput(out float speed)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

#if !MOBILE_INPUT
            m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
#endif
            speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
            input = new Vector2(horizontal, vertical);

            if (input.sqrMagnitude > 1)
            {
                input.Normalize();
            }
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            if (collisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(characterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
        }

        #endregion
    }
}