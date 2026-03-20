# Selenium Sandbox

Public, minimal Selenium + xUnit sandbox for validating browser automation patterns and multi-agent workflow specialization.

## Status

- Public-safe sandbox targets only (`example.com`, `iana.org`)
- Single clean branch expected (`main`)
- Headless + visual tests available

## Why this repo exists

- Demonstrates a clean Selenium test harness in .NET.
- Provides a safe sandbox with public example targets only (example.com and iana.org).
- Shows role-based agent workflow boundaries (Planner, Designer, Coder, Orchestrator).

## Multi-agent workflow notes

This repository is structured so each agent has a narrow responsibility:

- Planner: ordered implementation plan and risks.
- Designer: conventions, readability, and DX guidance.
- Coder: focused file changes with minimal diffs.
- Orchestrator: coordinates flow and verification.

Can each agent run a different model?

- Not from these markdown role files alone.
- These files define role behavior.
- Per-agent model routing depends on the host/orchestration platform configuration.

Is this more effective than one agent with one prompt?

- Usually yes for multi-step changes: better planning, traceability, and reviewability.
- Usually no for very small one-step tasks where a single prompt is faster.

## Implementing the multi-agent delineation system

Use this pattern to add role-specialized collaboration to a repository.

### UI setup in chat (click-by-click)

The exact labels can vary by extension version, but the flow is typically:

1. Open the Chat panel in VS Code.
2. In the Chat input toolbar, open the agent selector (for example, `Agent`, `@`, or `Tools/Agents`).
3. Choose **Create agent** (or **Add custom agent**).
4. Point the agent to the corresponding role file in `docs/agents`.
5. Repeat for all four roles: Orchestrator, Planner, Designer, Coder.
6. Save and verify each agent appears in the selector list.

Recommended default for daily use:

- Start conversations with **Orchestrator**.
- Let Orchestrator delegate to Planner/Designer/Coder as needed.

### 1) Define role contracts

Create one markdown file per role under `docs/agents`:

- `Orchestrator.agent.md`
- `Planner.agent.md`
- `Designer.agent.md`
- `Coder.agent.md`

Each role file should include:

- Scope and responsibilities
- Hard constraints (what not to do)
- Expected output format
- Escalation behavior for ambiguity/blockers

This repo includes baseline versions in `docs/agents`.

### 1.1) Configure each agent profile

For each created agent profile:

- Name: match file name (`Orchestrator`, `Planner`, `Designer`, `Coder`).
- Instructions source: corresponding markdown file under `docs/agents`.
- Scope: current workspace/repository only.
- Behavior: concise, deterministic, and minimal-diff by default.

### 2) Enforce delegation order

For non-trivial work, use this sequence:

1. Designer (optional) for conventions/readability impact
2. Planner for file-level plan and risks
3. Coder for implementation with minimal diffs
4. Orchestrator for synthesis, verification, and handoff

In practice, this means:

- You ask Orchestrator for the task.
- Orchestrator requests a plan from Planner.
- Orchestrator requests optional structure/readability guidance from Designer.
- Orchestrator requests code changes from Coder.
- Orchestrator returns one consolidated result to you.

### 3) Use a standard task lifecycle

Apply the same lifecycle to every request:

1. Intake: restate goal and constraints
2. Plan: produce ordered steps with file paths
3. Execute: implement scoped diffs only
4. Validate: run focused tests, then broader tests
5. Handoff: summarize changes, risks, and next actions

### 4) Keep role boundaries strict

- Planner should not implement code.
- Coder should not redesign architecture unless explicitly requested.
- Orchestrator should resolve conflicts between recommendations and keep scope tight.

### 4.1) Make agents reference each other explicitly

To avoid drift, include explicit handoff references in prompts and outputs:

- Planner output should reference expected Coder inputs (files + steps).
- Coder output should map changes back to Planner step numbers.
- Orchestrator output should summarize Designer guidance and Planner/Coder traceability.

Simple prompt pattern for Orchestrator:

1. "Ask Planner for an ordered implementation plan with file paths."
2. "Ask Designer only if naming/structure/readability is impacted."
3. "Ask Coder to implement only approved plan steps with minimal diffs."
4. "Return one consolidated summary with verification results."

### 5) Add quality gates

Before merge/push, require:

- Passing tests
- No internal URLs/secrets/PII
- Minimal, reviewable diffs
- Clear commit message and PR summary

### 6) Decide model routing strategy

Role files define behavior, not model selection. If your platform supports model routing, map roles to models based on task type (for example: planning-focused model for Planner, code-focused model for Coder). If your platform does not support routing, use one model with strict role prompts.

## Multi-agent effectiveness checklist

Use this quick checklist to confirm the system is working well:

- Work starts with Orchestrator, not directly with Coder.
- Planner provides file-level steps before code is changed.
- Designer is used only when structure/readability conventions matter.
- Coder changes are minimal and traceable to the approved plan.
- Validation is run after implementation and reported back.
- Final summary includes what changed, how verified, and remaining risks.

## What reviewers can evaluate quickly

- Test framework clarity: `Core` + `Pages` + `Tests` separation
- Reliability pattern: popup fallback handling in headless/visual navigation tests
- Extensibility: custom visual test attribute/discoverer/runner pipeline

## Prerequisites

- .NET 10 SDK
- Chrome browser

## Quick start

Build:

```bash
dotnet build
```

Run all tests:

```bash
dotnet test
```

## Test filters

```bash
# Headless tests
dotnet test --filter "Mode=Headless"

# Visual tests
dotnet test --filter "Mode=Visual"

# Smoke tests
dotnet test --filter "Category=Smoke"

# Visual smoke tests
dotnet test --filter "Category=Smoke&Mode=Visual"
```

Headless smoke tests validate Example Domain content and navigation to IANA Example Domains.

Visual test failures save screenshots to:

```text
artifacts/screenshots/visual/{yyyyMMdd}/{TestClass}/{TestMethod}__{HHmmssfff}.png
```

## Project structure

SeleniumSandbox.Tests/
├── Core/            Framework infrastructure (DriverFactory, WaitHelper, BasePage)
├── Pages/           Page Object Model classes
├── Tests/           Headless and visual test classes
└── VisualTesting/   Visual test attribute, discoverer, runner, and base class

## Public-sharing guardrails

- Keep targets public and non-sensitive.
- Do not commit generated bin/obj artifacts.
- Do not commit generated `artifacts/` or `TestResults/` folders.
- Do not include internal endpoints, credentials, tokens, or organization-specific identifiers.
