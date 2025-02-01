using System;

namespace NEA.Questions
{
    public class NoDiagramException : NotImplementedException
    {
        public NoDiagramException() : base() { }

        public NoDiagramException(string message) : base(message) { }
    }
}
