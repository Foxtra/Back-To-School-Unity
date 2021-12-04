using System;
using System.Collections.Concurrent;


namespace Assets.BackToSchool.Scripts.Extensions
{
    public static class EnumExtensions
    {
        private static readonly ConcurrentDictionary<Enum, string> s_cache = new ConcurrentDictionary<Enum, string>();

        public static string ToStringCached(this Enum value) => s_cache.GetOrAdd(value, v => v.ToString());
    }
}