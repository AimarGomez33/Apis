using RestVSoapDemo.Models;

namespace RestVSoapDemo.Services
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product> CreateAsync(Product product);
        Task<Product?> UpdateAsync(int id, Product product);
        Task<bool> DeleteAsync(int id);


    }

    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> _products;


        public ProductRepository()
        {
            _products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Description = "High performance laptop", Price = 999.99m, Category = "Electronics" },
                new Product { Id = 2, Name = "Smartphone", Description = "Latest model smartphone", Price = 699.99m, Category = "Electronics" },
                new Product { Id = 3, Name = "Tablet", Description = "Portable tablet with stylus", Price = 499.99m, Category = "Electronics" }

            };
        }
        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Product>>(_products);
        }

        public Task<Product?> GetByIdAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(product);
        }

        public Task<Product> CreateAsync(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            // Asignar ID de forma segura
            product.Id = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
            
            // Asegurar que CreateDate se establezca
            if (product.CreateDate == default)
                product.CreateDate = DateTime.UtcNow;
            
            _products.Add(product);
            return Task.FromResult(product);
        }

        public Task<Product?> UpdateAsync(int id, Product product)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
            {
                return Task.FromResult<Product?>(null);
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;
            existingProduct.Category = product.Category;
            return Task.FromResult<Product?>(existingProduct);
        }

       
        public Task<bool> DeleteAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return Task.FromResult(false);
            }

            _products.Remove(product);
            return Task.FromResult(true);
        }

    }

}