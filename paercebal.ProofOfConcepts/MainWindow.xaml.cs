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

            InititalizeOneElement<Pocs.CanvasAndShapesInShapes>("Canvas + Shapes");
            var visible = InititalizeOneElement<Pocs.ShapesAndBrushes>("Shapes + Brushes");

            this.HideAllContainersButOne(visible);
        }

        private void HideAllContainersButOne(UIElement activeElement)
        {
            foreach(var element in this.elements)
            {
                element.Visibility = (element == activeElement) ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}
