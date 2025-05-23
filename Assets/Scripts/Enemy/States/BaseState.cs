using UnityEngine;

namespace Enemy.States
{
    /// <summary>
    /// Base class for all enemy states.
    /// Contains abstract methods for entering, updating, and exiting states.
    /// </summary>
    public abstract class BaseState : MonoBehaviour
    {
        /// <summary>
        /// Defines the behavior to execute when entering a specific state in the enemy's state machine.
        /// </summary>
        public abstract void EnterState();
        /// <summary>
        /// Defines the behavior to execute while in a specific state in the enemy's state machine.
        /// </summary>
        public abstract void UpdateState();
        /// <summary>
        /// Executes the behavior to perform when exiting a specific state in the enemy's state machine.
        /// </summary>
        public abstract void ExitState();
    }
}