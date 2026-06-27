# Kinetic Studios

Kinetic Studios is an early-stage virtual prototyping environment for kinetic art. The project aims to help artists, designers, researchers, educators, and engineers explore motion-based sculptures in a virtual studio before committing time, materials, and cost to physical construction.

The repository currently contains only the clean Unity foundation. Authoring tools, simulation workflows, persistence, and XR support are planned work and are not yet implemented.

## Project status

Phase 0 establishes a single, reproducible Unity 6 project at the repository root. There is not yet a playable studio or kinetic-sculpture feature set.

## Requirements

- Unity Hub
- Unity Editor **6000.3.17f1**
- Git with Git LFS installed (for large binary media assets)

## Setup

1. Clone the repository.
2. Run `git lfs install` if Git LFS is not already configured on your machine.
3. Add the repository root as a project in Unity Hub.
4. Confirm that Unity Hub selects Editor `6000.3.17f1`.
5. Open the project and allow Unity to restore packages and import assets.

No scene is currently configured as a product entry point. That will be introduced with the first vertical slice.

## Project evolution

Kinetic Studios began as **SimplePendulum**, an exploratory concept demonstrating how virtual environments could make pendulum motion easier to observe and manipulate. The earlier repository did not contain the documented pendulum implementation, and its generated Unity caches and duplicate project root made it unsuitable as a production baseline.

The project is now being rebuilt as Kinetic Studios: a desktop-first, document-oriented tool for prototyping kinetic art, with XR interaction and a lightweight web companion considered as later extensions.

The original concept videos are retained here as historical references:

- [Simple pendulum concept video](https://github.com/user-attachments/assets/90379d1d-73a5-4150-adbc-98b1c88eec39)
- [Multi-pendulum / virtual studio concept video](https://github.com/user-attachments/assets/b98e6284-e10c-4058-8cd9-b91bed887d34)

## Roadmap

- **Phase 0 — Foundation:** repository recovery, Unity 6 baseline, project identity, and contributor documentation.
- **Phase 1 — First vertical slice:** create and edit one pendulum, control simulation, inspect basic measurements, and save/reload a design.
- **Phase 2 — Studio authoring:** reusable components, assembly tools, inspectors, undo/redo, and iteration workflows.
- **Phase 3 — Simulation and analysis:** richer constraints, actuators, telemetry, comparison tools, and exports.
- **Phase 4 — Immersive interaction:** XR adapters for supported headsets using the same project and design model.
- **Phase 5 — Web companion:** browser-based exploration and simplified prototypes interoperable with a supported subset of studio documents.

Roadmap items describe intent, not currently available functionality.

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md) before opening an issue or pull request.

## Citation

Citation metadata is available in [CITATION.cff](CITATION.cff).

## License

Kinetic Studios is licensed under the [MIT License](LICENSE).
