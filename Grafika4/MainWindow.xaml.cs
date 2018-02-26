using Grafika4.Model;
using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Serialization;

namespace Grafika4
{
     /// <summary>
     /// Interaction logic for MainWindow.xaml
     /// </summary>
     public partial class MainWindow : Window
     {
          private NetworkModel networkModel { get; set; }
          private Dictionary<string, CubeVisual3D> allNodes { get; set; }
          private List<Model3D> lines { get; set; }
          
          
          public MainWindow()
          {
               DataContext = new ViewModel();
               InitializeComponent();
               DrawPicture();
               allNodes = new Dictionary<string, CubeVisual3D>();
               lines = new List<Model3D>();
               LoadXml();
               SetupHelix();
               Display();
               LoadLines();
          }

          #region Draw Picture
          private void DrawPicture()
          {
               var nesto = Directory.GetCurrentDirectory();
               var putanja = System.IO.Path.GetFullPath(System.IO.Path.Combine(nesto, @"..\..\Image\RG PSI - slika mape za PZ4.jpg"));

               GeometryModel3D geometryModel3D = new GeometryModel3D();
               MeshGeometry3D meshGeometry3D = new MeshGeometry3D();

               geometryModel3D.Geometry = meshGeometry3D;
               geometryModel3D.Material = new DiffuseMaterial(new ImageBrush(new BitmapImage(new Uri(putanja))));

               Point3DCollection point3DCollection = new Point3DCollection
               {
                    new Point3D(0,0,0),
                    new Point3D(Constants.width, 0,0),
                    new Point3D(0,Constants.height,0),
                    new Point3D(Constants.width, Constants.height, 0)
               };

               Int32Collection triangleIndices = new Int32Collection { 0, 1, 2, 1, 3, 2 };
               PointCollection textureCollection = new PointCollection{new System.Windows.Point(0,1), new
                    System.Windows.Point(1,1), new System.Windows.Point(0,0), new System.Windows.Point(1,0)};

               meshGeometry3D.Positions = point3DCollection;
               meshGeometry3D.TriangleIndices = triangleIndices;
               meshGeometry3D.TextureCoordinates = textureCollection;

               Slika.Children.Add(geometryModel3D);
          } 
          #endregion

          #region Display
          private void Display()
          {
               foreach(var element in allNodes.Values)
               {
                    viewport3d.Children.Add(element);
               }
          } 
          #endregion

          #region SetupHelix
          private void SetupHelix()
          {
               viewport3d.RotateGesture = new MouseGesture(MouseAction.RightClick);
               viewport3d.PanGesture = new MouseGesture(MouseAction.LeftClick);
          }
          #endregion

          #region Load Objects

          #region LoadXML
          private void LoadXml()
          {
               var path = Directory.GetCurrentDirectory();
               path = System.IO.Path.GetFullPath(System.IO.Path.Combine(path, @"..\..\XML\Geographic.xml"));

               using (var reader = new StreamReader(path))
               {
                    var serializer = new XmlSerializer(typeof(NetworkModel));
                    networkModel = (NetworkModel)serializer.Deserialize(reader);
               }

               FixNumbers();
          }
          #endregion

          #region FixNumbers
          private void FixNumbers()
          {
               foreach (var v in networkModel.Substations.SubstationEntity)
               {
                    ToLatLon(v.X, v.Y, out double x, out double y);
                    if (x < Constants.BottomX || x > Constants.UpperX || y < Constants.BottomY || y > Constants.UpperY) continue;

                    else
                    {
                         Model.Point point = Scale(x, y);
                         v.X = point.X;
                         v.Y = point.Y;
                         CreateModel(v.Id, NodeType.Substation, v.X, v.Y);
                         
                    }
               }
               foreach (var v in networkModel.Switches.SwitchEntity)
               {
                    ToLatLon(v.X, v.Y, out double x, out double y);
                    if (x < Constants.BottomX || x > Constants.UpperX || y < Constants.BottomY || y > Constants.UpperY) continue;
                    else
                    {
                         Model.Point point = Scale(x, y);
                         v.X = point.X;
                         v.Y = point.Y;
                         CreateModel(v.Id, NodeType.Switch, v.X, v.Y);

                    }
               }

               foreach (var v in networkModel.Nodes.NodeEntity)
               {
                    ToLatLon(v.X, v.Y, out double x, out double y);
                    if (x < Constants.BottomX || x > Constants.UpperX || y < Constants.BottomY || y > Constants.UpperY) continue;

                    else
                    {
                         Model.Point point = Scale(x, y);
                         v.X = point.X;
                         v.Y = point.Y;
                         CreateModel(v.Id, NodeType.Node, v.X, v.Y);
                    }
               }

          }

          #region Scale
          private Model.Point Scale(double x, double y)
          {
               Model.Point ret = new Model.Point();
               ret.X = Constants.ScaleX * (y - Constants.MinLon);
               ret.Y = Constants.ScaleY * (x - Constants.MinLat);
               return ret;
          } 
          #endregion

          #endregion

          #region ToLatLon
          public static void ToLatLon(double utmX, double utmY, out double latitude, out double longitude)
          {
               bool isNorthHemisphere = true;
               int zoneUTM = 34;
               var diflat = -0.00066286966871111111111111111111111111;
               var diflon = -0.0003868060578;

               var zone = zoneUTM;
               var c_sa = 6378137.000000;
               var c_sb = 6356752.314245;
               var e2 = Math.Pow((Math.Pow(c_sa, 2) - Math.Pow(c_sb, 2)), 0.5) / c_sb;
               var e2cuadrada = Math.Pow(e2, 2);
               var c = Math.Pow(c_sa, 2) / c_sb;
               var x = utmX - 500000;
               var y = isNorthHemisphere ? utmY : utmY - 10000000;

               var s = ((zone * 6.0) - 183.0);
               var lat = y / (c_sa * 0.9996);
               var v = (c / Math.Pow(1 + (e2cuadrada * Math.Pow(Math.Cos(lat), 2)), 0.5)) * 0.9996;
               var a = x / v;
               var a1 = Math.Sin(2 * lat);
               var a2 = a1 * Math.Pow((Math.Cos(lat)), 2);
               var j2 = lat + (a1 / 2.0);
               var j4 = ((3 * j2) + a2) / 4.0;
               var j6 = ((5 * j4) + Math.Pow(a2 * (Math.Cos(lat)), 2)) / 3.0;
               var alfa = (3.0 / 4.0) * e2cuadrada;
               var beta = (5.0 / 3.0) * Math.Pow(alfa, 2);
               var gama = (35.0 / 27.0) * Math.Pow(alfa, 3);
               var bm = 0.9996 * c * (lat - alfa * j2 + beta * j4 - gama * j6);
               var b = (y - bm) / v;
               var epsi = ((e2cuadrada * Math.Pow(a, 2)) / 2.0) * Math.Pow((Math.Cos(lat)), 2);
               var eps = a * (1 - (epsi / 3.0));
               var nab = (b * (1 - epsi)) + lat;
               var senoheps = (Math.Exp(eps) - Math.Exp(-eps)) / 2.0;
               var delt = Math.Atan(senoheps / (Math.Cos(nab)));
               var tao = Math.Atan(Math.Cos(delt) * Math.Tan(nab));

               longitude = ((delt * (180.0 / Math.PI)) + s) + diflon;
               latitude = ((lat + (1 + e2cuadrada * Math.Pow(Math.Cos(lat), 2) - (3.0 / 2.0) * e2cuadrada * Math.Sin(lat) * Math.Cos(lat) * (tao - lat)) * (tao - lat)) * (180.0 / Math.PI)) + diflat;
          } 
          #endregion

          #endregion

          #region Create Model
          private void CreateModel(string id, NodeType type, double x, double y)
          {
               var model = new CubeVisual3D
               {
                    Center = new Point3D(x, y, 0),
                    SideLength = 5
               };
               if (type == NodeType.Substation) model.Material = Materials.Blue;
               if (type == NodeType.Switch) model.Material = Materials.Yellow;
               if (type == NodeType.Node) model.Material = Materials.Red;

               allNodes.Add(id, model);
          }
          #endregion

          #region Checkbox
          private void CheckBox_Checked(object sender, RoutedEventArgs e)
          {
               foreach(var line in lines)
               {
                    Linije.Children.Add(line);
               }
          }

          private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
          {
               foreach (var line in lines)
               {
                    Linije.Children.Remove(line);
               }
          }
          #endregion

          #region Lines
          private void LoadLines()
          {
               foreach (var line in networkModel.Lines.LineEntity)
               {
                    if (!allNodes.ContainsKey(line.FirstEnd) || !allNodes.ContainsKey(line.SecondEnd)) continue;
                    for (int i = 0; i < line.Vertices.Point.Count - 1; i++)
                    {
                         var start = line.Vertices.Point[i];
                         var end = line.Vertices.Point[i + 1];
                         ToLatLon(start.X, start.Y, out double lat1, out double lon1);
                         ToLatLon(end.X, end.Y, out double lat2, out double lon2);
                         lines.Add(DrawLine(lat1, lon1, lat2, lon2));
                    }
               }
          }

          private Model3D DrawLine(double lat1, double lon1, double lat2, double lon2)
          {
               double x1 = Constants.ScaleX * (lon1 - Constants.MinLon);
               double y1 = Constants.ScaleY * (lat1 - Constants.MinLat);
               double x2 = Constants.ScaleX * (lon2 - Constants.MinLon);
               double y2 = Constants.ScaleY * (lat2 - Constants.MinLat);

               double a = Math.Abs(x2 - x1);
               double b = Math.Abs(y2 - y1);
               double c = Math.Sqrt(a * a + b * b);

               double angle = 180 / Math.PI * Math.Acos(a / c);

               angle = x1 > x2 ? (y1 > y2 ? 180 + angle : 180 - angle) : (y1 > y2 ? 360 - angle : 360 + angle);

               GeometryModel3D geometryModel3D = new GeometryModel3D();
               MeshGeometry3D meshGeometry = new MeshGeometry3D();

               geometryModel3D.Geometry = meshGeometry;
               geometryModel3D.Material = new DiffuseMaterial(Brushes.Black);

               double w = 0.05;
               Point3DCollection geometryPositions = new Point3DCollection
                                                  {
                                                     new Point3D(x1, y1 + w, 0),
                                                     new Point3D(x1 + c, y1 + w, 0),
                                                     new Point3D(x1, y1 - w, 0),
                                                     new Point3D(x1 + c, y1 - w, 0),
                                                     new Point3D(x1, y1 + w, 1),
                                                     new Point3D(x1 + c, y1 + w, 1),
                                                     new Point3D(x1, y1 - w, 1),
                                                     new Point3D(x1 + c, y1 - w, 1)
                                                  };

               Int32Collection triangleIndices = new Int32Collection
                                              {
                                                 0,
                                                 1,
                                                 2,
                                                 2,
                                                 1,
                                                 3,
                                                 0,
                                                 2,
                                                 4,
                                                 2,
                                                 6,
                                                 4,
                                                 4,
                                                 6,
                                                 5,
                                                 6,
                                                 7,
                                                 5,
                                                 1,
                                                 0,
                                                 5,
                                                 0,
                                                 4,
                                                 5,
                                                 3,
                                                 1,
                                                 7,
                                                 1,
                                                 5,
                                                 7,
                                                 2,
                                                 3,
                                                 6,
                                                 3,
                                                 7,
                                                 6
                                              };

               meshGeometry.Positions = geometryPositions;
               meshGeometry.TriangleIndices = triangleIndices;

               geometryModel3D.Transform = new RotateTransform3D(
                  new AxisAngleRotation3D(new Vector3D(0, 0, 1), angle),
                  new Point3D(x1, y1, 0));

               return geometryModel3D;
          }
          #endregion
          
     }
}
