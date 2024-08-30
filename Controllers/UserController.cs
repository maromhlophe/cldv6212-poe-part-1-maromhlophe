using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using cloud_2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cloud_2.Controllers
{
    public class UserController : Controller
    {
        // GET: UserController
        public async Task<ActionResult> Index()
        {
            TableClient table = new TableClient("DefaultEndpointsProtocol=https;AccountName=st10084808;AccountKey=B+hnzy/UzHj7kgNzih4iD7fC+lBbz/w97vJUA3pdptFJ8reisYYKHjTou7c3FeKgpu8D+Ce4qfXz+AStvb1MEw==;EndpointSuffix=core.windows.net", "CustomerProfiles");
            List<User> user = new List<User>();
            await foreach (var item in table.QueryAsync<User>())
            {
                user.Add(item);
            }
            return View(user);
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(User user)
        {
            TableClient table = new TableClient("DefaultEndpointsProtocol=https;AccountName=st10084808;AccountKey=B+hnzy/UzHj7kgNzih4iD7fC+lBbz/w97vJUA3pdptFJ8reisYYKHjTou7c3FeKgpu8D+Ce4qfXz+AStvb1MEw==;EndpointSuffix=core.windows.net", "CustomerProfiles");

            User userInfo = new User
            {
                Username = user.Username,
                contact = user.contact,
                address = user.address,
                email = user.email,
                PartitionKey = Guid.NewGuid().ToString(),
                RowKey = Guid.NewGuid().ToString(),
            };
            table.AddEntity(userInfo);
            return RedirectToAction("Index");
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
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

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
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
