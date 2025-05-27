using System.Collections.Generic;

namespace OnlineStoreCLI.DAL
{
    public interface IProductRepository
    {
        List<Product> GetAll();
        Product? GetById(int productId);
        void Add(Product product);
        int UpdatePrice(int productId, decimal newPrice);
        int Delete(int productId);
    }
}
