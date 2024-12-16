using PetFeast_Backend2.Models.ProductModels.DTOs;

namespace PetFeast_Backend2.Services.ProductService
{
    public interface IProductService
    {
        Task<List<ProductOutDTO>> GetProducts();
        Task<ProductOutDTO> GetProductById(int id);
        Task<List<ProductOutDTO>> GetProductByCategory(int categoryId);
        Task<List<ProductOutDTO>> ProductPagination(int pagenumber = 1, int size = 10);
        Task<List<ProductOutDTO>> SearchProduct(string search);
        Task<bool> AddProduct(ProductCreateDTO product, IFormFile image);
        Task<bool> UpdateProduct(int id, ProductCreateDTO updatedProduct, IFormFile image);
        Task<ProductOutDTO> UpdateProduct2(int id, productUpdateDTO updatedProduct, IFormFile image);
        Task<bool> DeleteProduct(int productId);
        Task<List<ProductOutDTO>> TopRatedProducts();
    }
}
