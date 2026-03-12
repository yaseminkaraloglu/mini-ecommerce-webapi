# mini-ecommerce-webapi

This project is a simple backend e-commerce system developed using ASP.NET Core Web API.  
It provides basic functionalities such as category management, product management, and order creation.  

The main goal of this project is to practice RESTful API development, layered architecture, and Entity Framework Core.

---

## 🚀 Technologies Used

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- RESTful API
- Layered Architecture
- DTO Pattern
- Transaction Management

---

## 📦 Project Structure

The project follows a layered architecture:

- Controller → Service → Data → Database

**Controllers**  
- Handle HTTP requests and responses.

**Services**  
- Contain the business logic of the application.

**Data**  
- Includes the DbContext used to communicate with the database.

**Models**  
- Represent database entities.

**DTOs**  
- Used to transfer data between the API and the client.

---

## ✨ Features

- Category CRUD operations
- Product CRUD operations
- Order creation
- Stock control
- Soft delete
- Transaction management for order creation
- Validation checks

---

## 📚 API Endpoints

**Categories**

- GET /api/categories
- GET /api/categories/{id}
- POST /api/categories
- PUT /api/categories/{id}
- DELETE /api/categories/{id}

**Products**

- GET /api/products
- GET /api/products/{id}
- POST /api/products
- PUT /api/products/{id}
- DELETE /api/products/{id}

**Orders**

- POST /api/orders
- GET /api/orders/{id}

---

## 🧪 Example Order Request

{
  "userId": 1,
  "items": [
    {
      "productId": 1,
      "quantity": 2
    }
  ]
}

## 🧪 When an Order is Created

When an order is created:

- Product stock is checked
- Stock is decreased
- Total order price is calculated
- Order items are created
- Transaction ensures data consistency

---

## 🎯 Purpose of the Project

This project was developed to improve my backend development skills and to better understand:

- REST API design
- Layered architecture
- Entity Framework Core
- Transaction management
- DTO usage
