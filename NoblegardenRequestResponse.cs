using Newtonsoft.Json.Linq;

namespace NoblegardenLauncherSharp
{
    public class NoblegardenRequestResponse
    {
        public string data;
        public bool isOK;
        public string err;

        public NoblegardenRequestResponse(bool isOK, string err) {
            this.isOK = isOK;
            this.err = err;
        }
        public NoblegardenRequestResponse(string data) {
            isOK = true;
            this.data = data;
        }
        public NoblegardenRequestResponse(string data, bool isOK, string err) {
            this.isOK = isOK;
            this.data = data;
            this.err = err;
        }

        public dynamic getDataAsJSON() {
            if (data[0] == '{')
                return JObject.Parse(data);
            else if (data[0] == '[')
                return JArray.Parse(data);

            return data;
        }
    }
}
