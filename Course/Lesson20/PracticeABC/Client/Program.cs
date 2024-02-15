﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace Client
{
    public class Program
    {
        private const string BaseUrl = "http://localhost";
        private const string Port = "5087";
        private const string AuthMethod = "/store/auth";
        private const string AddProductMethod = "/store/add";
        private const string DeleteProductMethod = "/store/delete";
        private const string ShowProductsMethod = "/store/show";
        private const string SortProductMethod = "/store/sortbystock";

        [Serializable]
        public class Product
        {
            [Required]
            [StringLength(100, MinimumLength = 3)]
            public string name { get; set; }

            [Range(0.01, 10000)]
            public double price { get; set; }

            [Range(0, 10000)]
            public int stock { get; set; }

            public Product() { }

            public Product(string Name, double Price, int Stock)
            {
                this.name = Name;
                this.price = Price;
                this.stock = Stock;
            }

        }

        private static bool IsAuthorized = false;
        private static readonly HttpClient Client = new HttpClient();

        private static void DisplayProducts()
        {
            var url = $"{BaseUrl}:{Port}{ShowProductsMethod}";

            var response = Client.GetAsync(url).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;

            var products = JsonSerializer.Deserialize<List<Product>>(responseContent);

            Console.WriteLine("-----------------------------------------------------------------");
            Console.WriteLine("| Название продукта | Цена | Количество на складе |");
            Console.WriteLine("-----------------------------------------------------------------");

            foreach (var product in products)
            {
                Console.WriteLine($"| {product.Name, -18} | {product.Price, -5} | {product.Stock, -19} |");
            }

            Console.WriteLine("-----------------------------------------------------------------");
        }

        private static void SendProduct()
        {
            if (!IsAuthorized)
            {
                Console.WriteLine("Вы не авторизованы");
                return;
            }

            var url = $"{BaseUrl}:{Port}{AddProductMethod}";

            Console.WriteLine("Введите название продукта:");
            var name = Console.ReadLine();

            Console.WriteLine("Введите цену продукта:");
            var price = double.Parse(Console.ReadLine());

            Console.WriteLine("Введите количество на складе:");
            var stock = int.Parse(Console.ReadLine());

            var product = new Product
            {
                Name = name,
                Price = price,
                Stock = stock
            };

            var json = JsonSerializer.Serialize(product);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = Client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(responseContent);
            }
            else
            {
                Console.WriteLine($"Error: {response}");
            }
        }

        private static void DeleteProduct()
        {
            if (!IsAuthorized)
            {
                Console.WriteLine("Вы не авторизованы");
                return;
            }

            var url = $"{BaseUrl}:{Port}{DeleteProductMethod}";

            Console.WriteLine("Введите название продукта:");
            Dictionary<string, string> nameObj = new Dictionary<string, string>();

            string NameProd = Console.ReadLine();

            nameObj.Add("name", NameProd);
            // var NameObj =             // {
            //     name = NameProd
            // };

            var json = JsonSerializer.Serialize(nameObj);
            var content = new StringContent(NameProd, Encoding.UTF8, "application/json");

            var response = Client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Продукт удален");
            }
            else
            {
                Console.WriteLine($"Error: {response}");
            }
        }

        private static void SortProducts()
        {
            var url = $"{BaseUrl}:{Port}{SortProductMethod}";

            var response = Client.GetAsync(url).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;

            List<Product> products = JsonSerializer.Deserialize<List<Product>>(responseContent);

            Console.WriteLine("Список отсортирован");
            Console.WriteLine(responseContent);
            Console.WriteLine("-----------------------------------------------------------------");
            Console.WriteLine("| Название продукта | Цена | Количество на складе |");
            Console.WriteLine("-----------------------------------------------------------------");

            foreach (Product product in products)
            {
                Console.WriteLine($"| {product.Name, -18} | {product.Price, -5} | {product.Stock, -19} |");
            }

            Console.WriteLine("-----------------------------------------------------------------");
        }

        private static void Auth()
        {
            var url = $"{BaseUrl}:{Port}{AuthMethod}";

            Console.WriteLine("Введите имя пользователя:");
            var user = Console.ReadLine();

            Console.WriteLine("Введите пароль:");
            var pass = Console.ReadLine();

            var userData = new
            {
                User = user,
                Pass = pass
            };

            var json = JsonSerializer.Serialize(userData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = Client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(responseContent);
                IsAuthorized = true;
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                IsAuthorized = false;
            }
        }

        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            while (true)
            {
                Console.WriteLine("Выберите опцию:");
                Console.WriteLine("1. Авторизация");
                Console.WriteLine("2. Отправить продукт");
                Console.WriteLine("3. Удалить продукты");
                Console.WriteLine("4. Вывести список");
                Console.WriteLine("5. Отсортировать список по кол-ву продуктов");
                Console.WriteLine("6. Выйти");
                Console.Write("Введите ваш выбор: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Auth();
                        break;
                    case "2":
                        if (!IsAuthorized)
                        {
                            Console.WriteLine("Вы не авторизованы.");
                            break;
                        }

                        SendProduct();
                        break;
                    case "3":
                        if (!IsAuthorized)
                        {
                            Console.WriteLine("Вы не авторизованы.");
                            break;
                        }

                        Console.WriteLine("Функция в разработке");
                        break;
                    case "4":
                        DisplayProducts();
                        break;
                    case "5":
                        SortProducts();
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }
    }
}