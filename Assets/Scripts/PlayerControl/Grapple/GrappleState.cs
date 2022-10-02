namespace PlayerControl
{
    public abstract class GrappleState
    {
        public GrappleStateMachine StateMachine { get; set; }
        
        public virtual void OnEnter() {}
        public virtual void Update() {}
        public virtual void OnExit() {}
    }
}