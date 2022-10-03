using DefaultNamespace.Utility;

namespace DefaultNamespace.Level
{
    public abstract class LevelState : State
    {
        public LevelStateMachine StateMachine { get; set; }
    }
}