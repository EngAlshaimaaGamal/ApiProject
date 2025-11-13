# Quick Start Guide - CurlyQueens API

Get the CurlyQueens E-Commerce API running in under 5 minutes!

## Prerequisites Check

Before starting, verify you have:
- ? .NET 9 SDK installed: `dotnet --version`
- ? SQL Server or LocalDB available

## 3-Step Setup

### Step 1: Create the Database (30 seconds)

Open a terminal in the ApiProject folder and run:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Step 2: Run the Application (10 seconds)

```bash
dotnet run
```

You should see:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
```

### Step 3: Test the API (30 seconds)

Open your browser or use curl:

**Test 1 - Get Categories:**
```
https://localhost:5001/api/categories
```

**Test 2 - Get Products:**
```
https://localhost:5001/api/products
```

?? **Success!** Your API is running with sample data!

---

## Quick API Test Workflow

### 1. Register a User (PowerShell/Terminal)

**PowerShell:**
```powershell
$body = @{
    email = "test@example.com"
    password = "Test123"
    confirmPassword = "Test123"
    firstName = "Test"
    lastName = "User"
} | ConvertTo-Json

$response = Invoke-RestMethod -Uri "https://localhost:5001/api/auth/register" -Method Post -Body $body -ContentType "application/json"
$token = $response.token
Write-Host "Token: $token"
```

**Bash/curl:**
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

Copy the `token` from the response!

### 2. Add Item to Cart

**PowerShell:**
```powershell
$cartBody = @{
    productId = 1
    quantity = 2
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:5001/api/carts/items" -Method Post -Body $cartBody -ContentType "application/json" -Headers @{Authorization="Bearer $token"}
```

**Bash/curl:**
```bash
curl -X POST https://localhost:5001/api/carts/items \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{
    "productId": 1,
    "quantity": 2
  }'
```

### 3. View Cart

**PowerShell:**
```powershell
Invoke-RestMethod -Uri "https://localhost:5001/api/carts" -Method Get -Headers @{Authorization="Bearer $token"}
```

**Bash/curl:**
```bash
curl https://localhost:5001/api/carts \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### 4. Create Order

**PowerShell:**
```powershell
$orderBody = @{
    shippingAddress = "123 Main St, City, State 12345"
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:5001/api/orders" -Method Post -Body $orderBody -ContentType "application/json" -Headers @{Authorization="Bearer $token"}
```

**Bash/curl:**
```bash
curl -X POST https://localhost:5001/api/orders \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{
    "shippingAddress": "123 Main St, City, State 12345"
  }'
```

---

## Using Postman (Easiest Method)

### 1. Import Collection
1. Open Postman
2. Click "Import"
3. Select `CurlyQueens.postman_collection.json`
4. Collection appears in left sidebar

### 2. Set Base URL (if needed)
1. Click on collection name
2. Go to "Variables" tab
3. Update `baseUrl` if different from `https://localhost:5001`

### 3. Register and Get Token
1. Expand "Authentication" folder
2. Click "Register"
3. Click "Send"
4. Token is automatically saved! ?

### 4. Test Other Endpoints
All cart and order endpoints will automatically use the saved token!

---

## Sample Data Available

After first run, the database includes:

**Categories:**
1. Shampoos
2. Conditioners
3. Styling Products
4. Hair Treatments
5. Tools & Accessories

**Products:**
- 12 products across all categories
- Prices ranging from $12.99 to $34.99
- All products have stock available

---

## Common Issues & Quick Fixes

### ? "Unable to connect to database"

**Fix:**
```bash
# Check if SQL Server is running
# For LocalDB (included with Visual Studio):
sqllocaldb start MSSQLLocalDB
```

### ? "Build failed"

**Fix:**
```bash
dotnet clean
dotnet restore
dotnet build
```

### ? "Migration already applied"

**Fix - Start Fresh:**
```bash
dotnet ef database drop -f
dotnet ef database update
```

### ? "401 Unauthorized" on cart/order endpoints

**Fix:** 
- Ensure you're including the Authorization header: `Bearer YOUR_TOKEN`
- Token expires after 60 minutes - register/login again if expired

### ? Port already in use

**Fix:**
```bash
# Use a different port
dotnet run --urls "https://localhost:5555"
```

---

## Project Files Overview

| File | Purpose |
|------|---------|
| `README.md` | Complete API documentation |
| `SETUP.md` | Detailed setup instructions |
| `PROJECT_SUMMARY.md` | Technical overview |
| `QUICKSTART.md` | You are here! |
| `CurlyQueens.postman_collection.json` | Postman test collection |

---

## API Endpoints Quick Reference

### Public (No Auth Required)
- `POST /api/auth/register` - Register
- `POST /api/auth/login` - Login
- `GET /api/categories` - List categories
- `GET /api/products` - List products
- `GET /api/products?categoryId=1` - Filter products

### Protected (Requires Token)
- `GET /api/carts` - View cart
- `POST /api/carts/items` - Add to cart
- `PUT /api/carts/items/{id}` - Update quantity
- `DELETE /api/carts/items/{id}` - Remove from cart
- `POST /api/orders` - Create order
- `GET /api/orders` - View order history

---

## Next Steps

1. ? **API is running** - Great start!
2. ?? **Read README.md** - Learn all endpoints
3. ?? **Import Postman Collection** - Test easily
4. ?? **Customize** - Add your features
5. ?? **Deploy** - Azure, AWS, or Docker

---

## Need Help?

1. Check **SETUP.md** for detailed instructions
2. Review **README.md** for API documentation
3. See **PROJECT_SUMMARY.md** for technical details

---

## Tips for Development

1. **Keep the terminal open** - See logs in real-time
2. **Use Postman** - Much easier than curl/PowerShell
3. **Check database** - Use SQL Server Object Explorer in Visual Studio
4. **Modify seed data** - Edit `Services/DatabaseSeeder.cs`
5. **Add breakpoints** - Debug in Visual Studio

---

**?? You're Ready!** Start building your e-commerce application!

For detailed documentation, see [README.md](README.md)
