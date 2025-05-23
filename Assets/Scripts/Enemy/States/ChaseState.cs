using UnityEngine;
using UnityEngine.AI;

namespace Enemy.States
{
    public class ChaseState : BaseState
    {
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private Transform target;
        [SerializeField] private float chaseSpeed = 3.5f;

        private Vector3 _stopOffset;

        public override void EnterState()
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = chaseSpeed;
        }

        public override void UpdateState()
        {
            navMeshAgent.SetDestination(target.position);
        }

        public override void ExitState()
        {
            navMeshAgent.isStopped = true;
        }
    }
}