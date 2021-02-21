namespace NoblegardenLauncherSharp.Models
{
    public class NobleRequestModel
    {
        protected ServerModel TargetServer;
        public NobleRequestModel(ServerModel server) {
            TargetServer = server;
        }
    }
}
