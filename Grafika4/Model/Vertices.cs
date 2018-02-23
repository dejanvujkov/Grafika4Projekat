using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Grafika4.Model
{
     [XmlRoot(ElementName = "Vertices")]
     public class Vertices
     {
          [XmlElement(ElementName = "Point")]
          public List<Point> Point { get; set; }
     }
}
