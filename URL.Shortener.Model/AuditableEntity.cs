using Newtonsoft.Json;
using System;

namespace URL.Shortener.Model
{
    public interface IAuditable
    {
        DateTimeOffset? CreatedOn { get; set; }
    }

    public class AuditableEntity : IAuditable
    {
        [JsonIgnore]
        public DateTimeOffset? CreatedOn { get; set; }
    }
}
