using Microsoft.EntityFrameworkCore;
using PetFeast_Backend2.Models.CartModels.DTOs;
using PetFeast_Backend2.Models.OrderModels;
using PetFeast_Backend2.Models.OrderModels.DTOs;
using Razorpay.Api;

namespace PetFeast_Backend2.Services.OrderService
{
    public class OrderService : IOrderService
    {

        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public OrderService(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<string> RazorOrderCreate(long price)
        {
            Dictionary<string, object> input = new Dictionary<string, object>();
            Random random = new Random();
            string TransactionId = random.Next(0, 1000).ToString();

            input.Add("amount", Convert.ToDecimal(price) * 100);
            input.Add("currency", "INR");
            input.Add("receipt", TransactionId);

            string key = _configuration["Razorpay:KeyId"];
            string secret = _configuration["Razorpay:KeySecret"];

            RazorpayClient client = new RazorpayClient(key, secret);
            Razorpay.Api.Order order = client.Order.Create(input);
            var OrderId = order["id"].ToString();

            return OrderId;
        }


        public bool RazorPayment(PaymentDTO payment)
        {
            if(payment == null ||
                string.IsNullOrEmpty(payment.razorpay_payment_id) ||
                string.IsNullOrEmpty(payment.razorpay_order_id) ||
                string.IsNullOrEmpty(payment.razorpay_signature))
            {
                return false;
            }

            try
            {
                RazorpayClient client = new RazorpayClient(
                    _configuration["Razorpay:KeyId"],
                    _configuration["Razorpay:KeySecret"]
                    );

                Dictionary<string, string> attributes = new Dictionary<string, string>
                {
                    { "razorpay_payment_id", payment.razorpay_payment_id },
                    { "razorpay_order_id", payment.razorpay_order_id },
                    { "razorpay_signature", payment.razorpay_signature }
                };

                Utils.verifyPaymentSignature(attributes);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> CreateOrder(int userId, CreateOrderDTO createOrderDto)
        {
            try
            {
                var cart = await _context.Carts
                    .Include(c => c.cartItems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null)
                {
                    throw new Exception("Your cart is empty");
                }

                var order = new OrderMain
                {
                   UserId = userId,
                   OrderDate = DateTime.Now,
                   AddressId = createOrderDto.AddressId,

                   OrderStatus = "Pending",

                   //CustomerName = createOrderDto.CustomerName,
                   //CustomerEmail = createOrderDto.CustomerEmail,
                   //CustomerCity = createOrderDto.CustomerCity,
                   //CustomerPhone = createOrderDto.CustomerPhone,
                   //HomeAddress = createOrderDto.HomeAddress,
                   TotalPrice = createOrderDto.TotalPrice,
                   OrderString = createOrderDto.OrderString,
                   TransactionId = createOrderDto.TransactionId,
                   OrderItems = cart.cartItems.Select(ci => new OrderItem
                   {
                       ProductId = ci.ProductId,
                       Quantity = ci.Quantity,
                       TotalPrice = ci.Quantity * ci.Product.Price
                   }).ToList()
                };

                // implemented logic for decrease stock after addinng order;
                foreach (var item in cart.cartItems)
                {
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == item.ProductId);
                    if(product != null)
                    {
                        if(product.Stock < item.Quantity)
                        {
                            throw new Exception("Product is out of stock");
                        }
                        product.Stock -= item.Quantity;
                    }
                }

                await _context.Orders.AddAsync(order);
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (DbUpdateException ex)
            {

                throw new Exception(ex.InnerException?.Message);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<OrderUserDetailViewDTO>> GetOrderDetails(int userId)
        {
            try
            {
                var orders = await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .Where(u => u.UserId == userId)
                    .ToListAsync();

                var orderDetails = new List<OrderUserDetailViewDTO>();
                foreach(var order in orders)
                {
                    var newOrderDetails = new OrderUserDetailViewDTO
                    {
                        Id = order.Id,
                        OrderDate = order.OrderDate,
                        OrderId = order.OrderString,
                        OrderStatus = order.OrderStatus,
                        TransactionId = order.TransactionId,
                        OrderProducts = order.OrderItems.Select(oi => new CartResDTO
                        {
                            ProductId = oi.ProductId,
                            ProductName = oi?.Product?.Name,
                            Price = oi.Product.Price,
                            Quantity = oi.Quantity,
                            TotalPrice = oi.TotalPrice,
                            Image = oi.Product.Image
                        }).ToList()
                    };
                    orderDetails.Add(newOrderDetails);
                }

                return orderDetails;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<OrderAdminViewDTO>> GetOrderDetailsAdmin()
        {
            try
            {
                var orders = await _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderItems)
                    .OrderByDescending(o => o.OrderDate)
                    .Take(10)
                    .ToListAsync();

                if(orders.Count != 0)
                {
                    var orderDetails = orders.Select(o => new OrderAdminViewDTO
                    {
                        Id = o.Id,
                        CustomerName = o.User?.Name,
                        CustomerEmail = o.User?.Email,
                        OrderId = o.OrderString,
                        TransactionId = o.TransactionId,
                        OrderDate = o.OrderDate,
                        TotalPrice = o.TotalPrice,
                        Orderstatus = o.OrderStatus
                    }).ToList();

                    return orderDetails;
                }

                return new List<OrderAdminViewDTO>();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<OrderUserDetailViewDTO>> GetOrdersByUserId(int userId)
        {
            try
            {
                var orders = await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .Where(u => u.UserId == userId)
                    .ToListAsync();
                    
                if(orders == null || !orders.Any())
                {
                    return new List<OrderUserDetailViewDTO>();
                }

                var orderDetails = orders.Select(o => new OrderUserDetailViewDTO
                {

                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    OrderId = o.OrderString,
                    TransactionId = o.TransactionId,
                    OrderStatus = o.OrderStatus,
                    OrderProducts = o.OrderItems.Select(oi => new CartResDTO
                    {
                        ProductId = oi.ProductId,
                        ProductName = oi?.Product?.Name,
                        Price = oi.Product.Price,
                        Quantity = oi.Quantity,
                        TotalPrice = oi.TotalPrice,
                        Image = oi.Product.Image
                    }).ToList()
                }).ToList();

                return orderDetails;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<decimal> TotalRevenue()
        {
            try
            {
                var total = await _context.OrderItems.SumAsync(oi => oi.TotalPrice);
                return total;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> TotalProductsPurchased()
        {
            try
            {
                var total = await _context.OrderItems.SumAsync(oi => oi.Quantity);
                return total;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateOrderStatus(int orderId, string newStatus)
        {
            try
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
                if (order == null) throw new Exception("Order with this id is not exist");

                order.OrderStatus = newStatus;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CancelOrder(int userId ,int orderId)
        {
            try
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
                if (order == null) throw new Exception("Order with this order id and user id is not exist");

                order.OrderStatus = "Cancelled";
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
