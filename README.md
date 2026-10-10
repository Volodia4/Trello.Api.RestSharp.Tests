# Trello API Test Automation

This repository contains an automated API testing framework for the Trello REST API. 
It focuses on backend business logic, demonstrating CRUD operations, test isolation, and negative testing scenarios.

## Tech Stack
- **Language:** C#
- **Testing Framework:** NUnit
- **HTTP Client:** RestSharp
- **CI/CD:** GitHub Actions

## Key Features
- **Test Isolation:** Each test dynamically creates and destroys its own test data (Boards -> Lists -> Cards) to prevent conflicts.
- **Teardown Mechanism:** Safely cleans up the environment after execution to avoid spamming the Trello account.
- **Negative Testing:** Covers edge cases and security validations (e.g., 400 Bad Request, 401 Unauthorized, 404 Not Found).
- **Environment Configuration:** Uses `appsettings.json` for secure API credential management.

## How to Run Locally

1. Clone the repository.
2. In the `Trello.Api.RestSharp.Tests` directory, create an `appsettings.json` file. 
3. Add your Trello API credentials (you can generate them in the [Trello Power-Ups Admin](https://trello.com/power-ups/admin)):
```json
{
  "Trello": {
    "BaseUrl": "https://api.trello.com",
    "Key": "YOUR_API_KEY",
    "Token": "YOUR_API_TOKEN"
  }
}
```
4. Restore dependencies and run the tests:
```bash
dotnet restore
dotnet test
```

## CI/CD Pipeline
The project is integrated with GitHub Actions (api-tests.yml). The workflow automatically builds the project and executes the test suite on every push to the main branch. Environment variables and API keys are securely managed via GitHub Secrets.
