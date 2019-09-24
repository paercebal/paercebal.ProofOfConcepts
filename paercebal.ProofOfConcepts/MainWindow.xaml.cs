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

namespace paercebal.ProofOfConcepts
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly List<UIElement> elements = new List<UIElement>();
        UIElement activeElement = null;

        public MainWindow()
        {
            InitializeComponent();


            UIElement InititalizeOneElement<T>(string label)
                where T: UIElement, new()
            {
                var g = new T();
                g.Visibility = Visibility.Collapsed;
                this.GridForExperiments.Children.Add(g);
                this.elements.Add(g);

                Button b = new Button();
                b.Content = label;
                b.Click += (object sender, RoutedEventArgs e) => { this.HideAllContainersButOne(g); };
                this.PanelForButtons.Children.Add(b);

                return g;
            }

            // Add tabs below =========================================================================
            InititalizeOneElement<Pocs.CanvasAndShapesInShapes>("Canvas + Shapes");
            InititalizeOneElement<Pocs.ShapesAndBrushes>("Shapes + Brushes");
            this.activeElement = InititalizeOneElement<Pocs.Viewing3DPoc.Viewing3D>("Viewing 3D");
            // Add tabs above =========================================================================

            this.HideAllContainersButOne(this.activeElement);
        }

        private void HideAllContainersButOne(UIElement activeElement)
        {
            foreach(var element in this.elements)
            {
                element.Visibility = (element == activeElement) ? Visibility.Visible : Visibility.Collapsed;
            }

            this.activeElement = activeElement;
        }

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            var viewing3D = this.activeElement as Pocs.Viewing3DPoc.Viewing3D;

            if(viewing3D != null)
            {
                viewing3D.OnKeyDown(sender, e);
            }
        }
    }
}
