using System.Net;
using System.Threading.Tasks;

namespace PhoneInfo.API.Domain.Interfaces
{
	public interface IDealService
	{
		Task<(string, HttpStatusCode)> GetDealsAsync();
	}
}
