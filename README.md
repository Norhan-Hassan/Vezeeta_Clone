# Vezeeta Clone API

A healthcare appointment booking platform API inspired by [Vezeeta](https://www.vezeeta.com/), built with **ASP.NET Core 9** and **Clean Architecture (Onion Architecture)**. This system enables patients to book appointments with doctors, manage medical records, and streamline healthcare provider operations.

> 🚧 **Project Status:** Under active development — some handlers contain `NotImplementedException` placeholders.

---

## 🏗️ Architecture Overview

The project follows a **5-layer Onion Architecture** combined with **CQRS (Command Query Responsibility Segregation)** via **MediatR**, ensuring separation of concerns, maintainability, and testability.

```
Vezeeta_Clone/
├── Vezeeta_Clone.Api        → Presentation Layer (Controllers, Middleware, Program.cs)
├── Vezeeta_Clone.Core    → Application Layer (CQRS Commands/Queries, Handlers, Validators, Mapping)
├── Vezeeta_Clone.Service          → Business Logic Layer (Service Interfaces & Implementations)
├── Vezeeta_Clone.Infrastructure   → Infrastructure Layer (EF Core, Repositories, Identity, JWT, Swagger)
└── Vezeeta_Clone.Data             → Domain Layer (Entities, Enums, DTOs, Constants)
```

### Request Flow

```
HTTP Request
    ↓
Controller (AppControllerBase)
    ↓
MediatR.Send(Command / Query)
    ↓
Pipeline Behaviors (ValidationBehavior)
    ↓
Command/Query Handler
    ↓
Service Layer (Business Logic)
    ↓
Repository Layer (Generic Repository)
    ↓
EF Core → SQL Server
    ↓
Response<T> → Controller → HTTP Response
```

---

## 🛠️ Technology Stack

| Category | Technology |
|---|---|
| **Framework** | ASP.NET Core 9.0 (.NET 9) |
| **Language** | C# 13.0 |
| **ORM** | Entity Framework Core 9.0.10 |
| **Database** | SQL Server |
| **Authentication** | ASP.NET Core Identity + JWT Bearer Tokens |
| **Authorization** | Role-based (Admin, Doctor, Patient) |
| **CQRS & Mediator** | MediatR 14.1.0 |
| **Object Mapping** | AutoMapper 16.1.0 |
| **Validation** | FluentValidation 12.1.1 (via MediatR Pipeline Behavior) |
| **Background Jobs** | Hangfire 1.8.13 (Async slot generation, recurring jobs) |
| **Localization** | IStringLocalizer with .resx resources (English & Arabic) |
| **API Documentation** | Swashbuckle.AspNetCore 9.0.6 with JWT support |

---

## 📋 Project Structure

```
Vezeeta_Clone.Api/
├── Base/
│   └── AppControllerBase.cs    # Base controller with MediatR + Response mapping
├── Controllers/
│   ├── AuthenticationController.cs      # Register, SignIn, RefreshToken, ChangePassword
│   └── SpecializationsController.cs     # CRUD for specializations
├── Program.cs       # App entry point, DI, middleware pipeline
└── appsettings.json

Vezeeta_Clone.Core/
├── Bases/
│   ├── Response.cs                # Generic API response wrapper
│   └── ResponseHandler.cs              # Standardized response factory methods
├── Behavior/
│   └── ValidationBehavior.cs          # MediatR pipeline for FluentValidation
├── Features/
│├── Auth/
│   │   ├── Commands/
│   │   │   ├── Models/                  # RegisterDoctorCommand, RegisterPatientCommand,
│   │   │   │           # SignInCommand, ChangePasswordCommand, RefreshTokenCommand
│   │   │ └── Handlers/             # DoctorAuthCommandHandler, SignInCommandHandler, etc.
│   │   ├── Queries/
│   │   │   └── Models/            # AuthenticateUserQuery
│   │   └── Shared/
│   │   └── RegisterUserBase.cs      # Shared registration fields
│   └── Specializations/
│       ├── Commands/
│       │   ├── Models/        # CreateSpecializationCommand, UpdateSpecializationCommand
│       │   └── Handlers/         # SpecializationCommandHandler
│       └── Queries/
│           ├── Models/       # GetSpecializationsQuery, GetSubSpecializationBySpecIDQuery
│         ├── Results/       # Query result DTOs
│    └── Handlers/     # SpecializationQueryHandler
├── Middleware/
│   └── ErrorHandlerMiddleware.cs        # Global exception handling
├── Resources/
│   ├── SharedResources.cs
│   └── SharedResourcesKeys.cs          # Localization key constants
└── ModuleCoreDependecies.cs       # MediatR, AutoMapper, FluentValidation DI

Vezeeta_Clone.Service/
├── Abstract/
│ ├── IAuthenticationService.cs
│   ├── IAutherizationService.cs
│   ├── IDoctorService.cs
│   ├── ISpecializationService.cs
│   ├── IDoctorAvailabilityService.cs
│   └── ISlotGenerationService.cs
├── AppUserAuthServices/
│ ├── Abstract/
│   │   └── ICurrentUserService.cs  # Get current authenticated user from JWT claims
│   └── Implementation/
│       └── CurrentUserService.cs
├── Implementation/
│   ├── AuthenticationService.cs   # JWT generation, refresh tokens, registration
│   ├── AutherizationService.cs          # Role CRUD operations
│   ├── DoctorService.cs
│   ├── SpecializationService.cs
│   ├── DoctorAvailabilityService.cs   # Availability management with validation
│   └── SlotGenerationService.cs        # Intelligent slot generation engine
├── BackgroundJobServices/
│   ├── Abstract/
│   │   └── IBackgroundJobService.cs   # Fire-and-forget, scheduled, recurring jobs
│   └── Implementation/
│       └── BackgroundJobService.cs    # Hangfire integration
└── ModuleServiceDependecies.cs     # Service layer DI

Vezeeta_Clone.Infrastructure/
├── Abstract/                 # Repository interfaces
├── Context/
│   └── ApplicationDbContext.cs          # EF Core DbContext with soft delete query filters
├── InfrastructureBases/
│ ├── IGenericRepositoryAsync.cs       # Generic repository interface
│   └── GenericRepositoryAsync.cs      # Generic repository with transactions
├── Repos/     # Concrete repository implementations
├── Seeder/
│   ├── RoleSeeder.cs                    # Seeds Admin, Doctor, Patient roles
│   ├── UserSeeder.cs              # Seeds default Admin user
│   └── SpecializationSeeder.cs# Seeds initial specializations
├── ServiceRegisteration.cs        # Identity, JWT, Swagger configuration
├── ModuleInfrastructureDependecies.cs   # Repository DI
└── Migrations/

Vezeeta_Clone.Data/
├── Entities/    # Domain models
├── Enums/           # Gender, BloodType, Status, Title, AvailabilityMethod
├── Commons/
│   └── Roles.cs # Role constants (Admin, Doctor, Patient)
├── Helper/
│   ├── JwtSettings.cs            # JWT configuration model
│   └── AppUserClaimModel.cs          # Custom claim model
├── Results/
│   └── JwtAuthResult.cs       # Access + Refresh token result
└── AppMetaData/
    └── Router.cs         # Centralized API route constants
```

---

## 🎯 Domain Entities

### User Management

| Entity | Description |
|---|---|
| `ApplicationUser` | Extends `IdentityUser` — shared base for all user types (FirstName, LastName, Gender, IsActive, CreatedAt) |
| `Doctor` | Title, Description, ExperienceInYears, WaitingTimeInMinutes, Picture, Specialization, SubSpecializations |
| `Patient` | DateOfBirth, BloodType, linked Appointments & Reviews |

### Appointments & Scheduling

| Entity | Description |
|---|---|
| `DoctorAvailability` | Weekly recurring (`DayOfWeek`) or one-time (`Date`) schedules with start/end times, duration, and availability method (Online/Offline) |
| `DoctorAvailabilitySlot` | Individual bookable time slots generated from availability |
| `Appointment` | Links Patient ↔ Slot with Status (Upcoming, Completed, Cancelled, Rescheduled) |

### Medical & Clinical

| Entity | Description |
|---|---|
| `Specialization` | Bilingual (NameAr, NameEn) with SubSpecializations |
| `SubSpecialization` | Child specializations, many-to-many with Doctors (via `DoctorSubSpecializations` join table) |
| `Clinic` | Name, Address, Region, Location (lat/lng), PhoneNumber |
| `DoctorClinic` | Links Doctor ↔ Clinic with Price |
| `MedicalRecord` | Patient medical history linked to Doctor and optional Appointment |
| `Diagnosis` | Doctor-recorded diagnoses per MedicalRecord |
| `EPrescription` | Electronic prescriptions (Medication, Dose, Notes) |

### Supporting

| Entity | Description |
|---|---|
| `Review` | Patient ratings with comments for Doctors (max 250 chars) |
| `DoctorPatient` | Tracks Doctor ↔ Patient relationship (FirstVisitAt, LastVisitAt) |
| `Notification` | System notifications per user |
| `UserToken` | JWT + Refresh token persistence with revocation support (JwtId, IsUsed, IsRevoked) |
| `City` / `Region` / `Location` | Hierarchical geographic structure |

---

## 🔑 Key Design Patterns & Techniques

### CQRS with MediatR

All operations are separated into **Commands** (write) and **Queries** (read):

```csharp
// Command — write operation
public class CreateSpecializationCommand : IRequest<Response<string>>
{
    public string NameAr { get; set; }
    public string NameEn { get; set; }
}

// Query — read operation
public class GetSubSpecializationBySpecIDQuery : IRequest<Response<List<GetSubSpecializationBySpecIDQueryResult>>>
{
    public int SpecializationID { get; set; }
}
```

### Pipeline Behavior (FluentValidation)

Validation runs automatically before handlers via `ValidationBehavior<TRequest, TResponse>`. Any `IValidator<TRequest>` registered in DI is invoked before the handler executes — invalid requests throw a `ValidationException` caught by the global middleware.

### Generic Repository Pattern

All repositories inherit from `GenericRepositoryAsync<T>` providing:

- `AddAsync`, `UpdateAsync`, `DeleteAsync`, `AddRangeAsync`, `UpdateRangeAsync`, `DeleteRangeAsync`
- `GetTableNoTracking()` / `GetTableAsTracking()`
- `GetByIntIdAsync()` / `GetByStringIdAsync()`
- `BeginTransaction()`, `Commit()`, `RollBack()`
- `SaveChangesAsync()`

### Standardized Response Wrapper

All API responses use `Response<T>`:

```csharp
public class Response<T>
{
    public HttpStatusCode StatusCode { get; set; }
    public bool Succeeded { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
  public List<string> Errors { get; set; }
    public object Meta { get; set; }
}
```

`ResponseHandler` provides factory methods: `Success()`, `Created()`, `NotFound()`, `BadRequest()`, `Unauthorized()`, `UnprocessableEntity()`, `Deleted()`, `Updated()`.

### Global Exception Handling

`ErrorHandlerMiddleware` catches all unhandled exceptions and maps them to appropriate HTTP status codes:

| Exception | HTTP Status |
|---|---|
| `UnauthorizedAccessException` | 401 Unauthorized |
| `ValidationException` (FluentValidation) | 422 Unprocessable Entity |
| `KeyNotFoundException` | 404 Not Found |
| `DbUpdateException` | 400 Bad Request |
| `Exception` (general) | 500 Internal Server Error |

### Soft Delete

Entities inheriting from `BaseEntity` have an `IsDeleted` flag. A **global query filter** is applied automatically in `ApplicationDbContext` using expression trees:

```csharp
// All queries on BaseEntity subclasses automatically filter: WHERE IsDeleted = false
foreach (var entityType in builder.Model.GetEntityTypes()
    .Where(et => typeof(BaseEntity).IsAssignableFrom(et.ClrType)))
{
    builder.Entity(entityType.ClrType).HasQueryFilter(entity => entity.IsDeleted == false);
}
```

`ApplicationUser` uses `IsActive` instead of `IsDeleted`.

### Database Seeding

On startup, the following are seeded automatically:

- **Roles**: Admin, Doctor, Patient
- **Admin User**: `Admin@vezeeta.com` / `Admin@123ADM567`
- **Specializations**: Initial medical specializations

---

## 🔐 Authentication & Authorization

### JWT Authentication Flow

```
1. User registers (Doctor/Patient) or signs in
2. Server validates credentials via ASP.NET Identity
3. Server generates JWT Access Token + Refresh Token
4. Refresh Token is persisted in UserToken table
5. Client sends Access Token in Authorization header
6. On expiry, client exchanges Refresh Token for new Access Token
7. Revoked tokens are tracked via IsRevoked flag
```

### JWT Claims

| Claim | Source |
|---|---|
| `Id` | ApplicationUser.Id |
| `Email` | ApplicationUser.Email |
| `UserName` | ApplicationUser.UserName |
| `Role` | First role from UserManager |

### Roles

| Role | Description |
|---|---|
| `Admin` | Full system access, seeded on startup |
| `Doctor` | Medical professional operations |
| `Patient` | Appointment booking, reviews |

### Current User Service

`ICurrentUserService` extracts the authenticated user from JWT claims at any layer:

```csharp
var user = await _currentUserService.GetCurrentUserAsync();
var userId = _currentUserService.GetCurrentUserId();
var roles = await _currentUserService.GetCurrentUserRolesAsync();
```

---

## 🌍 Localization (i18n)

Supports **English (en-US)** and **Arabic (ar-EG)** via `IStringLocalizer<SharedResources>`.

### Language Selection

Clients specify language via:

- **Query string**: `?culture=ar-EG`
- **Accept-Language header**: `Accept-Language: ar-EG`

### Localization Keys

All keys are centralized in `SharedResourcesKeys`:

```csharp
public const string NotFound = "NotFound";
public const string AddSuccess = "AddSuccess";
public const string EmailAlreadyExists = "EmailAlreadyExists";
public const string FailedToRegister = "FailedToRegister";
public const string PasswordChangedSuccess = "PasswordChangedSuccess";
// ... and more
```

---

## 📡 API Endpoints

### Authentication (`api/v1/auth/`)

| Method | Route | Description | Auth |
|---|---|---|---|
| `POST` | `doctor-register` | Register a new doctor | ❌ |
| `POST` | `patient-register` | Register a new patient | ❌ |
| `POST` | `signIn` | Sign in and get JWT tokens | ❌ |
| `POST` | `refresh-token` | Get new access token via refresh token | ❌ |
| `GET` | `check-token-validation` | Validate a JWT token | ❌ |
| `POST` | `change-password` | Change current user password | ✅ |
| `POST` | `role/create` | Create a new role | ✅ |
| `PUT` | `role/update` | Update an existing role | ✅ |
| `DELETE` | `role/delete` | Delete a role | ✅ |

### Doctors (`api/v1/doctor/`)

| Method | Route | Description | Auth |
|---|---|---|---|
| `GET` | `list` | List all doctors | ❌ |
| `GET` | `{Id:Guid}` | Get doctor by ID | ❌ |

### Specializations (`api/v1/specialization/`)

| Method | Route | Description | Auth |
|---|---|---|---|
| `POST` | `create` | Create a specialization | ✅ |
| `PUT` | `update` | Update a specialization | ✅ |
| `GET` | `{SpecializationID:int}/sub-specializations` | Get sub-specializations by specialization ID | ❌ |

### Scheduling (`api/v1/schedule/`)

| Method | Route | Description | Auth |
|---|---|---|---|
| `POST` | `set-availability` | Create availability & trigger slot generation | ✅ |
| `GET` | `{doctorId}` | Get doctor's availability patterns | ❌ |

---

## ✨ New Features (Latest Release)

### 1. **Intelligent Slot Generation System**

Automatically generates bookable appointment slots based on flexible doctor scheduling patterns.

**Features:**
- **Weekly Recurring**: Generate slots for specific days (e.g., every Monday, Wednesday, Friday)
- **One-Time Special Dates**: Create availability for special open days
- **Smart Duplicate Prevention**: O(1) deduplication using HashSet
- **Configurable Duration**: Customize slot lengths (in minutes)

### 2. **Background Job Processing with Hangfire**

Asynchronous slot generation via fire-and-forget and recurring jobs.

**Features:**
- Automatic job enqueueing on availability creation
- Hangfire Dashboard at `/Hangfire-Dashboard`
- Persistent job storage in SQL Server
- Automatic cleanup of orphaned jobs

### 3. **Doctor Availability Management Service**

Validation and slot generation triggering on availability creation.

**Validations:**
- Start time < End time
- Offline availability requires clinic
- Prevents overlapping schedules
- Auto-triggers background slot generation

---

## 💾 Database Diagram (Entity Relationships)

```
ApplicationUser (IdentityUser)
 ├── 1:1 → Doctor
 │           ├── N:1 → Specialization
    │           │       └── 1:N → SubSpecialization
  │           ├── N:N → SubSpecialization (via DoctorSubSpecializations)
    │           ├── 1:N → DoctorClinic → Clinic
    │           │             ├── N:1 → Region → City
    │           │        └── N:1 → Location
    │           ├── 1:N → DoctorAvailability
    │           │           └── 1:N → DoctorAvailabilitySlot
    │     │      └── 1:1 → Appointment
    │   ├── 1:N → DoctorPatient
    │      └── 1:N → Review
    │
    ├── 1:1 → Patient
    │           ├── 1:N → Appointment
    │           ├── 1:N → DoctorPatient
    │└── 1:N → Review
    │
    ├── 1:N → Notification
    └── 1:N → UserToken

MedicalRecord
    ├── N:1 → Doctor
  ├── N:1 → Patient
    ├── N:1 → Appointment (optional)
    ├── 1:N → Diagnosis
    └── 1:N → EPrescription
```

All foreign key relationships use `DeleteBehavior.Restrict` to prevent cascading deletes.

---

## 📦 NuGet Dependencies

| Package | Version | Purpose |
|---|---|---|
| `Microsoft.EntityFrameworkCore.SqlServer` | 9.0.10 | SQL Server provider |
| `Microsoft.AspNetCore.Identity.EntityFrameworkCore` | 9.0.10 | Identity with EF Core |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | 9.0.13 | JWT authentication |
| `MediatR` | 14.1.0 | CQRS mediator pattern |
| `AutoMapper` | 16.1.0 | Object-to-object mapping |
| `FluentValidation` | 12.1.1 | Request validation |
| `FluentValidation.DependencyInjectionExtensions` | 12.1.1 | FluentValidation DI integration |
| `Swashbuckle.AspNetCore` | 9.0.6 | Swagger/OpenAPI documentation |
| `Swashbuckle.AspNetCore.Annotations` | 9.0.6 | Swagger endpoint annotations |
| `Hangfire.Core` | 1.8.13 | Background job processing |
| `Hangfire.SqlServer` | 1.8.13 | SQL Server job storage for Hangfire |

---

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server (LocalDB, Express, or full)
- Visual Studio 2022+ or VS Code

### Setup

1. **Clone the repository**

   ```bash
   git clone https://github.com/Norhan-Hassan/Vezeeta_Clone.git
   cd Vezeeta_Clone
   ```

2. **Configure the database connection**

   Update `appsettings.json` in `Vezeeta_Clone.Api`:

   ```json
 {
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=VezeetaCloneDb;Trusted_Connection=true;TrustServerCertificate=true;"
     },
     "JwtSettings": {
       "Secret": "YourSuperSecretKeyHere_MustBeAtLeast32Characters",
        "Issuer": "VezeetaCloneAPI",
       "Audience": "VezeetaCloneClient",
       "ValidateIssuer": true,
       "ValidateAudience": true,
       "ValidateLifeTime": true,
       "ValidateIssuerSigningKey": true,
       "AccessTokenExpireDate": 7,
       "RefreshTokenExpireDate": 2
     }
   }
   ```

3. **Apply migrations**

   ```bash
dotnet ef database update --project Vezeeta_Clone.Infrastructure --startup-project Vezeeta_Clone.Api
   ```

4. **Run the application**

   ```bash
   dotnet run --project Vezeeta_Clone.Api
   ```

5. **Access Swagger UI**

   ```
   https://localhost:{port}/swagger
   ```

### Default Admin Credentials

| Field | Value |
|---|---|
| **Email** | `Admin@vezeeta.com` |
| **Password** | `Admin@123ADM567` |

---

## ✅ Best Practices Implemented

- **Clean Architecture** — strict layer separation with dependency inversion
- **CQRS Pattern** — Commands and Queries are separate classes with dedicated handlers
- **MediatR Pipeline** — cross-cutting concerns (validation) via `IPipelineBehavior`
- **Generic Repository** — reusable data access with full transaction support
- **Centralized Routing** — all API routes defined as constants in `Router.cs`
- **Standardized Responses** — consistent `Response<T>` wrapper for all endpoints
- **Global Error Handling** — middleware catches and formats all exceptions consistently
- **Soft Delete** — global EF Core query filters on `IsDeleted` via expression trees
- **Database Seeding** — roles, admin user, and specializations seeded on startup
- **Bilingual Support** — full Arabic & English localization via resource files
- **JWT Security** — access + refresh token pattern with database-backed revocation
- **Current User Abstraction** — `ICurrentUserService` for clean auth context access
- **Restrict Delete Behavior** — all foreign keys use `DeleteBehavior.Restrict`
- **Background Jobs** — Hangfire for async slot generation without blocking API
- **Slot Deduplication** — HashSet-based O(1) duplicate detection
- **UTC Timezone Consistency** — all time operations use UTC for global compatibility
- **Transaction Support** — generic repository provides transaction management (Begin, Commit, Rollback)
