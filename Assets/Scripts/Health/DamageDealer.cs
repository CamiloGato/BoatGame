using UnityEngine;

namespace Health
{
    public class DamageDealer : MonoBehaviour
    {
        public int damage = 10;
        public GameObject ownerGo;

        public void OnTriggerEnter(Collider other)
        {
            if(other.gameObject == ownerGo) return;
        
            if(other.TryGetComponent(out DamageReceiver danageReceiver))
            {
                danageReceiver.SetDamage(damage);
            }
        }
    }
}
