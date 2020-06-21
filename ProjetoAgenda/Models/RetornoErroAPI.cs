using Newtonsoft.Json;
using System;

namespace ProjetoAgenda.Models
{
    public class RetornoErroAPI
    {
        [JsonProperty("type")]
        public Uri Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("detail")]
        public string Detail { get; set; }

        [JsonProperty("instance")]
        public string Instance { get; set; }

        [JsonProperty("traceId")]
        public string TraceId { get; set; }
    }
}
