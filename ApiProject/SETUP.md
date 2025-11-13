# CurlyQueens API - Setup Instructions

This guide will help you set up and run the CurlyQueens E-Commerce API on your local machine.

## Prerequisites

Before you begin, ensure you have the following installed:

1. **.NET 9 SDK** - [Download here](https://dotnet.microsoft.com/download/dotnet/9.0)
2. **SQL Server** or **SQL Server Express LocalDB** (included with Visual Studio)
3. **Visual Studio 2022** (recommended) or **VS Code** with C# extension

To verify .NET installation:
```bash
dotnet --version
```

## Step-by-Step Setup

### Step 1: Clone or Download the Project

If you haven't already, get the project files to your local machine.

### Step 2: Restore NuGet Packages

Open a terminal/command prompt in the project directory and run:

```bash
dotnet restore
```

### Step 3: Update Connection String (if needed)

Open `appsettings.json` and review the connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CurlyQueensDB;Trusted_Connection=true;MultipleActiveResultSets=true"
}
```

**For SQL Server Express:**
```json
"DefaultConnection": "Server=.\\SQLEXPRESS;Database=CurlyQueensDB;Trusted_Connection=true;MultipleActiveResultSets=true"
```

**For SQL Server with credentials:**
```json
"DefaultConnection": "Server=YOUR_SERVER;Database=CurlyQueensDB;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;TrustServerCertificate=true"
```

### Step 4: Install Entity Framework Tools (if not already installed)

```bash
dotnet tool install --global dotnet-ef
```

Or update if already installed:
```bash
dotnet tool update --global dotnet-ef
```

### Step 5: Create Database Migration

Navigate to the project directory (where the .csproj file is located):

```bash
cd ApiProject
```

Create the initial migration:

```bash
dotnet ef migrations add InitialCreate
```

This will create a `Migrations` folder with migration files.

### Step 6: Apply Migration to Database

```bash
dotnet ef database update
```

This command will:
- Create the database (if it doesn't exist)
- Create all tables (Categories, Products, Carts, CartItems, Orders, OrderItems, Identity tables)
- Apply all relationships and constraints

### Step 7: Run the Application

```bash
dotnet run
```

Or if using Visual Studio, press `F5` or click the "Run" button.

The API will start and be available at:
- HTTPS: `https://localhost:5001`
- HTTP: `http://localhost:5000`

### Step 8: Verify the Setup

The application will automatically seed the database with sample data on first run.

You can verify by:

1. **Check Categories:**
   ```
   GET https://localhost:5001/api/categories
   ```

2. **Check Products:**
   ```
   GET https://localhost:5001/api/products
   ```

## Testing the API

### Method 1: Using a Web Browser (for GET requests only)

Navigate to:
- `https://localhost:5001/api/categories`
- `https://localhost:5001/api/products`

### Method 2: Using PowerShell/Terminal

**Get Categories:**
```powershell
Invoke-RestMethod -Uri "https://localhost:5001/api/categories" -Method Get
```

**Register a User:**
```powershell
$body = @{
    email = "test@example.com"
    password = "Test123"
    confirmPassword = "Test123"
    firstName = "Test"
    lastName = "User"
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:5001/api/auth/register" -Method Post -Body $body -ContentType "application/json"
```

### Method 3: Using Postman or Insomnia

1. Import the API endpoints from the README.md
2. Start with authentication endpoints to get a JWT token
3. Use the token in the Authorization header for protected endpoints

### Method 4: Using curl

**Register a User:**
```bash
curl -X POST https://localhost:5001/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "Test123",
    "confirmPassword": "Test123",
    "firstName": "Test",
    "lastName": "User"
  }'
```

**Login:**
```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "Test123"
  }'
```

## Sample Workflow

Here's a complete workflow to test the e-commerce functionality:

### 1. Register a New User
```http
POST /api/auth/register
{
  "email": "customer@example.com",
  "password": "Customer123",
  "confirmPassword": "Customer123",
  "firstName": "Jane",
  "lastName": "Doe"
}
```

Save the returned `token` for subsequent requests.

### 2. Browse Products
```http
GET /api/products
```

### 3. Add Items to Cart
```http
POST /api/carts/items
Authorization: Bearer YOUR_TOKEN_HERE
{
  "productId": 1,
  "quantity": 2
}
```

### 4. View Cart
```http
GET /api/carts
Authorization: Bearer YOUR_TOKEN_HERE
```

### 5. Create Order
```http
POST /api/orders
Authorization: Bearer YOUR_TOKEN_HERE
{
  "shippingAddress": "123 Main Street, City, State 12345"
}
```

### 6. View Order History
```http
GET /api/orders
Authorization: Bearer YOUR_TOKEN_HERE
```

## Troubleshooting

### Issue: "Unable to connect to database"

**Solution:**
1. Verify SQL Server is running
2. Check connection string in `appsettings.json`
3. For LocalDB, ensure Visual Studio is installed

### Issue: "Build failed" or "Package not found"

**Solution:**
```bash
dotnet clean
dotnet restore
dotnet build
```

### Issue: "Migration already applied"

**Solution:**
If you need to reset the database:
```bash
dotnet ef database drop
dotnet ef database update
```

### Issue: "Cannot create database"

**Solution:**
1. Check SQL Server permissions
2. Try running Visual Studio or terminal as Administrator
3. Verify SQL Server service is running

### Issue: JWT Token errors

**Solution:**
1. Ensure token is included in Authorization header: `Bearer YOUR_TOKEN`
2. Check token expiration (default: 60 minutes)
3. Register/Login again to get a fresh token

## Database Management

### View Database in Visual Studio

1. Open **View > SQL Server Object Explorer**
2. Expand **SQL Server > (localdb)\MSSQLLocalDB > Databases**
3. Find **CurlyQueensDB**
4. Expand to view tables

### Reset Database

To start fresh:
```bash
dotnet ef database drop -f
dotnet ef database update
```

The application will re-seed with sample data on next run.

### Add New Migration

After modifying models:
```bash
dotnet ef migrations add YourMigrationName
dotnet ef database update
```

## Next Steps

- Review the API documentation in `README.md`
- Test all endpoints using Postman or similar tool
- Customize the JWT settings in `appsettings.json`
- Add more sample data by modifying `Services/DatabaseSeeder.cs`
- Implement additional features as needed

## Support

If you encounter issues not covered here, check:
1. Ensure all prerequisites are installed
2. Verify .NET 9 SDK is being used: `dotnet --version`
3. Check that all NuGet packages were restored successfully
4. Review error messages in the console for specific issues

## Security Notes

?? **Important for Production:**
1. Change the JWT `SecretKey` in `appsettings.json`
2. Update connection strings with proper credentials
3. Enable HTTPS only
4. Configure proper CORS policies
5. Use environment variables for sensitive data
6. Implement rate limiting
7. Add proper logging and monitoring
