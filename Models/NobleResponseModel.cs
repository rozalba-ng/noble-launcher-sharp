using Newtonsoft.Json.Linq;

namespace NoblegardenLauncherSharp.Models
{
    public class NobleResponseModel
    {
        private readonly string data;
        public bool isOK;
        public string err;

        public NobleResponseModel(bool isOK, string err) {
            this.isOK = isOK;
            this.err = err;
        }
        public NobleResponseModel(string data) {
            isOK = true;
            this.data = data;
        }
        public NobleResponseModel(string data, bool isOK, string err) {
            this.isOK = isOK;
            this.data = data;
            this.err = err;
        }

        public dynamic GetFormattedData() {
            if (data[0] == '{')
                return JObject.Parse(data);
            else if (data[0] == '[')
                return JArray.Parse(data);

            return data;
        }
    }
}
