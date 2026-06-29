using UnityEngine;

namespace KineticStudios.Builder
{
    public sealed class PendulumAssembly : MonoBehaviour
    {
        private const float MinimumLength = 0.5f;
        private const float MaximumLength = 6f;

        private Transform pivot;
        private Transform connector;
        private Rigidbody bobBody;
        private HingeJoint hinge;
        private Renderer[] renderers;
        private Vector3 pausedLinearVelocity;
        private Vector3 pausedAngularVelocity;
        private bool isRunning;

        public string DisplayName { get; private set; } = "Pendulum 01";
        public float Length { get; private set; } = 2.6f;
        public float Mass { get; private set; } = 1.2f;
        public float Gravity { get; private set; } = 9.81f;
        public float Damping { get; private set; } = 0.08f;
        public float InitialAngle { get; private set; } = 24f;
        public bool IsRunning => isRunning;

        public void Build(Material anchorMaterial, Material connectorMaterial, Material weightMaterial)
        {
            GameObject anchor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            anchor.name = "Anchor";
            anchor.transform.SetParent(transform);
            anchor.transform.localPosition = new Vector3(0f, 4.2f, 0f);
            anchor.transform.localScale = new Vector3(0.7f, 0.32f, 0.7f);
            anchor.GetComponent<Renderer>().sharedMaterial = anchorMaterial;
            Rigidbody anchorBody = anchor.AddComponent<Rigidbody>();
            anchorBody.isKinematic = true;
            PlacedComponent anchorComponent = anchor.AddComponent<PlacedComponent>();
            anchorComponent.Configure(StudioComponentType.Anchor, "Pendulum Anchor", this);
            pivot = anchor.transform;

            GameObject connectorObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            connectorObject.name = "Rod / String";
            connectorObject.transform.SetParent(transform);
            connectorObject.GetComponent<Renderer>().sharedMaterial = connectorMaterial;
            Object.Destroy(connectorObject.GetComponent<Collider>());
            PlacedComponent connectorComponent = connectorObject.AddComponent<PlacedComponent>();
            connectorComponent.Configure(StudioComponentType.Connector, "Pendulum Connector", this);
            connector = connectorObject.transform;

            GameObject weight = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            weight.name = "Weight";
            weight.transform.SetParent(transform);
            weight.transform.localScale = Vector3.one * 0.68f;
            weight.GetComponent<Renderer>().sharedMaterial = weightMaterial;
            bobBody = weight.AddComponent<Rigidbody>();
            bobBody.useGravity = true;
            bobBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            PlacedComponent weightComponent = weight.AddComponent<PlacedComponent>();
            weightComponent.Configure(StudioComponentType.Weight, "Pendulum Weight", this);

            hinge = weight.AddComponent<HingeJoint>();
            hinge.connectedBody = anchorBody;
            hinge.autoConfigureConnectedAnchor = false;
            hinge.connectedAnchor = Vector3.zero;
            hinge.axis = Vector3.forward;
            hinge.enableCollision = false;

            renderers = GetComponentsInChildren<Renderer>();
            ApplyProperties(Length, Mass, Gravity, Damping, InitialAngle);
        }

        public void ApplyProperties(float length, float mass, float gravity, float damping, float initialAngle)
        {
            Length = Mathf.Clamp(length, MinimumLength, MaximumLength);
            Mass = Mathf.Clamp(mass, 0.05f, 50f);
            Gravity = Mathf.Clamp(gravity, 0f, 30f);
            Damping = Mathf.Clamp(damping, 0f, 5f);
            InitialAngle = Mathf.Clamp(initialAngle, -85f, 85f);

            Physics.gravity = Vector3.down * Gravity;
            bobBody.mass = Mass;
            bobBody.linearDamping = Damping;
            bobBody.angularDamping = Damping;
            ResetSimulation();
        }

        public void PlaySimulation()
        {
            if (isRunning)
            {
                return;
            }

            bobBody.isKinematic = false;
            bobBody.linearVelocity = pausedLinearVelocity;
            bobBody.angularVelocity = pausedAngularVelocity;
            bobBody.WakeUp();
            isRunning = true;
        }

        public void PauseSimulation()
        {
            if (!isRunning)
            {
                return;
            }

            pausedLinearVelocity = bobBody.linearVelocity;
            pausedAngularVelocity = bobBody.angularVelocity;
            bobBody.isKinematic = true;
            isRunning = false;
        }

        public void ResetSimulation()
        {
            isRunning = false;

            if (!bobBody.isKinematic)
            {
                bobBody.linearVelocity = Vector3.zero;
                bobBody.angularVelocity = Vector3.zero;
            }

            bobBody.isKinematic = true;
            pausedLinearVelocity = Vector3.zero;
            pausedAngularVelocity = Vector3.zero;

            Vector3 direction = Quaternion.AngleAxis(InitialAngle, Vector3.forward) * Vector3.down;
            bobBody.transform.position = pivot.position + direction * Length;
            bobBody.transform.rotation = Quaternion.identity;
            hinge.anchor = bobBody.transform.InverseTransformVector(pivot.position - bobBody.transform.position);
            UpdateConnector();
        }

        public void ApplyMaterial(Material material)
        {
            renderers ??= GetComponentsInChildren<Renderer>();
            foreach (Renderer componentRenderer in renderers)
            {
                componentRenderer.sharedMaterial = material;
            }
        }

        private void LateUpdate()
        {
            UpdateConnector();
        }

        private void UpdateConnector()
        {
            Vector3 start = pivot.position;
            Vector3 end = bobBody.position;
            Vector3 delta = end - start;
            connector.position = start + delta * 0.5f;
            connector.up = delta.normalized;
            connector.localScale = new Vector3(0.055f, delta.magnitude * 0.5f, 0.055f);
        }
    }
}
