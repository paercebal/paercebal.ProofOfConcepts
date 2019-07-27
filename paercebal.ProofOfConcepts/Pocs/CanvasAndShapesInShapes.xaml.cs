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
        public CanvasAndShapesInShapes()
        {
            InitializeComponent();
        }

        private void DoActionButton_Click(object sender, RoutedEventArgs e)
        {
            this.MyCanvas.Children.Clear();
            int zIndex = 0;

            Shape createRectangle(in Canvas canvas, ref int zIndex_, double left, double top, double width, double height, Brush brush)
            {
                var shape = new Rectangle();
                shape.Width = width;
                shape.Height = height;
                shape.Fill = brush;
                Canvas.SetLeft(shape, left);
                Canvas.SetTop(shape, top);
                Canvas.SetZIndex(shape, zIndex_++);
                shape.MouseDown += (object sender1, MouseButtonEventArgs e1) => { this.OnShapeMouseDown(shape, e1); };
                shape.MouseUp += (object sender1, MouseButtonEventArgs e1) => { this.OnShapeMouseUp(shape, e1); };
                shape.MouseMove += (object sender1, MouseEventArgs e1) => { this.OnShapeMouseMove(shape, e1); };
                shape.MouseLeave += (object sender1, MouseEventArgs e1) => { this.OnShapeMouseLeave(shape, e1); }; ;
                this.MyCanvas.Children.Add(shape);

                return shape;
            }

            Shape createCircle(in Canvas canvas, ref int zIndex_, double left, double top, double radius, Brush brush)
            {
                var shape = new Ellipse();
                shape.Width = radius;
                shape.Height = radius;
                shape.Fill = brush;
                Canvas.SetLeft(shape, left);
                Canvas.SetTop(shape, top);
                Canvas.SetZIndex(shape, zIndex_++);
                shape.MouseDown += (object sender1, MouseButtonEventArgs e1) => { this.OnShapeMouseDown(shape, e1); };
                shape.MouseUp += (object sender1, MouseButtonEventArgs e1) => { this.OnShapeMouseUp(shape, e1); };
                shape.MouseMove += (object sender1, MouseEventArgs e1) => { this.OnShapeMouseMove(shape, e1); };
                shape.MouseLeave += (object sender1, MouseEventArgs e1) => { this.OnShapeMouseLeave(shape, e1); }; ;
                this.MyCanvas.Children.Add(shape);

                return shape;
            }

            Shape createDiamond(in Canvas canvas, ref int zIndex_, double left, double top, double width, double height, Brush brush)
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
                shape.MouseDown += (object sender1, MouseButtonEventArgs e1) => { this.OnShapeMouseDown(shape, e1); };
                shape.MouseUp += (object sender1, MouseButtonEventArgs e1) => { this.OnShapeMouseUp(shape, e1); };
                shape.MouseMove += (object sender1, MouseEventArgs e1) => { this.OnShapeMouseMove(shape, e1); };
                shape.MouseLeave += (object sender1, MouseEventArgs e1) => { this.OnShapeMouseLeave(shape, e1); }; ;
                this.MyCanvas.Children.Add(shape);

                return shape;
            }

            createRectangle(this.MyCanvas, ref zIndex, 50, 100, 100, 100, Brushes.Aquamarine);
            createRectangle(this.MyCanvas, ref zIndex, 75, 75, 75, 150, Brushes.LightSalmon);
            createCircle(this.MyCanvas, ref zIndex, 150, 150, 50, Brushes.Cyan);
            createDiamond(this.MyCanvas, ref zIndex, 300, 100, 50, 200, Brushes.MediumPurple);
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

        private void OnShapeMouseDown(Shape shape, MouseButtonEventArgs e)
        {
            if (this.MouseDraggingShape == null)
            {
                this.MouseDraggingShape = shape;
                this.OldPosition = e.GetPosition(this.MyCanvas);
                this.MouseDraggingShapeOldZIndex = Canvas.GetZIndex(shape);
                Canvas.SetZIndex(shape, 1000);
            }
        }

        private void StopShapeDragging(Shape shape)
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

        private void OnShapeMouseUp(Shape shape, MouseButtonEventArgs e)
        {
            this.StopShapeDragging(shape);
        }

        private void OnShapeMouseMove(Shape shape, MouseEventArgs e)
        {
            this.PrintDebugInfo(e.GetPosition(this.MyCanvas), new Point(Canvas.GetLeft(shape), Canvas.GetTop(shape)));

            if (shape != null && this.MouseDraggingShape == shape)
            {
                var position = new Point(Canvas.GetLeft(shape), Canvas.GetTop(shape));
                position += e.GetPosition(this.MyCanvas) - this.OldPosition;
                Canvas.SetLeft(shape, position.X);
                Canvas.SetTop(shape, position.Y);
                this.OldPosition = e.GetPosition(this.MyCanvas);
                shape.Opacity = 0.5;
            }
        }

        private void OnShapeMouseLeave(Shape shape, MouseEventArgs e)
        {
            this.PrintDebugInfo(e.GetPosition(this.MyCanvas), new Point(Canvas.GetLeft(shape), Canvas.GetTop(shape)));

            if (shape != null && this.MouseDraggingShape == shape)
            {
                var position = new Point(Canvas.GetLeft(shape), Canvas.GetTop(shape));
                position += e.GetPosition(this.MyCanvas) - this.OldPosition;
                Canvas.SetLeft(shape, position.X);
                Canvas.SetTop(shape, position.Y);
                this.OldPosition = e.GetPosition(this.MyCanvas);
                shape.Opacity = 0.5;
            }
        }

        private void Canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            this.StopShapeDragging(this.MouseDraggingShape);
        }
    }
}

