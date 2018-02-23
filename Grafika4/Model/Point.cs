using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Grafika4.Model
{
     [XmlRoot(ElementName = "Point")]
     public class Point
     {
          [XmlElement(ElementName = "X")]
          public double X { get; set; }
          [XmlElement(ElementName = "Y")]
          public double Y { get; set; }
     }
}
