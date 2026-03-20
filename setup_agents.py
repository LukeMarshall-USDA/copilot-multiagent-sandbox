#!/usr/bin/env python3
"""
setup_agents.py - Deploy multi-agent workflow to .github/agents/ with per-agent model assignment.

Run this once after cloning the repo:

    python setup_agents.py

What it does:
  1. Reads agent definitions from .claude/agents/ (the repo source of truth)
  2. Copies them to .github/agents/ (where VS Code discovers them natively)
  3. Rewrites the model: field in each agent to the recommended model for that role

Why this exists:
  - .claude/agents/ schema only accepts Claude model names (sonnet, opus, haiku, inherit)
  - .github/agents/ is the VS Code Copilot default path and accepts any model
  - Org push rules may block committing to .github/ paths
  - This script bridges all three constraints by deploying locally after clone

The .github/agents/ directory is gitignored and never pushed to the remote.
"""

import re
import shutil
from pathlib import Path

# ---------------------------------------------------------------------------
# Model assignments per agent role
#
# Edit these to match the models available in your Copilot subscription.
# Run `python setup_agents.py` again after changing to re-deploy.
# ---------------------------------------------------------------------------
MODEL_MAP: dict[str, str] = {
    "Orchestrator": "GPT-5.4",
    "Planner":      "Gemini 3.1 Pro (preview)",
    "Designer":     "Gemini 2.5 Pro",
    "Coder":        "GPT-5.3 Codex",
}

# ---------------------------------------------------------------------------
# Paths
# ---------------------------------------------------------------------------
SCRIPT_DIR = Path(__file__).resolve().parent
SOURCE_DIR = SCRIPT_DIR / ".claude" / "agents"
TARGET_DIR = SCRIPT_DIR / ".github" / "agents"


def rewrite_model(content: str, agent_stem: str) -> str:
    """Replace the model: line in YAML frontmatter with the assigned model."""
    model = MODEL_MAP.get(agent_stem)
    if not model:
        return content
    # Match model: <anything> inside frontmatter (between --- lines)
    return re.sub(
        r"(?m)^(model:\s*)(.+)$",
        rf"\g<1>{model}",
        content,
        count=1,
    )


def main() -> None:
    if not SOURCE_DIR.exists():
        print(f"ERROR: Source directory not found: {SOURCE_DIR}")
        print("Make sure you're running this from the repo root.")
        raise SystemExit(1)

    # Create target directory
    TARGET_DIR.mkdir(parents=True, exist_ok=True)

    agent_files = sorted(SOURCE_DIR.glob("*.agent.md"))
    if not agent_files:
        print(f"No .agent.md files found in {SOURCE_DIR}")
        raise SystemExit(1)

    print(f"Deploying {len(agent_files)} agent(s) to {TARGET_DIR}\n")

    for src in agent_files:
        stem = src.stem.replace(".agent", "")  # "Orchestrator.agent.md" -> "Orchestrator"
        content = src.read_text(encoding="utf-8")
        updated = rewrite_model(content, stem)

        dst = TARGET_DIR / src.name
        dst.write_text(updated, encoding="utf-8")

        assigned_model = MODEL_MAP.get(stem, "(unchanged)")
        print(f"  {src.name:30s} -> model: {assigned_model}")

    print(f"\nDone. Agents deployed to {TARGET_DIR}")
    print("Reload VS Code to pick up the new agents.")
    print("\nTo change model assignments, edit MODEL_MAP in setup_agents.py and re-run.")


if __name__ == "__main__":
    main()
