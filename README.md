# Vezeeta Clone API

A comprehensive healthcare appointment booking platform API built with **ASP.NET Core** and **Clean Architecture (Onion Architecture)**. This system enables patients to book appointments with doctors, manage medical records, and streamline healthcare provider operations.

## 🏗️ Architecture Overview

The API follows a **5-layer Onion Architecture** pattern, ensuring separation of concerns, maintainability, and testability:

```
┌─────────────────────────────────────────────────┐
│              API Layer (Controllers)             │
│          - HTTP Endpoints & Routing              │
│          - Request/Response Handling             │
└─────────────────────────────────────────────────┘
                        ↓
┌─────────────────────────────────────────────────┐
│         Infrastructure Layer (Infra)            │
│   - Database Context & EF Core Configuration    │
│   - External Service Integrations                │
│   - Authentication & Authorization              │
│   - Email/Notification Services                 │
└─────────────────────────────────────────────────┘
                        ↓
┌─────────────────────────────────────────────────┐
│         Services & Logic Layer (Services)       │
│   - Business Logic Implementation               │
│   - Service Classes                             │
│   - Validation & Processing                     │
└─────────────────────────────────────────────────┘
                        ↓
┌─────────────────────────────────────────────────┐
│         Data Layer (Data Access)                │
│   - Repository Pattern Implementation           │
│   - Database Queries & Persistence              │
│   - Data Mapping & Transformation               │
└─────────────────────────────────────────────────┘
                        ↓
┌─────────────────────────────────────────────────┐
│         Core Layer (Domain Models)              │
│   - Entity Definitions                          │
│   - Base Classes & Interfaces                   │
│   - Domain Enums & Constants                    │
│   - Business Rules (as attributes)              │
└─────────────────────────────────────────────────┘
```

## 📋 Project Structure

```
VezeetaCloneAPI/
├── 1. API/                          # Presentation Layer
│   ├── Controllers/
│   ├── DTOs/
│   ├── Middleware/
│   └── appsettings.json
│
├── 2. Infrastructure/               # External Integration & Services
│   ├── DbContext/
│   ├── Repositories/
│   ├── Authentication/
│   ├── Services/
│   │   ├── EmailService/
│   │   ├── NotificationService/
│   └── Migrations/
│
├── 3. Services/                     # Business Logic Layer
│   ├── AppointmentService/
│   ├── DoctorService/
│   ├── PatientService/
│   ├── MedicalRecordService/
│   ├── NotificationService/
│   └── ReviewService/
│
├── 4. Data/                         # Data Access Layer
│   ├── Repositories/
│   │   ├── IRepository/
│   │   └── Repository/
│   ├── UnitOfWork/
│   └── Specifications/
│
└── 5. Core/                         # Domain Layer
    ├── Entities/
    │   ├── ApplicationUser
    │   ├── Appointment
    │   ├── Doctor
    │   ├── Patient
    │   ├── Clinic
    │   ├── MedicalRecord
    │   ├── Review
    │   ├── Specialization
    │   ├── Notification
    │   └── Others...
    ├── Enums/
    │   ├── Status
    │   ├── Gender
    │   ├── BloodType
    │   ├── AvailabilityMethod
    │   └── Title
    ├── Interfaces/
    └── Constants/
```

## 🎯 Core Entities

### User Management
- **ApplicationUser**: Base user class extending `IdentityUser` with profile information
- **Patient**: Patient-specific details including blood type and date of birth
- **Doctor**: Medical professional information with specialization and experience

### Appointment System
- **Appointment**: Links patients with available doctor slots
- **DoctorAvailability**: Weekly recurring or one-time special schedules
- **DoctorAvailabilitySlot**: Individual appointment time slots (auto-generated)

### Clinic & Location Management
- **Clinic**: Healthcare facility with location and contact info
- **DoctorClinic**: Links doctors to clinics with pricing
- **Location**: Geographic coordinates (latitude/longitude)
- **Region/City**: Hierarchical location structure

### Medical Records & Health Data
- **MedicalRecord**: Patient health records linked to appointments
- **Diagnosis**: Doctor-recorded diagnoses with descriptions
- **EPrescription**: Electronic prescription management

### Social & Review System
- **Review**: Doctor ratings (0-5) with patient comments
- **DoctorPatient**: Relationship tracking between doctors and patients

### Notifications & Tokens
- **Notification**: System notifications for users
- **UserToken**: JWT token management with refresh token support

## 🔑 Key Features

### Authentication & Authorization
- JWT-based authentication
- Refresh token mechanism
- Role-based access control (Patient, Doctor, Admin)
- Secure token storage and revocation

### Appointment Management
- Book appointments with available slots
- Support for recurring doctor schedules
- One-time special availability
- Appointment status tracking (Upcoming, Completed, Cancelled)
- Auto-generated time slots based on duration

### Medical Records
- Complete patient medical history
- Diagnosis tracking per appointment
- Electronic prescription management
- Medical record creation independent of appointments

### Doctor Management
- Specialization-based search
- Doctor availability management
- Clinic associations with pricing
- Experience and qualification tracking
- Patient review system with ratings

### Location & Search
- City and region management
- Clinic location with coordinates
- Geographic-based search capabilities

### Data Integrity
- Soft delete mechanism using `IsDeleted` and `IsActive` flags
- Audit trails with `CreatedAt` timestamps
- Referential integrity through foreign keys

## 🚀 Getting Started

### Prerequisites
- .NET 9.0 
- SQL Server / SQL Server Express
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd VezeetaCloneAPI
   ```

2. **Install dependencies**
   ```bash
   dotnet restore
   ```

3. **Configure database connection**
   Update `appsettings.json` in the API project:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=your_server;Database=VezeetaDb;Trusted_Connection=true;"
     }
   }
   ```

4. **Run migrations**
   ```bash
   dotnet ef database update
   ```

5. **Run the application**
   ```bash
   dotnet run
   ```

The API will be available at `` (production).

## 📡 API Endpoints

### Authentication
- `POST /api/auth/register` - User registration
- `POST /api/auth/login` - User login
- `POST /api/auth/refresh` - Refresh access token
- `POST /api/auth/logout` - User logout

### Doctors
- `GET /api/doctors` - List all doctors
- `GET /api/doctors/{id}` - Get doctor details
- `GET /api/doctors/specialization/{id}` - Get doctors by specialization
- `POST /api/doctors/{id}/availability` - Add availability
- `GET /api/doctors/{id}/availability` - Get availability

### Appointments
- `GET /api/appointments` - List user appointments
- `POST /api/appointments` - Book appointment
- `GET /api/appointments/{id}` - Get appointment details
- `PATCH /api/appointments/{id}/status` - Update appointment status
- `DELETE /api/appointments/{id}` - Cancel appointment

### Medical Records
- `GET /api/medical-records` - List patient records
- `POST /api/medical-records` - Create medical record
- `GET /api/medical-records/{id}` - Get record details
- `POST /api/medical-records/{id}/diagnosis` - Add diagnosis
- `POST /api/medical-records/{id}/prescription` - Add prescription

### Reviews
- `GET /api/reviews/doctor/{id}` - Get doctor reviews
- `POST /api/reviews` - Create review
- `PATCH /api/reviews/{id}` - Update review

### Clinics & Locations
- `GET /api/clinics` - List clinics
- `GET /api/cities` - List cities
- `GET /api/regions/{cityId}` - Get regions by city

## 🔐 Authentication Flow

```
User Registration/Login
         ↓
Validate Credentials
         ↓
Generate JWT Token + Refresh Token
         ↓
Store Refresh Token in Database (UserToken)
         ↓
Return Access Token + Refresh Token
         ↓
Client stores tokens (Access: Memory, Refresh: Secure Cookie)
         ↓
On Token Expiry: Use Refresh Token to get new Access Token
         ↓
Revoke old tokens on logout
```

## 💾 Database Design Highlights

### Soft Delete Strategy
Instead of permanently deleting records, we use:
- `BaseEntity.IsDeleted` flag
- `ApplicationUser.IsActive` flag
- All queries filter out soft-deleted records

### Relationships
- **One-to-Many**: City → Regions, Doctor → Appointments
- **Many-to-Many**: Doctor ↔ Patient (via DoctorPatient), Doctor ↔ Clinic (via DoctorClinic)
- **One-to-One**: Patient ↔ ApplicationUser, Doctor ↔ ApplicationUser

### Appointment Slot Generation
Doctor availability is defined once with duration, and the system auto-generates individual slots:
- **DoctorAvailability**: Defines 10 AM - 5 PM, 30-min slots
- **DoctorAvailabilitySlot**: Auto-generates [10:00-10:30], [10:30-11:00], etc.

## 🛠️ Technology Stack

| Layer | Technology |
|-------|-----------|
| **API** | ASP.NET Core 6+ |
| **Database** | SQL Server, Entity Framework Core |
| **Authentication** | ASP.NET Identity, JWT |
| **Validation** | Data Annotations, FluentValidation |
| **Mapping** | AutoMapper |
| **Logging** | Serilog |
| **Testing** | xUnit, Moq |

## 📦 Dependencies

Key NuGet packages:
- `Microsoft.EntityFrameworkCore`
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore`
- `System.IdentityModel.Tokens.Jwt`
- `AutoMapper`
- `FluentValidation`
- `Serilog`

## 🧪 Testing Strategy

Each layer should have corresponding unit tests:

```
Tests/
├── API.Tests/
├── Services.Tests/
├── Data.Tests/
└── Core.Tests/
```

Use xUnit for the testing framework and Moq for mocking dependencies.

## 🔄 Development Workflow

1. **Create/Modify Entity** in Core layer
2. **Create Migration** in Infrastructure layer
3. **Implement Repository** in Data layer
4. **Create Service** in Services layer
5. **Create DTOs & Controller** in API layer
6. **Write Unit Tests** alongside development

## 🚦 Status Tracking

Appointments support multiple statuses:
- `Upcoming` - Scheduled appointment
- `Completed` - Appointment finished
- `Cancelled` - Appointment cancelled by patient/doctor
- `NoShow` - Patient didn't attend
- `Rescheduled` - Appointment rescheduled

## 📝 Best Practices Implemented

✅ **Separation of Concerns** - Each layer has specific responsibilities  
✅ **DRY Principle** - Reusable components and services  
✅ **SOLID Principles** - Dependency injection, interface-based design  
✅ **Error Handling** - Centralized exception handling middleware  
✅ **Validation** - Input validation at multiple levels  
✅ **Security** - JWT authentication, password hashing, data protection  
✅ **Performance** - Async/await, efficient queries  
✅ **Maintainability** - Clear naming conventions, documentation  

## 🐛 Error Handling

Global exception handling middleware catches and logs:
- `NotFoundException` - Resource not found (404)
- `UnauthorizedException` - Authentication failures (401)
- `ForbiddenException` - Authorization failures (403)
- `BadRequestException` - Validation errors (400)
- `ConflictException` - Business logic conflicts (409)
- `InternalServerException` - Unexpected errors (500)

## 📚 API Documentation

Swagger/OpenAPI documentation is auto-generated at:
```
https://localhost:7000/swagger/ui
```

All endpoints are documented with:
- Request/response schemas
- Required/optional parameters
- Authorization requirements
- Example values

## 🔗 Contributing

1. Create a feature branch
2. Follow the architecture guidelines
3. Write unit tests for new features
4. Ensure all tests pass
5. Submit a pull request

## 👥 Support

For issues, questions, or suggestions, please create an issue in the repository.

---

**Built with ❤️ using Clean Architecture principles**
