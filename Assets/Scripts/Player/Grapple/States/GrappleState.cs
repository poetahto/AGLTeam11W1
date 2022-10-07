using UnityEngine;
using UnityEngine.Events;

namespace Player.Grapple
{
    public abstract class GrappleState
    {
        public GrappleStateMachine StateMachine { get; set; }

        public UnityEvent onEnter;
        public UnityEvent onExit;
        
        public virtual void OnEnter() { onEnter.Invoke(); }
        public virtual void Update()  {}
        public virtual void OnExit()  { onExit.Invoke(); }

        protected Transform PlayerTransform => StateMachine.playerTransform;
        protected Transform GrappleTransform => StateMachine.grappleTransform;

        // Common Utility Functions
        
        protected Ray2D GetAimRay()
        {
            Vector2 playerPosition = PlayerTransform.position;
            Vector2 aimPosition = StateMachine.Camera.ScreenToWorldPoint(Input.mousePosition);
            
            return new Ray2D(playerPosition, (aimPosition - playerPosition).normalized);
        }
    }
}