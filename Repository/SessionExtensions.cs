﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
namespace Fruit_N12.Repository
{
    public static class SessionExtensions
    {
        public static void SetJson(this ISession session, string key, object value) {
            session.SetString(key, JsonConvert.SerializeObject(value));

        }
        public static T GetJson<T>(this ISession session, string key)
        {
            var sessionData = session.GetString(key);
            return sessionData == null ? default(T) : JsonConvert.DeserializeObject<T>(sessionData);
        }
    }
}
