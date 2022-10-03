using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace.UI
{
    public class StartGameBehavior : MonoBehaviour
    {
        // todo: improve level selection, not just same start.
        // todo: scene transition out of menu
        
        [SerializeField] private string sceneName;
        
        public void StartGame()
        {
            SceneManager.LoadScene(sceneName);
            Debug.LogError("Game loading doesn't allow level selection! Check todo.");
            Debug.LogError("Scene transitions don't work yet! Check todo.");
        }
    }
}