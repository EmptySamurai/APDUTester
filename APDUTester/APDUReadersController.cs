using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCSC;
using PCSC.Iso7816;

namespace APDUTester
{
    public class APDUReadersController : IReadersController
    {

        public string[] GetReaders()
        {
            using (var context = new SCardContext())
            {
                try
                {
                    context.Establish(SCardScope.System);
                }
                catch (Exception ex)
                {
                    return new string[0];
                }

                try
                {
                    return context.GetReaders();
                }
                catch (Exception ex)
                {                   
                    return new string[0];
                }
                finally
                {
                    context.Release();                    
                }
            }
        }

        public APDUResponse SendAPDU(string reader, APDURequest request)
        {
            using (var context = new SCardContext())
            {
                context.Establish(SCardScope.System);
                using (var isoReader = new IsoReader(context, reader, SCardShareMode.Shared, SCardProtocol.Any))
                {
                    var apdu = new CommandApdu(IsoCase.Case3Short, isoReader.ActiveProtocol)
                    {
                        CLA = request.CLA, // Class
                        INS = request.INS, //Instruction
                        P1 = request.P1,  // Parameter 1
                        P2 = request.P2,  // Parameter 2
                        Data = request.Data
                    };

                    byte[] id = null;
                    var response = isoReader.Transmit(apdu);
                    return new APDUResponse()
                    {
                        Data = response.GetData(),
                        SW1 = response.SW1,
                        SW2 = response.SW2
                    };     
                };                
            }
        }
    }
}
