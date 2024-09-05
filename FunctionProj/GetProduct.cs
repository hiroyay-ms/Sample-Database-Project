using System.Linq;
using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using FunctionProj.Data;

namespace functionproj
{
    public class GetProduct
    {
        private readonly ILogger _logger;
        private readonly AdventureWorksContext _context;

        public GetProduct(ILoggerFactory loggerFactory, AdventureWorksContext context)
        {
            _logger = loggerFactory.CreateLogger<GetProduct>();
            _context = context;
        }

        [Function("GetProduct")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var query = from p in _context.Products
                        select new {
                            p.ProductId,
                            p.ProductName,
                            p.ProductNumber,
                            p.Color,
                            p.StandardCost,
                            p.ListPrice,
                            p.Size,
                            p.Weight,
                            p.ProductCategoryId,
                            p.ProductModelId,
                            p.SellStartDate,
                            p.SellEndDate,
                            p.ModifiedDate
                        };

            var products = query.ToList();

            _logger.LogInformation($"Found {products.Count} products");

            string jsonStr = JsonSerializer.Serialize(products);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString(jsonStr);

            return response;
        }
    }
}
