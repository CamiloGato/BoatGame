using UnityEngine;
using UnityEngine.AI;

namespace Enemy.States
{
    public class AttackState : BaseState
    {
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private Transform target;
        [SerializeField] private float attackDistance = 2f;
        [SerializeField] private float attackSpeed = 1.5f;

        private Vector3 _attackPointOffset;

        public override void EnterState()
        {
            navMeshAgent.speed = attackSpeed;
            Vector3 rightOffset = target.right * attackDistance;
            Vector3 leftOffset = target.right * attackDistance;
            float rightDistance = Vector3.Distance(transform.position, target.position + rightOffset);
            float leftDistance = Vector3.Distance(transform.position, target.position - leftOffset);
            _attackPointOffset = rightDistance < leftDistance ? rightOffset : leftOffset;
            navMeshAgent.isStopped = false;
        }

        public override void UpdateState()
        {
            navMeshAgent.SetDestination(target.position + _attackPointOffset);
        }

        public override void ExitState()
        {
            navMeshAgent.isStopped = true;
        }
    }
}