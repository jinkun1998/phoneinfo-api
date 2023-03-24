using System.Net;
using System.Threading.Tasks;

namespace PhoneInfo.API.Domain.Interfaces
{
	public interface ISearchService
	{
		Task<(string, HttpStatusCode)> SearchAll(string keyword);
	}
}
