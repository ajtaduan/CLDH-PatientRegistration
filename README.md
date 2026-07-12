# CLDH Patient Mini Registration System

A simple clinic patient registration system built for the CLDH technical assessment.
Backend: ASP.NET Core Web API. Database: SQLite (via EF Core). Frontend: plain HTML/CSS/JS.

## Features
- Login / logout with cookie-based authentication
- Protected patient management screen (requires login)
- Create, view, edit, and delete patient records
- Patient fields: Full Name, Birth Date, Gender, Contact Number, Address

## Prerequisites
- [.NET SDK 8.0+](https://dotnet.microsoft.com/download) (built and tested on .NET 10)

## Setup & Run

1. Clone the repo:
```bash
   git clone https://github.com/ajtaduan/CLDH-PatientRegistration.git
   cd CLDH-PatientRegistration/CLDH.PatientRegistration
```

2. Restore packages:
```bash
   dotnet restore
```

3. Apply the database migration (creates `clinic.db` with the required tables):
```bash
   dotnet ef database update
```
   (If `dotnet-ef` isn't installed: `dotnet tool install --global dotnet-ef`)

4. Run the app:
```bash
   dotnet run
```

5. Open your browser to the URL shown in the terminal (e.g. `http://localhost:5078`)

## Login Credentials
A single admin account is seeded automatically on first run:
- **Username:** `admin`
- **Password:** `Cldh@2026!`

> Note: This is a demo credential for assessment purposes. In a production system this would be moved to a secrets/config file instead of being hardcoded.

## Project Structure
- `Controllers/` — API endpoints (Auth, Patients)
- `Models/` — Patient and User data models
- `Data/` — EF Core database context
- `Services/` — Password hashing service
- `DTOs/` — Request/response shapes
- `Migrations/` — EF Core migration history
- `wwwroot/` — Frontend (login page, patient management page, CSS, JS)

## Security Notes
- Passwords are hashed using ASP.NET Core's built-in `PasswordHasher`, never stored in plain text
- Login cookie is `HttpOnly` (not accessible via JavaScript) to reduce XSS risk
- All patient endpoints require authentication (`[Authorize]`)
- SQLite database file (`clinic.db`) is excluded from version control via `.gitignore`