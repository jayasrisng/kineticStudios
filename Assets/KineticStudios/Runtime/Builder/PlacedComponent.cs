using UnityEngine;

namespace KineticStudios.Builder
{
    public sealed class PlacedComponent : MonoBehaviour
    {
        [SerializeField] private StudioComponentType componentType;
        [SerializeField] private string displayName;
        [SerializeField] private PendulumAssembly owningPendulum;

        private Renderer[] renderers;

        public StudioComponentType ComponentType => owningPendulum == null
            ? componentType
            : StudioComponentType.Pendulum;

        public string DisplayName => owningPendulum == null ? displayName : owningPendulum.DisplayName;
        public PendulumAssembly OwningPendulum => owningPendulum;

        public void Configure(StudioComponentType type, string newDisplayName, PendulumAssembly owner = null)
        {
            componentType = type;
            displayName = newDisplayName;
            owningPendulum = owner;
            renderers = GetComponentsInChildren<Renderer>();
        }

        public void SetSelected(bool selected)
        {
            renderers ??= GetComponentsInChildren<Renderer>();
            MaterialPropertyBlock properties = new();

            foreach (Renderer componentRenderer in renderers)
            {
                componentRenderer.GetPropertyBlock(properties);
                if (selected)
                {
                    Color baseColor = componentRenderer.sharedMaterial != null
                        && componentRenderer.sharedMaterial.HasProperty("_Color")
                        ? componentRenderer.sharedMaterial.color
                        : Color.gray;
                    properties.SetColor("_Color", Color.Lerp(baseColor, new Color(1f, 0.72f, 0.18f), 0.38f));
                }
                else
                {
                    properties.Clear();
                }

                componentRenderer.SetPropertyBlock(properties);
            }
        }
    }
}
