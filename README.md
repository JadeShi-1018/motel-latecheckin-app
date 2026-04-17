# Motel Late Check-in System

## Overview
A web-based system designed to streamline motel late check-in processes.  
Guests can submit their check-in details online, while front desk staff can securely manage bookings and payments through an admin dashboard.

This project focuses on real-world scenarios including payment handling, identity verification, and secure access control.

---

## Features

### Guest Side
- Online check-in form
- ID upload and preview
- E-signature
- Date validation (check-in / check-out logic)
- Stripe payment / pre-authorization

### Admin Side
- Secure admin dashboard
- Authentication using ASP.NET Core Identity
- Role-based access (admin only)
- Capture / release payment functionality

### System Features
- Input validation
- File upload handling
- Configuration-based secrets (no hardcoded credentials)
- Database seeding for admin user

---

## Tech Stack

- **Backend:** ASP.NET Core (.NET 8)
- **Frontend:** Razor Pages
- **Authentication:** ASP.NET Core Identity
- **Database:** SQLite (development)
- **Payments:** Stripe API

---

## Architecture Highlights

- Clean separation of concerns (Pages / Models / Services)
- Dependency Injection used across the application
- Configuration-driven design (appsettings + environment variables)
- Secure handling of sensitive data (no secrets in source code)

---

## How to Run

```bash
git clone <your-repository-url>
cd LateCheckInApp
dotnet restore
dotnet run