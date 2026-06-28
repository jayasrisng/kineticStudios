# Demo v1 recording guide

Demo v1 is a guided, approximately 80-second walkthrough of the existing Kinetic Studios MVP. It does not add or simulate capabilities beyond the current builder.

## Storyboard

| Time | Beat | Screen action | Callout |
|---:|---|---|---|
| 0–4s | Open | Clean studio title frame | Design motion before building it. |
| 4–9s | Orient | Smooth move into the studio overview | A focused workspace for kinetic prototypes. |
| 9–16s | Create | Cursor selects Create Demo Pendulum | Create a complete pendulum in one click. |
| 16–38s | Configure | Length, mass, damping, gravity, angle, then Apply | Shape and tune the motion. |
| 38–43s | Material | Switch the selected assembly to Wood | Preview material direction. |
| 43–58s | Simulate | Play, pause, and reset | Watch, inspect, and return to the study. |
| 58–73s | Frame | Overview, Front, and Detail recording views | Consistent views for comparison. |
| 73–80s | Hero | Smooth three-quarter camera move and hold | From motion study to physical possibility. |

## Recording checklist

1. Open the repository root in Unity `6000.3.17f1`.
2. Open `Assets/KineticStudios/Scenes/Bootstrap.unity`.
3. Set the Game view to 16:9 at 1920×1080 or 2560×1440.
4. Enable Maximize On Play and hide overlays that are not part of the application.
5. Enter Play Mode and wait for `StudioWorkspace` to finish loading.
6. Start screen recording before pressing `F9`.
7. Press `F9` once and avoid physical mouse or keyboard input during playback.
8. Hold the final hero frame for at least two additional seconds if editing needs room for a fade.
9. Stop recording after the walkthrough returns to the Overview view.
10. Review cursor alignment, callout legibility, motion smoothness, and absence of Console warnings.

## Capture recommendations

- Record at 60 fps when possible; deliver at 30 or 60 fps.
- Capture clean application audio or remain silent for later music/voiceover.
- Keep the Game view scale at 1× so the simulated cursor aligns with UI controls.
- Use a gentle fade in and fade out during editing; avoid additional animated transitions.
- Do not accelerate the physics segment independently from the cursor and callouts.

## Pre-commit visual checks

- Verify the simulated cursor remains aligned at the selected Game view resolution.
- Confirm long callouts stay on one or two lines.
- Check that the Wood material remains legible against the floor and lighting.
- Confirm view-preset controls are not obscured by the demo callout.
- Confirm the final hero frame leaves clear negative space around the pendulum.
