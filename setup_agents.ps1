<#
.SYNOPSIS
    Deploy multi-agent workflow to .github\agents\ with per-agent model assignment.

.DESCRIPTION
    Run once after cloning:

        .\setup_agents.ps1

    Copies agents from .claude\agents\ to .github\agents\ (VS Code default
    discovery path) and rewrites the model: field per role.

    .github\agents\ is gitignored and never pushed.

    To change model assignments, edit $ModelMap below and re-run.
#>

# --- Model assignments per agent role -------------------------------------------
# Edit these to match the models available in your Copilot subscription.
$ModelMap = @{
    "Orchestrator" = "GPT-5.4"
    "Planner"      = "Gemini 3.1 Pro (preview)"
    "Designer"     = "Gemini 2.5 Pro"
    "Coder"        = "GPT-5.3 Codex"
}
# --------------------------------------------------------------------------------

$ErrorActionPreference = "Stop"
$RepoRoot  = Split-Path -Parent $MyInvocation.MyCommand.Path
$SourceDir = Join-Path $RepoRoot ".claude" "agents"
$TargetDir = Join-Path $RepoRoot ".github" "agents"

if (-not (Test-Path $SourceDir)) {
    Write-Host "ERROR: Source not found: $SourceDir" -ForegroundColor Red
    Write-Host "Make sure you cloned the repo and are running from the repo root."
    exit 1
}

if (-not (Test-Path $TargetDir)) {
    New-Item -ItemType Directory -Path $TargetDir -Force | Out-Null
}

$agents = Get-ChildItem -Path $SourceDir -Filter "*.agent.md"

if ($agents.Count -eq 0) {
    Write-Host "No .agent.md files found in $SourceDir" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "Deploying $($agents.Count) agent(s)..." -ForegroundColor Cyan
Write-Host ""

foreach ($file in $agents) {
    $stem = $file.BaseName -replace "\.agent$", ""
    $content = Get-Content -Path $file.FullName -Raw -Encoding UTF8

    if ($ModelMap.ContainsKey($stem)) {
        $model = $ModelMap[$stem]
        $content = $content -replace "(?m)^(model:\s*)(.+)$", "`${1}$model"
        Write-Host "  $($file.Name.PadRight(30)) -> $model" -ForegroundColor Green
    }
    else {
        Write-Host "  $($file.Name.PadRight(30)) -> (unchanged)" -ForegroundColor Yellow
    }

    $dst = Join-Path $TargetDir $file.Name
    [System.IO.File]::WriteAllText($dst, $content, [System.Text.Encoding]::UTF8)
}

Write-Host ""
Write-Host "Done. Agents deployed to $TargetDir" -ForegroundColor Cyan
Write-Host "Reload VS Code to pick up the agents (Ctrl+Shift+P -> Reload Window)."
Write-Host ""
Write-Host "To change models, edit `$ModelMap at the top of this script and re-run." -ForegroundColor Gray
