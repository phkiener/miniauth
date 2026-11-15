using System.Text.Json.Serialization;

namespace MiniAuth.PAM.Proto.Internal;

[JsonSourceGenerationOptions(WriteIndented = false)]
[JsonSerializable(typeof(AuthenticateRequest.Command))]
[JsonSerializable(typeof(AuthenticateResponse.Response))]
internal partial class SerializerContext : JsonSerializerContext;
