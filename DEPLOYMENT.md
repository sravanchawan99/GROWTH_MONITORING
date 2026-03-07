# Deployment Guide (Manual GitHub Pipelines)

This repository now uses two manual GitHub Actions workflows:

- `Provision Azure Infrastructure (Manual)`
- `Deploy Backend and Frontend (Manual)`

## 1) Configure Azure auth for GitHub Actions (OIDC)

Create a Microsoft Entra app/service principal and grant it access to your subscription or resource group.

In GitHub repository:

1. Go to `Settings` -> `Secrets and variables` -> `Actions`.
2. Add these repository secrets:
   - `AZURE_CLIENT_ID`
   - `AZURE_TENANT_ID`
   - `AZURE_SUBSCRIPTION_ID`

If you use a GitHub Environment named `growthmonitoring`, add the same secrets there:

1. Go to `Settings` -> `Environments` -> `growthmonitoring`.
2. Add environment secrets with the same names.

## 2) Run infrastructure pipeline

1. Open `Actions` tab.
2. Select workflow `Provision Azure Infrastructure (Manual)`.
3. Click `Run workflow`.
4. Wait for job `provision` to complete.

This creates/updates:

- Resource Group: `rg-growthmonitoring`
- Function App: `fa-growthmonitoring`
- Web App: `app-growthmonitoring`
- Location: `eastasia`

## 3) Run code deployment pipeline

1. Open `Actions` tab.
2. Select workflow `Deploy Backend and Frontend (Manual)`.
3. Click `Run workflow`.
4. Wait for both jobs:
   - `Deploy Azure Function`
   - `Build and Deploy Angular App`

## 4) Validate deployment

- Frontend URL: `https://app-growthmonitoring.azurewebsites.net`
- Backend health test (example):
  - `POST https://fa-growthmonitoring.azurewebsites.net/api/predict_zone`
  - JSON body:
    ```json
    { "height": 120, "weight": 20 }
    ```

## Notes

- Workflows are manual only (`workflow_dispatch`), no auto-trigger on `push`.
- Frontend API target is set to:
  - `https://fa-growthmonitoring.azurewebsites.net/api/predict_zone`
