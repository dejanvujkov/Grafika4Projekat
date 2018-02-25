using Grafika4.Model;
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
          private Dictionary<string, Model.Point> allNodes { get; set; }
          
          public MainWindow()
          {
               allNodes = new Dictionary<string, Model.Point>();
               LoadXml();
               
               InitializeComponent();
               SetupHelix();
          }

          private void SetupHelix()
          {
               viewport3d.RotateGesture = new MouseGesture(MouseAction.RightClick);
               viewport3d.PanGesture = new MouseGesture(MouseAction.LeftClick);
          }

          #region LoadXMl
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

          private void FixNumbers()
          {
               var removeSubstation = new List<SubstationEntity>();

               foreach (var v in networkModel.Substations.SubstationEntity)
               {
                    ToLatLon(v.X, v.Y, out double x, out double y);
                    if (x < Constants.BottomX || x > Constants.UpperX || y < Constants.BottomY || y > Constants.UpperY)
                    {
                         removeSubstation.Add(v);
                    }
                    else
                    {
                         v.X = x;
                         v.Y = y;
                         var point = new Model.Point
                         {
                              X = x,
                              Y = y
                         };
                         allNodes.Add(v.Id, point);
                    }
               }

               //obrisi visak substation-a
               foreach(var s in removeSubstation)
               {
                    networkModel.Substations.SubstationEntity.Remove(s);
               }

               var removeSwitch = new List<SwitchEntity>();
               foreach (var v in networkModel.Switches.SwitchEntity)
               {
                    ToLatLon(v.X, v.Y, out double x, out double y);
                    if (x < Constants.BottomX || x > Constants.UpperX || y < Constants.BottomY || y > Constants.UpperY)
                    {
                         removeSwitch.Add(v);
                    }
                    else
                    {
                         v.X = x;
                         v.Y = y;
                         var point = new Model.Point
                         {
                              X = x,
                              Y = y
                         };
                         allNodes.Add(v.Id, point);
                    }
               }

               //obrisi visak switcheva
               foreach(var s in removeSwitch)
               {
                    networkModel.Switches.SwitchEntity.Remove(s);
               }

               var removeNode = new List<NodeEntity>();

               foreach (var v in networkModel.Nodes.NodeEntity)
               {
                    ToLatLon(v.X, v.Y, out double x, out double y);
                    if (x < Constants.BottomX || x > Constants.UpperX || y < Constants.BottomY || y > Constants.UpperY)
                    {
                         removeNode.Add(v);
                    }
                    else
                    {
                         v.X = x;
                         v.Y = y;
                         var point = new Model.Point
                         {
                              X = x,
                              Y = y
                         };
                         allNodes.Add(v.Id, point);
                    }
               }

               //obrisi visak nodova
               foreach(var node in removeNode)
               {
                    networkModel.Nodes.NodeEntity.Remove(node);
               }

               var removeLine = new List<LineEntity>();

               foreach(var line in networkModel.Lines.LineEntity)
               {
                    if(!allNodes.ContainsKey(line.FirstEnd) || !allNodes.ContainsKey(line.SecondEnd))
                    {
                         removeLine.Add(line);
                    }
               }

               //obrisi visak line-ova
               foreach(var line in removeLine)
               {
                    networkModel.Lines.LineEntity.Remove(line);
               }
          }

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

          #region Checkbox
          private void CheckBox_Checked(object sender, RoutedEventArgs e)
          {
               //TODO on checked
               //napisati viewport3d.children.add(svaki element iz liste linija)
               
               MessageBox.Show("Checked");
          }

          private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
          { 
               //TODO on unchecked
               //napisati viewport3d.children.remove(svaki element iz liste linija)
               MessageBox.Show("Unchecked");
          }
          #endregion
     }
}
