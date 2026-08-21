#!/usr/bin/env bash
set -euo pipefail

if [ "$#" -lt 1 ]; then
  echo "Usage: $0 <ProblemName>"
  exit 1
fi

dotnet run --project LeetCodeProblems/LeetCodeProblems.csproj -- "$@"
