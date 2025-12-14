# Book Review API

A comprehensive RESTful API built with .NET 9.0 for managing books, authors, genres, reviews, and reviewers. This project demonstrates a clean architecture with repository patterns, dependency injection, AutoMapper, and Entity Framework Core.

## Table of Contents

- [Project Overview](#project-overview)
- [Technologies & Dependencies](#technologies--dependencies)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [API Endpoints](#api-endpoints)
- [Database Setup](#database-setup)
- [Running the Project](#running-the-project)
- [Key Features](#key-features)

## Project Overview

This API provides a complete backend solution for a book review platform where users can:
- Browse books and authors
- View and manage book genres
- Read and write reviews
- Track reviewer information

The project follows best practices including:
- Repository pattern for data access
- Dependency injection for loose coupling
- DTOs (Data Transfer Objects) for API contracts
- AutoMapper for object mapping
- Entity Framework Core for data persistence
- Swagger documentation for API exploration

## Technologies & Dependencies

- **.NET Framework**: 9.0
- **Database**: SQL Server
- **ORM**: Entity Framework Core 9.0.10
- **API Documentation**: Swagger/Swashbuckle 9.0.6
- **Mapping**: AutoMapper 12.0.1
- **Architecture**: Repository Pattern, Dependency Injection

### NuGet Packages
```xml
- AutoMapper 12.0.1
- AutoMapper.Extensions.Microsoft.DependencyInjection 12.0.1
- Microsoft.EntityFrameworkCore.SqlServer 9.0.10
- Microsoft.EntityFrameworkCore.Design 9.0.10
- Swashbuckle.AspNetCore 9.0.6
```

## Project Structure

```
├── Controllers/              # API endpoints
│   ├── AuthorController.cs
│   ├── BookController.cs
│   ├── GenreController.cs
│   ├── ReviewController.cs
│   └── ReviewerController.cs
├── Data/                     # Database context
│   └── DataContext.cs
├── DTO's/                    # Data Transfer Objects
│   ├── AuthorDTO.cs
│   ├── BookDTO.cs
│   ├── BookPatchDTO.cs
│   ├── GenreDTO.cs
│   ├── ReviewDTO.cs
│   ├── ReviewerDTO.cs
│   ├── ReviewerUpdateDTO.cs
│   └── ReviewPatchDTO.cs
├── Models/                   # Database models
│   ├── Author.cs
│   ├── Book.cs
│   ├── BookAuthor.cs (Junction table)
│   ├── BookGenre.cs (Junction table)
│   ├── Genre.cs
│   ├── Review.cs
│   └── Reviewer.cs
├── Interfaces/               # Repository interfaces
│   ├── IAuthorRepository.cs
│   ├── IBookRepository.cs
│   ├── IGenreRepository.cs
│   ├── IReviewerRepository.cs
│   └── IReviewRepository.cs
├── Repository/               # Repository implementations
│   ├── AuthorRepository.cs
│   ├── BookRepository.cs
│   ├── GenreRepository.cs
│   ├── ReviewerRepository.cs
│   └── ReviewRepository.cs
├── Helper/                   # Utility classes
│   └── MappingProfiles.cs
├── Migrations/               # Entity Framework migrations
├── Properties/               # Project properties
├── Program.cs                # Application startup
├── Seed.cs                   # Database seeding
├── appsettings.json          # Configuration
├── appsettings.Development.json
└── dotnet.http               # HTTP requests for testing
```

## Getting Started

### Prerequisites

- .NET 9.0 SDK or later
- SQL Server (local or remote)
- Visual Studio Code, Visual Studio, or Rider

### Installation

1. **Clone the repository** (if using git):
   ```bash
   git clone <repository-url>
   cd dotnet
   ```

2. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

3. **Configure the database connection** in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=<your-server>;Database=BookReviewDb;Trusted_Connection=true;"
     }
   }
   ```

4. **Apply migrations**:
   ```bash
   dotnet ef database update
   ```

5. **Seed the database** (optional):
   ```bash
   dotnet run seeddata
   ```

## API Endpoints

### Authors
- `GET /api/authors` - Get all authors
- `GET /api/authors/{id}` - Get author by ID
- `POST /api/authors` - Create new author
- `PUT /api/authors/{id}` - Update author
- `DELETE /api/authors/{id}` - Delete author

### Books
- `GET /api/books` - Get all books
- `GET /api/books/{id}` - Get book by ID
- `POST /api/books` - Create new book
- `PUT /api/books/{id}` - Update book
- `PATCH /api/books/{id}` - Partial update of book
- `DELETE /api/books/{id}` - Delete book

### Genres
- `GET /api/genres` - Get all genres
- `GET /api/genres/{id}` - Get genre by ID
- `POST /api/genres` - Create new genre
- `PUT /api/genres/{id}` - Update genre
- `DELETE /api/genres/{id}` - Delete genre

### Reviews
- `GET /api/reviews` - Get all reviews
- `GET /api/reviews/{id}` - Get review by ID
- `POST /api/reviews` - Create new review
- `PUT /api/reviews/{id}` - Update review
- `PATCH /api/reviews/{id}` - Partial update of review
- `DELETE /api/reviews/{id}` - Delete review

### Reviewers
- `GET /api/reviewers` - Get all reviewers
- `GET /api/reviewers/{id}` - Get reviewer by ID
- `POST /api/reviewers` - Create new reviewer
- `PUT /api/reviewers/{id}` - Update reviewer
- `DELETE /api/reviewers/{id}` - Delete reviewer

## Database Setup

The project uses SQL Server with Entity Framework Core for data persistence. The database includes the following main entities:

- **Authors**: Authors of books
- **Books**: Books with many-to-many relationships to Authors and Genres
- **Genres**: Book categories/genres
- **Reviews**: User reviews of books
- **Reviewers**: Users who write reviews

### Database Migrations

Migrations are stored in the `Migrations/` folder. To add a new migration:

```bash
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

## Running the Project

### Development Mode

```bash
dotnet watch run
```

This will start the API with live reload enabled.

### Production Mode

```bash
dotnet run
```

### Access the API

Once running, access:
- **API Base URL**: `https://localhost:5001` or `http://localhost:5000`
- **Swagger UI**: `https://localhost:5001/swagger` (Development mode only)

## Key Features

- ✅ **RESTful API Design** - Follows REST conventions
- ✅ **Repository Pattern** - Abstracted data access layer
- ✅ **Dependency Injection** - Loosely coupled components
- ✅ **AutoMapper Integration** - Automatic DTO mapping
- ✅ **Entity Framework Core** - ORM for database operations
- ✅ **SQL Server Database** - Reliable data persistence
- ✅ **Swagger Documentation** - Interactive API documentation
- ✅ **Data Seeding** - Populate database with initial data
- ✅ **CRUD Operations** - Full Create, Read, Update, Delete support
- ✅ **PATCH Support** - Partial resource updates
- ✅ **Error Handling** - Consistent error responses

## Configuration

### appsettings.json

Key configuration options:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=...;Trusted_Connection=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

## Development Notes

- AutoMapper profiles are configured in `Helper/MappingProfiles.cs`
- DTOs define API contracts and validation
- Repositories provide consistent data access patterns
- Controllers handle HTTP requests and responses
- Models represent database entities

## Future Enhancements

- [ ] User authentication and authorization
- [ ] Advanced filtering and search capabilities
- [ ] Pagination for large datasets
- [ ] API versioning
- [ ] Comprehensive unit testing
- [ ] Rate limiting
- [ ] Caching strategies

---

**Created**: 2025  
**Framework**: .NET 9.0  
**Database**: SQL Server
