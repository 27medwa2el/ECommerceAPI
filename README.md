# E-Commerce API

A RESTful API built with .NET Core for managing e-commerce operations including customers, products, and orders.

## Features

- **Customer Management**: Create and retrieve customer information
- **Order Management**: Create orders, retrieve order details, and update order status
- **Product Catalog**: Pre-seeded product data with stock management
- **Data Validation**: FluentValidation for robust input validation
- **Repository Pattern**: Clean architecture with separation of concerns
- **Database Integration**: Entity Framework Core with SQL Server
- **Unit Testing**: Comprehensive test coverage with xUnit

## Project Structure

```
ECommerceAPI/
├── Controllers/          # API Controllers
├── Models/              # Entity models
├── DTOs/                # Data Transfer Objects
├── Data/                # DbContext and seeding
├── Repositories/        # Repository pattern implementation
├── Validators/          # FluentValidation validators
└── SampleRequests.json  # API testing examples

ECommerceAPI.Tests/      # Unit test project
```

## Technology Stack

- **.NET 9.0** - Framework
- **Entity Framework Core** - ORM
- **SQL Server** - Database
- **FluentValidation** - Input validation
- **Swagger** - API documentation
- **xUnit** - Unit testing

## Getting Started

### Prerequisites

- .NET 9.0 SDK
- SQL Server LocalDB (or SQL Server)

### Setup and Installation

1. **Navigate to the project directory**
   ```bash
   cd ECommerceAPI
   ```

2. **Restore dependencies for the entire solution**
   ```bash
   dotnet restore
   ```

3. **Update database connection string** (if needed)
   - Open `ECommerceAPI/appsettings.json`
   - Modify the `DefaultConnection` string if using a different SQL Server instance

4. **Build the solution**
   ```bash
   dotnet build
   ```

5. **Run the application**
   ```bash
   dotnet run --project ECommerceAPI
   ```

6. **Access the API**
   - API Base URL: `https://localhost:7xxx` (port may vary)
   - Swagger UI: `https://localhost:7xxx/swagger`

### Database

The application uses Entity Framework Code First approach. The database will be automatically created and seeded with sample product data when the application starts.

#### Seeded Products
- Laptop Dell XPS 13 - $1299.99
- iPhone 14 Pro - $999.99
- Samsung 4K Monitor - $349.99
- Wireless Bluetooth Headphones - $199.99
- Gaming Mechanical Keyboard - $89.99
- Ergonomic Office Chair - $299.99
- Portable SSD 1TB - $149.99
- Webcam HD 1080p - $79.99

## API Endpoints

### Customer Management

#### GET /api/customers
Retrieve all customers.

**Response:**
```json
[
  {
    "id": 1,
    "name": "John Doe",
    "email": "john@example.com",
    "phone": "+1234567890"
  }
]
```

#### POST /api/customers
Create a new customer.

**Request Body:**
```json
{
  "name": "John Doe",
  "email": "john@example.com",
  "phone": "+1234567890"
}
```

**Validation Rules:**
- Name: Required, max 100 characters
- Email: Required, valid email format, unique, max 150 characters
- Phone: Optional, max 20 characters

#### GET /api/customers/{id}
Retrieve a specific customer by ID.

### Order Management

#### POST /api/orders
Create a new order.

**Request Body:**
```json
{
  "customerId": 1,
  "products": [
    {
      "productId": 1,
      "quantity": 2
    },
    {
      "productId": 3,
      "quantity": 1
    }
  ]
}
```

**Validation Rules:**
- Customer ID: Required, must exist
- Products: At least one product required
- Product ID: Must exist in database
- Quantity: Must be greater than 0
- Stock validation: Sufficient stock must be available

#### GET /api/orders/{id}
Retrieve order details.

**Response:**
```json
{
  "id": 1,
  "customerName": "John Doe",
  "status": "Pending",
  "productCount": 2,
  "orderDate": "2024-01-15T10:30:00Z",
  "totalPrice": 2949.97
}
```

#### POST /api/orders/UpdateOrderStatus/{id}
Update order status.

**Request Body:**
```json
{
  "status": "Delivered"
}
```

**Valid Status Values:**
- "Pending"
- "Delivered"

**Note:** When an order is marked as "Delivered", product stock is automatically reduced.

## Error Handling

The API returns appropriate HTTP status codes:

- **200 OK** - Successful requests
- **201 Created** - Successful resource creation
- **400 Bad Request** - Validation errors or invalid data
- **404 Not Found** - Resource not found
- **500 Internal Server Error** - Server errors

Error responses include descriptive messages:

```json
{
  "message": "Validation failed",
  "errors": [
    "Customer name is required",
    "Invalid email format"
  ]
}
```

## Testing

### Running Unit Tests

```bash
dotnet test
```

This will run all tests in the solution, including both repository and controller tests.

### API Testing

Sample requests are provided in `ECommerceAPI/SampleRequests.json`. You can use tools like:
- Swagger UI (built-in)
- Postman
- curl
- REST Client extensions for VS Code

### Test Data

The application automatically seeds the database with 8 sample products. Use these product IDs (1-8) when creating test orders.

## Architecture Highlights

### Repository Pattern
Clean separation between data access and business logic, making the application testable and maintainable.

### Dependency Injection
All services are registered in the DI container for loose coupling and testability.

### Validation
FluentValidation provides robust, testable validation rules separate from the models.

### Entity Relationships
- Customer → Orders (One-to-Many)
- Order → Products (Many-to-Many through OrderProduct junction table)

### Error Handling
Comprehensive exception handling with meaningful error messages and appropriate HTTP status codes.

## Development Notes

- Database schema is created automatically via Entity Framework migrations
- Product stock is automatically managed when orders are delivered
- Email uniqueness is enforced at database level
- All monetary values use decimal precision for accuracy
- UTC timestamps are used for order dates

## Future Enhancements

- Authentication and authorization
- Product CRUD operations
- Order cancellation
- Inventory management
- Payment processing integration
- Email notifications
- Advanced search and filtering
- Audit logging
