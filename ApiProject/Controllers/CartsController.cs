using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiProject.Data;
using ApiProject.DTOs;
using ApiProject.Models;
using System.Security.Claims;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/carts
        [HttpGet]
        public async Task<ActionResult<CartDto>> GetCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                // Return empty cart if not found
                return Ok(new CartDto
                {
                    Id = 0,
                    UserId = userId,
                    Items = new List<CartItemDto>()
                });
            }

            var cartDto = new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Items = cart.CartItems.Select(ci => new CartItemDto
                {
                    Id = ci.Id,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product.Name,
                    Price = ci.Product.Price,
                    Quantity = ci.Quantity,
                    ImageUrl = ci.Product.ImageUrl
                }).ToList()
            };

            return Ok(cartDto);
        }

        // POST: api/carts/items
        [HttpPost("items")]
        public async Task<ActionResult<CartDto>> AddItemToCart(AddToCartDto addToCartDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            // Check if product exists and has sufficient stock
            var product = await _context.Products.FindAsync(addToCartDto.ProductId);
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            if (product.StockQuantity < addToCartDto.Quantity)
            {
                return BadRequest(new { message = $"Insufficient stock. Only {product.StockQuantity} items available." });
            }

            // Get or create cart
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            // Check if item already exists in cart
            var existingCartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == addToCartDto.ProductId);
            if (existingCartItem != null)
            {
                // Update quantity
                var newQuantity = existingCartItem.Quantity + addToCartDto.Quantity;
                if (product.StockQuantity < newQuantity)
                {
                    return BadRequest(new { message = $"Insufficient stock. Only {product.StockQuantity} items available." });
                }
                existingCartItem.Quantity = newQuantity;
            }
            else
            {
                // Add new item to cart
                var cartItem = new CartItem
                {
                    ProductId = addToCartDto.ProductId,
                    Quantity = addToCartDto.Quantity,
                    CartId = cart.Id
                };
                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();

            // Return updated cart
            return await GetCart();
        }

        // PUT: api/carts/items/5
        [HttpPut("items/{productId}")]
        public async Task<ActionResult<CartDto>> UpdateCartItem(int productId, UpdateCartItemDto updateCartItemDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return NotFound(new { message = "Cart not found" });
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem == null)
            {
                return NotFound(new { message = "Item not found in cart" });
            }

            // Check stock availability
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            if (product.StockQuantity < updateCartItemDto.Quantity)
            {
                return BadRequest(new { message = $"Insufficient stock. Only {product.StockQuantity} items available." });
            }

            cartItem.Quantity = updateCartItemDto.Quantity;
            await _context.SaveChangesAsync();

            // Return updated cart
            return await GetCart();
        }

        // DELETE: api/carts/items/5
        [HttpDelete("items/{productId}")]
        public async Task<ActionResult<CartDto>> RemoveItemFromCart(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return NotFound(new { message = "Cart not found" });
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem == null)
            {
                return NotFound(new { message = "Item not found in cart" });
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            // Return updated cart
            return await GetCart();
        }

        // DELETE: api/carts
        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null)
            {
                _context.CartItems.RemoveRange(cart.CartItems);
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }
    }
}
