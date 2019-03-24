namespace EventBus.Events
{
    using System;
    using Newtonsoft.Json;

    public abstract class IntegrationEvent
    {
        public IntegrationEvent() :
            this(Guid.Empty, Guid.NewGuid(), DateTime.Now, null)
        {
        }

        public IntegrationEvent(string description) :
            this(Guid.Empty, Guid.NewGuid(), DateTime.Now, description)
        {
        }

        public IntegrationEvent(Guid parentId) :
            this(parentId, Guid.NewGuid(), DateTime.Now, null)
        {
        }

        public IntegrationEvent(Guid parentId, string description) :
            this(parentId, Guid.NewGuid(), DateTime.Now, description)
        {
        }

        [JsonConstructor]
        public IntegrationEvent(Guid parentId, Guid id, DateTime createDate, string description)
        {
            ParentId = parentId;
            Id = id;
            CreationDate = createDate;
            Description = description;
        }

        [JsonProperty]
        public Guid ParentId { get; private set; }

        [JsonProperty]
        public Guid Id { get; private set; }

        [JsonProperty]
        public DateTime CreationDate { get; private set; }

        [JsonProperty]
        public string Description { get; set; }

        [JsonProperty]
        public string PayLoadVersion { get; set; }

        public override string ToString()
        {
            return ParentId != Guid.Empty ?
                       $"{nameof(ParentId)}: {ParentId}, {nameof(Id)}: {Id}, {nameof(CreationDate)}: {CreationDate}, {nameof(Description)}: {Description}" :
                       $"{nameof(Id)}: {Id}, {nameof(CreationDate)}: {CreationDate}, {nameof(Description)}: {Description}";
        }
    }
}
