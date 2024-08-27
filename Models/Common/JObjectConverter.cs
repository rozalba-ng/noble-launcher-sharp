using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NobleLauncher.Models
{
    class JObjectConverter
    {
        public static List<JToken> ConvertToTokenList(JObject Target) {
            var convertedList = new List<JToken>();

            int tokensCount = Target.Count;

            if (Target.Count == 0)
                return convertedList;

            int appendedTokensCount = 1;
            convertedList.Add(Target.First);

            while (appendedTokensCount < tokensCount) {
                convertedList.Add(convertedList[appendedTokensCount - 1].Next);
                appendedTokensCount++;
            }

            return convertedList;
        }

        private static PatchModel ConvertTokenToPatch(JToken token) {
            PatchModel patch = new PatchModel();
            var reader = new JTokenReader(token);
            bool isObjectStarted = false;

            while (reader.Read()) {
                var tokenType = reader.TokenType;

                if (tokenType == JsonToken.StartObject) {
                    isObjectStarted = true;
                }

                if (tokenType == JsonToken.PropertyName) {
                    string property = (string)reader.Value;

                    if (isObjectStarted) {
                        switch (property) {
                            case "path":
                                patch.RemotePath = reader.ReadAsString();
                                break;
                            case "crc32-hash":
                                patch.RemoteHash = reader.ReadAsString();
                                break;
                            case "description":
                                patch.Description = reader.ReadAsString();
                                break;
                            default:
                                break;
                        }
                    }
                    else {
                        patch.LocalPath = property;
                    }
                }
            }
            patch.CalcNameFromPath();
            return patch;
        }

        private static FileModel ConvertTokenToClientFile(JToken token)
        {
            FileModel file = new FileModel("");
            var reader = new JTokenReader(token);
            bool isObjectStarted = false;

            while (reader.Read())
            {
                var tokenType = reader.TokenType;

                if (tokenType == JsonToken.StartObject)
                {
                    isObjectStarted = true;
                }

                if (tokenType == JsonToken.PropertyName)
                {
                    string property = (string)reader.Value;

                    if (isObjectStarted)
                    {
                        switch (property)
                        {
                            case "localpath":
                                file.SetRelativePath(reader.ReadAsString());
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            return file;
        }

        public static List<NecessaryPatchModel> ConvertToNecessaryPatchesList(JObject Target) {
            var tokens = ConvertToTokenList(Target);
            var patches = new NecessaryPatchModel[tokens.Count];

            Parallel.For(0, tokens.Count, (i) => {
                var patch = ConvertTokenToPatch(tokens[i]);
                patch.Index = i;
                var patchAsNecessaryPatch = patch.ToNecessaryPatch();
                patches[i] = patchAsNecessaryPatch;
            });

            return new List<NecessaryPatchModel>(patches);
        }

        public static List<PatchModel> ConvertToPatchesList(JObject Target)
        {
            var tokens = ConvertToTokenList(Target);
            var patches = new PatchModel[tokens.Count];

            Parallel.For(0, tokens.Count, (i) => {
                patches[i] = ConvertTokenToPatch(tokens[i]);
                patches[i].Index = i;
            });

            return new List<PatchModel>(patches);
        }


        public static List<CustomPatchModel> ConvertToCustomPatchesList(JObject Target) {
            var tokens = ConvertToTokenList(Target);
            var patches = new CustomPatchModel[tokens.Count];

            Parallel.For(0, tokens.Count, (i) => {
                var patch = ConvertTokenToPatch(tokens[i]);
                patch.Index = i;
                var customPatch = patch.ToCustomPatchModel();
                patches[i] = customPatch;
            });

            return new List<CustomPatchModel>(patches);
        }

        public static List<FileModel> ConvertToClientFileList(JObject Target)
        {
            var tokens = ConvertToTokenList(Target);
            var files = new FileModel[tokens.Count];

            Parallel.For(0, tokens.Count, (i) => {
                FileModel file = ConvertTokenToClientFile(tokens[i]);
                files[i] = file;
            });

            return new List<FileModel>(files);
        }
    }
}
