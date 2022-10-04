using System;
using UnityEngine.SceneManagement;

namespace DefaultNamespace.Level
{
    [Serializable]
    public class Restarting : LevelState
    {
        // reload level, transition animation, go straight into running
        public override void OnEnter()
        {
            base.OnEnter();
            SceneManager.LoadScene(StateMachine.gameObject.scene.name);
        }
    }
}