using System.Net;
using System.Threading.Tasks;

namespace PhoneInfo.API.Domain.Interfaces
{
	public interface IGlossaryService
	{
		Task<(string, HttpStatusCode)> GetGlossaries();
		Task<(string, HttpStatusCode)> GetTerm(string term);
	}
}
