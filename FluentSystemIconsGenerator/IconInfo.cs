namespace FluentSystemIconsGenerator
{
    public class IconInfo
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public string Path { get; set; }

        public override string ToString()
        {
            return $"{Name}: {Key} ({Path})";
        }
    }
}