using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace DurableAzureFunctions.Samples
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IDurableAzureFunctionService service = new DurableAzureFunctionService
            {
                BaseUrl = "{Insert base URL here}",
                Code = "{Insert code here}"
            };

            var response = await service.GetAllInstancesStatusAsync();
            if (response.OriginalHttpResponse.IsSuccessStatusCode)
            {
                var instances = await response.Data;
                foreach (var instance in instances)
                {
                    Console.WriteLine(JsonConvert.SerializeObject(instance));
                }
            }
            else
            {
                Console.WriteLine("Response did not indicate success.");
            }

            Console.ReadKey();
        }
    }
}
