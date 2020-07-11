using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FluentSystemIconsGenerator
{
    static class Program
    {
        static void Main()
        {
            var solution = Path.GetFullPath("../../../../");
            var assets = Path.Combine(solution, "Assets/assets");

            var icons = new List<IconInfo>();

            Debug.WriteLine("扫描图标");

            foreach (var dir in Directory.EnumerateDirectories(assets))
            {
                var name = Path.GetFileName(dir);
                foreach (var svg in Directory.EnumerateFiles(Path.Combine(dir, "SVG"), "*.svg"))
                {
                    var key = Path.GetFileNameWithoutExtension(svg);
                    var csKey = ToUpperKey(key);
                    var svgPath = LoadSvg2Path(svg);
                    icons.Add(new IconInfo
                    {
                        Name = name,
                        Key = csKey,
                        Path = svgPath.Replace("/", "\\")
                    });
                }
            }

            Debug.WriteLine($"发现图标 {icons.GroupBy(x => x.Name).Count()} 个");

            Debug.WriteLine("开始转换");
            SaveIcons2File(icons, Path.Combine(solution, "FluentSystemIcons/FluentIconKey.cs"));

            Debug.WriteLine("完成");
        }

        static void SaveIcons2File(List<IconInfo> icons, string file)
        {
            var sb = new StringBuilder();
            sb.AppendLine("using System.Diagnostics.CodeAnalysis;");
            sb.AppendLine();
            sb.AppendLine("namespace FluentSystemIcons");
            sb.AppendLine("{");
            sb.AppendLine("    [SuppressMessage(\"ReSharper\", \"UnusedMember.Global\")]");
            sb.AppendLine("    [SuppressMessage(\"ReSharper\", \"IdentifierTypo\")]");
            sb.AppendLine("    [SuppressMessage(\"ReSharper\", \"InconsistentNaming\")]");
            sb.AppendLine("    public enum FluentIconKey");
            sb.AppendLine("    {");
            sb.AppendLine("        None,");

            for (var i = 0; i < icons.Count; i++)
            {
                var icon = icons[i];

                sb.AppendLine($"        [FluentIconInfo(\"{icon.Name}\", \"{ReadSvg2Path(icon.Path)}\")]");
                sb.AppendLine($"        {icon.Key},");

                if (i < icons.Count - 1)
                    sb.AppendLine();
            }

            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();
            File.WriteAllText(file, sb.ToString(), Encoding.UTF8);
        }

        static readonly Regex regex = new Regex(@"<path d=""(.*[zZ])"" ");

        static string ReadSvg2Path(string file)
        {
            var sb = new StringBuilder();
            var svgStr = File.ReadAllText(file);
            var matches = regex.Matches(svgStr);
            for (var index = 0; index < matches.Count; index++)
            {
                var match = matches[index];
                sb.Append(match.Groups[1].Value);
                if (index < matches.Count - 1)
                    sb.Append(" ");
            }

            return sb.ToString();
        }

        static string ToUpperKey(string key)
        {
            var sb = new StringBuilder();

            var strings = key.Split('_');
            for (var i = 0; i < strings.Length; i++)
            {
                var str = strings[i];

                if (string.IsNullOrWhiteSpace(str))
                    continue;

                // 跳过 ic_fluent
                if (i <= 1)
                    continue;

                // 分割 大小 & 类型
                if (i >= strings.Length - 2)
                    sb.Append("_");

                sb.Append(str.Substring(0, 1).ToUpper());
                sb.Append(str.Substring(1));
            }

            return sb.ToString();
        }

        static string LoadSvg2Path(string svg)
        {
            return svg;
        }
    }
}