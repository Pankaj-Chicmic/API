using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
namespace EasyAPI
{
    public static class TypeFinder
    {
        [AttributeUsage(AttributeTargets.Field)]
        public class DisplayNameAttribute : Attribute
        {
            public string Name { get; }
            public DisplayNameAttribute(string Name)
            {
                this.Name = Name;
            }
        }
        public static string GetDisplayName(this Enum value)
        {
            return value.GetType()
                        .GetMember(value.ToString())[0]
                        .GetCustomAttribute<DisplayNameAttribute>()?
                        .Name ?? value.ToString();
        }
        public static Type FindTypeByName(string typeName)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                string assemblyName = assembly.FullName.ToLowerInvariant();
                if (assemblyName.StartsWith("unity") || assemblyName.StartsWith("system") || assemblyName.StartsWith("mscorlib") || assemblyName.StartsWith("mono."))
                    continue;

                try
                {
                    // Try case-sensitive first
                    var type = assembly.GetType(typeName, false, false);
                    if (type != null) return type;

                    // Try case-insensitive if not found
                    type = assembly.GetTypes().FirstOrDefault(t => t.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase));
                    if (type != null) return type;
                }
                catch (ReflectionTypeLoadException ex)
                {
                    Debug.LogWarning($"Enum Updater: Could not fully load types from assembly {assembly.FullName} while searching for '{typeName}'. Errors: {string.Join(", ", ex.LoaderExceptions.Select(e => e?.Message ?? "N/A"))}");
                    // Check the types that *did* load
                    foreach (var type in ex.Types)
                    {
                        if (type != null && type.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase))
                        {
                            return type;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"Enum Updater: Error searching assembly {assembly.FullName} for '{typeName}': {ex.Message}");
                }
            }
            return null; // Type not found
        }
    }
}