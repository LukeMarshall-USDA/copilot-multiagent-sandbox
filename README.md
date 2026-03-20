# Multiagent Sandbox

This repository is a practical template for setting up a role-based multi-agent workflow.

The main goal is to make agent collaboration repeatable across teams. The Selenium project in this repo is included only as a concrete example for planning, implementation, and validation.

## What is included

- Shared agent definitions for workspace use
- A clear Orchestrator/Planner/Designer/Coder operating model
- A runnable .NET test project used as a demonstration target

## Quick setup in VS Code

1. Clone this repository.
2. Open the repo in VS Code.
3. Open Chat.
4. Select `Create new custom agent`.
5. Choose `.claude/agents` as the agent location.
6. Add or select these files:
   - `Orchestrator.agent.md`
   - `Planner.agent.md`
   - `Designer.agent.md`
   - `Coder.agent.md`
7. Assign each agent a GPT or Gemini model.
8. Start with `Orchestrator` selected.

`.claude/agents` is just a folder convention for agent files. It does not require Claude models.

## Agent roles

- `Orchestrator`: owns coordination and final handoff
- `Planner`: produces ordered file-level plans and risk notes
- `Designer`: advises on structure, readability, and conventions
- `Coder`: implements scoped changes with minimal diffs

## Recommended operating flow

1. Prompt `Orchestrator` with the goal and constraints.
2. `Orchestrator` gets a plan from `Planner`.
3. `Orchestrator` consults `Designer` when structure/readability decisions are needed.
4. `Orchestrator` sends approved steps to `Coder` for implementation.
5. `Orchestrator` reports what changed, how it was validated, and any remaining risks.

## Model guidance (current best)

As of March 2026, these are the best options from the currently available model set for this workflow.

### Recommended per-agent defaults (quality first)

- `Orchestrator`: `gpt 5.4`
  - Best for cross-step reasoning, synthesis, and final handoff quality.
- `Planner`: `gemini 3.1 pro (preview)`
  - Strong planning and long-context organization for file-level implementation plans.
- `Designer`: `gemini 2.5 pro`
  - Strong at clarity, structure, and concise documentation/style refinements.
- `Coder`: `gpt 5.3 codex`
  - Best overall coding accuracy for targeted diffs and implementation-heavy tasks.

### Balanced value profile (recommended for daily use)

- `Orchestrator`: `gpt 5.2`
- `Planner`: `gemini 2.5 pro`
- `Designer`: `gpt 4.1`
- `Coder`: `gpt 5.2 codex`

### Fast/low-cost profile

- `Orchestrator`: `gpt 5 mini`
- `Planner`: `gemini 3 flash (preview)`
- `Designer`: `gpt 4o`
- `Coder`: `gpt 5.1 codex mini (preview)` or `grok code fast 1`

### Practical notes

- Keep all agents scoped to the current workspace.
- Keep outputs concise and deterministic.
- Use preview models for speed/experimentation; prefer non-preview models for high-stakes merges.
- If you use one model for all roles, use `gpt 5.4` (quality) or `gpt 5.2` (balanced cost/performance).

Agent files are available in:

- `.claude/agents` (primary runtime location)
- `docs/agents` (reference copy)

## Quality checklist

- Work starts with `Orchestrator`
- `Planner` provides a plan before code edits
- `Coder` changes trace back to plan steps
- Validation is run after implementation
- Final handoff includes changes, validation, and risk notes

## Example project in this repo (secondary)

The sample implementation uses Selenium + xUnit.

### Prerequisites

- .NET 10 SDK
- Chrome browser

### Build and test

```bash
dotnet build
dotnet test
```

### Useful test filters

```bash
dotnet test --filter "Mode=Headless"
dotnet test --filter "Mode=Visual"
dotnet test --filter "Category=Smoke"
dotnet test --filter "Category=Smoke&Mode=Visual"
```

Visual failures save screenshots to:

```text
artifacts/screenshots/visual/{yyyyMMdd}/{TestClass}/{TestMethod}__{HHmmssfff}.png
```

## Public sharing guardrails

- Keep targets public and non-sensitive
- Do not commit generated `bin/`, `obj/`, `artifacts/`, or `TestResults/` folders
- Do not include internal endpoints, credentials, tokens, or organization-specific identifiers
