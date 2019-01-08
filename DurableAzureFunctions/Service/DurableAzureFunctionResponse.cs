using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace DurableAzureFunctions
{
    public class DurableAzureFunctionResponse
    {
        public HttpResponseMessage OriginalHttpResponse { get; }

        public DurableAzureFunctionResponse(HttpResponseMessage httpResponse)
        {
            OriginalHttpResponse = httpResponse ?? throw new System.ArgumentNullException(nameof(httpResponse));
        }
    }

    public class DurableAzureFunctionResponse<T> : DurableAzureFunctionResponse
    {
        public Task<T> Data { get; }

        public DurableAzureFunctionResponse(HttpResponseMessage httpResponse) : base(httpResponse)
        {
            if (httpResponse.IsSuccessStatusCode)
            {
                Data = httpResponse.Content.ReadAsStringAsync().ContinueWith(readContentTask
                    => JsonConvert.DeserializeObject<T>(readContentTask.Result));
            }
        }
    }
}