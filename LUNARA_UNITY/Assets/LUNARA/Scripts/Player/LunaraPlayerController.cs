using UnityEngine;

namespace Lunara.Gameplay
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class LunaraPlayerController : MonoBehaviour
    {
        [SerializeField] private float walkSpeed = 6f;
        [SerializeField] private float sprintSpeed = 9f;
        [SerializeField] private float jumpHeight = 1.6f;
        [SerializeField] private float gravity = -22f;
        [SerializeField] private Transform cameraPivot;
        [SerializeField] private float mouseSensitivity = 2f;
        [SerializeField] private float attackRange = 3f;
        [SerializeField] private int attackDamage = 25;
        [SerializeField] private float attackCooldown = 0.55f;

        private CharacterController controller;
        private Camera viewCamera;
        private float verticalSpeed;
        private float pitch;
        private float nextAttack;
        public int Health { get; private set; } = 100;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            viewCamera = GetComponentInChildren<Camera>();
            if (cameraPivot == null && viewCamera != null)
                cameraPivot = viewCamera.transform.parent;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            HandleLook();
            HandleMovement();
            if (Input.GetMouseButtonDown(0)) Attack();
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                bool wasLocked = Cursor.lockState == CursorLockMode.Locked;
                Cursor.lockState = wasLocked ? CursorLockMode.None : CursorLockMode.Locked;
                Cursor.visible = wasLocked;
            }
        }

        private void HandleLook()
        {
            if (Cursor.lockState != CursorLockMode.Locked || cameraPivot == null) return;
            transform.Rotate(Vector3.up * (Input.GetAxisRaw("Mouse X") * mouseSensitivity));
            pitch = Mathf.Clamp(pitch - Input.GetAxisRaw("Mouse Y") * mouseSensitivity, -35f, 65f);
            cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }

        private void HandleMovement()
        {
            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            input = Vector3.ClampMagnitude(input, 1f);
            Vector3 direction = transform.TransformDirection(input);
            float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

            if (controller.isGrounded && verticalSpeed < 0f) verticalSpeed = -2f;
            if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
                verticalSpeed = Mathf.Sqrt(jumpHeight * -2f * gravity);
            verticalSpeed += gravity * Time.deltaTime;

            Vector3 motion = direction * speed;
            motion.y = verticalSpeed;
            controller.Move(motion * Time.deltaTime);
        }

        private void Attack()
        {
            if (Time.time < nextAttack || viewCamera == null) return;
            nextAttack = Time.time + attackCooldown;
            if (Physics.Raycast(viewCamera.transform.position, viewCamera.transform.forward,
                out RaycastHit hit, attackRange))
            {
                LunaraDamageable target = hit.collider.GetComponentInParent<LunaraDamageable>();
                if (target != null) target.TakeDamage(attackDamage);
            }
        }

        public void TakeDamage(int amount) => Health = Mathf.Max(0, Health - Mathf.Max(0, amount));
    }
}
