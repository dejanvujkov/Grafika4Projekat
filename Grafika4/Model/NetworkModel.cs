using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Grafika4.Model
{
     [XmlRoot(ElementName = "NetworkModel")]
     public class NetworkModel
     {
          [XmlElement(ElementName = "Substations")]
          public Substations Substations { get; set; }
          [XmlElement(ElementName = "Nodes")]
          public Nodes Nodes { get; set; }
          [XmlElement(ElementName = "Switches")]
          public Switches Switches { get; set; }
          [XmlElement(ElementName = "Lines")]
          public Lines Lines { get; set; }
     }

     public enum NodeType
     {
          Unknown = 0,
          Substation,
          Switch,
          Node
     }
}
