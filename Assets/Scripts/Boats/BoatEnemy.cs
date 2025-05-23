using Enemy;
using Enemy.States;
using UnityEngine;

namespace Boats
{
    public class BoatEnemy : BoatController
    {
        [SerializeField] private EnemyStateMachine enemyStateMachine;
        [SerializeField] private Transform target;
        [SerializeField] private float distanceToChase = 10f;
        [SerializeField] private float distanceToAttack = 2f;
        [SerializeField] private float distanceToAlign = 5f;

        private void Start()
        {
            enemyStateMachine.Start();
        }

        private void Update()
        {
            float remainingDistance = Vector3.Distance(transform.position, target.position);

            if (remainingDistance < distanceToAlign)
            {
                enemyStateMachine.ChangeState(EnemyStates.AttackAlignment);
            }
            else if (remainingDistance < distanceToAttack)
            {
                enemyStateMachine.ChangeState(EnemyStates.Attack);
            }
            else if (remainingDistance < distanceToChase)
            {
                enemyStateMachine.ChangeState(EnemyStates.Chase);
            }
            else
            {
                enemyStateMachine.ChangeState(EnemyStates.Patrol);
            }

            enemyStateMachine.Update();
        }
    }
}