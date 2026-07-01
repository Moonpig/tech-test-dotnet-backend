# Development environment

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Quick start

From the repository root:

```bash
./scripts/setup-dev.sh
dotnet run --project Moonpig.PostOffice.Api
```

The API listens on `http://localhost:5000`.

## Common commands

```bash
# Restore and build
dotnet restore Moonpig.PostOffice.sln
dotnet build Moonpig.PostOffice.sln

# Run tests
dotnet test Moonpig.PostOffice.sln

# Run the API
dotnet run --project Moonpig.PostOffice.Api
```

## API smoke test

```bash
curl "http://localhost:5000/api/DespatchDate?ProductIds=1&orderDate=2018-01-29T00:00:00"
```

Expected response:

```json
{"date":"2018-01-30T00:00:00"}
```

## Solution structure

| Project | Purpose |
|---------|---------|
| `Moonpig.PostOffice.Api` | ASP.NET Core Web API |
| `Moonpig.PostOffice.Data` | Stub data access layer |
| `Moonpig.PostOffice.Tests` | xUnit tests |
