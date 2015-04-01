using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APDUTester
{
    public class APDUResponse
    {
        public byte SW1 { get; set; }
        public byte SW2 { get; set; }
        public byte[] Data { get; set; }
    }
}
