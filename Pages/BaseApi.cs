using RestSharp;
using System.Threading.Tasks;

namespace apiautomation.Pages
{
    public class BaseApi
    {
        protected RestClient _client;

        public BaseApi()
        {
            _client = new RestClient("http://localhost:3200");
        }

        public async Task<RestResponse> ExecuteRequest(RestRequest request)
        {
            return await _client.ExecuteAsync(request);
        }
    }
}
