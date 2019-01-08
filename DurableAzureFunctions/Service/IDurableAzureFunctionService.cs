using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DurableAzureFunctions
{
    public interface IDurableAzureFunctionService
    {
        Task<DurableAzureFunctionResponse<IEnumerable<InstanceStatus>>> GetAllInstancesStatusAsync(string code = null, string taskHub = null, string connection = null, DateTime? createdTimeFrom = null, DateTime? createdTimeTo = null, IEnumerable<OrchestrationRuntimeStatus> runtimeStatus = null, bool? showInput = null, bool? showHistory = null, bool? showHistoryOutput = null, int? top = null);
        Task<DurableAzureFunctionResponse<InstanceStatus>> GetInstanceStatusAsync(string instanceId, string code = null, string taskHub = null, string connection = null, bool? showHistory = null, bool? showHistoryOutput = null);
        Task<DurableAzureFunctionResponse> RaiseEventAsync(string instanceId, string eventName, dynamic eventContent, string code = null, string taskHub = null, string connection = null);
        Task<DurableAzureFunctionResponse> RewindInstanceAsync(string instanceId, string reason = null, string code = null, string taskHub = null, string connection = null);
        Task<DurableAzureFunctionResponse> TerminateInstanceAsync(string instanceId, string reason = null, string code = null, string taskHub = null, string connection = null);
    }
}