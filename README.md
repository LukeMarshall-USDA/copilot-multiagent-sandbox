> **Early Sandbox — expect rough edges.** If something breaks, [open an issue](../../issues/new).

# Multiagent Sandbox

Practical template for role-based multi-agent execution in VS Code.

## What is included

- Shared agent definitions in `.claude/agents`
- Orchestrator/Planner/Designer/Coder role split
- Setup script for per-agent model assignment
- A runnable Selenium + xUnit sample to validate the workflow

## Quickstart

```bash
git clone https://github.com/LukeMarshall-USDA/copilot-multiagent-sandbox.git
cd copilot-multiagent-sandbox
python setup_agents.py
```

Open in VS Code. Copilot Chat → Agent mode → select `Orchestrator`. Done.

The setup script copies agents from `.claude/agents/` to `.github/agents/` (VS Code default path) and sets the recommended model per role. No settings changes needed.

## Agent roles

| Agent | Role | Invoked By |
|-------|------|------------|
| **Orchestrator** | Coordinates workflow, delegates, verifies output | You |
| **Planner** | Produces step-by-step plan with file paths and risks | Orchestrator |
| **Designer** | Structure/readability guidance when needed | Orchestrator |
| **Coder** | Implements approved scope with minimal diffs | Orchestrator |

## Operating flow

1. User prompts `Orchestrator` with goal + constraints.
2. Orchestrator delegates automatically.
3. Orchestrator returns: changes, validation, risks, and multi-agent benefit.

## Pilot exercise

Set active agent to `Orchestrator` and paste:

```text
Run the pilot exercise with max 1 file change and no refactor.
During execution, post succinct progress lines each time an agent is used.
Final output must clearly show each agent's role, what they did, and validation results.
```

Verify output includes per-agent progress lines, role-by-role summary, validation results, and a statement of why the multi-agent split improved the outcome.

## Model assignments

Default models set by `setup_agents.py` (March 2026):

| Agent | Model | Rationale |
|-------|-------|-----------|
| Orchestrator | GPT-5.4 | Best reasoning for coordination |
| Planner | Gemini 3.1 Pro (preview) | Strong structured analysis |
| Designer | Gemini 2.5 Pro | Good pattern recognition |
| Coder | GPT-5.3 Codex | Optimized for code gen |

To change: edit `MODEL_MAP` in `setup_agents.py` and re-run.

If you use one model for all roles: `GPT-5.4` (quality) or `GPT-5.2` (balanced).

## How agent deployment works

Agent source files live in `.claude/agents/` with `model: inherit`. The `.claude/` schema only accepts Claude model names, so per-agent model assignment happens locally.

`setup_agents.py` copies agents to `.github/agents/` and rewrites the `model:` field per role. `.github/agents/` is gitignored — never pushed.

- **`.claude/agents/`** — source of truth (committed)
- **`.github/agents/`** — runtime agents with models applied (local only, gitignored)

## Quality checklist (before merge)

- Work starts with `Orchestrator`
- `Planner` provides a plan before code edits
- `Coder` changes trace back to plan steps
- Validation is run after implementation
- Final handoff includes changes, validation, and risk notes

## Troubleshooting

**Agents don't appear in chat picker:**
Run `python setup_agents.py` first. Confirm `.github/agents/` exists locally. Reload VS Code. Check `chat.agent.enabled` is `true`. Make sure you're in Agent mode, not Ask or Edit.

**All agents use the same model:**
Check `MODEL_MAP` in `setup_agents.py`. Edit values to match your available models. Re-run the script. Reload VS Code.

**Orchestrator doesn't hand off:**
Verify `handoffs:` in Orchestrator frontmatter references correct agent names (no `.agent.md` extension). Confirm sub-agent files exist in `.github/agents/`.

**Tool not available warnings:**
Agent can only use tools in its `tools:` frontmatter. Add the missing tool to the list.

**Selenium build/test failures:**
The xUnit project is a demo target. Needs .NET 10 SDK, Chrome, and `dotnet restore`. Not an agent issue.

**Org-level agent distribution:**
```json
{ "github.copilot.chat.organizationCustomAgents.enabled": true }
```

## Example project (secondary)

Selenium + xUnit demo. Requires .NET 10 SDK and Chrome.

```bash
dotnet build
dotnet test
```

Test filters:
```bash
dotnet test --filter "Mode=Headless"
dotnet test --filter "Mode=Visual"
dotnet test --filter "Category=Smoke"
dotnet test --filter "Category=Smoke&Mode=Visual"
```

Visual failures save screenshots to `artifacts/screenshots/visual/{yyyyMMdd}/{TestClass}/{TestMethod}__{HHmmssfff}.png`

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md).

## Public sharing guardrails

- Keep targets public and non-sensitive
- Do not commit `bin/`, `obj/`, `artifacts/`, or `TestResults/`
- Do not include internal endpoints, credentials, tokens, or org-specific identifiers
