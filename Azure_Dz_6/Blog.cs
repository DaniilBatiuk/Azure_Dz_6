using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Azure_Dz_6
{
    public class Blog
    {
        [JsonProperty("id")]
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public BestComment BestComment { get; set; } = default!;

        public override string ToString()
        {
            JsonSerializerOptions options = new()
            {
                WriteIndented = true,
            };
            return System.Text.Json.JsonSerializer.Serialize(this, options);

        }


    }

    public class BestComment
    {
        public string Text { get; set; } = default!;
    }
}
