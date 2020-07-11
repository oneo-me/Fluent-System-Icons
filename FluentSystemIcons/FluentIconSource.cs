using System;
using System.Windows.Markup;
using System.Windows.Media;

namespace FluentSystemIcons
{
    public class FluentIconSource : MarkupExtension
    {
        public FluentIconKey Key { get; set; }
        public Brush Brush { get; set; } = Brushes.Black;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Create(Key, Brush);
        }

        public static ImageSource Create(FluentIconKey iconKey, Brush brush)
        {
            var geometry = FluentIconInfoAttribute.GetPathData(iconKey) ?? Geometry.Empty;
            var visual = new DrawingVisual();

            using (var dc = visual.RenderOpen())
                dc.DrawGeometry(brush, null, geometry);

            return new DrawingImage(visual.Drawing);
        }
    }
}