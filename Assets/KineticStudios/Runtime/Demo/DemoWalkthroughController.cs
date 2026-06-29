using System;
using System.Collections;
using KineticStudios.Builder;
using KineticStudios.CameraSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace KineticStudios.Demo
{
    /// <summary>
    /// Recording-only Demo v1 walkthrough. Press F9 in Play Mode to run it.
    /// </summary>
    public sealed class DemoWalkthroughController : MonoBehaviour
    {
        private const string WorkspaceScene = "StudioWorkspace";

        private StudioBuilderController builder;
        private StudioCameraController cameraController;
        private UIDocument document;
        private VisualElement root;
        private VisualElement demoLayer;
        private VisualElement cursor;
        private VisualElement ripple;
        private VisualElement callout;
        private VisualElement standardHint;
        private Label calloutKicker;
        private Label calloutText;
        private bool isRunning;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Install()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
            SceneManager.sceneLoaded += HandleSceneLoaded;
            HandleSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }

        private static void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != WorkspaceScene || FindAnyObjectByType<DemoWalkthroughController>() != null)
            {
                return;
            }

            new GameObject("Demo v1 Walkthrough").AddComponent<DemoWalkthroughController>();
        }

        private void Awake()
        {
            builder = FindAnyObjectByType<StudioBuilderController>();
            cameraController = FindAnyObjectByType<StudioCameraController>();
            document = FindAnyObjectByType<UIDocument>();

            if (builder == null || cameraController == null || document == null)
            {
                enabled = false;
                return;
            }

            root = document.rootVisualElement;
            standardHint = root.Q("viewport-hint");
            BuildDemoOverlay();
        }

        private void Update()
        {
            if (!isRunning && Input.GetKeyDown(KeyCode.F9))
            {
                StartCoroutine(RunWalkthrough());
            }
        }

        private void BuildDemoOverlay()
        {
            demoLayer = new VisualElement { name = "demo-v1-layer", pickingMode = PickingMode.Ignore };
            demoLayer.AddToClassList("demo-layer");

            callout = new VisualElement { pickingMode = PickingMode.Ignore };
            callout.AddToClassList("demo-callout");
            calloutKicker = new Label("KINETIC STUDIOS") { pickingMode = PickingMode.Ignore };
            calloutKicker.AddToClassList("demo-callout-kicker");
            calloutText = new Label { pickingMode = PickingMode.Ignore };
            calloutText.AddToClassList("demo-callout-text");
            callout.Add(calloutKicker);
            callout.Add(calloutText);

            ripple = new VisualElement { pickingMode = PickingMode.Ignore };
            ripple.AddToClassList("demo-ripple");

            cursor = new VisualElement { pickingMode = PickingMode.Ignore };
            cursor.AddToClassList("demo-cursor");

            demoLayer.Add(callout);
            demoLayer.Add(ripple);
            demoLayer.Add(cursor);
            root.Add(demoLayer);
            SetOverlayVisible(false);
        }

        private IEnumerator RunWalkthrough()
        {
            isRunning = true;
            SetOverlayVisible(true);
            if (standardHint != null)
            {
                standardHint.style.display = DisplayStyle.None;
            }
            cursor.style.left = 800f;
            cursor.style.top = 520f;

            ShowCallout("DEMO V1", "Design motion before building it.");
            yield return Wait(3.5f);

            ShowCallout("THE STUDIO", "A focused workspace for kinetic prototypes.");
            yield return AnimateCamera(new Vector3(8.2f, 6.1f, -8.2f), new Vector3(0f, 1f, 0f), 2.2f);
            yield return Wait(2.8f);

            yield return FocusAndActivate(
                "demo-button",
                "START WITH A SYSTEM",
                "Create a complete pendulum in one click.",
                builder.CreateDemoPendulum,
                3.8f);

            yield return Wait(1.2f);
            yield return AnimateCamera(new Vector3(5.8f, 4.1f, -5.4f), new Vector3(0f, 1.7f, 0f), 1.7f);

            FloatField length = root.Q<FloatField>("length-field");
            FloatField mass = root.Q<FloatField>("mass-field");
            FloatField damping = root.Q<FloatField>("damping-field");
            FloatField gravity = root.Q<FloatField>("gravity-field");
            FloatField angle = root.Q<FloatField>("initial-angle-field");

            yield return FocusAndActivate("length-field", "SHAPE THE MOTION", "Set the connector length.", () => length.SetValueWithoutNotify(3.4f), 2.8f);
            yield return FocusAndActivate("mass-field", "TUNE THE WEIGHT", "Adjust mass for the physical prototype.", () => mass.SetValueWithoutNotify(2.2f), 2.8f);
            yield return FocusAndActivate("damping-field", "CONTROL THE DECAY", "Use damping to soften the movement.", () => damping.SetValueWithoutNotify(0.16f), 2.8f);
            yield return FocusAndActivate("gravity-field", "TEST ANOTHER WORLD", "Explore behavior under different gravity.", () => gravity.SetValueWithoutNotify(7.4f), 2.8f);
            angle.SetValueWithoutNotify(32f);

            yield return FocusAndActivate(
                "apply-properties-button",
                "APPLY AS A STUDY",
                "Parameters update together and reset cleanly.",
                () => builder.ApplyPendulumProperties(length.value, mass.value, gravity.value, damping.value, angle.value),
                3.4f);

            DropdownField material = root.Q<DropdownField>("material-field");
            yield return FocusAndActivate(
                "material-field",
                "EXPLORE MATERIAL",
                "Preview the assembly with a wood finish.",
                () =>
                {
                    material.SetValueWithoutNotify("Wood");
                    builder.ApplySelectedMaterial(StudioMaterialType.Wood);
                },
                3.5f);

            yield return FocusAndActivate("play-button", "WATCH IT MOVE", "Play the configured simulation.", builder.PlaySimulation, 5f);
            yield return FocusAndActivate("pause-button", "FREEZE THE MOMENT", "Pause to inspect motion from any view.", builder.PauseSimulation, 3.6f);
            yield return FocusAndActivate("reset-button", "RETURN TO THE STUDY", "Reset restores the chosen starting angle.", builder.ResetSimulation, 3.5f);

            yield return FocusAndCamera(
                "overview-view-button",
                "RECORDING VIEW 01",
                "Overview frames the complete workspace.",
                new Vector3(8.2f, 6.1f, -8.2f),
                new Vector3(0f, 1f, 0f));
            yield return FocusAndCamera(
                "front-view-button",
                "RECORDING VIEW 02",
                "Front view makes motion easy to compare.",
                new Vector3(0f, 3.6f, -9f),
                new Vector3(0f, 1.7f, 0f));
            yield return FocusAndCamera(
                "detail-view-button",
                "RECORDING VIEW 03",
                "Detail view brings the mechanism closer.",
                new Vector3(5.3f, 3.7f, -5.3f),
                new Vector3(0f, 1.7f, 0f));

            ShowCallout("KINETIC STUDIOS", "From motion study to physical possibility.");
            yield return AnimateCamera(new Vector3(7.4f, 4.8f, -7.4f), new Vector3(0f, 1.4f, 0f), 2.6f);
            yield return Wait(5f);

            builder.ResetSimulation();
            cameraController.enabled = true;
            cameraController.ApplyViewPreset(StudioCameraController.ViewPreset.Overview);
            if (standardHint != null)
            {
                standardHint.style.display = DisplayStyle.Flex;
            }
            SetOverlayVisible(false);
            isRunning = false;
        }

        private IEnumerator FocusAndActivate(
            string targetName,
            string kicker,
            string message,
            Action action,
            float holdDuration)
        {
            VisualElement target = root.Q(targetName);
            if (target == null)
            {
                yield break;
            }

            ShowCallout(kicker, message);
            target.AddToClassList("demo-focus");
            yield return MoveCursor(target.worldBound.center, 0.75f);
            yield return ClickRipple();
            action?.Invoke();
            yield return Wait(holdDuration);
            target.RemoveFromClassList("demo-focus");
        }

        private IEnumerator FocusAndCamera(
            string targetName,
            string kicker,
            string message,
            Vector3 position,
            Vector3 lookAt)
        {
            VisualElement target = root.Q(targetName);
            ShowCallout(kicker, message);
            target?.AddToClassList("demo-focus");

            if (target != null)
            {
                yield return MoveCursor(target.worldBound.center, 0.7f);
                yield return ClickRipple();
            }

            yield return AnimateCamera(position, lookAt, 1.6f);
            yield return Wait(2.2f);
            target?.RemoveFromClassList("demo-focus");
        }

        private IEnumerator MoveCursor(Vector2 destination, float duration)
        {
            Vector2 start = new(cursor.resolvedStyle.left, cursor.resolvedStyle.top);
            destination -= new Vector2(7f, 7f);
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / duration));
                Vector2 position = Vector2.Lerp(start, destination, t);
                cursor.style.left = position.x;
                cursor.style.top = position.y;
                yield return null;
            }
        }

        private IEnumerator ClickRipple()
        {
            float centerX = cursor.resolvedStyle.left + 7f;
            float centerY = cursor.resolvedStyle.top + 7f;
            ripple.style.display = DisplayStyle.Flex;
            float elapsed = 0f;

            while (elapsed < 0.45f)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / 0.45f);
                float size = Mathf.Lerp(10f, 44f, t);
                ripple.style.width = size;
                ripple.style.height = size;
                ripple.style.left = centerX - size * 0.5f;
                ripple.style.top = centerY - size * 0.5f;
                ripple.style.opacity = 1f - t;
                yield return null;
            }

            ripple.style.display = DisplayStyle.None;
        }

        private IEnumerator AnimateCamera(Vector3 destination, Vector3 lookAt, float duration)
        {
            Camera studioCamera = cameraController.GetComponent<Camera>();
            cameraController.enabled = false;
            Vector3 startPosition = studioCamera.transform.position;
            Quaternion startRotation = studioCamera.transform.rotation;
            Quaternion endRotation = Quaternion.LookRotation(lookAt - destination, Vector3.up);
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / duration));
                studioCamera.transform.SetPositionAndRotation(
                    Vector3.Lerp(startPosition, destination, t),
                    Quaternion.Slerp(startRotation, endRotation, t));
                yield return null;
            }
        }

        private void ShowCallout(string kicker, string message)
        {
            calloutKicker.text = kicker;
            calloutText.text = message;
        }

        private void SetOverlayVisible(bool visible)
        {
            demoLayer.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private static IEnumerator Wait(float seconds)
        {
            float elapsed = 0f;
            while (elapsed < seconds)
            {
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }
        }
    }
}
