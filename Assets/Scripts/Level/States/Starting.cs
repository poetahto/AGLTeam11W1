using System;
using UnityEngine;

namespace DefaultNamespace.Level
{
    [Serializable]
    public class Starting : LevelState
    {
        [SerializeField] private GameObject introText;

        public override void OnEnter()
        {
            base.OnEnter();
            introText.SetActive(true);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (Input.anyKey)
            {
                // spawn player
                StateMachine.TransitionTo(StateMachine.RunningState);
            }
        }
    }
}