# AngularProject Web API

AngularProject is a robust and efficient Web API built with ASP.NET Core. It serves as the backend for an e-commerce application, providing a wide range of features including product and category management, as well as user authentication.

## Features

- **Product Management**: The API provides comprehensive endpoints for creating, reading, updating, and deleting products. Each product has properties such as ID, name, price, description, image URL, category ID, brand, stock, and model year.

- **Category Management**: The API also offers endpoints for managing categories. Each category has an ID and a name.

- **User Authentication**: The API includes features for user registration and login. It uses JWT (JSON Web Tokens) for authentication.

## Key Components

- **Controllers**: The `ProductController` and `CategoryController` handle HTTP requests and responses. They use the Unit of Work and Repository patterns to interact with the database.

- **Data Models**: The `Product` and `Category` classes represent the data in the application. The `ProductDTO` class is a Data Transfer Object used for sending product data over the network.

- **Database Context**: The `ApplicationDbContext` class is the Entity Framework database context. It manages the entities during run time, which includes populating objects with data from a database, change tracking, and persisting data to the database.

- **Repository and Unit of Work**: The `IGenericRepository` interface and its implementation `GenericRepository` provide a way to query and save data in a database-agnostic way. The `IUnitOfWorks` interface and its implementation `UnitOfWorks` coordinate the work of multiple repositories by creating a single database context class shared among all of them.

- **Authentication**: The `AuthManagementController` handles user registration and login. It uses the `UserManager` and `RoleManager` classes from ASP.NET Core Identity for user and role management. The `JwtConfig` class holds the secret key for signing the JWT.

- **DTOs and Responses**: The `UserRegistrationRequestDTO` and `UserLoginRequestDTO` classes represent the data for user registration and login requests. The `AuthResult`, `RegistrationRequestResponse`, and `LoginRequestResponse` classes represent the data for the responses to these requests.

## Configuration

The application's configuration is in the `appsettings.json` file. This includes the JWT secret key and the connection string for the database.

## Running the Application

To run the application, you need to have .NET Core installed. You can then use the `dotnet run` command in the root directory of the project.

## Contributing

Contributions are welcome. Please open an issue or submit a pull request.

## License

This project is licensed under the MIT License.
