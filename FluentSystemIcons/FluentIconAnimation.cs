using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace FluentSystemIcons
{
    public class FluentIconAnimation : Shape
    {
        static FluentIconAnimation()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FluentIconAnimation), new FrameworkPropertyMetadata(typeof(FluentIconAnimation)));
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Key), typeof(FluentIconKey), typeof(FluentIconAnimation), new FrameworkPropertyMetadata(FluentIconKey.None, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnIconChanged));

        public FluentIconKey Key
        {
            get => (FluentIconKey)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var animationPath = (FluentIconAnimation)d;
            animationPath.data = FluentIconInfoAttribute.GetPathData(animationPath.Key) ?? Geometry.Empty;
            animationPath.UpdatePath();
        }

        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register(nameof(Duration), typeof(int), typeof(FluentIconAnimation), new FrameworkPropertyMetadata(1000));

        public int Duration
        {
            get => (int)GetValue(DurationProperty);
            set => SetValue(DurationProperty, value);
        }

        protected override Geometry DefiningGeometry
        {
            get => data;
        }

        Geometry data = Geometry.Empty;
        Storyboard storyboard;

        public FluentIconAnimation()
        {
            Loaded += OnLoaded;
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            UpdatePath();
        }

        void UpdatePath()
        {
            if (!IsLoaded)
                return;

            storyboard?.Stop();

            if (data == Geometry.Empty)
                return;

            var pathLength = GetGeometryLength(data, ActualWidth, ActualHeight, StrokeThickness);

            StrokeDashOffset = pathLength;
            StrokeDashArray = new DoubleCollection { pathLength };

            var duration = Duration / 360d * pathLength;

            storyboard = new Storyboard();
            var doubleAnimation = new DoubleAnimation(pathLength, 0, new Duration(TimeSpan.FromMilliseconds(duration)));
            Storyboard.SetTarget(doubleAnimation, this);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(StrokeDashOffsetProperty));
            storyboard.Children.Add(doubleAnimation);
            storyboard.Begin();
        }

        static double GetGeometryLength(Geometry geometry, double width, double height, double strokeThickness)
        {
            var flattenedPathGeometry = geometry.GetFlattenedPathGeometry();
            if (flattenedPathGeometry.Figures.Count == 0)
                return 0;
            var maxFiguresLength = flattenedPathGeometry.Figures.Max(GetFigureLength);
            var sw = geometry.Bounds.Width / width;
            var sh = geometry.Bounds.Height / height;
            var min = Math.Min(sw, sh);
            return maxFiguresLength / min / strokeThickness;
        }

        static double GetFigureLength(PathFigure pathFigure)
        {
            var length = 0.0;
            var start = pathFigure.StartPoint;

            foreach (var pathSegment in pathFigure.Segments)
                switch (pathSegment)
                {
                    case LineSegment lineSegment:
                        length += GetPointDistance(start, lineSegment.Point);
                        start = lineSegment.Point;
                        break;

                    case PolyLineSegment polyLineSegment:
                        foreach (var point in polyLineSegment.Points)
                        {
                            length += GetPointDistance(start, point);
                            start = point;
                        }

                        break;

                    default:
                        throw new Exception();
                }

            return length;
        }

        static double GetPointDistance(Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }
    }
}