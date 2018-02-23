using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Grafika4.Model
{
     [XmlRoot(ElementName = "LineEntity")]
     public class LineEntity
     {
          [XmlElement(ElementName = "Id")]
          public string Id { get; set; }
          [XmlElement(ElementName = "Name")]
          public string Name { get; set; }
          [XmlElement(ElementName = "IsUnderground")]
          public bool IsUnderground { get; set; }
          [XmlElement(ElementName = "R")]
          public string R { get; set; }
          [XmlElement(ElementName = "ConductorMaterial")]
          public string ConductorMaterial { get; set; }
          [XmlElement(ElementName = "LineType")]
          public string LineType { get; set; }
          [XmlElement(ElementName = "ThermalConstantHeat")]
          public string ThermalConstantHeat { get; set; }
          [XmlElement(ElementName = "FirstEnd")]
          public string FirstEnd { get; set; }
          [XmlElement(ElementName = "SecondEnd")]
          public string SecondEnd { get; set; }
          [XmlElement(ElementName = "Vertices")]
          public Vertices Vertices { get; set; }
     }
}
