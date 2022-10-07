using System;
using System.Collections;
using DefaultNamespace.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace.Level
{
    [Serializable]
    public class Restarting : LevelState
    {
        [SerializeField] private float restartTime = 3;
        [SerializeField] private WipeEffect wipeEffect;
        
        private float _elapsedTime;
        private bool _isRestarting;

        // reload level, transition animation, go straight into running
        public override void OnEnter()
        {
            base.OnEnter();
            _elapsedTime = 0;
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();

            if (_elapsedTime > restartTime && !_isRestarting)
            {
                _isRestarting = true;
                WipeEffect.Instance.StartCoroutine(SceneTransitionCoroutine());
            }

            _elapsedTime += Time.deltaTime;
        }

        [SerializeField] private float transitionOutTime;
        [SerializeField] private float transitionInTime;

        private IEnumerator SceneTransitionCoroutine()
        {
            yield return WipeEffect.Instance.ShowEffect(transitionInTime);
            yield return SceneManager.LoadSceneAsync(StateMachine.gameObject.scene.name);
            yield return WipeEffect.Instance.HideEffect(transitionOutTime);
        }
    }
}