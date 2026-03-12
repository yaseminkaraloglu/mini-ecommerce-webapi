# mini-ecommerce-webapi
This project is a simple backend e-commerce system developed using ASP.NET Core Web API.

## Technologies

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- RESTful API
- Layered Architecture

## Features

- Category management
- Product management
- Order creation
- Stock control
- Soft delete
- Transaction management

## Architecture

The project follows a layered architecture:

Controller → Service → Data → Database

DTOs are used for data transfer between API and client.

## Endpoints

### Categories
- GET /api/categories
- POST /api/categories
- PUT /api/categories/{id}
- DELETE /api/categories/{id}

### Products
- GET /api/products
- POST /api/products
- PUT /api/products/{id}
- DELETE /api/products/{id}

### Orders
- POST /api/orders
- GET /api/orders/{id}
