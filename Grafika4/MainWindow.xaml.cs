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

          public MainWindow()
          {
               LoadXml();
               InitializeComponent();
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
          } 
          #endregion
     }
}
