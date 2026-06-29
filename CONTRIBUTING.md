# Contributing to Kinetic Studios

Kinetic Studios is in an early foundation stage. Keep changes focused, reviewable, and consistent with the phase described in the relevant issue.

## Development environment

- Use Unity `6000.3.17f1` exactly.
- Open the repository root as the Unity project.
- Install Git LFS before adding large binary assets.
- Do not commit `Library`, `Temp`, `Logs`, `Obj`, `UserSettings`, builds, IDE files, or other generated content.

## Before starting work

1. Search existing issues and pull requests.
2. For substantial features or architectural changes, open an issue and agree on scope first.
3. Create a focused branch from the current default branch.

## Pull requests

- Keep each pull request limited to one coherent change.
- Describe user-visible behavior, technical decisions, and verification performed.
- Include screenshots or recordings for visual changes.
- Update documentation when setup, behavior, or constraints change.
- Do not mix generated cache cleanup with feature development.
- Confirm that the project imports without errors in Unity `6000.3.17f1`.

## Unity asset practices

- Commit `.meta` files with their assets.
- Keep Asset Serialization set to Force Text and Version Control mode set to Visible Meta Files.
- Move or rename assets inside Unity when practical so GUID relationships are retained.
- Keep scenes and prefabs text-diffable; reserve Git LFS for large binary media.

## Issues and conduct

Use the issue templates for reproducible bugs and scoped feature proposals. Be specific, constructive, and respectful in all project discussions.
