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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

// Inspired by: http://www.ucancode.net/WPF-3D-Article-Tutorial-with-Chart-Graphics-CSharp-Code.htm

namespace paercebal.ProofOfConcepts.Pocs.Viewing3DPoc
{
    /// <summary>
    /// Interaction logic for Viewing3D.xaml
    /// </summary>
    public partial class Viewing3D : UserControl
    {
        TransformMatrix m_transformMatrix = new TransformMatrix();

        public Viewing3D()
        {
            InitializeComponent();
            this.CreateShapes();
        }

        void CreateShapes()
        {
            var point0 = new Point3D(-0.5, 0, 0);
            var point1 = new Point3D(0.5, 0.5, 0.3);
            var point2 = new Point3D(0, 0.5, 0);

            // This creates the mesh, using the points above
            var triangleMesh = new MeshGeometry3D();
            triangleMesh.Positions.Add(point0);
            triangleMesh.Positions.Add(point1);
            triangleMesh.Positions.Add(point2);

            // This decides which triangle face is "front" and which is "back"
            int n0 = 0;
            int n1 = 1;
            int n2 = 2;
            triangleMesh.TriangleIndices.Add(n0);
            triangleMesh.TriangleIndices.Add(n1);
            triangleMesh.TriangleIndices.Add(n2);

            // This creates the "normal" (WHAT?)
            var norm = new Vector3D(0, 0, 1);
            triangleMesh.Normals.Add(norm);
            triangleMesh.Normals.Add(norm);
            triangleMesh.Normals.Add(norm);

            // Here, we create the material to "apply" to our shape
            var frontMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Blue));

            // Here, we merge together the mesh and the material
            var triangleModel = new GeometryModel3D(triangleMesh, frontMaterial);

            //
            triangleModel.Transform = new Transform3DGroup();

            // Here, we add the triangle model into the viewport, so it can be seen and manipulated
            var visualModel = new ModelVisual3D();
            visualModel.Content = triangleModel;
            this.mainViewport.Children.Add(visualModel);
        }

        private void OnViewportMouseUp(object sender, MouseButtonEventArgs e)
        {
            Point pt = e.GetPosition(mainViewport);

            if (e.ChangedButton == MouseButton.Left)
            {
                m_transformMatrix.OnMouseLeftButtonUp();
            }
        }

        private void OnViewportMouseDown(object sender, MouseButtonEventArgs e)
        {
            Point pt = e.GetPosition(mainViewport);

            if (e.ChangedButton == MouseButton.Left)         // rotate or drag 3d model
            {
                m_transformMatrix.OnMouseLeftButtonDown(pt);
            }
        }

        private void OnViewportMouseMove(object sender, MouseEventArgs e)
        {
            Point pt = e.GetPosition(mainViewport);

            if (e.LeftButton == MouseButtonState.Pressed)                // rotate or drag 3d model
            {
                m_transformMatrix.OnMouseMove(pt, mainViewport);

                this.TransformChart();
            }
        }

        // zoom in 3d display
        public void OnKeyDown(object sender, KeyEventArgs args)
        {
            m_transformMatrix.OnKeyDown(args);
            this.TransformChart();
        }

        // this function is used to rotate, drag and zoom the 3d chart
        private void TransformChart()
        {
            foreach(var v in this.mainViewport.Children)
            {
                ModelVisual3D visual3d = (ModelVisual3D)(v);
                if (visual3d.Content == null) continue;
                if (visual3d.Content.Transform == null) continue;
                var group1 = visual3d.Content.Transform as Transform3DGroup;
                if (group1 == null) continue;
                group1.Children.Clear();
                group1.Children.Add(new MatrixTransform3D(m_transformMatrix.m_totalMatrix));
            }
        }
    }
}
