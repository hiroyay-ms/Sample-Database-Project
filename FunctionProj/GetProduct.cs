using System.Linq;
using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using FunctionProj.Data;
using System.Runtime.InteropServices;
using System.Data.Common;

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
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "Product/{id:int}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var query = from pc in _context.ProductCategories 
                        join psc in _context.ProductCategories on pc.ProductCategoryId equals psc.ParentProductCategoryId 
                        join p in _context.Products on psc.ProductCategoryId equals p.ProductCategoryId 
                        join pm in _context.ProductModels on p.ProductModelId equals pm.ProductModelId 
                        join pmd in _context.ProductModelProductDescriptions on pm.ProductModelId equals pmd.ProductModelId 
                        join pd in _context.ProductDescriptions on pmd.ProductDescriptionId equals pd.ProductDescriptionId 
                        where pmd.Culture == "en" && psc.ProductCategoryId == id  
                        select new 
                        {
                            p.ProductId,
                            p.ProductName,
                            pm.ModelName,
                            p.ProductNumber,
                            p.Color,
                            p.StandardCost,
                            p.ListPrice,
                            p.Size,
                            p.Weight,
                            p.ProductCategoryId,
                            CategoryName = pc.CategoryName,
                            SubCategoryName = psc.CategoryName,
                            pd.Description,
                            p.SellStartDate,
                            p.SellEndDate,
                            p.ThumbnailPhotoFileName
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
