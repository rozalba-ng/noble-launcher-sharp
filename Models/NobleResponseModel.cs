using Newtonsoft.Json.Linq;

namespace NoblegardenLauncherSharp.Models
{
    public class NobleResponseModel
    {
        private readonly string Data;
        public bool IsOK;
        public string Err;

        public NobleResponseModel(bool isOK, string err) {
            IsOK = isOK;
            Err = err;
        }
        public NobleResponseModel(string data) {
            IsOK = true;
            Data = data;
        }
        public NobleResponseModel(string data, bool isOK, string err) {
            IsOK = isOK;
            Data = data;
            Err = err;
        }

        public dynamic GetFormattedData() {
            if (Data[0] == '{')
                return JObject.Parse(Data);
            else if (Data[0] == '[')
                return JArray.Parse(Data);

            return Data;
        }
    }
}
