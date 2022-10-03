using DefaultNamespace.Utility;
using UnityEngine;

namespace DefaultNamespace.Level
{
    public class LevelStateMachine : StateMachine
    {
        [SerializeField] private Starting startingState;
        [SerializeField] private Running runningState;
        [SerializeField] private Paused pausedState;
        [SerializeField] private Restarting restartingState;
        [SerializeField] private Completed completedState;

        public LevelState StartingState => startingState;
        public LevelState RunningState => runningState;
        public LevelState PausedState => pausedState;
        public LevelState RestartingState => restartingState;
        public LevelState CompletedState => completedState;

        protected override State GetDefaultInitialState()
        {
            return StartingState;
        }

        private void Awake()
        {
            StartingState.StateMachine = this;
            RunningState.StateMachine = this;
            PausedState.StateMachine = this;
            RestartingState.StateMachine = this;
            CompletedState.StateMachine = this;
        }
    }
}