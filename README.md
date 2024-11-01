
# StandingsAPI

StandingsAPI is a .NET Core API designed to manage and track standings in an educational context. 
It provides services for managing exams, students, results, roles, and user authentication, with 
a structured layer approach for better maintainability and scalability.

## Project Structure

- **Standings.Application**: Contains application logic, including DTOs, interfaces, services, and validation.
- **Standings.Domain**: Holds the domain entities and database context configurations.
- **Standings.Infrastructure**: Implements repositories and services for data persistence.
- **Standings.API**: The presentation layer which exposes endpoints via controllers.

## Features

- **Authentication and Authorization**: User registration, login, and role-based authorization.
- **Student Management**: Create, update, retrieve, and delete student records.
- **Exam Management**: Create, update, retrieve, and delete exams.
- **Result Tracking**: Store and retrieve exam results for students.
- **Role and User Management**: Handle user roles and permissions.

## Prerequisites

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- A database setup compatible with Entity Framework Core (e.g., SQL Server, PostgreSQL).

## Getting Started

1. **Clone the Repository**
   ```bash
   git clone https://github.com/justshfarida/StandingsAPI.git
   cd StandingsAPI
   ```

2. **Configure Database**  
   Update the connection string in `appsettings.json` in `Standings.API` to point to your database.

3. **Run Migrations**  
   Run the following command to apply the initial database schema:
   ```bash
   dotnet ef database update --project Infrastructure/Standings.Persistence
   ```

4. **Build and Run the Application**  
   In the main project directory:
   ```bash
   dotnet build
   dotnet run --project Presentation/Standings.API
   ```

The API should now be running locally at `https://localhost:5001`.

## API Endpoints

Below are some primary endpoints available in the API:

- **Authentication**
  - `POST /api/auth/login`: User login.
  - `POST /api/auth/register`: Register a new user.

- **Student**
  - `GET /api/student/{id}`: Get a student by ID.
  - `POST /api/student`: Create a new student.

- **Exam**
  - `GET /api/exam/{id}`: Get an exam by ID.
  - `POST /api/exam`: Create a new exam.

- **Result**
  - `GET /api/result/{id}`: Get result by ID.
  - `POST /api/result`: Record a new result.

- **Roles and Users**
  - `GET /api/user/{id}`: Get a user by ID.
  - `POST /api/role`: Create a new role.

## Technologies

- **.NET 6.0** - Core framework for building the API.
- **Entity Framework Core** - ORM for database access.
- **Automapper** - For object-to-object mapping.
- **JWT** - For secure token-based authentication.

## Contributing

1. Fork the repository.
2. Create a feature branch.
3. Commit your changes.
4. Push to the branch.
5. Open a pull request.

## License

This project is licensed under the MIT License.

---

**Note**: For detailed API documentation, refer to the Swagger documentation available at `https://localhost:5001/swagger` when the application is running.
