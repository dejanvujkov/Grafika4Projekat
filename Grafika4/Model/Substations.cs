using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Grafika4.Model
{
     [XmlRoot(ElementName = "Substations")]
     public class Substations
     {
          [XmlElement(ElementName = "SubstationEntity")]
          public List<SubstationEntity> SubstationEntity { get; set; }
     }
}
