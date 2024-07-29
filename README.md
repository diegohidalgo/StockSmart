# StockSmart

## Overview

Welcome to **StockSmart!** This document provides an overview of the architecture behind the StockSmart application. Designed with modern software engineering principles, StockSmart utilizes Clean Architecture, CQRS, Mediator and several other design patterns and technologies to ensure scalability, maintainability, and performance.

## Architecture Summary

The StockSmart application is built on a robust architectural foundation featuring:

1. **Clean Architecture**
2. **Rich Domain Models**
3. **CQRS (Command Query Responsibility Segregation)**
4. **MediatR**
5. **Repository and Unit of Work Patterns**
6. **Decorator Pattern**
7. **Entity Framework**
8. **Autofac**
9. **Distributed Cache (Redis)**
10. **Swagger**
11. **API Versioning**
12. **Serilog**
13. **Docker Compose**
14. **Github Actions**
15. **Audit Trails**

## Detailed Architecture

### 1. Clean Architecture

Clean Architecture organizes the application into distinct layers to ensure separation of concerns and independence. The key layers are:

- **Domain**: This is the heart of StockSmart, containing the business logic and domain models. It is designed to be independent of other layers, focusing purely on business rules.
- **Application**: Manages use cases and orchestrates business logic through commands and queries. It employs MediatR to dispatch these operations.
- **Infrastructure**: Implements data access using the Repository and Unit of Work patterns with Entity Framework. It also manages external integrations and distributed caching using decorator pattern.
- **Presentation**: Contains API controllers and presentation logic.
- **Web**: It handles dependency injection with Autofac and provides API documentation through Swagger.

### 2. Rich Domain Models

StockSmart's domain models are designed to encapsulate business logic and rules comprehensively. They ensure that business rules are consistently enforced and are central to the application’s functionality.

### 3. CQRS (Command Query Responsibility Segregation)

CQRS in StockSmart separates the data modification (commands) from data retrieval (queries). This separation allows for optimized performance and clear responsibility boundaries.

- **Commands**: Manage data changes and execute business logic.
- **Queries**: Focus on data retrieval and are designed to be read-only operations.

### 4. MediatR

MediatR facilitates the implementation of CQRS by handling in-process messaging. It decouples request senders from request handlers, which simplifies the codebase and improves maintainability.

### 5. Repository and Unit of Work Patterns

- **Repository Pattern**: Provides an abstraction layer for data access, simplifying interactions with the database and enabling easier testing.
- **Unit of Work Pattern**: Manages transactions and ensures that multiple data operations are committed or rolled back as a single unit, ensuring data consistency.

### 6. Decorator Pattern

The Decorator Pattern is used to dynamically enhance the functionality of services or handlers. This allows for flexible additions like logging, validation, or caching without altering the core functionality. Cached repository decorator helps to handle the caching when composing entities.

### 7. Entity Framework

Entity Framework (EF) is an ORM framework that abstracts database interactions by mapping relational tables to .NET objects. At its core, DbContext manages database connections and tracks entity changes, while DbSet represents collections of these entities. Entity Classes define the structure of the data and map to tables, and Migrations facilitate schema changes over time. EF simplifies data access with LINQ queries and automatic change tracking, streamlining CRUD operations.

### 8. Autofac

Autofac is used for dependency injection, providing a flexible and powerful way to manage service lifetimes and dependencies throughout the application.

### 9. Distributed Cache (Redis)

Redis is used as the distributed cache in StockSmart to enhance performance and scalability. Redis caches frequently accessed data across multiple servers, reducing the load on the database and speeding up response times.

### 10. Swagger

Swagger provides interactive API documentation, making it easy for developers to explore and test API endpoints. It enhances usability and helps in understanding the API’s capabilities.

### 11. API Versioning

API versioning allows StockSmart to support multiple versions of the API, ensuring backward compatibility and smooth transitions for clients as the API evolves.

### 12. Serilog

Serilog is used for structured logging, providing rich and detailed logs that are easy to search and analyze. This helps in monitoring application performance and diagnosing issues effectively.

### 13. Docker Compose

Docker Compose is employed to manage and orchestrate multi-container Docker applications. It simplifies the setup of development and testing environments by defining services and their dependencies in a single file.

### 14. Github Actions

GitHub Actions is a CI/CD platform that automates workflows directly within GitHub repositories. It allows you to define custom workflows using YAML files, which specify a series of steps and actions to automate tasks such as building, testing, and deploying code. Actions are configured for the Pull Request on each push to confirm the dependencies and run automated tests.

### 15. Audit Trails (Audit.EntityFramework)

Audit.EntityFramework.Core provides seamless auditing for Entity Framework Core by automatically logging database changes such as inserts, updates, and deletes. It offers flexible storage options, including file-based and database logging, and allows for easy configuration of log paths and settings. StockSmart is logging the audit trail to a file.

## Getting Started

### Prerequisites

- Docker and Docker Compose

### Installation

1. Clone the StockSmart repository:
    ```bash
    git clone https://github.com/diegohidalgo/StockSmart.git
    ```

2. Navigate to the project directory and run:
    ```bash
    cd .build
    ```

3. Run the initialization script:
    ```bash
    .\build_and_run.ps1
    ```

4. Access the API documentation via Swagger at:
    ```
    https://localhost:5001/swagger
    ```

### Visual Studio

### Prerequisites

- .NET Core 3.1
- .NET 6
- Visual Studio
- Docker for windows (optional)

### Installation

1. Clone the StockSmart repository:
    ```bash
    git clone https://github.com/diegohidalgo/StockSmart.git
    ```

2. Open the solution and run the migrations in package manager console:
    ```bash
    update-database
    ```

3. Setup the docker-compose as startup project and run:
    ```
    https://localhost:5001/swagger
    ```

## Contribution

We welcome contributions to StockSmart! Please refer to the [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines on how to contribute.

## License

StockSmart is licensed under the [MIT License](LICENSE).
