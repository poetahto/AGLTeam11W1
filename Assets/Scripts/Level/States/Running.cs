using System;
using UnityEngine;

namespace DefaultNamespace.Level
{
    [Serializable]
    public class Running : LevelState
    {
        // check for pause input, deaths, restart requests, and winning

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (Input.GetKeyDown(KeyCode.Escape))
                StateMachine.TransitionTo(StateMachine.PausedState);
            
            if (Input.GetKeyDown(KeyCode.R))
                StateMachine.TransitionTo(StateMachine.RestartingState);
            
            // if victory
            // StateMachine.TransitionTo(StateMachine.CompletedState);
        }
    }
}