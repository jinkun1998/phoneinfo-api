using System.Net;
using System.Threading.Tasks;

namespace PhoneInfo.API.Domain.Interfaces
{
	public interface ICatalogService
	{
		Task<(string content, HttpStatusCode statusCode)> GetBrandsAsync();
		Task<(string content, HttpStatusCode statusCode)> GetProductByBrandAsync(string brand);
		Task<(string content, HttpStatusCode statusCode)> GetProductDetailAsync(string product);
	}
}