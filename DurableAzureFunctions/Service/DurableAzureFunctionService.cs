using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DurableAzureFunctions
{
    public class DurableAzureFunctionService : IDurableAzureFunctionService
    {
        #region Properties

        public string BaseUrl { get; set; }
        public string Code { get; set; }

        #endregion

        #region IDurableAzureFunctionService implementation

        public Task<DurableAzureFunctionResponse<IEnumerable<InstanceStatus>>> GetAllInstancesStatusAsync(string code = null, string taskHub = null, string connection = null, DateTime? createdTimeFrom = null, DateTime? createdTimeTo = null, IEnumerable<OrchestrationRuntimeStatus> runtimeStatus = null, bool? showInput = null, bool? showHistory = null, bool? showHistoryOutput = null, int? top = null)
        {
            var queryString = CreateQueryString(new[]
            {
                KeyValuePair.Create("code", code ?? Code),
                KeyValuePair.Create("taskHub", taskHub),
                KeyValuePair.Create("connection", connection),
                KeyValuePair.Create("createdTimeFrom", createdTimeFrom?.ToString("s", CultureInfo.InvariantCulture)),
                KeyValuePair.Create("createdTimeTo", createdTimeTo?.ToString("s", CultureInfo.InvariantCulture)),
                KeyValuePair.Create("runtimeStatus", runtimeStatus != null ? string.Join(",", runtimeStatus) : null),
                KeyValuePair.Create("showInput", showInput?.ToString()),
                KeyValuePair.Create("showHistory", showHistory?.ToString()),
                KeyValuePair.Create("showHistoryOutput", showHistoryOutput?.ToString()),
                KeyValuePair.Create("top", top?.ToString())
            });
            
            return SendRequest<IEnumerable<InstanceStatus>>(HttpMethod.Get, $"{BaseUrl}/runtime/webhooks/durableTask/instances/{queryString}");
        }

        public Task<DurableAzureFunctionResponse<InstanceStatus>> GetInstanceStatusAsync(string instanceId, string code = null, string taskHub = null, string connection = null, bool? showHistory = null, bool? showHistoryOutput = null)
        {
            var queryString = CreateQueryString(new[]
            {
                KeyValuePair.Create("code", code ?? Code),
                KeyValuePair.Create("taskHub", taskHub),
                KeyValuePair.Create("connection", connection),
                KeyValuePair.Create("showHistory", showHistory?.ToString()),
                KeyValuePair.Create("showHistoryOutput", showHistoryOutput?.ToString())
            });
            
            return SendRequest<InstanceStatus>(HttpMethod.Get, $"{BaseUrl}/runtime/webhooks/durabletask/instances/{instanceId}{queryString}");
        }

        public Task<DurableAzureFunctionResponse> RaiseEventAsync(string instanceId, string eventName, dynamic eventContent, string code = null, string taskHub = null, string connection = null)
        {
            var queryString = CreateQueryString(new[]
            {
                KeyValuePair.Create("code", code ?? Code),
                KeyValuePair.Create("taskHub", taskHub),
                KeyValuePair.Create("connection", connection)
            });

            return SendRequest(HttpMethod.Post, $"{BaseUrl}/runtime/webhooks/durabletask/instances/{instanceId}/raiseEvent/{eventName}{queryString}", eventContent);
        }

        public Task<DurableAzureFunctionResponse> RewindInstanceAsync(string instanceId, string reason = null, string code = null, string taskHub = null, string connection = null)
        {
            var queryString = CreateQueryString(new[]
            {
                KeyValuePair.Create("reason", reason),
                KeyValuePair.Create("code", code ?? Code),
                KeyValuePair.Create("taskHub", taskHub),
                KeyValuePair.Create("connection", connection)
            });

            return SendRequest(HttpMethod.Post, $"{BaseUrl}/runtime/webhooks/durabletask/instances/{instanceId}/rewind{queryString}");
        }

        public Task<DurableAzureFunctionResponse> TerminateInstanceAsync(string instanceId, string reason = null, string code = null, string taskHub = null, string connection = null)
        {
            var queryString = CreateQueryString(new[]
            {
                KeyValuePair.Create("reason", reason),
                KeyValuePair.Create("code", code ?? Code),
                KeyValuePair.Create("taskHub", taskHub),
                KeyValuePair.Create("connection", connection)
            });
            
            return SendRequest(HttpMethod.Post, $"{BaseUrl}/runtime/webhooks/durabletask/instances/{instanceId}/terminate{queryString}");
        }

        #endregion

        #region Private helpers

        private string CreateQueryString(IEnumerable<KeyValuePair<string, string>> queryParameters)
        {
            return $"?{string.Join("&", queryParameters.Where(x => !string.IsNullOrWhiteSpace(x.Value)).Select(x => $"{x.Key}={x.Value}") )}";
        }

        private async Task<DurableAzureFunctionResponse> SendRequest(HttpMethod method, string requestUri, dynamic content = null)
        {
            var response = await SendHttpRequest(method, requestUri, content);

            return new DurableAzureFunctionResponse(response);
        }

        private async Task<DurableAzureFunctionResponse<T>> SendRequest<T>(HttpMethod method, string requestUri, dynamic content = null)
        {
            var response = await SendHttpRequest(method, requestUri, content);

            return new DurableAzureFunctionResponse<T>(response);
        }

        private async Task<HttpResponseMessage> SendHttpRequest(HttpMethod method, string requestUri, dynamic content = null)
        {
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = new StringContent(content ?? string.Empty, Encoding.UTF8, "application/json")
            };

            using (var httpClient = new HttpClient())
            {
                return await httpClient.SendAsync(request);
            }
        }

        #endregion
    }
}
