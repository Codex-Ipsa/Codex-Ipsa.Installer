using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codex_Ipsa.Installer
{
    internal class UpdateJson
    {
        public String id { get; set; }
        public String name { get; set; }
        public String version { get; set; }
        public String url { get; set; }
        public String info { get; set; }
        public bool available { get; set; }
    }
}
