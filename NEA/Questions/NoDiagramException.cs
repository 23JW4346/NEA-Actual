using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA.Questions
{
    public class NoDiagramException : NotImplementedException
    {
        public NoDiagramException() : base() { }

        public NoDiagramException(string message) : base(message) { }
    }
}
