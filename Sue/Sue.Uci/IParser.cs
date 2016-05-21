using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sue.Uci
{
    public interface IParser
    {
        void Parse(string inputLine, IAdapter adapter);
    }
}