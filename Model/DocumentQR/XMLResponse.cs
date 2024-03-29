using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DocumentQR
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class PrintLetterBarcodeData
    {
        [JsonProperty("@uid")]
        public string uid { get; set; }

        [JsonProperty("@name")]
        public string name { get; set; }

        [JsonProperty("@gender")]
        public string gender { get; set; }

        [JsonProperty("@yob")]
        public string yob { get; set; }

        [JsonProperty("@co")]
        public string co { get; set; }

        [JsonProperty("@house")]
        public string house { get; set; }

        [JsonProperty("@street")]
        public string street { get; set; }

        [JsonProperty("@lm")]
        public string lm { get; set; }

        [JsonProperty("@loc")]
        public string loc { get; set; }

        [JsonProperty("@vtc")]
        public string vtc { get; set; }

        [JsonProperty("@dist")]
        public string dist { get; set; }

        [JsonProperty("@state")]
        public string state { get; set; }

        [JsonProperty("@pc")]
        public string pc { get; set; }
    }

    public class XMLResponse
    {
        [JsonProperty("?xml")]
        public Xml xml { get; set; }
        public PrintLetterBarcodeData PrintLetterBarcodeData { get; set; }
    }

    public class Xml
    {
        [JsonProperty("@version")]
        public string version { get; set; }

        [JsonProperty("@encoding")]
        public string encoding { get; set; }
    }


}
