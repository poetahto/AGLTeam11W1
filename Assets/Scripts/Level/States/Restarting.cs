using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace.Level
{
    [Serializable]
    public class Restarting : LevelState
    {
        [SerializeField] private float restartTime = 3;
        
        private float _elapsedTime;

        // reload level, transition animation, go straight into running
        public override void OnEnter()
        {
            base.OnEnter();
            _elapsedTime = 0;
        }

        
        public override void OnUpdate()
        {
            base.OnUpdate();
            
            if (_elapsedTime > restartTime)
                SceneManager.LoadScene(StateMachine.gameObject.scene.name);

            _elapsedTime += Time.deltaTime;
        }
    }
}