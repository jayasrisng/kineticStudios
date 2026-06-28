using System;
using UnityEngine;

namespace KineticStudios.Builder
{
    public sealed class StudioBuilderController : MonoBehaviour
    {
        [SerializeField] private Camera studioCamera;
        [SerializeField] private Material woodMaterial;
        [SerializeField] private Material metalMaterial;
        [SerializeField] private Material ropeMaterial;
        [SerializeField] private Material glassMaterial;
        [SerializeField] private float leftPanelWidth = 264f;
        [SerializeField] private float rightPanelWidth = 320f;
        [SerializeField] private float topBarHeight = 56f;
        [SerializeField] private float bottomPanelHeight = 128f;

        private StudioComponentType? pendingPlacement;
        private PlacedComponent selectedComponent;
        private PendulumAssembly activePendulum;
        private int anchorCount;
        private int connectorCount;
        private int weightCount;

        public event Action<PlacedComponent> SelectionChanged;
        public event Action<PendulumAssembly> PendulumChanged;

        public PlacedComponent SelectedComponent => selectedComponent;
        public PendulumAssembly ActivePendulum => activePendulum;
        public string PendingPlacementName => pendingPlacement?.ToString() ?? string.Empty;

        public void Configure(Camera camera, Material wood, Material metal, Material rope, Material glass)
        {
            studioCamera = camera;
            woodMaterial = wood;
            metalMaterial = metal;
            ropeMaterial = rope;
            glassMaterial = glass;
        }

        private void Start()
        {
            CreateDemoPendulum();
        }

        private void Update()
        {
            if ((Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Backspace)) && selectedComponent != null)
            {
                DeleteSelection();
            }

            if (!Input.GetMouseButtonDown(0) || !IsPointerInViewport())
            {
                return;
            }

            if (pendingPlacement.HasValue && TryGetFloorPoint(out Vector3 floorPoint))
            {
                PlaceComponent(pendingPlacement.Value, floorPoint);
                pendingPlacement = null;
                return;
            }

            SelectFromPointer();
        }

        public void BeginPlacement(StudioComponentType type)
        {
            pendingPlacement = type;
        }

        public void CreateDemoPendulum()
        {
            if (activePendulum != null)
            {
                Destroy(activePendulum.gameObject);
            }

            GameObject root = new("Pendulum 01");
            root.transform.position = Vector3.zero;
            activePendulum = root.AddComponent<PendulumAssembly>();
            activePendulum.Build(metalMaterial, ropeMaterial, glassMaterial);

            PlacedComponent selectionTarget = root.GetComponentInChildren<PlacedComponent>();
            Select(selectionTarget);
            PendulumChanged?.Invoke(activePendulum);
        }

        public void DeleteSelection()
        {
            if (selectedComponent == null)
            {
                return;
            }

            GameObject objectToDelete = selectedComponent.OwningPendulum != null
                ? selectedComponent.OwningPendulum.gameObject
                : selectedComponent.gameObject;

            if (selectedComponent.OwningPendulum == activePendulum)
            {
                activePendulum = null;
                PendulumChanged?.Invoke(null);
            }

            selectedComponent.SetSelected(false);
            selectedComponent = null;
            SelectionChanged?.Invoke(null);
            Destroy(objectToDelete);
        }

        public void ApplyPendulumProperties(float length, float mass, float gravity, float damping, float initialAngle)
        {
            activePendulum?.ApplyProperties(length, mass, gravity, damping, initialAngle);
            PendulumChanged?.Invoke(activePendulum);
        }

        public void PlaySimulation()
        {
            activePendulum?.PlaySimulation();
            PendulumChanged?.Invoke(activePendulum);
        }

        public void PauseSimulation()
        {
            activePendulum?.PauseSimulation();
            PendulumChanged?.Invoke(activePendulum);
        }

        public void ResetSimulation()
        {
            activePendulum?.ResetSimulation();
            PendulumChanged?.Invoke(activePendulum);
        }

        public void ApplySelectedMaterial(StudioMaterialType materialType)
        {
            if (selectedComponent == null)
            {
                return;
            }

            Material material = GetMaterial(materialType);
            if (selectedComponent.OwningPendulum != null)
            {
                selectedComponent.OwningPendulum.ApplyMaterial(material);
            }
            else
            {
                selectedComponent.GetComponentInChildren<Renderer>().sharedMaterial = material;
            }

            selectedComponent.SetSelected(true);
        }

        private void PlaceComponent(StudioComponentType type, Vector3 floorPoint)
        {
            GameObject placedObject;
            string displayName;

            switch (type)
            {
                case StudioComponentType.Anchor:
                    placedObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    displayName = $"Anchor {++anchorCount:00}";
                    placedObject.transform.position = floorPoint + Vector3.up * 0.25f;
                    placedObject.transform.localScale = new Vector3(0.65f, 0.5f, 0.65f);
                    placedObject.GetComponent<Renderer>().sharedMaterial = metalMaterial;
                    break;
                case StudioComponentType.Connector:
                    placedObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    displayName = $"Rod / String {++connectorCount:00}";
                    placedObject.transform.position = floorPoint + Vector3.up;
                    placedObject.transform.localScale = new Vector3(0.08f, 1f, 0.08f);
                    placedObject.GetComponent<Renderer>().sharedMaterial = ropeMaterial;
                    break;
                case StudioComponentType.Weight:
                    placedObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    displayName = $"Weight {++weightCount:00}";
                    placedObject.transform.position = floorPoint + Vector3.up * 0.45f;
                    placedObject.transform.localScale = Vector3.one * 0.75f;
                    placedObject.GetComponent<Renderer>().sharedMaterial = glassMaterial;
                    break;
                default:
                    return;
            }

            placedObject.name = displayName;
            PlacedComponent component = placedObject.AddComponent<PlacedComponent>();
            component.Configure(type, displayName);
            Select(component);
        }

        private void SelectFromPointer()
        {
            Ray ray = studioCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 250f))
            {
                PlacedComponent component = hit.collider.GetComponentInParent<PlacedComponent>();
                Select(component);
            }
            else
            {
                Select(null);
            }
        }

        private void Select(PlacedComponent component)
        {
            selectedComponent?.SetSelected(false);
            selectedComponent = component;
            selectedComponent?.SetSelected(true);
            SelectionChanged?.Invoke(selectedComponent);
            if (selectedComponent?.OwningPendulum != null)
            {
                activePendulum = selectedComponent.OwningPendulum;
                PendulumChanged?.Invoke(activePendulum);
            }
        }

        private bool TryGetFloorPoint(out Vector3 point)
        {
            Ray ray = studioCamera.ScreenPointToRay(Input.mousePosition);
            Plane floorPlane = new(Vector3.up, Vector3.zero);
            if (floorPlane.Raycast(ray, out float distance))
            {
                point = ray.GetPoint(distance);
                return true;
            }

            point = default;
            return false;
        }

        private bool IsPointerInViewport()
        {
            Vector3 pointer = Input.mousePosition;
            return pointer.x >= leftPanelWidth
                   && pointer.x <= Screen.width - rightPanelWidth
                   && pointer.y >= bottomPanelHeight
                   && pointer.y <= Screen.height - topBarHeight;
        }

        private Material GetMaterial(StudioMaterialType materialType)
        {
            return materialType switch
            {
                StudioMaterialType.Wood => woodMaterial,
                StudioMaterialType.Metal => metalMaterial,
                StudioMaterialType.Rope => ropeMaterial,
                StudioMaterialType.Glass => glassMaterial,
                _ => metalMaterial
            };
        }
    }
}
