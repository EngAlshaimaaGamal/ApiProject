# CurlyQueens E-Commerce API

A complete e-commerce API built with ASP.NET Core 9.0, featuring products, categories, shopping carts, and order management.

## Features

- **User Authentication**: JWT-based authentication with Identity framework
- **Product Management**: CRUD operations for products and categories
- **Shopping Cart**: Add, update, and remove items from cart
- **Order Management**: Create orders from cart, view order history
- **Stock Management**: Automatic stock deduction on order placement

## Technology Stack

- ASP.NET Core 9.0 Web API
- Entity Framework Core 9.0
- ASP.NET Core Identity
- JWT Authentication
- SQL Server (LocalDB)

## Prerequisites

- .NET 9 SDK
- SQL Server or SQL Server Express LocalDB
- Visual Studio 2022 or VS Code

## Getting Started

### 1. Update Database Connection String

Edit `appsettings.json` and update the connection string if needed:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CurlyQueensDB;Trusted_Connection=true;MultipleActiveResultSets=true"
}
```

### 2. Create Database Migration

Open Package Manager Console or Terminal and run:

```bash
dotnet ef migrations add InitialCreate
```

### 3. Update Database

```bash
dotnet ef database update
```

### 4. Run the Application

```bash
dotnet run
```

The API will be available at `https://localhost:5001` or `http://localhost:5000`

## API Endpoints

### Authentication

#### Register a New User
```http
POST /api/auth/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "Password123",
  "confirmPassword": "Password123",
  "firstName": "John",
  "lastName": "Doe"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "email": "user@example.com",
  "userId": "user-id-guid",
  "expiresAt": "2024-01-01T12:00:00Z"
}
```

#### Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "Password123"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "email": "user@example.com",
  "userId": "user-id-guid",
  "expiresAt": "2024-01-01T12:00:00Z"
}
```

### Categories

#### Get All Categories
```http
GET /api/categories
```

#### Get Category by ID
```http
GET /api/categories/{id}
```

#### Create Category
```http
POST /api/categories
Content-Type: application/json

{
  "name": "Hair Products",
  "description": "Professional hair care products"
}
```

#### Update Category
```http
PUT /api/categories/{id}
Content-Type: application/json

{
  "id": 1,
  "name": "Hair Products",
  "description": "Updated description"
}
```

#### Delete Category
```http
DELETE /api/categories/{id}
```

### Products

#### Get All Products
```http
GET /api/products
GET /api/products?categoryId=1
```

#### Get Product by ID
```http
GET /api/products/{id}
```

#### Create Product
```http
POST /api/products
Content-Type: application/json

{
  "name": "Curly Hair Shampoo",
  "description": "Natural ingredients for curly hair",
  "price": 29.99,
  "stockQuantity": 100,
  "imageUrl": "https://example.com/image.jpg",
  "categoryId": 1
}
```

#### Update Product
```http
PUT /api/products/{id}
Content-Type: application/json

{
  "name": "Curly Hair Shampoo",
  "description": "Updated description",
  "price": 29.99,
  "stockQuantity": 150,
  "imageUrl": "https://example.com/image.jpg",
  "categoryId": 1
}
```

#### Delete Product
```http
DELETE /api/products/{id}
```

### Shopping Cart (Requires Authentication)

**Note**: All cart endpoints require the `Authorization: Bearer {token}` header.

#### Get Current User's Cart
```http
GET /api/carts
Authorization: Bearer {token}
```

#### Add Item to Cart
```http
POST /api/carts/items
Authorization: Bearer {token}
Content-Type: application/json

{
  "productId": 1,
  "quantity": 2
}
```

#### Update Cart Item Quantity
```http
PUT /api/carts/items/{productId}
Authorization: Bearer {token}
Content-Type: application/json

{
  "quantity": 3
}
```

#### Remove Item from Cart
```http
DELETE /api/carts/items/{productId}
Authorization: Bearer {token}
```

#### Clear Cart
```http
DELETE /api/carts
Authorization: Bearer {token}
```

### Orders (Requires Authentication)

**Note**: All order endpoints require the `Authorization: Bearer {token}` header.

#### Create Order from Cart
```http
POST /api/orders
Authorization: Bearer {token}
Content-Type: application/json

{
  "shippingAddress": "123 Main St, City, State 12345"
}
```

**What happens:**
1. Retrieves the user's cart
2. Verifies product availability (checks stock)
3. Creates an Order and OrderItem records
4. Deducts quantity from Product.StockQuantity
5. Clears the user's cart
6. Returns the created order details

#### Get All Orders for Current User
```http
GET /api/orders
Authorization: Bearer {token}
```

#### Get Order by ID
```http
GET /api/orders/{orderId}
Authorization: Bearer {token}
```

#### Update Order Status
```http
PUT /api/orders/{orderId}/status
Authorization: Bearer {token}
Content-Type: application/json

"Shipped"
```

Valid statuses: `Pending`, `Paid`, `Processing`, `Shipped`, `Delivered`, `Cancelled`

## Database Models

### Category
- `Id` (int, PK)
- `Name` (string, required)
- `Description` (string, optional)

### Product
- `Id` (int, PK)
- `Name` (string, required)
- `Description` (string, optional)
- `Price` (decimal)
- `StockQuantity` (int)
- `ImageUrl` (string, optional)
- `CategoryId` (int, FK)

### Cart
- `Id` (int, PK)
- `UserId` (string, FK to Identity User)

### CartItem
- `Id` (int, PK)
- `Quantity` (int)
- `ProductId` (int, FK)
- `CartId` (int, FK)

### Order
- `Id` (int, PK)
- `UserId` (string, FK to Identity User)
- `OrderDate` (DateTime)
- `TotalAmount` (decimal)
- `Status` (string)
- `ShippingAddress` (string)

### OrderItem
- `Id` (int, PK)
- `Quantity` (int)
- `Price` (decimal - price at time of order)
- `ProductId` (int, FK)
- `OrderId` (int, FK)

## Security

- JWT tokens expire after 60 minutes (configurable in `appsettings.json`)
- Passwords must be at least 6 characters with uppercase, lowercase, and digits
- All cart and order endpoints require authentication
- Users can only access their own carts and orders

## Error Handling

The API returns appropriate HTTP status codes:

- `200 OK` - Successful GET request
- `201 Created` - Successful POST request
- `204 No Content` - Successful PUT/DELETE request
- `400 Bad Request` - Invalid input or business logic error
- `401 Unauthorized` - Missing or invalid authentication
- `403 Forbidden` - User doesn't have permission
- `404 Not Found` - Resource not found

Error responses include a message:
```json
{
  "message": "Error description here"
}
```

## Next Steps

1. Add Swagger/OpenAPI documentation
2. Implement payment integration
3. Add email notifications
4. Implement admin roles and permissions
5. Add product reviews and ratings
6. Implement product search and filtering
7. Add image upload functionality

## License

This project is for educational purposes.
