using UnityEngine;
using UnityEngine.AI;

namespace Enemy.States
{
    public class PatrolState : BaseState
    {
        [SerializeField] private int currentWayPointIndex;
        [SerializeField] private Transform[] wayPoints;
        [SerializeField] private NavMeshAgent navMeshAgent;

        public override void EnterState()
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(wayPoints[currentWayPointIndex].position);
        }

        public override void UpdateState()
        {
            float remainingDistance = Vector3.Distance(transform.position, wayPoints[currentWayPointIndex].position);
            if(remainingDistance < 0.5f)
            {
                currentWayPointIndex = currentWayPointIndex == (wayPoints.Length - 1) ? 0 : currentWayPointIndex + 1;
                navMeshAgent.SetDestination(wayPoints[currentWayPointIndex].position);
            }
        }

        public override void ExitState()
        {
            navMeshAgent.isStopped = true;
        }
    }
}