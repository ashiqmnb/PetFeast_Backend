using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetFeast_Backend2.Models.WishListModels;
using PetFeast_Backend2.Models.WishListModels.DTOs;

namespace PetFeast_Backend2.Services.WishListService
{
    public class WishListService : IWishListService
    {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public WishListService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        public async Task<string> AddOrRemove(int userId, int productId)
        {
            try
            {
                var product = await _context.WishLists
                    .Include(p => p.Product)
                    .FirstOrDefaultAsync(w => w.ProductId == productId && w.UserId == userId);

                if (product == null)
                {   
                    var wishListCreate = new WishListCreateDTO
                    {
                        UserId = userId,
                        ProductId = productId
                    };

                    var newWishList = _mapper.Map<WishList>(wishListCreate);

                    _context.WishLists.Add(newWishList);
                    await _context.SaveChangesAsync();

                    return "Product added to wishlist";
                }
                else
                {
                    _context.WishLists.Remove(product);
                    await _context.SaveChangesAsync();

                    return "Product removed from wishlist";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<WishListResDTO>> GetWishList(int userId)
        {
            try
            {
                var wishLists = await _context.WishLists
                    .Include(w => w.Product)
                    .ThenInclude(p => p.Category)
                    .Where(w => w.UserId == userId)
                    .ToListAsync();

                if (wishLists.Count > 0)
                {
                    return wishLists.Select(w => new WishListResDTO
                    {
                        ProductId = w.ProductId,
                        Name = w.Product.Name,
                        Description = w.Product.Description,
                        Price = w.Product.Price,
                        Category = w.Product.Category.Name,
                        Image = w.Product.Image,


                        //
                        //MRP = w.Product.MRP
                    }).ToList();
                }

                return new List<WishListResDTO>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> checkInWishlist(int producId, int userId)
        {
            try
            {
                var exists = await _context.WishLists
                    .AnyAsync(w => w.ProductId == producId && w.UserId == userId);

                return exists;
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
