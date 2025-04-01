using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Client
{
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

        public Product(string nname, double nprice, int nstock)
        {
            name = nname;
            price = nprice;
            stock = nstock;
        }

        public Product()
        {
            // Пустой конструктор
        }
    }

    public class Program
    {
        private const string BaseUrl = "http://localhost";
        private const string Port = "5087";
        private const string AuthMethod = "/store/auth";
        private const string AddProductMethod = "/store/add";
        private const string ShowProductsMethod = "/store/show";

        private static bool IsAuthorized = false;
        private static readonly HttpClient Client = new HttpClient();

        private static async Task DisplayProductsAsync()
        {
            try
            {
                var url = $"{BaseUrl}:{Port}{ShowProductsMethod}";
                var response = await Client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var products = JsonSerializer.Deserialize<List<Product>>(responseContent);

                    Console.WriteLine("-----------------------------------------------------------------");
                    Console.WriteLine("| Название продукта | Цена | Количество на складе |");
                    Console.WriteLine("-----------------------------------------------------------------");

                    foreach (var product in products)
                    {
                        Console.WriteLine($"| {product.name,-18} | {product.price,-5} | {product.stock,-19} |");
                    }

                    Console.WriteLine("-----------------------------------------------------------------");
                }
                else
                {
                    Console.WriteLine($"Ошибка: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Ошибка при запросе к серверу: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неизвестная ошибка: {ex.Message}");
            }
        }

        private static async Task SendProductAsync()
        {
            if (!IsAuthorized)
            {
                Console.WriteLine("Вы не авторизованы");
                return;
            }

            try
            {
                var url = $"{BaseUrl}:{Port}{AddProductMethod}";

                Console.WriteLine("Введите название продукта:");
                var name = Console.ReadLine();

                Console.WriteLine("Введите цену продукта:");
                if (!double.TryParse(Console.ReadLine(), out double price))
                {
                    Console.WriteLine("Неверный формат цены. Пожалуйста, введите число.");
                    return;
                }

                Console.WriteLine("Введите количество на складе:");
                if (!int.TryParse(Console.ReadLine(), out int stock))
                {
                    Console.WriteLine(" формат количества указан неверно. Пожалуйста, введите целое число.");
                    return;
                }

                var product = new Product(name, price, stock);
                var json = JsonSerializer.Serialize(product);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await Client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                }
                else
                {
                    Console.WriteLine($"Ошибка: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Ошибка при запросе к серверу: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неизвестная ошибка: {ex.Message}");
            }
        }

        private static async Task AuthAsync()
        {
            try
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

                var response = await Client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                    IsAuthorized = true;
                }
                else
                {
                    Console.WriteLine($"Ошибка: {response.StatusCode}");
                    IsAuthorized = false;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Ошибка при запросе к серверу: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неизвестная ошибка: {ex.Message}");
            }
        }

        private static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            while (true)
            {
                Console.WriteLine("Выберите опцию:");
                Console.WriteLine("1. Авторизация");
                Console.WriteLine("2. Отправить продукт");
                Console.WriteLine("3. Вывести список");
                Console.WriteLine("4. Выйти");
                Console.Write("Введите ваш выбор: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await AuthAsync();
                        break;
                    case "2":
                        if (!IsAuthorized)
                        {
                            Console.WriteLine("Вы не авторизованы.");
                            break;
                        }

                        await SendProductAsync();
                        break;
                    case "3":
                        await DisplayProductsAsync();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }
    }
}