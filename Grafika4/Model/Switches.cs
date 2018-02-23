using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Grafika4.Model
{
     [XmlRoot(ElementName = "Switches")]
     public class Switches
     {
          [XmlElement(ElementName = "SwitchEntity")]
          public List<SwitchEntity> SwitchEntity { get; set; }
     }
}
