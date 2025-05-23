using System;

namespace Enemy.States
{
    [Serializable]
    public enum EnemyStates
    {
        Chase,
        Patrol,
        Attack,
        AttackAlignment,
        Die,
    }
}