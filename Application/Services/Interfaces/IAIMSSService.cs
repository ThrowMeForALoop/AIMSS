using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IAIMSSService
    {
        public Task<IEnumerable<string>> GetAllCompanyProductsAsync(IEnumerable<string> categories);
    }
}
