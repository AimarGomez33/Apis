using RestVSoapDemo.Models;
using RestVSoapDemo.Services;

namespace RestVSoapDemo.Soap
{
    /// <summary>
    /// SOAP service implementation for product operations.
    ///   </summary>
    public class ProductSoapService : IProductSoapService

    {
        private readonly IProductRepository _productRepository;

        public ProductSoapService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            try
            {
                return await _productRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new System.ServiceModel.FaultException($"Error retrieving products: {ex.Message}");
            }
        }

        public async Task<Product?> GetById(int id)
        {
            try
            {
                return await _productRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new System.ServiceModel.FaultException($"Error retrieving product with ID {id}: {ex.Message}");
            }
        }

        public async Task<Product> Create(string name, string description, decimal price, string category)
        {
            try
            {
                // Validación de parámetros
                if (string.IsNullOrEmpty(name))
                {
                    throw new System.ServiceModel.FaultException("Product name is required");
                }

                // Crear el objeto producto
                var product = new Product
                {
                    Name = name,
                    Description = description ?? string.Empty,
                    Price = price,
                    Category = category ?? string.Empty
                };

                return await _productRepository.CreateAsync(product);
            }
            catch (System.ServiceModel.FaultException)
            {
                // Re-lanzar FaultExceptions tal como están
                throw;
            }
            catch (Exception ex)
            {
                throw new System.ServiceModel.FaultException($"Error creating product: {ex.Message}");
            }
        }

        public async Task<Product?> Update(int id, Product product)
        {
            try
            {
                return await _productRepository.UpdateAsync(id, product);
            }
            catch (Exception ex)
            {
                throw new System.ServiceModel.FaultException($"Error updating product with ID {id}: {ex.Message}");
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                return await _productRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new System.ServiceModel.FaultException($"Error deleting product with ID {id}: {ex.Message}");
            }
        }
    }
}


        