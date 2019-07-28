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

        static private Brush GetBrushFromImage(int size, string filename)
        {
            var id = string.Format("{0}|{1}", filename, size);

            if (brushes.TryGetValue(id, out Brush brush))
            {
                return brush;
            }

            var bi = new BitmapImage();
            // BitmapImage.UriSource must be in a BeginInit/EndInit block.
            bi.BeginInit();
            bi.UriSource = new Uri("pack://siteoforigin:,,," + filename, UriKind.RelativeOrAbsolute);
            bi.DecodePixelWidth = size;
            bi.EndInit();

            var ib = new ImageBrush();
            ib.ImageSource = bi;
            ib.TileMode = TileMode.Tile;
            ib.Stretch = Stretch.None;
            //ib.Viewbox = new Rect(0, 0, 200, 200);
            //ib.ViewboxUnits = BrushMappingMode.Absolute;
            ib.Viewport = new Rect(0, 0, size, size);
            ib.ViewportUnits = BrushMappingMode.Absolute;
            brushes[id] = ib;

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

            CreateRectangle(this.MyCanvas, ref zIndex, 100, 25, 100, 100, Brushes.AliceBlue);
            CreateRectangle(this.MyCanvas, ref zIndex, 200, 25, 400, 100, Brushes.Aqua);
            CreateRectangle(this.MyCanvas, ref zIndex, 100, 125, 200, 200, Brushes.LightGreen);

            CreateRectangle(this.MyCanvas, ref zIndex, 100, 25, 100, 100, GetBrushFromImage(50, @"/media/tiles/tile_gradient_red_border.png"));
            CreateRectangle(this.MyCanvas, ref zIndex, 200, 25, 400, 100, GetBrushFromImage(25, @"/media/tiles/tile_gradient_green_border.png"));
            CreateRectangle(this.MyCanvas, ref zIndex, 100, 125, 200, 200, GetBrushFromImage(50, @"/media/tiles/tile_gradient_blue_border.png"));
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
