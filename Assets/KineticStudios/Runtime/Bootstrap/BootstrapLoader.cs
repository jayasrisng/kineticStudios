using UnityEngine;
using UnityEngine.SceneManagement;

namespace KineticStudios.Bootstrap
{
    /// <summary>
    /// Owns the application entry point and forwards into the studio workspace.
    /// </summary>
    public sealed class BootstrapLoader : MonoBehaviour
    {
        [SerializeField] private string workspaceSceneName = "StudioWorkspace";

        private void Start()
        {
            SceneManager.LoadScene(workspaceSceneName, LoadSceneMode.Single);
        }
    }
}
