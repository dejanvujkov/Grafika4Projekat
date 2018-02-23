using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Grafika4.Model
{
     [XmlRoot(ElementName = "Nodes")]
     public class Nodes
     {
          [XmlElement(ElementName = "NodeEntity")]
          public List<NodeEntity> NodeEntity { get; set; }
     }
}
