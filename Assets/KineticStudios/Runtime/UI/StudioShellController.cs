using System;
using System.Collections.Generic;
using KineticStudios.Builder;
using UnityEngine;
using UnityEngine.UIElements;

namespace KineticStudios.UserInterface
{
    [RequireComponent(typeof(UIDocument))]
    public sealed class StudioShellController : MonoBehaviour
    {
        private StudioBuilderController builder;
        private Label statusLabel;
        private Label selectionName;
        private Label selectionType;
        private Label simulationState;
        private VisualElement inspectorEmpty;
        private VisualElement inspectorContent;
        private VisualElement pendulumProperties;
        private DropdownField materialField;
        private FloatField lengthField;
        private FloatField massField;
        private FloatField gravityField;
        private FloatField dampingField;
        private FloatField initialAngleField;

        private void OnEnable()
        {
            builder = FindAnyObjectByType<StudioBuilderController>();
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            statusLabel = root.Q<Label>("status-label");
            selectionName = root.Q<Label>("selection-name");
            selectionType = root.Q<Label>("selection-type");
            simulationState = root.Q<Label>("simulation-state");
            inspectorEmpty = root.Q("inspector-empty");
            inspectorContent = root.Q("inspector-content");
            pendulumProperties = root.Q("pendulum-properties");
            materialField = root.Q<DropdownField>("material-field");
            lengthField = root.Q<FloatField>("length-field");
            massField = root.Q<FloatField>("mass-field");
            gravityField = root.Q<FloatField>("gravity-field");
            dampingField = root.Q<FloatField>("damping-field");
            initialAngleField = root.Q<FloatField>("initial-angle-field");

            materialField.choices = new List<string>(Enum.GetNames(typeof(StudioMaterialType)));
            materialField.index = 1;

            if (builder == null)
            {
                statusLabel.text = "BUILDER NOT INTEGRATED";
                simulationState.text = "SETUP REQUIRED";
                SetBuilderControlsEnabled(root, false);
                RefreshSelection(null);
                return;
            }

            Button anchorButton = root.Q<Button>("anchor-button");
            Button connectorButton = root.Q<Button>("connector-button");
            Button weightButton = root.Q<Button>("weight-button");
            Button demoButton = root.Q<Button>("demo-button");
            Button deleteButton = root.Q<Button>("delete-button");
            Button applyButton = root.Q<Button>("apply-properties-button");
            Button resetButton = root.Q<Button>("reset-button");
            Button playButton = root.Q<Button>("play-button");
            Button pauseButton = root.Q<Button>("pause-button");

            anchorButton.clicked += () => BeginPlacement(StudioComponentType.Anchor);
            connectorButton.clicked += () => BeginPlacement(StudioComponentType.Connector);
            weightButton.clicked += () => BeginPlacement(StudioComponentType.Weight);
            demoButton.clicked += builder.CreateDemoPendulum;
            deleteButton.clicked += builder.DeleteSelection;
            applyButton.clicked += ApplyPendulumProperties;
            resetButton.clicked += builder.ResetSimulation;
            playButton.clicked += builder.PlaySimulation;
            pauseButton.clicked += builder.PauseSimulation;

            materialField.RegisterValueChangedCallback(evt =>
            {
                if (Enum.TryParse(evt.newValue, out StudioMaterialType materialType))
                {
                    builder.ApplySelectedMaterial(materialType);
                }
            });

            builder.SelectionChanged += RefreshSelection;
            builder.PendulumChanged += RefreshPendulum;
            statusLabel.text = "STUDIO READY";
            RefreshSelection(builder.SelectedComponent);
            RefreshPendulum(builder.ActivePendulum);
        }

        private static void SetBuilderControlsEnabled(VisualElement root, bool enabled)
        {
            string[] names =
            {
                "anchor-button",
                "connector-button",
                "weight-button",
                "demo-button",
                "delete-button",
                "apply-properties-button",
                "reset-button",
                "play-button",
                "pause-button",
                "material-field"
            };

            foreach (string controlName in names)
            {
                root.Q(controlName)?.SetEnabled(enabled);
            }
        }

        private void OnDisable()
        {
            if (builder == null)
            {
                return;
            }

            builder.SelectionChanged -= RefreshSelection;
            builder.PendulumChanged -= RefreshPendulum;
        }

        private void BeginPlacement(StudioComponentType type)
        {
            builder.BeginPlacement(type);
            statusLabel.text = $"PLACE {type.ToString().ToUpperInvariant()} IN VIEWPORT";
        }

        private void RefreshSelection(PlacedComponent selected)
        {
            bool hasSelection = selected != null;
            inspectorEmpty.style.display = hasSelection ? DisplayStyle.None : DisplayStyle.Flex;
            inspectorContent.style.display = hasSelection ? DisplayStyle.Flex : DisplayStyle.None;

            if (!hasSelection)
            {
                return;
            }

            selectionName.text = selected.DisplayName;
            selectionType.text = selected.ComponentType switch
            {
                StudioComponentType.Connector => "ROD / STRING",
                StudioComponentType.Pendulum => "PENDULUM ASSEMBLY",
                _ => selected.ComponentType.ToString().ToUpperInvariant()
            };
            pendulumProperties.style.display = selected.OwningPendulum != null
                ? DisplayStyle.Flex
                : DisplayStyle.None;
            statusLabel.text = "SELECTION ACTIVE";
        }

        private void RefreshPendulum(PendulumAssembly pendulum)
        {
            if (pendulum == null)
            {
                simulationState.text = "NO PENDULUM";
                return;
            }

            lengthField.SetValueWithoutNotify(pendulum.Length);
            massField.SetValueWithoutNotify(pendulum.Mass);
            gravityField.SetValueWithoutNotify(pendulum.Gravity);
            dampingField.SetValueWithoutNotify(pendulum.Damping);
            initialAngleField.SetValueWithoutNotify(pendulum.InitialAngle);
            simulationState.text = pendulum.IsRunning ? "RUNNING" : "PAUSED";
        }

        private void ApplyPendulumProperties()
        {
            builder.ApplyPendulumProperties(
                lengthField.value,
                massField.value,
                gravityField.value,
                dampingField.value,
                initialAngleField.value);
            statusLabel.text = "PARAMETERS APPLIED";
        }
    }
}
