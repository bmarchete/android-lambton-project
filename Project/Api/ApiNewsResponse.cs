using System.Collections.Generic;
using Newtonsoft.Json;
using Project.Model;

namespace Project.Api
{
    public class ApiNewsResponse
    {
        [JsonProperty(PropertyName = "totalResults")]
        public string totalResults { get; set; }
        [JsonProperty(PropertyName = "status")]
        public string status { get; set; }
        [JsonProperty(PropertyName = "articles")]
        public List<News> articles { get; set; }
        public override string ToString()
        {
            return totalResults;
        }
    }
}