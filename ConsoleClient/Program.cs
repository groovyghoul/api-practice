using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            
            RunAsync().Wait();
            
            Console.WriteLine("Responded...");
            Console.ReadKey();
        }

        static async Task RunAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:1275/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // bWlja2V5Om1vdXNl  ISO-8859-1 ===> mickey:mouse
                //var x = Encoding.GetEncoding("ISO-8859-1");
                var y = "richard:oneil";
                //var z = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(y));
                var encoded = Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(y));
                // ZG9uYWxkOmR1Y2s=      ISO-8859-1 ===> donald:duck
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encoded);

                client.DefaultRequestHeaders.Add("X-Api-Key", "ABC123");

                // New code:
                HttpResponseMessage response = await client.GetAsync("api/products/1");
                if (response.IsSuccessStatusCode)
                {
                    Product product = await response.Content.ReadAsAsync<Product>();
                    Console.WriteLine("{0}\t${1}\t{2}\t{3}", product.Name, product.Price, product.Category, product.Creds);
                }
            }
        }
    }

    class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
        public string Creds { get; set; }
    }
}
