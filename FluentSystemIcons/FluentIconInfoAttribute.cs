using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media;

namespace FluentSystemIcons
{
    public class FluentIconInfoAttribute : Attribute
    {
        static readonly Type type = typeof(FluentIconInfoAttribute);
        static readonly Type infoType = typeof(FluentIconKey);
        static readonly Dictionary<FluentIconKey, FieldInfo> infos = new Dictionary<FluentIconKey, FieldInfo>();

        static FieldInfo GetField(FluentIconKey iconKey)
        {
            if (!infos.ContainsKey(iconKey))
                infos[iconKey] = infoType.GetField($"{iconKey}");
            return infos[iconKey];
        }

        readonly string path;
        readonly string name;

        public FluentIconInfoAttribute(string name, string path)
        {
            this.name = name;
            this.path = path;
        }

        static readonly Dictionary<FluentIconKey, Geometry> geometries = new Dictionary<FluentIconKey, Geometry>();

        public static string GetCategory(FluentIconKey iconKey)
        {
            if (iconKey == FluentIconKey.None)
                return null;

            if (!(GetCustomAttribute(GetField(iconKey), type) is FluentIconInfoAttribute pathAttribute))
                return null;

            return pathAttribute.name;
        }

        public static Geometry GetPathData(FluentIconKey iconKey)
        {
            if (iconKey == FluentIconKey.None)
                return null;

            if (geometries.ContainsKey(iconKey))
                return geometries[iconKey];

            if (!(GetCustomAttribute(GetField(iconKey), type) is FluentIconInfoAttribute pathAttribute))
                return null;

            geometries[iconKey] = Geometry.Parse(pathAttribute.path);

            return geometries[iconKey];
        }
    }
}