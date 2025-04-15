using ProductsApi.Models;
using System.Text.Json;

namespace Products.Repository
{
    public interface IJsonRepository<T> where T : class
    {
        public List<T> GetAll();
        public List<T> GetByColor(string colour);

        public void Add(T newRecord);

        public void Save(List<T> data);
        public bool Delete(int id);

        public bool Update(int id, T updated);
    }

    public class ProductRepository : IJsonRepository<Product>
    {
        private readonly string _filePath;

        public ProductRepository()
        {
            _filePath =  Path.Combine(Directory.GetCurrentDirectory(), "products.json");
        }


        // Method to get all products
        public List<Product> GetAll()
        {
            if (!File.Exists(_filePath))
                return new List<Product>();

            var jsonString = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Product>>(jsonString) ?? new List<Product>();
        }

        // Method to get products by colour
        public List<Product> GetByColor(string colour)
        {
            var allProducts = GetAll();
            return allProducts.Where(p => p.Color.Equals(colour, System.StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // Method to add a new product
        public void Add(Product product)
        {
            var products = GetAll();
            products.Add(product);
            Save(products);
        }

        // Method to save the product list back to the file
        public void Save(List<Product> products)
        {
            var jsonString = JsonSerializer.Serialize(products, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, jsonString);
        }

        public bool Update(int id, Product updatedProduct)
        {
            var products = GetAll();
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return false;

            product.Name = updatedProduct.Name;
            product.Color = updatedProduct.Color;
            product.Price = updatedProduct.Price;

            Save(products);
            return true;
        }

        // Optional: Method to delete a product
        public bool Delete(int id)
        {
            var products = GetAll();
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return false;

            products.Remove(product);
            Save(products);
            return true;
        }




    }
}
