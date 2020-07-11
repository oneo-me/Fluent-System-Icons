using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FluentSystemIcons
{
    public class FluentIcon : Shape
    {
        static FluentIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FluentIcon), new FrameworkPropertyMetadata(typeof(FluentIcon)));
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Key), typeof(FluentIconKey), typeof(FluentIcon), new FrameworkPropertyMetadata(FluentIconKey.None, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public FluentIconKey Key
        {
            get => (FluentIconKey)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        protected override Geometry DefiningGeometry
        {
            get => FluentIconInfoAttribute.GetPathData(Key) ?? Geometry.Empty;
        }
    }
}