# Multiagent Sandbox

Practical template for role-based multi-agent execution in VS Code.

## What is included

- Shared agent definitions in `.claude/agents`
- Orchestrator/Planner/Designer/Coder role split
- A runnable Selenium + xUnit sample to validate the workflow

## Quick setup in VS Code

1. Clone and open this repository in VS Code.
2. Open Chat and select `Create new custom agent`.
3. Choose `.claude/agents` as location.
4. Add/select:
   - `Orchestrator.agent.md`
   - `Planner.agent.md`
   - `Designer.agent.md`
   - `Coder.agent.md`
5. Confirm each agent model (pre-filled from frontmatter, editable in UI).
6. Start with `Orchestrator` selected.

`.claude/agents` is just a folder convention for agent files. It does not require Claude models.

Model assignment is now declared in `.agent.md` frontmatter via a `model:` field.
Your chat client may still allow runtime override per agent/session.

## Agent roles (quick reference)

- `Orchestrator`: delegates, tracks progress, and delivers final handoff
- `Planner`: produces ordered steps with file paths and risks
- `Designer`: provides structure/readability guidance only when needed
- `Coder`: implements approved scope with minimal diffs

## Operating flow

1. User prompts `Orchestrator` with goal + constraints.
2. Orchestrator delegates automatically.
3. Orchestrator returns: changes, validation, risks, and multi-agent benefit.

## Pilot exercise

Use this to verify that role delineation is visible and useful.

1. Set the active agent to `Orchestrator`.
2. Paste this prompt:

```text
Run the pilot exercise with max 1 file change and no refactor.
During execution, post succinct progress lines each time an agent is used.
Final output must clearly show each agent's role, what they did, and validation results.
```

3. Verify the chat output includes:

- Per-agent progress lines as the run happens (Orchestrator/Planner/Designer-if-used/Coder)
- A final summary with role-by-role contributions
- Validation results (targeted test + full suite)
- A short statement of why the multi-agent split improved the outcome

## Model guidance

Current recommendations (March 2026):

- `Orchestrator`: `gpt 5.4`
- `Planner`: `gemini 3.1 pro (preview)`
- `Designer`: `gemini 2.5 pro`
- `Coder`: `gpt 5.3 codex`

## Model frontmatter mapping

Current `.claude/agents` frontmatter values:

- `Orchestrator.agent.md` → `GPT-5.4 (copilot)`
- `Planner.agent.md` → `Gemini 3.1 Pro (preview) (google)`
- `Designer.agent.md` → `Gemini 2.5 Pro (google)`
- `Coder.agent.md` → `GPT-5.3 Codex (copilot)`

### Practical notes

- Keep all agents scoped to the current workspace.
- Keep outputs concise and deterministic.
- Use preview models for speed/experimentation; prefer non-preview models for high-stakes merges.
- If you use one model for all roles, use `gpt 5.4` (quality) or `gpt 5.2` (balanced cost/performance).

Agent files: `.claude/agents` (single source of truth).

## Quality checklist (before merge)

- Work starts with `Orchestrator`
- `Planner` provides a plan before code edits
- `Coder` changes trace back to plan steps
- Validation is run after implementation
- Final handoff includes changes, validation, and risk notes

## Example project in this repo (secondary)

The sample project uses Selenium + xUnit.

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
