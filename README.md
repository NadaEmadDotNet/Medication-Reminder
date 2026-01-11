# 🚀 Medication Reminder API

A .NET Core Web API for managing patients, medications, and dose logs with a focus on clean architecture, security, and scalability. This project is a complete API built from scratch, including authentication, email confirmation, caching, pagination, and logging.

⚡ Features

User Management: Support for Admin, Doctor, and Patient roles.

Authentication & Authorization:

JWT-based authentication.

Refresh tokens for session management.

Password change and token versioning.

Email Confirmation: Users must confirm their email upon registration.

CRUD Operations: Full management of Patients, Medications, and Dose Logs.

Data Validation: Using FluentValidation for DTOs to ensure data integrity.

Admin Actions: Activate/deactivate users.

Logging: Key events and errors logged using ILogger.

Caching: Frequently accessed endpoints are cached to improve performance.

Pagination: Large datasets can be efficiently retrieved with pagination.

Clean Architecture: Business logic separated from data access (Services, DTOs, Validators).

🛠 Tech Stack

Backend: .NET 7, ASP.NET Core Web API

Authentication: Identity + JWT + Refresh Tokens

ORM: Entity Framework Core

Validation: FluentValidation

Database: SQL Server / LocalDb

Logging: ILogger

Caching: In-memory cache

Email: SMTP / Email Confirmation
