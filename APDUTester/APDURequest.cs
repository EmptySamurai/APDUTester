using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APDUTester
{
    public class APDURequest
    {
        public byte CLA { get; set; }
        public byte INS { get; set; }
        public byte P1 { get; set; }
        public byte P2 { get; set; }
        public byte[] Data { get; set; }
    }
}
