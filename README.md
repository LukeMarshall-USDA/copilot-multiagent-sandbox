> **Early Sandbox — Expect Rough Edges**
>
> This repo is an active pilot exploring multi-agent workflows with GitHub Copilot in VS Code.
> It works, but it's evolving fast. If you hit a snag, check the [Troubleshooting](#troubleshooting) section
> or [open an issue](../../issues/new). (Or just ping me on teams, Marshall, Edward (CTR) - FPAC-FBC)

# Multiagent Sandbox

Practical template for role-based multi-agent execution in VS Code.

## What is included

- Shared agent definitions in `.claude/agents`
- Orchestrator/Planner/Designer/Coder role split
- A runnable Selenium + xUnit sample to validate the workflow

## Quickstart (2 minutes)

**Step 1** — Clone the repo:

```bash
git clone https://github.com/LukeMarshall-USDA/copilot-multiagent-sandbox.git
cd copilot-multiagent-sandbox
```

**Step 2** — Tell VS Code where the agents live. Open your **Settings** (`Cmd/Ctrl + ,`), search for `agentFilesLocations`, and add:

```json
{
  "chat.agentFilesLocations": [
    { "pattern": ".claude/agents" }
  ]
}
```

**Step 3** — Open the Copilot Chat panel, switch to **Agent mode**, and select **Orchestrator** from the agent picker dropdown.

**Step 4** — Try it: type something like *"Plan and implement a new page object for the login page"* and watch the Orchestrator delegate to the Planner, Designer, and Coder.

That's it. See [Agent Setup](#agent-setup) below for details and [Troubleshooting](#troubleshooting) if anything doesn't work.

## Agent Setup

### How It Works

This repo uses a **role-based multi-agent workflow** built on VS Code custom agents:

| Agent | Role | Invoked By |
|-------|------|------------|
| **Orchestrator** | Coordinates the full workflow, delegates to sub-agents, verifies output | You (via chat picker) |
| **Planner** | Analyzes requests, produces step-by-step implementation plans | Orchestrator |
| **Designer** | Reviews design, provides architectural guidance | Orchestrator |
| **Coder** | Implements code following the approved plan and design | Orchestrator |

### Prerequisites

1. **VS Code** with GitHub Copilot extension installed
2. **Copilot Chat** with Agent mode enabled (`chat.agent.enabled: true` in settings)
3. **Agent file discovery** — this repo keeps agents in `.claude/agents/`. Add this to your VS Code `settings.json`:

   ```json
   {
     "chat.agentFilesLocations": [
       { "pattern": ".claude/agents" }
     ]
   }
   ```

4. Clone the repo and open it in VS Code. The agents should appear in the Copilot Chat model/agent picker.

`.claude/agents` is just a folder convention for agent files. It does not require Claude models. Model assignment is now declared in `.agent.md` frontmatter via a `model:` field. Your chat client may still allow runtime override per agent/session.

### Model Configuration

Agents are configured with different models optimized for each role. If your Copilot subscription doesn't include a specified model, the agent will fall back to whatever model you have selected in the VS Code chat picker.

To change the model, edit the `model:` field in each `.agent.md` file under `.claude/agents/`.

*Last verified: March 2026*

### Runtime vs. Reference Copies

- **`.claude/agents/`** — This is the **runtime source of truth**. VS Code reads agent definitions from here.
- **`docs/agents/`** (if present) — Reference copies for documentation purposes only. Do not edit these expecting runtime behavior to change.

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

## Troubleshooting

### Agents don't appear in the VS Code chat picker

1. Confirm you added `.claude/agents` to `chat.agentFilesLocations` in your VS Code settings (see [Agent Setup](#agent-setup)).
2. Reload VS Code (`Cmd/Ctrl + Shift + P` → "Developer: Reload Window").
3. Check that `chat.agent.enabled` is `true` in settings.
4. Make sure you're in **Agent mode** in the Copilot Chat panel (not "Ask" or "Edit" mode).

### Agents all use the same model / wrong model

Each agent file has a `model:` field in its YAML frontmatter. If the specified model isn't available in your Copilot subscription, VS Code silently falls back to the model selected in the chat picker. To verify:

1. Open any `.agent.md` file in `.claude/agents/`
2. Check the `model:` field matches a model you have access to
3. If not, update it to a model from your available list

### Orchestrator doesn't hand off to sub-agents

- Verify the Orchestrator's `handoffs:` frontmatter references the correct agent filenames (without the `.agent.md` extension).
- Confirm sub-agent files exist in the same `.claude/agents/` directory.
- Handoff buttons appear after the Orchestrator's response completes — look for them at the bottom of the chat message.

### "Tool not available" warnings

Agents can only use tools declared in their `tools:` frontmatter (or all tools if `tools:` is omitted). If an agent tries to use a tool it doesn't have access to, you'll see a warning. Check the agent's `.agent.md` file and add the needed tool to the `tools:` list.

### Build or test failures in the Selenium project

The Selenium xUnit project is included as a **demo target only**. It may require:

- .NET 10 SDK installed locally
- A browser driver (ChromeDriver, etc.) on your PATH
- `dotnet restore` before first build

These are not issues with the agent setup itself.

### Recommended VS Code Settings

Add these to your workspace or user `settings.json`:

```json
{
  "chat.agent.enabled": true,
  "chat.agentFilesLocations": [
    { "pattern": ".claude/agents" }
  ]
}
```

If your organization distributes agents centrally, also enable:

```json
{
  "github.copilot.chat.organizationCustomAgents.enabled": true
}
```

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

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md) for how to file issues, suggest agents, and submit changes.

## Public sharing guardrails

- Keep targets public and non-sensitive
- Do not commit generated `bin/`, `obj/`, `artifacts/`, or `TestResults/` folders
- Do not include internal endpoints, credentials, tokens, or organization-specific identifiers
