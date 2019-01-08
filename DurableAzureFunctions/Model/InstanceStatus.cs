using System;

namespace DurableAzureFunctions
{
    public class InstanceStatus
    {
        public DateTime CreatedTime { get; set; }
        public dynamic CustomStatus { get; set; }
        public dynamic HistoryEvents { get; set; }
        public dynamic Input { get; set; }
        public string Instance { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public dynamic Output { get; set; }
        public OrchestrationRuntimeStatus RuntimeStatus { get; set; }
    }
}
