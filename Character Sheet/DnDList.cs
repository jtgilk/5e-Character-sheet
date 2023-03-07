using Newtonsoft.Json;
using System;
using System.Collections;

namespace CharSheet
{

    public class DnDList
    {
        [JsonProperty(PropertyName = "results")]
        public Result[] Results { get; set; }

    }
    public class Result
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "index")]
        public string Index { get; set; }
    }
}
