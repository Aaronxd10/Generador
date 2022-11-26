//Aaron David Briseño Rivera
using System;
using System.IO;

namespace Generador
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (Lenguaje a = new Lenguaje("C:\\Users\\aaron\\Documents\\Lenguajes y automatas II\\Unidad 4\\Generador\\c2.gram"))
                {
                    a.gramatica();
                    /*while(!a.FinArchivo())
                    {
                        a.NextToken();
                    }*/
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}