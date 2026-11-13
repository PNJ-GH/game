using UnityEngine;

namespace Lunara.Gameplay
{
    public sealed class LunaraDamageable : MonoBehaviour
    {
        [SerializeField] private int maximumHealth = 100;
        public int CurrentHealth { get; private set; }

        private void Awake() => CurrentHealth = maximumHealth;

        public void TakeDamage(int amount)
        {
            CurrentHealth = Mathf.Max(0, CurrentHealth - Mathf.Max(0, amount));
            if (CurrentHealth == 0) Destroy(gameObject);
        }
    }
}
