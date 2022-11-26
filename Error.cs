//Aaron David Briseño Rivera
using System;
using System.IO;

namespace Generador
{
    public class Error : Exception
    {
        public Error(string mensaje, StreamWriter log) : base(mensaje)
        {
            log.WriteLine(mensaje);
        }
    }
}