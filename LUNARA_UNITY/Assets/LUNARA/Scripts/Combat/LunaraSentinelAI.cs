using UnityEngine;

namespace Lunara.Gameplay
{
    [RequireComponent(typeof(LunaraDamageable))]
    public sealed class LunaraSentinelAI : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float detectionRange = 22f;
        [SerializeField] private float attackRange = 2.2f;
        [SerializeField] private float moveSpeed = 2.8f;
        [SerializeField] private float attackInterval = 1.4f;
        [SerializeField] private int damage = 8;
        private float nextAttack;

        private void Update()
        {
            if (target == null) return;
            Vector3 delta = target.position - transform.position;
            delta.y = 0f;
            float distance = delta.magnitude;
            if (distance > detectionRange || distance < 0.01f) return;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(delta.normalized), Time.deltaTime * 5f);
            if (distance > attackRange)
                transform.position += delta.normalized * (moveSpeed * Time.deltaTime);
            else if (Time.time >= nextAttack)
            {
                nextAttack = Time.time + attackInterval;
                target.GetComponent<LunaraPlayerController>()?.TakeDamage(damage);
            }
        }

        public void SetTarget(Transform player) => target = player;
    }
}
