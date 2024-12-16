using Microsoft.EntityFrameworkCore;
using PetFeast_Backend2.Models.CartModels;
using PetFeast_Backend2.Models.CartModels.DTOs;

namespace PetFeast_Backend2.Services.CartService
{
    public class CartService : ICartService
    {

        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public CartService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<CartApiResDTO> GetCartItems(int userId)
        {
            try
            {
                if (userId == 0) throw new Exception("User id is invalid ");

                var cart = await _context.Carts
                    .Include(c => c.cartItems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                if (cart?.cartItems == null) throw new Exception("Cart is empty  ");

                if (cart != null)
                {
                    var cartItems = cart.cartItems.Select(ci => new CartResDTO
                    {
                        ProductId = ci.ProductId,
                        ProductName = ci.Product.Name,
                        Price = ci.Product.Price,
                        Image = ci.Product.Image,
                        Quantity = ci.Quantity,
                        TotalPrice = ci.Product.Price * ci.Quantity,
                        MRP = ci.Product.MRP,
                        Stock = ci.Product.Stock,
                    }).ToList();

                    var totalCount = cartItems.Count();
                    var totalPrice = cartItems.Sum(ci => ci.TotalPrice);

                    return new CartApiResDTO
                    {
                        TotalCount = totalCount,
                        TotalPrice = totalPrice,
                        CartProducts = cartItems
                    };
                }

                return new CartApiResDTO();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }





        public async Task<bool> AddToCart(int userId, int productId)
        {
            try
            {
                if (userId == 0) throw new Exception("User not valid  ");

                var user = await _context.Users
                    .Include(u => u.Cart)
                    .ThenInclude(c => c.cartItems)
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                var product = await _context.Products.FirstOrDefaultAsync(u => u.ProductId == productId);

                if (product == null) throw new Exception("Product with this id is not exist ");

                if(user.Cart == null)
                {
                    user.Cart = new Cart
                    {
                        UserId = userId,
                        cartItems = new List<CartItem>()
                    };

                    _context.Carts.Add(user.Cart);
                    await _context.SaveChangesAsync();
                }

                CartItem existingProd = user.Cart.cartItems.FirstOrDefault(p =>  p.ProductId == productId);
                if (existingProd == null)
                {
                    // Not existing product, add cart item
                    CartItem cartItem = new CartItem
                    {
                        CartId = user.Cart.CartId,
                        ProductId = productId,
                        Quantity = 1
                    };

                    _context.CartItems.Add(cartItem);
                }
                else
                {
                    // Checking existing product's quantity before adding.....
                    if (existingProd.Quantity < 10)
                    {
                        existingProd.Quantity++;
                    }
                }

                await _context.SaveChangesAsync();
                return true;

            }
            catch(Exception ex)
            {
                //throw new Exception("__", ex.Message);
                Console.WriteLine("error occure while adding product to cart  ", ex.Message);
                return false;
            }
        }



        public async Task<bool> RemoveFromCart(int userId, int productId)
        {
            try
            {
                if (userId == 0) throw new Exception("User not valid  ");

                var user = await _context.Users
                    .Include(u => u.Cart)
                    .ThenInclude(c => c.cartItems)
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                var product = await _context.Products.FirstOrDefaultAsync(u => u.ProductId == productId);
                //if (product == null) throw new Exception("Product with this id is not exist");

                if (user != null && product != null)
                {
                    var item = await _context.CartItems.FirstOrDefaultAsync(ci => ci.ProductId == productId);

                    if (item != null)
                    {
                        _context.CartItems.Remove(item);
                        await _context.SaveChangesAsync();

                        return true;
                    }
                }

                return false;
                throw new Exception("exception occure while deleting a product from cart  ");
            }
            catch(Exception ex)
            {
                return false;
                throw new Exception("exception occure while deleting a product from cart  " + ex.Message);
            }

        }



        public async Task<bool> RemoveAllItems(int userId)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Cart)
                    .ThenInclude(c => c.cartItems)
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                if (user == null)
                {
                    throw new Exception("user not found");
                    //return false;
                }

                user?.Cart?.cartItems.Clear();
                await _context.SaveChangesAsync();

                return true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




        public async Task<bool> IncreaseQuantity(int userId, int productId)
        {
            try
            {
                if (userId == 0) throw new Exception("User with the current id is not found  ");

                var user = await _context.Users
                    .Include(u => u.Cart)
                    .ThenInclude(c => c.cartItems)
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);

                if(user != null && product != null)
                {
                    var item = user?.Cart?.cartItems.FirstOrDefault(ci => ci.ProductId == productId);

                    if(item == null) throw new Exception("Product with current id is not in cart  ");

                    if(item.Quantity == 10)
                    {
                        throw new Exception("Maximum quantity is 10  ");
                    }

                    item.Quantity++;
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    throw new Exception("User or Product cannot to be null  ");
                }

                //return false;
            }
            catch(Exception ex)
            {
                throw new Exception("An exception occured while increasing the quantity of the product  " + ex.Message);
            }
        }


        public async Task<bool> DecreaseQuantity(int userId, int productId)
        {
            try
            {
                if (userId == 0) throw new Exception("User with the current id is not found  ");

                var user = await _context.Users
                    .Include(u => u.Cart)
                    .ThenInclude(c => c.cartItems)
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);

                if (user != null && product != null)
                {
                    var item = user?.Cart?.cartItems.FirstOrDefault(ci => ci.ProductId == productId);

                    if (item == null) throw new Exception("Product with current id is not in cart  ");

                    if(item.Quantity == 1)
                    {
                        //return false;
                        //throw new Exception("Minimum quantity is 1");
                        user?.Cart?.cartItems.Remove(item);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    item.Quantity--;
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    throw new Exception("User or Product cannot to be null");
                }

                //return false;
            }
            catch (Exception ex)
            {
                throw new Exception("An exception occured while increasing the quantity of the product  " + ex.Message);
            }
        }
    }
}
