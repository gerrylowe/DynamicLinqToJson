using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicLinq
{
    public class Application
    {
        public int IntProp { get; set; }
        public string StringProp { get; set; }

        public Application(int intProp, string stringProp)
        {
            this.IntProp = intProp;
            this.StringProp = stringProp;
        }
    }
}
