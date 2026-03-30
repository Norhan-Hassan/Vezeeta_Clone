# Vezeeta Clone API

A healthcare appointment booking platform API inspired by [Vezeeta](https://www.vezeeta.com/), built with **ASP.NET Core 9** and **Clean Architecture (Onion Architecture)**. This system enables patients to book appointments with doctors, manage medical records, process payments, and streamline healthcare provider operations.

---

## 🏗️ Architecture Overview

The project follows a **5-layer Onion Architecture** combined with **CQRS (Command Query Responsibility Segregation)** via **MediatR**, ensuring separation of concerns, maintainability, and testability.

```
Vezeeta_Clone/
├── Vezeeta_Clone.Api        →  Presentation Layer (Controllers, Middleware, Program.cs)
├── Vezeeta_Clone.Core       →  Application Layer (CQRS Commands/Queries, Handlers, Validators, Mapping)
├── Vezeeta_Clone.Service    →  Business Logic Layer (Service Interfaces & Implementations)
├── Vezeeta_Clone.Infrastructure →  Infrastructure Layer (EF Core, Repositories, Identity, JWT, Payments, Stripe)
└── Vezeeta_Clone.Data       →  Domain Layer (Entities, Enums, DTOs, Constants)
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

| Category               | Technology                                     |
| ---------------------- | ---------------------------------------------- |
| **Framework**          | ASP.NET Core 9.0 (.NET 9)                      |
| **Language**           | C# 13.0                                        |
| **ORM**                | Entity Framework Core 9.0.10                   |
| **Database**           | SQL Server                                     |
| **Authentication**     | ASP.NET Core Identity + JWT Bearer Tokens      |
| **Authorization**      | Role-based (Admin, Doctor, Patient)            |
| **CQRS & Mediator**    | MediatR 14.1.0                                 |
| **Object Mapping**     | AutoMapper 16.1.0                              |
| **Validation**         | FluentValidation 12.1.1 (via MediatR Pipeline) |
| **Background Jobs**    | Hangfire 1.8.13 (Async slot generation)        |
| **Localization**       | IStringLocalizer with .resx (English & Arabic) |
| **API Documentation**  | Swashbuckle.AspNetCore 9.0.6 with JWT support  |
| **Payment Processing** | Stripe API with strategy pattern               |

---

## Domain Entities

### User Management

| Entity            | Description                                                                                                |
| ----------------- | ---------------------------------------------------------------------------------------------------------- |
| `ApplicationUser` | Extends `IdentityUser` — shared base for all user types (FirstName, LastName, Gender, IsActive, CreatedAt) |
| `Doctor`          | Title, Description, ExperienceInYears, Picture, IsProfileComplete, Specialization, SubSpecializations      |
| `Patient`         | DateOfBirth, BloodType, Age calculation, linked Appointments, Reviews, Medical Records                     |

### Appointments & Scheduling

| Entity                   | Description                                                                                                           |
| ------------------------ | --------------------------------------------------------------------------------------------------------------------- |
| `DoctorAvailability`     | Weekly recurring (`DayOfWeek`) or one-time (`Date`) schedules with start/end times, duration, method (Online/Offline) |
| `DoctorAvailabilitySlot` | Individual bookable time slots generated from availability, booking status, lock state                                |
| `Appointment`            | Links Patient ↔ Slot with Status (Upcoming, Completed, Cancelled); optional Payment link                              |

### Payments

| Entity         | Description                                                                                                                                     |
| -------------- | ----------------------------------------------------------------------------------------------------------------------------------------------- |
| `Payment`      | Provider (Cash/Stripe/PayPal), Amount, Currency, Status, Client secret, Timestamps, Failure tracking, Idempotency key, Provider metadata (JSON) |
| `PaymentEvent` | Webhook events from providers, Raw payload storage, Event data, Retry tracking                                                                  |

### Medical & Clinical

| Entity              | Description                                                                                   |
| ------------------- | --------------------------------------------------------------------------------------------- |
| `Specialization`    | Bilingual (NameAr, NameEn) with SubSpecializations                                            |
| `SubSpecialization` | Child specializations, many-to-many with Doctors                                              |
| `Clinic`            | Name, Address, Region, GPS Location, PhoneNumber, WaitingTime, Price, linked to single Doctor |
| `MedicalRecord`     | Links Doctor ↔ Patient (via DoctorPatient), optional Appointment, Diagnoses, Prescriptions    |
| `DoctorPatient`     | Tracks Doctor ↔ Patient relationship (FirstVisitAt, LastVisitAt, TotalVisits)                 |
| `Diagnosis`         | Doctor-recorded findings linked to MedicalRecord                                              |
| `EPrescription`     | Medications, dosages, notes per MedicalRecord with multiple PrescriptionItems                 |
| `PrescriptionItem`  | Medication details (name, dose, frequency, duration)                                          |

### Supporting

| Entity                         | Description                                                                                          |
| ------------------------------ | ---------------------------------------------------------------------------------------------------- |
| `Review`                       | Patient 0-5 star ratings with optional comments (max 250 chars), anonymity flag, CreatedAt/UpdatedAt |
| `Notification`                 | System notifications per user                                                                        |
| `UserToken`                    | JWT + Refresh token persistence with revocation (JwtId, IsUsed, IsRevoked)                           |
| `City` / `Region` / `Location` | Hierarchical geographic structure with GPS coordinates                                               |
| `University`                   | Educational institution reference for doctors                                                        |

---

## Key Design Patterns & Techniques

### CQRS with MediatR

All operations are separated into **Commands** (write) and **Queries** (read):

```csharp
//  Command — write operation
public class CreateSpecializationCommand : IRequest<Response<string>>
{
    public string NameAr { get; set; }
    public string NameEn { get; set; }
}

//  Query — read operation
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

🚨 `ErrorHandlerMiddleware` catches all unhandled exceptions and maps them to appropriate HTTP status codes:

| Exception                                | HTTP Status               |
| ---------------------------------------- | ------------------------- |
| `UnauthorizedAccessException`            | 401 Unauthorized          |
| `ValidationException` (FluentValidation) | 422 Unprocessable Entity  |
| `KeyNotFoundException`                   | 404 Not Found             |
| `DbUpdateException`                      | 400 Bad Request           |
| `Exception` (general)                    | 500 Internal Server Error |

### Soft Delete

Entities inheriting from `BaseEntity` have an `IsDeleted` flag. A **global query filter** is applied automatically in `ApplicationDbContext` using expression trees:

```csharp
//  All queries on BaseEntity subclasses automatically filter: WHERE IsDeleted = false
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
- **Specializations**: Initial medical specializations with sub-specializations

---

## Authentication & Authorization

### JWT Authentication Flow

```
1️⃣ User registers (Doctor/Patient) or signs in
2️⃣ Server validates credentials via ASP.NET Identity
3️⃣ Server generates JWT Access Token + Refresh Token
4️⃣ Refresh Token is persisted in UserToken table
5️⃣ Client sends Access Token in Authorization header
6️⃣ On expiry, client exchanges Refresh Token for new Access Token
7️⃣ Revoked tokens are tracked via IsRevoked flag
```

### JWT Claims

| Claim      | Source                      |
| ---------- | --------------------------- |
| `Id`       | ApplicationUser.Id          |
| `Email`    | ApplicationUser.Email       |
| `UserName` | ApplicationUser.UserName    |
| `Role`     | First role from UserManager |

### Roles

| Role      | Description                                   |
| --------- | --------------------------------------------- |
| `Admin`   | Full system access, seeded on startup         |
| `Doctor`  | Medical professional operations               |
| `Patient` | Appointment booking, reviews, medical records |

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

| Method   | Route                    | Description                            | Auth |
| -------- | ------------------------ | -------------------------------------- | ---- |
| `POST`   | `doctor-register`        | Register a new doctor                  | ❌   |
| `POST`   | `patient-register`       | Register a new patient                 | ❌   |
| `POST`   | `signIn`                 | Sign in and get JWT tokens             | ❌   |
| `POST`   | `refresh-token`          | Get new access token via refresh token | ❌   |
| `GET`    | `check-token-validation` | Validate a JWT token                   | ❌   |
| `POST`   | `change-password`        | Change current user password           | ✅   |


### Authorization (`api/v1/authorization/`)

| Method   | Route    | Description          | Auth |
| -------- | -------- | ------------------------ | ---- |
| `POST`   | `add`    | Create a new role | ✅   |
| `PUT`    | `update` | Update an existing role  | ✅   |
| `DELETE` | `delete` | Delete a role         | ✅   |

### Doctors (`api/v1/doctors/`)

| Method | Route                      | Description                     | Auth |
| ------ | -------------------------- | ------------------------------- | ---- |
| `GET`  | `list`                     | List all doctors                | ❌   |
| `GET`  | `{Id:Guid}`                | Get doctor by ID                | ❌   |
| `GET`  | `{Id}/reviews`             | Get doctor's reviews            | ❌   |
| `GET`  | `{Id}/examination-details` | Get examination info            | ❌   |
| `GET`  | `{Id}/available-slots`     | Get available appointment slots | ❌   |
| `POST` | `complete-info`            | Complete doctor profile         | ✅   |
| `GET`  | `appointments/`            | List doctor's appointments      | ✅   |

### Specializations (`api/v1/specializations/`)

| Method | Route                      | Description                   | Auth |
| ------ | -------------------------- | ----------------------------- | ---- |
| `POST` | `create`                   | Create a specialization       | ✅   |
| `PUT`  | `{Id}`                     | Update a specialization       | ✅   |
| `GET`  | `list`                     | List all specializations      | ❌   |
| `GET`  | `{Id}/sub-specializations` | Get sub-specializations by ID | ❌   |

### Appointments (`api/v1/appointments/`)

| Method | Route         | Description                  | Auth |
| ------ | ------------- | ---------------------------- | ---- |
| `POST` | `/`           | Book appointment             | ✅   |
| `GET`  | `{Id}`        | Get appointment details      | ✅   |
| `POST` | `{Id}`        | Complete appointment booking | ✅   |
| `POST` | `{Id}/cancel` | Cancel appointment           | ✅   |

### Schedules (`api/v1/schedules/`)

| Method | Route            | Description                   | Auth |
| ------ | ---------------- | ---------------------------------- | ---- |
| `POST` | `/`          | Set doctor availability schedule   | ✅   |
| `PUT`  | `{Id}/lock-slot` | Lock specific appointment slot     | ✅   |

### Medical Records (`api/v1/medical-records/`)

| Method | Route       | Description        | Auth |
| ------ | ---------------------- | ------------------------ | ---- |
| `POST` | `/`     | Create medical record    | ✅   |
| `POST` | `{Id}/diagnosis`       | Create diagnosis in medical record | ✅   |
| `POST` | `{Id}/e-prescription`  | Create E-prescription in medical record | ✅   |
| `GET`  | `generate-report`| Generate medical report PDF | ✅   |

### Reviews (`api/v1/reviews/`)

| Method   | Route  | Description              | Auth |
| -------- | ------ | ------------------------ | ---- |
| `POST`   | `/`    | Create review for doctor | ✅   |
| `PUT`    | `{Id}` | Update review            | ✅   |
| `DELETE` | `{Id}` | Delete review            | ✅   |

### Clinics (`api/v1/clinics/`)

| Method | Route                 | Description                | Auth |
| ------ | --------------------- | -------------------------- | ---- |
| `GET`  | `list/`               | List all clinics           | ❌   |
| `POST` | `register-to-doctor/` | Register clinic for doctor | ✅   |
| `POST` | `/`                   | Add images for clinic      | ✅   |
| `GET`  | `{Id}/images/`        | Get images of clinic       | ❌  |

### Patients (`api/v1/patients/`)

| Method | Route           | Description                                 | Auth |
| ------ | --------------- | ------------------------------------------- | ---- |
| `GET`  | `appointments/` | List patient's appointments with pagination | ✅   |

---

## Features

### 1️⃣ **Intelligent Slot Generation System**

Automatically generates bookable appointment slots based on flexible doctor scheduling patterns.

**Features:**

- **Weekly Recurring**: Generate slots for specific days (e.g., every Monday, Wednesday, Friday)
- **One-Time Special Dates**: Create availability for special open days
- **Smart Duplicate Prevention**: O(1) deduplication using HashSet
- **Configurable Duration**: Customize slot lengths (in minutes)

### 2️⃣ **Background Job Processing with Hangfire**

Asynchronous slot generation via fire-and-forget and recurring jobs.

**Features:**

- Automatic job enqueueing on availability creation
- Hangfire Dashboard at `/Hangfire-Dashboard`
- Persistent job storage in SQL Server
- Automatic cleanup of orphaned jobs

### 3️⃣ **Doctor Availability Management Service**

Validation and slot generation triggering on availability creation.

**Validations:**

- Start time < End time
- Offline availability requires clinic
- Prevents overlapping schedules
- Auto-triggers background slot generation

### 4️⃣ **Payment Processing System**

- Payment status tracking (Pending, Paid, Failed)
- Payment event logging 
- Idempotent operations with provider metadata storage (JSON)
- Payment integration with Appointments

### 5️⃣ **Medical Records Management**

- Create medical records post-appointment
- Record diagnoses per medical record
- Generate electronic prescriptions with medication items
- Doctor-patient visit tracking (FirstVisitAt, LastVisitAt, TotalVisits)
- Automatic appointment validation
- One medical record per appointment enforced

### 6️⃣ **Patient Reviews & Ratings**

- Rate doctors 0-5 stars
- Optional anonymous reviews (max 250 chars)
- Update/delete review capability
- Automatic duplicate review prevention
- Review timestamps (CreatedAt, UpdatedAt)

### 7️⃣ **Clinic Registration for Doctors**

- Register clinic with address, region, GPS location
- Set consultation price per clinic
- Configure waiting time in minutes
- Phone number validation (Egyptian format)
- Prevent multiple clinics per doctor
- Require profile completion before clinic registration

### 8️⃣ **Clinic Image Management**

- **Batch Upload**: Upload multiple clinic images in one request
- **Azure Blob Storage**: Images stored in cloud with unique naming (GUID-based)
- **Authorization**: Only clinic owner (doctor) can upload images
- **Validation**: Input validation for file presence and uniqueness
- **Error Handling**: Graceful handling of upload failures with proper logging
- **Automatic URL Generation**: Returns blob URLs for uploaded images

### 9️⃣ **External Services Integration** 

- **Azure Blob Storage**: File upload/download with configurable containers and validation
- **Email Notifications**: Background email sending via Hangfire
- **PDF Generation**: Medical report creation with QuestPDF
- Proper placement in **Service Layer** following SOLID principles and Dependency Inversion

### **Enhanced Payment Workflow** 

- **Success Path**: Appointment confirmation with status-specific email
- **Failure Path**: Automatic slot release and failure notification
- **Refund Processing**: Stripe automatic refunds and cash refund tracking
- **Cancellation Workflow**: Complete cancellation with refund coordination

### 1️⃣1️⃣ **Comprehensive API Documentation** 

- SwaggerOperation attributes on all 10 controllers
- 50+ endpoints with meaningful summaries and descriptions
- Full Swagger/OpenAPI integration with JWT support

---

## Database Diagram (Entity Relationships)

```
ApplicationUser (IdentityUser)
 ├── 1:1 → Doctor
 │    ├── N:1 → Specialization
 │    │    └── 1:N → SubSpecialization
 │    ├── N:N → SubSpecialization (via DoctorSubSpecialSpecializations)
 │    ├── 1:1 → Clinic
 │    │         ├── N:1 → Region → City
 │    │         └── N:1 → Location (GPS)
 │    │         └── 1:N(1-5) → ClinicImage 
 │    ├── 1:N → DoctorAvailability
 │          └── 1:N → DoctorAvailabilitySlot
 │    │     └── 1:N → Appointment
 │    │         └── 1:N → Payment
 │    ├── 1:N → DoctorPatient
 │    │         └── 1:N → MedicalRecord
 │    │     ├── 1:N → Diagnosis
 │    │             └── 1:N → EPrescription
 │    │                 └── 1:N → PrescriptionItem
 │    └── 1:N → Review
 ├── 1:1 → Patient
 │    ├── 1:N → Appointment
 │    ├── 1:N → DoctorPatient
 │    ├── 1:N → Review
 │    └── 1:N → MedicalRecord
 ├── 1:N → Notification
 └── 1:N → UserToken

Payment
 ├── N:1 → Appointment
 └── 1:N → PaymentEvent 
```

All foreign key relationships use `DeleteBehavior.Restrict` to prevent cascading deletes.

---

## NuGet Dependencies

| Package                                             | Version | Purpose                       |
| --------------------------------------------------- | ------- | ----------------------------- |
| `Microsoft.EntityFrameworkCore.SqlServer`           | 9.0.10  | SQL Server provider           |
| `Microsoft.AspNetCore.Identity.EntityFrameworkCore` | 9.0.10  | Identity with EF Core         |
| `Microsoft.AspNetCore.Authentication.JwtBearer`     | 9.0.13  | JWT authentication            |
| `MediatR`                                           | 14.1.0  | CQRS mediator pattern         |
| `AutoMapper`                                        | 16.1.0  | Object-to-object mapping      |
| `FluentValidation`                                  | 12.1.1  | Request validation            |
| `FluentValidation.DependencyInjectionExtensions`    | 12.1.1  | FluentValidation DI           |
| `Swashbuckle.AspNetCore`                            | 9.0.6   | Swagger/OpenAPI documentation |
| `Swashbuckle.AspNetCore.Annotations`                | 9.0.6   | Swagger endpoint annotations  |
| `Stripe.net`                                        | Latest  | Stripe payment processing     |
| `Hangfire.Core`                                     | 1.8.13  | Background job processing     |
| `Hangfire.SqlServer`                                | 1.8.13  | SQL Server job storage        |
| `Azure.Storage.Blobs`                               | Latest  | Azure Blob Storage integration|
| `QuestPDF`                                          | Latest  | PDF generation                |

---

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server (LocalDB, Express, or full)
- Visual Studio 2022+ or VS Code
- Stripe account (for payment processing)

### Setup

1. **Clone the repository**

   ```bash
   git clone https://github.com/Norhan-Hassan/Vezeeta_Clone.git
   cd Vezeeta_Clone
   ```

2. **Configure the database connection and Stripe settings**

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
     },
     "AzureStorageSettings":{
        "DefaultContainer":"name",
        "ReportContainer":"name",
        "ConnectionString":"connection_string_"
     },
     "StripeSettings": {
       "SecretKey": "sk_test_YOUR_STRIPE_SECRET_KEY",
       "PublishableKey": "pk_test_YOUR_STRIPE_PUBLISHABLE_KEY"
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

5. **Access Swagger UI & Hangfire Dashboard**

   ```
    Swagger: https://localhost:{port}/swagger
    Hangfire: https://localhost:{port}/Hangfire-Dashboard
   ```

### Default Admin Credentials

| Field        | Value               |
| ------------ | ------------------- |
| **Email**    | `Admin@vezeeta.com` |
| **Password** | `Admin@123ADM567`   |

---

## ✅ Best Practices Implemented

- **Clean Architecture** — strict layer separation with dependency inversion
- **CQRS Pattern** — Commands and Queries are separate classes with dedicated handlers
- **Strategy Pattern** — Payment processors (Stripe, PayPal, Cash) implement swappable strategies
- **Generic Repository** — reusable data access with full transaction support
- **Centralized Routing** — all API routes defined as constants in `Router.cs`
- **Standardized Responses** — consistent `Response<T>` wrapper for all endpoints
- **Global Error Handling** — middleware catches and formats all exceptions consistently
- **Soft Delete** — global EF Core query filters on `IsDeleted` via expression trees
- **Database Seeding** — roles, admin user, and specializations with sub-specializations seeded on startup
- **Bilingual Support** — full Arabic & English localization via resource files
- **JWT Security** — access + refresh token pattern with database-backed revocation
- **Current User Abstraction** — `ICurrentUserService` for clean auth context access
- **Restrict Delete Behavior** — all foreign keys use `DeleteBehavior.Restrict`
- **Background Jobs** — Hangfire for async slot generation and email notifications
- **Slot Deduplication** — HashSet-based O(1) duplicate detection
- **UTC Timezone Consistency** — all time operations use UTC for global compatibility
- **Transaction Support** — generic repository provides transaction management (Begin, Commit, Rollback)
- **Concurrency Handling** — optimistic locking with retry logic for slot booking
- **Payment Event logging** — event logging system for payment provider 
- **Refund Processing** — Stripe refunds tracking with proper error handling 
- **Appointment Validation** — multi-level checks for slot availability, past slots, double bookings
- **Clinic Validation** — Egyptian phone format validation, Doctor profile completion checks
- **API Documentation** — SwaggerOperation attributes on all endpoints 
