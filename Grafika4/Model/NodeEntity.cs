using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Grafika4.Model
{
     [XmlRoot(ElementName = "NodeEntity")]
     public class NodeEntity : IdentifyObject
     {
          [XmlElement(ElementName = "Id")]
          public string Id { get; set; }
          [XmlElement(ElementName = "Name")]
          public string Name { get; set; }
          [XmlElement(ElementName = "X")]
          public double X { get; set; }
          [XmlElement(ElementName = "Y")]
          public double Y { get; set; }

          public override string ToString()
          {
               return $"ID: {Id}, Name: {Name}";
          }
     }
}
