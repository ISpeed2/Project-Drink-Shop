using OnlineStoreCLI.DAL;
using System.Collections.Generic;

namespace OnlineStoreCLI.BLL
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public List<Product> GetAllProducts()
        {
            // Здесь можно добавить бизнес-логику, например, фильтрацию, сортировку, и т.д.
            return _productRepository.GetAll();
        }

        public Product? GetProductById(int productId)
        {
            // Здесь можно добавить бизнес-логику, например, проверку прав доступа
            return _productRepository.GetById(productId);
        }

        public void AddProduct(Product product)
        {
            // Здесь можно добавить бизнес-логику, например, валидацию данных
            if (string.IsNullOrEmpty(product.Name))
            {
                throw new ArgumentException("Название товара не может быть пустым.");
            }
            _productRepository.Add(product);
        }

        public void UpdateProductPrice(int productId, decimal newPrice)
        {
            // Здесь можно добавить бизнес-логику, например, проверку допустимости новой цены
            if (newPrice <= 0)
            {
                throw new ArgumentException("Цена товара должна быть больше нуля.");
            }
            _productRepository.UpdatePrice(productId, newPrice);
        }

        public void DeleteProduct(int productId)
        {
            // Здесь можно добавить бизнес-логику, например, проверку прав доступа
            _productRepository.Delete(productId);
        }
    }
}