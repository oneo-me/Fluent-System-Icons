using FluentSystemIcons;

namespace FluentSystemIconsDemo
{
    public class IconInfo
    {
        public FluentIconKey Key { get; }
        public string Name { get; }

        public IconInfo(FluentIconKey key)
        {
            Key = key;
            Name = FluentIconInfoAttribute.GetCategory(key) ?? "None";
        }
    }
}