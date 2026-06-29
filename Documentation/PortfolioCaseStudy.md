# Kinetic Studios — Portfolio Case Study

## Problem

Kinetic sculpture is expensive to iterate. A change to a connector length, suspended mass, material, or damping can alter an entire composition, yet the behavior is difficult to judge from a static drawing. Physical prototypes provide the answer, but consume fabrication time, space, and materials before the design is settled.

Kinetic Studios explores a practical question: what would a purpose-built virtual studio look like if motion—not static geometry—were the primary design material?

## Why I built it

The project began as SimplePendulum, a narrow visualization experiment. That prototype exposed a larger opportunity. Artists and designers need more than a physics sample: they need a calm workspace where assemblies can be composed, tuned, observed, compared, and eventually carried from an early idea to a buildable design.

I rebuilt the repository as Kinetic Studios to establish that product direction without pretending the entire platform already exists. Version 0.1.0 is a deliberately constrained vertical slice. It demonstrates the interaction loop—create, edit, simulate, inspect, and present—using one pendulum.

## Architecture

The Unity project has one root and two build scenes. `Bootstrap` provides a stable entry point; `StudioWorkspace` contains the environment and application shell. A runtime assembly separates five responsibilities:

- `StudioShellController` binds the UI Toolkit document to application actions and state.
- `StudioBuilderController` owns placement, selection, deletion, and the active prototype.
- `PendulumAssembly` maps user properties to the rendered and simulated pendulum.
- `StudioCameraController` handles desktop navigation and repeatable presentation views.
- `DemoWalkthroughController` stages the portfolio walkthrough without changing product behavior.

The scene remains explicit and inspectable. A temporary editor integration command can regenerate the MVP’s material and scene references, which reduced hand-authored YAML risk while the vertical slice was evolving.

## Challenges

The original repository mixed project roots and tracked generated Unity output. Recovery required identifying the true source assets, removing caches safely, and establishing a reproducible Unity 6 baseline before feature work began.

The second challenge was translating continuous physical properties into a compact interface. The MVP needed enough controls to communicate the product idea without becoming a general-purpose physics editor. Length, mass, gravity, damping, initial angle, material, and transport controls form the smallest coherent demonstration.

Presentation was a third engineering problem. A portfolio demo must be repeatable, paced, and readable at video resolution. Camera presets, concise guidance, simulated cursor emphasis, and a deterministic F9 walkthrough make the same build usable for both interaction and recording.

## Engineering decisions

**Desktop first, XR-ready—not XR-first.** Desktop is the highest-leverage authoring surface. Future XR support should adapt input and presentation around the same studio model instead of forking the application.

**One vertical slice before a broad framework.** The project avoids premature abstractions for springs, gears, generalized joints, persistence, or device platforms. The pendulum validates the workflow and reveals the abstractions that are actually needed.

**UI Toolkit for the product shell.** A document-and-style approach keeps layout concerns separate from scene behavior and is a better fit for a dense authoring interface than world-space UI.

**Explicit simulation states.** Play, pause, and reset are treated as product controls rather than incidental Rigidbody behavior. Reset avoids writing velocity while the body is kinematic, keeping the Console clean.

**Presentation logic is isolated.** The walkthrough calls existing controls and changes focus treatment; it does not introduce a second implementation of the product.

## Future roadmap

The next milestone is not more component types. It is a durable authoring foundation: serializable design documents, stable identifiers, undo/redo, save/load, and reusable assemblies. Once that model is trustworthy, simulation can expand to constraints, actuators, measurements, comparisons, and fabrication-oriented exports.

XR interaction can then sit above the same command and document layers. A later web companion should exchange a documented subset of the same design format while Unity remains the simulation and authoring source of truth.

## Lessons learned

A clean repository is part of the product. Reproducible scenes, controlled package scope, contributor guidance, and honest capability claims materially improve the ability to iterate.

The most useful prototype is not the one with the most features. It is the one that makes the intended workflow obvious and exposes the next architectural decision. By keeping v0.1.0 narrow, Kinetic Studios now has a credible product demonstration and a foundation that can grow from evidence rather than speculation.
