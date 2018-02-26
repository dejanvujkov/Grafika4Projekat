using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Grafika4.Model
{
    public class IdentifyObject
    {
          public string Id { get; set; }

          [XmlElement(ElementName = "Name")]
          public string Name { get; set; }

          [XmlElement(ElementName = "X")]
          public double X { get; set; }

          [XmlElement(ElementName = "Y")]
          public double Y { get; set; }

          public override string ToString()
          {
               return $"ID: {Id}\nName: {Name}";
          }
     }
}
