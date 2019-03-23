namespace EventBus.Events
{
    using System;
    using Newtonsoft.Json;

    public abstract class IntegrationEvent
    {
        public IntegrationEvent() : this(Guid.Empty, Guid.NewGuid(), DateTime.UtcNow)
        {
        }

        public IntegrationEvent(Guid parentId) : 
            this(parentId, Guid.NewGuid(), DateTime.UtcNow)
        {
        }

        [JsonConstructor]
        public IntegrationEvent(Guid parentId, Guid id, DateTime createDate)
        {
            ParentId = parentId;
            Id = id;
            CreationDate = createDate;
        }

        [JsonProperty]
        public Guid ParentId { get; private set; }

        [JsonProperty]
        public Guid Id { get; private set; }

        [JsonProperty]
        public DateTime CreationDate { get; set; }

        [JsonProperty]
        public string PayLoadVersion { get; set; }
    }
}