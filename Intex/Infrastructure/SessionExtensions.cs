using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.Json;


namespace Intex.Infrastructure
{
    public static class SessionExtensions
    {
        public static void SetJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        // Ensure this method has the 'this' keyword to indicate it's an extension method
        public static T? GetJson<T>(this ISession session, string key)
        {
            var sessionData = session.GetString(key);
            return sessionData == null ? default : JsonSerializer.Deserialize<T>(sessionData);
        }
    }

}
