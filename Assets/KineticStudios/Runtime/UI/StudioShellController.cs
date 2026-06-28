using UnityEngine;
using UnityEngine.UIElements;

namespace KineticStudios.UserInterface
{
    /// <summary>
    /// Binds presentation-only state for the Phase 1 desktop studio shell.
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public sealed class StudioShellController : MonoBehaviour
    {
        private const string ReadyStatus = "STUDIO READY";

        private void OnEnable()
        {
            UIDocument document = GetComponent<UIDocument>();
            Label status = document.rootVisualElement.Q<Label>("status-label");

            if (status != null)
            {
                status.text = ReadyStatus;
            }
        }
    }
}
