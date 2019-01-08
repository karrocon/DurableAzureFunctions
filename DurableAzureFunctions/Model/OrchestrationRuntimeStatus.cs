using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DurableAzureFunctions
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OrchestrationRuntimeStatus
    {
        Running = 0,
        Completed = 1,
        ContinuedAsNew = 2,
        Failed = 3,
        Canceled = 4,
        Terminated = 5,
        Pending = 6
    }
}