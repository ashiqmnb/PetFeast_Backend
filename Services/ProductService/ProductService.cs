using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetFeast_Backend2.Models.CategoryModels;
using PetFeast_Backend2.Models.ProductModels;
using PetFeast_Backend2.Models.ProductModels.DTOs;
using PetFeast_Backend2.Services.CloudinaryService;

namespace PetFeast_Backend2.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;

        public ProductService(AppDbContext context, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _context = context;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<List<ProductOutDTO>> GetProducts()
        {
            try
            {
                var products = await _context.Products.Include(p => p.Category).ToListAsync();
                if(products.Count > 0)
                {
                    var productOut = products.Select(p => new ProductOutDTO
                    {
                        ProductId = p.ProductId,
                        Name = p.Name,
                        Description = p.Description,
                        CategoryName = p.Category.Name,
                        Price = p.Price,
                        Image = p.Image,
                        Rating = p.Rating,

                        //
                        MRP = p.MRP,
                        Stock = p.Stock

                    }).ToList();
                    return productOut;
                }
                return [];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"An Exception has been occuured while fetching all products {ex.Message}");
            }
        }


        public async Task<ProductOutDTO> GetProductById(int id)
        {
            try
            {
                var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == id);
                if(product == null) return null;

                ProductOutDTO productOut = new ProductOutDTO
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Description = product.Description,
                    CategoryName = product.Category.Name,
                    Price = product.Price,
                    Image = product.Image,
                    Rating = product.Rating,

                    //
                    MRP = product.MRP,
                    Stock= product.Stock
                };
                return productOut;

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"An Exception has been occuured while fetching  products by id {ex.Message}");
            }
        }


        public async Task<List<ProductOutDTO>> GetProductByCategory(int categoryId)
        {
            try
            {
                var products = await _context.Products
                    .Include(p => p.Category)
                    .Where(p => p.CategoryId == categoryId)
                    .Select(p => new ProductOutDTO
                    {
                        ProductId = p.ProductId,
                        Name = p.Name,
                        Description = p.Description,
                        CategoryName = p.Category.Name,
                        Price = p.Price,
                        Image = p.Image,
                        Rating = p.Rating,

                        //
                        MRP = p.MRP,
                        Stock= p.Stock

                    }).ToListAsync();

                if (products != null) return products;
                return [];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"An Exception has been occuured while fetching  products by category id {ex.Message}");
            }
        }


        public async Task<List<ProductOutDTO>> ProductPagination(int pagenumber = 1, int size = 10)
        {
            try
            {
                var products = await _context.Products
                    .Include(p => p.Category)
                    .Skip((pagenumber - 1 ) * size)
                    .Take(size)
                    .ToListAsync();

                return products.Select(p => new ProductOutDTO
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    CategoryName = p.Category.Name,
                    Price = p.Price,
                    Image = p.Image,
                    Rating = p.Rating,

                    //
                    MRP = p.MRP,
                    Stock = p.Stock

                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<ProductOutDTO>> SearchProduct(string search)
        {
            try{
                if (string.IsNullOrEmpty(search))
                {
                    return new List<ProductOutDTO>();
                }

                var products = await _context.Products
                    .Include(p => p.Category)
                    .Where(p => p.Name.ToLower().Contains(search.ToLower()))
                    .ToListAsync();

                return products.Select(p => new ProductOutDTO
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    CategoryName = p.Category.Name,
                    Price = p.Price,
                    Image = p.Image,
                    Rating = p.Rating,

                    //
                    MRP = p.MRP,
                    Stock = p.Stock

                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> AddProduct(ProductCreateDTO newProduct, IFormFile image)
        {
            try
            {
                if (newProduct == null) throw new Exception("Product cannot to be null");

                if (newProduct.MRP < newProduct.Price) throw new Exception("MRP must greater than Price");

                var categoryExist = _context.Categories.FirstOrDefault(c => c.CategoryId == newProduct.CategoryId);

                if (categoryExist == null) throw new Exception("Category with this id is not exist");

                var product = _mapper.Map<Product>(newProduct);

                if (image == null) throw new Exception("Image is required");

                string imageUrl = await _cloudinaryService.UploadImageAsync(image);
                product.Image = imageUrl;
                
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




        public async Task<bool> UpdateProduct(int id, ProductCreateDTO updatedProduct, IFormFile image)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
                var categoryExist = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == updatedProduct.CategoryId);

                if (categoryExist == null) throw new Exception("Category with this id is not exist");
                if (updatedProduct.MRP < updatedProduct.Price) throw new Exception("MRP must greater than Price");


                if (product != null)
                {
                    product.Name = updatedProduct.Name;
                    product.Description = updatedProduct.Description;
                    product.Price = updatedProduct.Price;
                    product.CategoryId = updatedProduct.CategoryId;
                    product.Rating = updatedProduct.Rating;

                    //
                    product.MRP = updatedProduct.MRP;
                    product.Stock = updatedProduct.Stock;

                    if(image != null && image.Length > 0)
                    {
                        string imageUrl = await _cloudinaryService.UploadImageAsync(image);
                        product.Image = imageUrl;
                    }


                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                //return false;
                throw new Exception(ex.Message);
            }

        }


        // new product update API
        public async Task<ProductOutDTO> UpdateProduct2(int id, productUpdateDTO updatedProduct, IFormFile image = null)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if(product == null) throw new Exception("Product with this id is not exist");


            // Checking and updating category Id

            if (updatedProduct?.CategoryId != null)
            {
                var categoryExist = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == updatedProduct.CategoryId);
                if (categoryExist == null)
                {
                    throw new Exception("Category with this id is not exist");
                }
                else
                {
                    product.CategoryId = updatedProduct.CategoryId;
                }
            }

            // Updating other properties
            if(updatedProduct?.Name != null) product.Name = updatedProduct.Name;
            if(updatedProduct?.Description != null) product.Description = updatedProduct.Description;

            // ----Comparing Price & MRP and update----
            // When MRP and Price is available
            if(updatedProduct?.Price != null && updatedProduct?.MRP != null)
            {
                if(updatedProduct?.Price > updatedProduct?.MRP)
                {
                    throw new Exception("MRP must be greater than Price");
                }
                else
                {
                    product.MRP = updatedProduct.MRP;
                    product.Price = updatedProduct.Price;
                }
            }

            // When Price is available and MRP not
            if (updatedProduct?.Price != null && updatedProduct?.MRP == null)
            {
                if(updatedProduct?.Price > product?.MRP)
                {
                    throw new Exception("MRP must be greater than Price");
                }
                product.Price = updatedProduct.Price;
            }

            // When MRP is available and Price not
            if (updatedProduct?.Price == null && updatedProduct?.MRP != null)
            {
                if (product?.Price > updatedProduct?.MRP)
                {
                    throw new Exception("MRP must be greater than Price");
                }
                product.MRP = updatedProduct.MRP;
            }


            if (updatedProduct.Rating != null) product.Rating = updatedProduct.Rating;
            if (updatedProduct.Stock != null) product.Stock = updatedProduct.Stock;

            if (image != null && image.Length > 0)
            {
                string imageUrl = await _cloudinaryService.UploadImageAsync(image);
                product.Image = imageUrl;
            }

            await _context.SaveChangesAsync();

            ProductOutDTO productOut = new ProductOutDTO
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                CategoryName = product?.Category?.Name,
                Price = product.Price,
                Image = product.Image,
                Rating = product.Rating,

                //
                MRP = product.MRP,
                Stock = product.Stock
            };
            return productOut;

            //return product;
        }




        public async Task<bool> DeleteProduct(int productId)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
                if(product != null)
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //return false;
                throw new Exception(ex.Message);
            }
        }



        public async Task<List<ProductOutDTO>> TopRatedProducts()
        {
            try
            {
                var products = await _context.Products
                    .Include(p => p.Category)
                    .Where(p => p.Rating >= 4)
                    .Take(8) // Setting limit for number of items
                    .Select(p => new ProductOutDTO
                    {
                        ProductId = p.ProductId,
                        Name = p.Name,
                        Description = p.Description,
                        CategoryName = p.Category.Name,
                        Price = p.Price,
                        Image = p.Image,
                        Rating = p.Rating,

                        //
                        MRP = p.MRP,
                        Stock = p.Stock
                }).ToListAsync();

                return products;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
