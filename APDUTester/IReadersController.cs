using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APDUTester
{
    public interface IReadersController
    {
        string[] GetReaders();
        APDUResponse SendAPDU(string reader, APDURequest request);
        string GetReaderPort(string reader);
    }
}
