using UnityEngine;
using UnityEngine.AI;

namespace Enemy.States
{
    public class AttackAlignmentState : BaseState
    {
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private float alignmentSpeed;
        [SerializeField] private Transform target;

        private Vector3 _initialPosition;
        private Vector3 _initialForward;
        private float _alignmentTimer;


        public override void EnterState()
        {
            navMeshAgent.isStopped = true;
            _alignmentTimer = 0f;
            _initialPosition = transform.position;
            _initialForward = transform.forward;
        }

        public override void UpdateState()
        {
            _alignmentTimer += Time.deltaTime;
            float time = _alignmentTimer / alignmentSpeed;

            transform.forward = Vector3.Slerp(_initialForward, target.forward, time);
        }

        public override void ExitState()
        {

        }
    }
}