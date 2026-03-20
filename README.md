# Selenium Sandbox

Public, minimal Selenium + xUnit sandbox for validating browser automation patterns and multi-agent workflow specialization.

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
- Do not include internal endpoints, credentials, tokens, or organization-specific identifiers.
