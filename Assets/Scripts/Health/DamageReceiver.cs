using UnityEngine;
using UnityEngine.Events;

namespace Health
{
    public class DamageReceiver : MonoBehaviour
    {
        public int health = 100;
        private int _currentHealth;

        public UnityEvent onDeath;
    
        void Start()
        {
            _currentHealth = health;
        }

        public void SetDamage(int damage)
        {
            if(_currentHealth == 0) return;
            _currentHealth -= damage;
            _currentHealth = Mathf.Max(_currentHealth, 0);

            if (_currentHealth == 0)
            {
                onDeath.Invoke();
            }
        }

        [ContextMenu("Test Set Damage")]
        public void SetDamageTest(){
            SetDamage(50);
        }
    }
}
