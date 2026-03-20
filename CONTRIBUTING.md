# Contributing to Copilot Multi-Agent Sandbox

Thanks for your interest in improving this project. This is an early-stage pilot — contributions, feedback, and bug reports are all welcome.

## How to Contribute

### Found a bug or have a question?

**Open an issue.** Please don't ping maintainers directly on Teams or email. Issues help everyone benefit from the answer.

When filing an issue, include:

- What you were trying to do
- What happened instead
- Your VS Code version and Copilot extension version
- Any relevant error messages or screenshots

### Want to propose a change?

1. **Open an issue first** to discuss the idea before writing code. This avoids wasted effort on changes that may not align with the project direction.
2. **Fork the repo** and create a feature branch from `main`.
3. **Make your changes** — keep them focused. One PR per concern.
4. **Test your changes** — if modifying agent files, verify they load correctly in VS Code and behave as expected.
5. **Open a pull request** against `main` with a clear description of what and why.

### Modifying agent definitions

Agent files live in `.claude/agents/`. If you're editing an agent:

- Preserve the YAML frontmatter structure (`name`, `description`, `model`, `tools`, `handoffs`).
- Test that the agent appears in the VS Code chat picker after your change.
- If adding a new tool to an agent's `tools:` list, verify the tool is available in your Copilot setup.
- Update `docs/agents/` reference copies if they exist.

### Code style

- For .NET/C# code (the Selenium demo project): follow existing conventions in the repo.
- For Markdown: use ATX-style headers, keep lines under 120 characters where practical.
- For agent prompts: be specific and actionable. Avoid vague instructions.

## What's in scope

This repo is focused on **demonstrating multi-agent workflows with GitHub Copilot in VS Code**. Contributions that improve the agent definitions, documentation, developer experience, or demo quality are all welcome.

Out of scope (for now):

- Adding entirely new demo projects beyond the Selenium xUnit target
- Integrations with non-Copilot AI tools
- Production deployment configurations

## Code of Conduct

Be professional, be kind, be constructive. This is a government pilot program — treat it accordingly.
