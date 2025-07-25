using System.ServiceModel;
using RestVSoapDemo.Models;

namespace RestVSoapDemo.Soap
{
    /// <summary>
    /// SOAP service interface for product operations.
    /// </summary>
    [ServiceContract(Namespace = "http://tempuri.org/")]
    public interface IProductSoapService
    {
        [OperationContract(Action = "http://tempuri.org/GetAll")]
        Task<IEnumerable<Product>> GetAll();

        [OperationContract(Action = "http://tempuri.org/GetById")]
        Task<Product?> GetById(int id);

        [OperationContract(Action = "http://tempuri.org/Create")]
        Task<Product> Create(string name, string description, decimal price, string category);

        [OperationContract(Action = "http://tempuri.org/Update")]
        Task<Product?> Update(int id, Product product);

        [OperationContract(Action = "http://tempuri.org/Delete")]
        Task<bool> Delete(int id);

    }

    /// <summary>
    /// SOAP response model for product operations.
    /// </summary>
    [System.Runtime.Serialization.DataContract]
    public class SoapResponse
    { 
        [System.Runtime.Serialization.DataMember]
        public bool Success { get; set; }

        [System.Runtime.Serialization.DataMember]
        public string Message { get; set; } = string.Empty;

        [System.Runtime.Serialization.DataMember]
        public object? Data { get; set; }
    }

}
