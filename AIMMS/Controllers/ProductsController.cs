using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

using Application.Services.Interfaces;

namespace AIMMS.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IAIMSSService _aimssService;
        public ProductsController(IAIMSSService aimssService)
        {
            _aimssService = aimssService;
        }
        // GET: api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get([FromQuery (Name = "categories")] string categoryQuery)
        {
            try
            {
                if (string.IsNullOrEmpty(categoryQuery))
                {
                    return await _aimssService.GetAllCompanyProductsAsync(Enumerable.Empty<string>());
                }

                var categories = categoryQuery.Split(",");

                return await _aimssService.GetAllCompanyProductsAsync(categories);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Enumerable.Empty<string>();
            }
        }
    }
}
