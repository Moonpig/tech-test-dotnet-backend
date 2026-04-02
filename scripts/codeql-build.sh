#!/bin/bash -e

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
WORKSPACE="${GITHUB_WORKSPACE:-$ROOT_DIR}"

SOLUTION_PATH="$WORKSPACE/Moonpig.PostOffice.sln"

cd "$WORKSPACE"
dotnet tool restore

if [[ -f "$SOLUTION_PATH" ]]; then
  dotnet build "$SOLUTION_PATH" -c Release
else
  dotnet build -c Release
fi
