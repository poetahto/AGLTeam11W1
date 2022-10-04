using System;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace.Utility
{
    public abstract class StateMachine : MonoBehaviour
    {
        public State CurrentState { get; private set; }
        public State InitialState { get; set; }

        protected abstract State GetDefaultInitialState();
        
        private void Start()
        {
            InitialState ??= GetDefaultInitialState();
            TransitionTo(InitialState);
        }

        private void Update()
        {
            CurrentState?.OnUpdate();
        }

        public void TransitionTo(State state)
        {
            if (CurrentState == state)
                return;
            
            CurrentState?.OnExit();
            CurrentState = state;
            CurrentState?.OnEnter();
        }
    }

    [Serializable]
    public abstract class State
    {
        public UnityEvent onEnter;
        public UnityEvent onExit;
        
        public virtual void OnEnter()  { onEnter.Invoke(); }
        public virtual void OnExit()   { onExit.Invoke(); }
        public virtual void OnUpdate() {}
    }
}