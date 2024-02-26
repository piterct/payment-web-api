# Payment API REST

# Used Technologies

The application was developed using the following technologies and tools:

- Platform: .NET Core 8
- ORM (Object-Relational Mapping): Entity Framework Core
- Object-Relational Mapping: AutoMapper
- Mocking Tool: Moq and AutoFixture with AutoMoq
- Data Validation: FluentValidation
- API Documentation: Swagger


# Description

 - The API was developed using .NET Core and features the following operations: Register Sale, Fetch Sale, Update Sale, and Create Seller. The API registers sales with the initial status "Awaiting payment" and allows the inclusion of information about the seller, date, order identifier, and sold items.

# Project Execution

- To execute the project, follow these steps:

- Open Visual Studio.

- Open the project solution.

- In the Solution Explorer, right-click on the API project and select "Set as StartUp Project" to set the API project as the initial project.

- Press F5 or click "Start" to begin project execution.

- Visual Studio will open a new instance of the default browser with the API URL and Swagger documentation.

# Swagger Documentation

The Swagger documentation of the API can be accessed at http://localhost:{port}/swagger/index.html, where {port} is the port on which the IIS Express is running the project.


# Unit Testing

The application includes a comprehensive suite of unit tests to ensure that each component is tested individually and functions as expected. Unit tests are written using the XUnit testing framework for .NET Core.

