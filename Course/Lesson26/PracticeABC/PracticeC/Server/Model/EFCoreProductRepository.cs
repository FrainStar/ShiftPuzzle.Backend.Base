using System;
using System.Collections.Generic;
using System.Linq;

namespace PracticeABC
{
    public class EFCoreProductRepository : IProductRepository
    {
        private event Action<Product> OnProductAdded;
        private event Action<Product> OnProductUpdated;
        private event Action<Product> OnProductDeleted;
        private readonly ProductContext _context;

        public EFCoreProductRepository(ProductContext context)
        {
            _context = context;
            OnProductAdded += SendNotificationToStatDepartmetn;
            OnProductUpdated += SendNotificationToStatDepartmetnU;
            OnProductDeleted += SendNotificationToStatDepartmetnD;
        }

        private void SendNotificationToStatDepartmetn(Product product)
        {
            Console.WriteLine("Отправляю отчет в отдел статистики...");
            Console.WriteLine($"Добавлен новый продукт: {product.Name}");
        }
        private void SendNotificationToStatDepartmetnU(Product product)
        {
            Console.WriteLine("Отправляю отчет в отдел статистики...");
            Console.WriteLine($"Обновлён продукт: {product.Name}");
        }
        private void SendNotificationToStatDepartmetnD(Product product)
        {
            Console.WriteLine("Отправляю отчет в отдел статистики...");
            Console.WriteLine($"Удалён продукт: {product.Name}");
        }

        public List<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public Product GetProductByName(string name)
        {
            return _context.Products.FirstOrDefault(p => p.Name == name);
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();

            OnProductAdded?.Invoke(product);
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();

            OnProductUpdated?.Invoke(product);
        }

        public void DeleteProduct(string name)
        {
            var product = _context.Products.FirstOrDefault(p => p.Name == name);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
                OnProductDeleted?.Invoke(product);
            }
        }
    }
}