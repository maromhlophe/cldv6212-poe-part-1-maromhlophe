using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using cloud_2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.File;
using Microsoft.WindowsAzure.Storage;

namespace cloud_2.Controllers
{
    public class ProductController : Controller
    {
        // GET: ProductController
        public async Task<ActionResult> Index()
        {
            TableClient table = new TableClient("DefaultEndpointsProtocol=https;AccountName=st10084808;AccountKey=B+hnzy/UzHj7kgNzih4iD7fC+lBbz/w97vJUA3pdptFJ8reisYYKHjTou7c3FeKgpu8D+Ce4qfXz+AStvb1MEw==;EndpointSuffix=core.windows.net", "ProductInfo");
            List<Product> products = new List<Product>();
            await foreach (var item in table.QueryAsync<Product>())
            {
                products.Add(item);
            }
            return View(products);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Product product, IFormFile file)
        {
            TableClient table = new TableClient("DefaultEndpointsProtocol=https;AccountName=st10084808;AccountKey=B+hnzy/UzHj7kgNzih4iD7fC+lBbz/w97vJUA3pdptFJ8reisYYKHjTou7c3FeKgpu8D+Ce4qfXz+AStvb1MEw==;EndpointSuffix=core.windows.net", "ProductInfo");
            BlobServiceClient blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=st10084808;AccountKey=B+hnzy/UzHj7kgNzih4iD7fC+lBbz/w97vJUA3pdptFJ8reisYYKHjTou7c3FeKgpu8D+Ce4qfXz+AStvb1MEw==;EndpointSuffix=core.windows.net");
            QueueClient queue = new QueueClient("DefaultEndpointsProtocol=https;AccountName=st10084808;AccountKey=B+hnzy/UzHj7kgNzih4iD7fC+lBbz/w97vJUA3pdptFJ8reisYYKHjTou7c3FeKgpu8D+Ce4qfXz+AStvb1MEw==;EndpointSuffix=core.windows.net", "storequeue");

            Product productInfo = new Product
            {
                name = product.name,
                category = product.category,
                image = file.FileName,
                price = product.price,
                PartitionKey = Guid.NewGuid().ToString(),
                RowKey = Guid.NewGuid().ToString(),
            };
            await blobServiceClient.GetBlobContainerClient("product-images").GetBlobClient(file.FileName).UploadAsync(file.OpenReadStream(), true);
            table.AddEntity(productInfo);
            queue.SendMessage($"{product.name} has been added to store");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Purchase(string id)
        {
            TableClient table = new TableClient("DefaultEndpointsProtocol=https;AccountName=st10084808;AccountKey=B+hnzy/UzHj7kgNzih4iD7fC+lBbz/w97vJUA3pdptFJ8reisYYKHjTou7c3FeKgpu8D+Ce4qfXz+AStvb1MEw==;EndpointSuffix=core.windows.net", "OrderInfo");
            QueueClient queue = new QueueClient("DefaultEndpointsProtocol=https;AccountName=st10084808;AccountKey=B+hnzy/UzHj7kgNzih4iD7fC+lBbz/w97vJUA3pdptFJ8reisYYKHjTou7c3FeKgpu8D+Ce4qfXz+AStvb1MEw==;EndpointSuffix=core.windows.net", "orderqueue");
            Order order = new Order
            {
                product = id.ToString(),
            };
            table.AddEntity(order);
            await queue.SendMessageAsync($"Product {id} purchased");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> FileUpload(IFormFile file)
        {
            //Connect to Azure
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=st10084808;AccountKey=B+hnzy/UzHj7kgNzih4iD7fC+lBbz/w97vJUA3pdptFJ8reisYYKHjTou7c3FeKgpu8D+Ce4qfXz+AStvb1MEw==;EndpointSuffix=core.windows.net");

            // Create a reference to the file client.
            CloudFileClient fileClient = storageAccount.CreateCloudFileClient();

            // Create a reference to the Azure path
            CloudFileDirectory cloudFileDirectory = fileClient.GetShareReference("contracts").GetRootDirectoryReference().GetDirectoryReference("file");

            //Create a reference to the filename that you will be uploading
            CloudFile cloudFile = cloudFileDirectory.GetFileReference(file.FileName);


            //Upload the file to Azure.
            await cloudFile.UploadFromStreamAsync(file.OpenReadStream());
            return View();
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
