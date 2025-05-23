using System;
using Enemy.States;

namespace Enemy
{
    /// <summary>
    /// State machine for the enemy AI.
    /// </summary>
    [Serializable]
    public class EnemyStateMachine
    {
        // States for the enemy AI
        public ChaseState chaseState;
        public PatrolState patrolState;
        public AttackState attackState;
        public AttackAlignmentState attackAlignmentState;
        public DieState dieState;

        public BaseState currentStateInstance;
        public EnemyStates currentState = EnemyStates.Patrol;

        public void Start()
        {
            currentStateInstance = patrolState;
            currentStateInstance?.EnterState();
        }

        public void ChangeState(EnemyStates newState)
        {
            if (currentState == newState) return;

            currentStateInstance?.ExitState();

            currentState = newState;

            switch (currentState)
            {
                case EnemyStates.Chase:
                    currentStateInstance = chaseState;
                    break;
                case EnemyStates.Patrol:
                    currentStateInstance = patrolState;
                    break;
                case EnemyStates.Attack:
                    currentStateInstance = attackState;
                    break;
                case EnemyStates.AttackAlignment:
                    currentStateInstance = attackAlignmentState;
                    break;
            }

            currentStateInstance?.EnterState();
        }

        public void Update()
        {
            currentStateInstance?.UpdateState();
        }
    }
}