using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Grafika4.Model
{
     [XmlRoot(ElementName = "Lines")]
     public class Lines
     {
          [XmlElement(ElementName = "LineEntity")]
          public List<LineEntity> LineEntity { get; set; }
     }
}
