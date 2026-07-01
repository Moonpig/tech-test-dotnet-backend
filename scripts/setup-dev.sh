#!/usr/bin/env bash
set -euo pipefail

DOTNET_VERSION="8.0"
DOTNET_INSTALL_DIR="${DOTNET_ROOT:-$HOME/.dotnet}"

if ! command -v dotnet >/dev/null 2>&1; then
  echo "Installing .NET SDK ${DOTNET_VERSION}..."
  curl -fsSL https://dot.net/v1/dotnet-install.sh -o /tmp/dotnet-install.sh
  bash /tmp/dotnet-install.sh --channel "${DOTNET_VERSION}" --install-dir "${DOTNET_INSTALL_DIR}"
fi

export DOTNET_ROOT="${DOTNET_INSTALL_DIR}"
export PATH="${PATH}:${DOTNET_ROOT}:${DOTNET_ROOT}/tools"

echo "Using $(dotnet --version)"

dotnet restore Moonpig.PostOffice.sln
dotnet build Moonpig.PostOffice.sln --no-restore

echo "Development environment is ready."
echo "Run the API with: dotnet run --project Moonpig.PostOffice.Api"
echo "Run tests with: dotnet test Moonpig.PostOffice.sln"
