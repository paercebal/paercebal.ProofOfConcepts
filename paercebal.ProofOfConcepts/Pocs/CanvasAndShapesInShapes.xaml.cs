using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace paercebal.ProofOfConcepts.Pocs
{
    /// <summary>
    /// Interaction logic for CanvasAndShapesInShapes.xaml
    /// </summary>
    public partial class CanvasAndShapesInShapes : UserControl
    {
        readonly Random random = new Random();

        public CanvasAndShapesInShapes()
        {
            InitializeComponent();
        }

        private void DoActionButton_Click(object sender, RoutedEventArgs e)
        {
            this.MyCanvas.Children.Clear();
            int zIndex = 0;

            void SetEventsOnShape(Shape shape)
            {
                shape.MouseDown += (object sender1, MouseButtonEventArgs e1) => { this.OnShapeMouseDown(shape, e1); };
                shape.MouseUp += (object sender1, MouseButtonEventArgs e1) => { this.OnShapeMouseUp(shape, e1); };
                shape.MouseMove += (object sender1, MouseEventArgs e1) => { this.OnShapeMouseMove(shape, e1); };
                shape.MouseLeave += (object sender1, MouseEventArgs e1) => { this.OnShapeMouseLeave(shape, e1); }; ;
            }

            Shape CreateRectangle(Canvas canvas, ref int zIndex_, double left, double top, double width, double height, Brush brush)
            {
                var shape = new Rectangle();
                shape.Width = width;
                shape.Height = height;
                shape.Fill = brush;
                Canvas.SetLeft(shape, left);
                Canvas.SetTop(shape, top);
                Canvas.SetZIndex(shape, zIndex_++);
                SetEventsOnShape(shape);
                canvas.Children.Add(shape);

                return shape;
            }

            Shape CreateCircle(Canvas canvas, ref int zIndex_, double left, double top, double radius, Brush brush)
            {
                var shape = new Ellipse();
                shape.Width = radius;
                shape.Height = radius;
                shape.Fill = brush;
                Canvas.SetLeft(shape, left);
                Canvas.SetTop(shape, top);
                Canvas.SetZIndex(shape, zIndex_++);
                SetEventsOnShape(shape);
                canvas.Children.Add(shape);

                return shape;
            }

            Shape CreateDiamond(Canvas canvas, ref int zIndex_, double left, double top, double width, double height, Brush brush)
            {
                var shape = new Polygon();
                shape.Points.Add(new Point(width / 2, 0));
                shape.Points.Add(new Point(width, height / 2));
                shape.Points.Add(new Point(width / 2, height));
                shape.Points.Add(new Point(0, height / 2));
                shape.Fill = brush;
                Canvas.SetLeft(shape, left);
                Canvas.SetTop(shape, top);
                Canvas.SetZIndex(shape, zIndex_++);
                SetEventsOnShape(shape);
                canvas.Children.Add(shape);

                return shape;
            }

            Shape CreateRandomPolygon(Canvas canvas, ref int zIndex_, double left, double top, double width, double height, Brush brush)
            {
                Point nextRandomPoint()
                {
                    return new Point(random.Next((int)width), random.Next((int)height));
                }

                Polygon createRandomPolygonSkeleton()
                {
                    var shape_ = new Polygon();
                    for(int i = 0, iMax = random.Next(4, 9); i < iMax; ++i)
                    {
                        shape_.Points.Add(nextRandomPoint());
                    }

                    return shape_;
                }

                var shape = createRandomPolygonSkeleton();
                shape.Fill = brush;
                Canvas.SetLeft(shape, left);
                Canvas.SetTop(shape, top);
                Canvas.SetZIndex(shape, zIndex_++);
                SetEventsOnShape(shape);
                canvas.Children.Add(shape);

                return shape;
            }

            CreateRectangle(this.MyCanvas, ref zIndex, 50, 100, 100, 100, Brushes.Aquamarine);
            CreateRectangle(this.MyCanvas, ref zIndex, 75, 75, 75, 150, Brushes.LightSalmon);
            CreateCircle(this.MyCanvas, ref zIndex, 150, 150, 50, Brushes.Cyan);
            CreateDiamond(this.MyCanvas, ref zIndex, 300, 100, 50, 200, Brushes.MediumPurple);
            CreateRandomPolygon(this.MyCanvas, ref zIndex, 300, 50, 250, 250, Brushes.Red);
        }

        private void SetDebugText(string text)
        {
            this.DebugTextBox.Text = text;
        }

        private void SetDebugText(string format, object o)
        {
            this.DebugTextBox.Text = string.Format(format, o);
        }

        private void SetDebugText(string format, params object[] o)
        {
            this.DebugTextBox.Text = string.Format(format, o);
        }

        private void PrintDebugInfo(Point mousePosition, Point shapePosition)
        {
            this.SetDebugText("Mouse: {0} x {1}\nShape: {2} x {3}\nOld {4} x {5}: ", mousePosition.X, mousePosition.Y, shapePosition.X, shapePosition.Y, this.OldPosition.X, this.OldPosition.Y);
        }

        private Shape MouseDraggingShape = null;
        private int MouseDraggingShapeOldZIndex = 0;
        private Point OldPosition = new Point(double.NaN, double.NaN);

        private void StartShapeDragging(Shape shape, Point mousePosition)
        {
            if (this.MouseDraggingShape == null)
            {
                this.MouseDraggingShape = shape;
                this.OldPosition = mousePosition;
                this.MouseDraggingShapeOldZIndex = Canvas.GetZIndex(shape);
                Canvas.SetZIndex(shape, 1000);
            }
        }

        private void StopShapeDragging(Shape shape, Point mousePosition)
        {
            if (this.MouseDraggingShape != null)
            {
                Canvas.SetZIndex(this.MouseDraggingShape, this.MouseDraggingShapeOldZIndex);
                this.MouseDraggingShape.Opacity = 1;
            }

            this.MouseDraggingShape = null;
            this.OldPosition = new Point(double.NaN, double.NaN);
            this.MouseDraggingShapeOldZIndex = 0;
        }

        private void ContinueShapeDragging(Shape shape, Point mousePosition)
        {
            var shapePosition = (shape != null) ? new Point(Canvas.GetLeft(shape), Canvas.GetTop(shape)) : new Point(double.NaN, double.NaN);
            this.PrintDebugInfo(mousePosition, shapePosition);

            if (shape != null && this.MouseDraggingShape == shape)
            {
                var move = mousePosition - this.OldPosition;

                if((Math.Abs(move.X) > this.GridSnappingValue) || (Math.Abs(move.Y) > this.GridSnappingValue))
                {
                    var newPosition = new Point(Canvas.GetLeft(shape), Canvas.GetTop(shape));
                    newPosition += move;
                    Canvas.SetLeft(shape, newPosition.X);
                    Canvas.SetTop(shape, newPosition.Y);
                    this.OldPosition = mousePosition;
                    shape.Opacity = 0.5;
                }
            }
        }

        private void OnShapeMouseDown(Shape shape, MouseButtonEventArgs e)
        {
            this.StartShapeDragging(shape, e.GetPosition(this.MyCanvas));
        }

        private void OnShapeMouseUp(Shape shape, MouseButtonEventArgs e)
        {
            this.StopShapeDragging(shape, e.GetPosition(this.MyCanvas));
        }

        private void OnShapeMouseMove(Shape shape, MouseEventArgs e)
        {
            this.ContinueShapeDragging(shape, e.GetPosition(this.MyCanvas));
        }

        private void OnShapeMouseLeave(Shape shape, MouseEventArgs e)
        {
            this.ContinueShapeDragging(shape, e.GetPosition(this.MyCanvas));
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            this.ContinueShapeDragging(this.MouseDraggingShape, e.GetPosition(this.MyCanvas));
        }

        private void Canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            this.StopShapeDragging(this.MouseDraggingShape, e.GetPosition(this.MyCanvas));
        }

        private double GridSnappingValuePrivate = 1;
        private double GridSnappingValue
        {
            get
            {
                if (double.IsNaN(this.GridSnappingValuePrivate)
                    || double.IsInfinity(this.GridSnappingValuePrivate)
                    || (this.GridSnappingValuePrivate < 1))
                {
                    return 1;
                }

                return this.GridSnappingValuePrivate;
            }
            set
            {
                this.GridSnappingValuePrivate = value;
            }
        }

        private void UpdateGridSnappingValueFromTextBox()
        {
            if (double.TryParse(this.GridSnappingTextBox.Text, out double result))
            {
                this.GridSnappingValue = result;
            }
        }

        private void GridSnappingTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            this.UpdateGridSnappingValueFromTextBox();
        }

        private void GridSnappingTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            this.UpdateGridSnappingValueFromTextBox();
        }
    }
}

