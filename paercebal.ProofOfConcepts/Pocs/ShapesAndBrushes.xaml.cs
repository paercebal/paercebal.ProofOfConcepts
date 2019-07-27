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
    /// Interaction logic for ShapesAndBrushes.xaml
    /// </summary>
    public partial class ShapesAndBrushes : UserControl
    {
        static readonly Dictionary<string, Brush> brushes = new Dictionary<string, Brush>();

        static private Brush GetBrushFromImage(string filename)
        {
            if (brushes.TryGetValue(filename, out Brush brush))
            {
                return brush;
            }

            var bi = new BitmapImage();
            // BitmapImage.UriSource must be in a BeginInit/EndInit block.
            bi.BeginInit();
            bi.UriSource = new Uri("pack://siteoforigin:,,," + filename, UriKind.RelativeOrAbsolute);
            bi.EndInit();

            var ib = new ImageBrush();
            ib.ImageSource = bi;
            ib.Viewbox = new Rect(0, 0, 0.25, 0.25);
            ib.Stretch = Stretch.None;
            //ib.ViewboxUnits = BrushMappingMode.Absolute;
            brushes[filename] = ib;

            return ib;
        }

        public ShapesAndBrushes()
        {
            InitializeComponent();
        }

        private void DoActionButton_Click(object sender, RoutedEventArgs e)
        {
            this.MyCanvas.Children.Clear();
            int zIndex = 0;

            Shape CreateRectangle(Canvas canvas, ref int zIndex_, double left, double top, double width, double height, Brush brush)
            {
                var shape = new Rectangle();
                shape.Width = width;
                shape.Height = height;
                shape.Fill = brush;
                Canvas.SetLeft(shape, left);
                Canvas.SetTop(shape, top);
                Canvas.SetZIndex(shape, zIndex_++);
                canvas.Children.Add(shape);

                return shape;
            }

            CreateRectangle(this.MyCanvas, ref zIndex, 50, 50, 100, 100, GetBrushFromImage(@"/media/tiles/tile_gray_gradient.png"));
            CreateRectangle(this.MyCanvas, ref zIndex, 200, 50, 200, 100, GetBrushFromImage(@"/media/tiles/tile_gray_gradient.png"));
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

    }
}
