using Newtonsoft.Json.Linq;

namespace NobleLauncher.Structures
{
    public struct NobleResponse
    {
        private readonly string Data;
        public bool IsOK;
        public string Err;
        public NobleResponse(bool isOK, string err) {
            IsOK = isOK;
            Err = err;
            Data = null;
        }
        public NobleResponse(string data) {
            IsOK = true;
            Err = null;
            Data = data;
        }
        public NobleResponse(string data, bool isOK, string err) {
            IsOK = isOK;
            Data = data;
            Err = err;
        }

        public dynamic FormattedData {
            get {
                if (Data[0] == '{')
                    return JObject.Parse(Data);
                else if (Data[0] == '[')
                    return JArray.Parse(Data);

                return Data;
            }
        }
    }
}
