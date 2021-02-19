namespace NoblegardenLauncherSharp
{
    public class NoblegardenRequestResponse
    {
        public dynamic data;
        public bool isOK;

        public NoblegardenRequestResponse(bool isOK) {
            this.isOK = isOK;
        }
        public NoblegardenRequestResponse(bool isOK, dynamic data) {
            this.isOK = isOK;
            this.data = data;
        }
    }
}
