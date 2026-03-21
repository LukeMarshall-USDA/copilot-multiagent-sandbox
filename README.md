# Multiagent Sandbox

Practical VS Code sandbox for role-based GitHub Copilot workflows, with committed agent templates and a small Selenium + xUnit demo project.

## Repository structure

```text
.
├── agent-templates/         # Committed source-of-truth agent definitions
├── setup_agents.ps1         # Generates local runtime agents and applies model mappings
├── SeleniumSandbox.sln      # Solution entry point for the demo project
├── SeleniumSandbox.Tests/
│   ├── Core/                # Driver factory, waits, base page
│   ├── Pages/               # Page objects
│   ├── Tests/               # Example xUnit tests
│   └── VisualTesting/       # Visual test helpers and runners
├── README.md                # Onboarding
└── CONTRIBUTING.md          # Contribution workflow and issue guidance
```

## Prerequisites

- VS Code with GitHub Copilot Chat
- PowerShell
- .NET 10 SDK and Chrome if you want to run the Selenium demo

## Setup agents

Clone the repo and open it in VS Code:

```bash
git clone https://github.com/LukeMarshall-USDA/copilot-multiagent-sandbox.git
cd copilot-multiagent-sandbox
```

Run the setup script:

```powershell
powershell -ExecutionPolicy Bypass -File .\setup_agents.ps1
```

The script:

- copies agent templates from `agent-templates/`
- generates local runtime agents in `.github/agents/`
- applies the model map defined in `setup_agents.ps1`
If agents do not appear after setup, reload VS Code.

This has been a little buggy for some reason. If you see issues with "model: " frontmatter in the agent.md files, do the following:

    - Delete entry from model: "model: GPT-5.3-Codex (copilot)" -> "model: "
    - Next to now empty colon in "model: ", begin typing the letter "G"
    - Dropdown list of available agents will appear, select the correct model for the agent


## First run in VS Code

1. Open Copilot Chat.
2. Switch to Agent mode.
3. Select `Orchestrator`.

Expected flow:

- `Orchestrator` coordinates the task and verifies the result.
- `Planner` proposes the implementation plan.
- `Designer` reviews structure and conventions when needed.
- `Coder` makes the approved change.

## Pilot exercise

Use this prompt for a first end-to-end run:

```text
Act as Orchestrator. Run a small end-to-end exercise in this repo with a one-file limit and no refactor. Have Planner produce a short plan first, use Designer only if needed, then have Coder make the smallest valid improvement. Finish with validation results and any remaining risk.
```

A good result should show:

- a scoped plan before edits
- a bounded change in one file
- validation after implementation
- a short note on remaining risk or follow-up

## Selenium demo project

This repo includes a small Selenium + xUnit project as a validation target.

```powershell
dotnet build
dotnet test
```

## Troubleshooting

**Agents do not appear**

- Re-run `setup_agents.ps1`
- Confirm `.github/agents/` exists
- Reload VS Code
- Make sure Copilot Chat is in Agent mode

**Script is blocked by execution policy**

- Use `powershell -ExecutionPolicy Bypass -File .\setup_agents.ps1`

**Need different model assignments**

- Edit `$ModelMap` in `setup_agents.ps1`
- Re-run the setup script
- OR: refer to instructions above -> open agent.md files, delete entry for "model:", type the letter 'G' to trigger dropdown list, and select the correct model.

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md). If you hit a bug or workflow issue, open an issue instead of contacting maintainers directly.
