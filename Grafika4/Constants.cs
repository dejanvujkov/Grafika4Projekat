using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafika4
{
     public static class Constants
     {
          public const double BottomY = 19.793909;
          public const double BottomX = 45.2325;
          public const double UpperY = 19.894459;
          public const double UpperX = 45.277031;

          public const double MaxLat = 45.277031;
          public const double MaxLon = 19.894459;

          public const double MinLat = 45.2325;
          public const double MinLon = 19.793909;

          public const double width = 1175;
          public const double height = 775;

          public const double ScaleX = width / (MaxLon - MinLon);
          public const double ScaleY = height / (MaxLat - MinLat);
     }
}
