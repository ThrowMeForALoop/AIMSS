using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;


using Application.Services.Interfaces;

using HtmlAgilityPack;

namespace Infrastructure.Services
{
    public class AIMMSService: IAIMSSService
    {
        private readonly HttpClient _httpClient;

        public AIMMSService(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("https://www.aimms.com/");
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<string>> GetAllCompanyProductsAsync(IEnumerable<string> categories)
        {
            _ = categories ?? throw new ArgumentNullException(nameof(categories));

            var response = await _httpClient.GetAsync("/");
            response.EnsureSuccessStatusCode();

            var html = await response.Content.ReadAsStringAsync();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var searchedCategories = new List<string>();
            searchedCategories.AddRange(categories);

            var products = new List<string>();

            var subMenuNodes = htmlDoc.DocumentNode.SelectNodes("//ul[contains(@class, 'sub-menu')]");
            foreach (var subMenuNode in subMenuNodes) 
            {
                var siblingNodes = subMenuNode.ParentNode?.ChildNodes;
                if (siblingNodes is null) { continue; }

                bool isRelevantSubMenu = siblingNodes.Any(node => searchedCategories.Contains(node.InnerText));

                if (isRelevantSubMenu)
                {
                    var productNodes = subMenuNode.SelectNodes(".//li/a") ?? Enumerable.Empty<HtmlNode>();
                    foreach (var menuItemNode in productNodes)
                    {
                        products.Add(HttpUtility.HtmlDecode(menuItemNode.InnerText));
                    }
                }
            }

            return products;
        }
    }
}
