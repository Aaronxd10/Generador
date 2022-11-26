//Aaron David Briseño Rivera 
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
//requerimiento 1. construir un metodo para escribir en el archivo lenguaje indentando el codigo
//                 "{"incrementa un tabulador,"}"decrementa un tabulador
//requerimiento 2. declarar un atributo "primera produccion" de tipo string y actualizarlo 
//                 con la primera produccion de la gramatica
//requerimiento 3. la primera produccion es publica y el resto privadas
//requerimiento 4. el constructor lexico parametrizado debe validar que la extencion del
//                 archivo a compilar sea .gen si no es .gen debe lanzar una excepcion
//requerimiento 5  resolver la ambiguedad de st y snt
//                 recorrer linea por linea el archivo .gram para extraer el nombre de cada produccion
//requerimiento 6  agregar el parentesis izquierdo y el derecho escapados en la matriz
//                 de transiciones
//requerimiento 7. implementar el or y la cerradura epsilon

namespace Generador
{
    public class Lenguaje : Sintaxis, IDisposable
    {
       
        List<string> listaSNT;
        public Lenguaje(string nombre) : base(nombre)
        {
            listaSNT = new List<string>();
            
        }
        public Lenguaje()
        {
            
            listaSNT = new List<string>();
        }
        public void Dispose()
        {
           
            cerrar();
        }
        private bool esSNT(string contenido)
        {
            return true;
           
        }
        private void agregarSNT(string contenido)
        {
            
            listaSNT.Add(contenido);
        }
        private void Programa(string produccionPrincipal)
        {
            
            
            agregarSNT("Programa");
            agregarSNT("Librerias");
            agregarSNT("Variables");
            agregarSNT("ListaIdentificadores");
            
            programa.WriteLine("using System;");
            
            programa.WriteLine("using System.IO;");
            
            programa.WriteLine("using System.Collections.Generic;");
            
            programa.WriteLine();
            
            programa.WriteLine("namespace Generico");
            
            programa.WriteLine("{");
            
            programa.WriteLine("public class Program");
            
            programa.WriteLine("{");
            
            programa.WriteLine("static void Main(string[] args)");
            
            programa.WriteLine("{");
            
            programa.WriteLine("try");
            
            programa.WriteLine("{");
            
            programa.WriteLine("using (Lenguaje a = new Lenguaje())");
            
            programa.WriteLine("{");
            
            programa.WriteLine("a." + produccionPrincipal + "();");
            
            programa.WriteLine("}");
            
            programa.WriteLine("}");
            
            programa.WriteLine("catch (Exception e)");
            
            programa.WriteLine("{");
            
            programa.WriteLine("Console.WriteLine(e.Message);");
            
            programa.WriteLine("}");
            
            programa.WriteLine("}");
            
            programa.WriteLine("}");
            
            programa.WriteLine("}");
        }
        public void gramatica()
        {
            cabecera();
            Programa("programa");
            cabeceraLenguaje();
            listaProducciones();
            lenguaje.WriteLine("\t}");
            lenguaje.WriteLine("}");
        }
        private void cabecera()
        {
            match("gramatica");
            match(":");
            match(Tipos.ST);
            match(Tipos.finProduccion);
        }
        private void cabeceraLenguaje()
        {
            lenguaje.WriteLine("using System;");
            lenguaje.WriteLine("using System.Collections.Generic;");
            lenguaje.WriteLine("namespace Generico");
            lenguaje.WriteLine("{");
            lenguaje.WriteLine("\tpublic class Lenguaje : Sintaxis, IDisposable");
            lenguaje.WriteLine("\t{");
            lenguaje.WriteLine("\t\tstring nombreProyecto;");
            lenguaje.WriteLine("\t\tpublic Lenguaje(string nombre) : base(nombre)");
            lenguaje.WriteLine("\t\t{");
            lenguaje.WriteLine("\t\t}");
            lenguaje.WriteLine("\t\tpublic Lenguaje()");
            lenguaje.WriteLine("\t\t{");
            lenguaje.WriteLine("\t\t}");
        }
        private void listaProducciones()
        {
            lenguaje.WriteLine("\t\tprivate void " + getContenido() + "()");
            lenguaje.WriteLine("\t\t{");
            match(Tipos.ST);
            match(Tipos.Produce);
            simbolos();
            match(Tipos.finProduccion);
            lenguaje.WriteLine("\t\t}");
            if (!FinArchivo())
            {
                listaProducciones();
            }
        }
        private void simbolos()
        {
            if (getContenido() == "(")
            {
                match("(");
                lenguaje.WriteLine("\t\tif (true)");
                lenguaje.WriteLine("\t\t{");
                simbolos();
                match(")");
                lenguaje.WriteLine("\t\t}");
            }
            else if (esTipo(getContenido()))
            {
                lenguaje.WriteLine("\t\t\tmatch(Tipos." + getContenido() + ");");
                match(Tipos.ST);
            }
            else if (esSNT(getContenido()))
            {
                lenguaje.WriteLine("\t\t\t" + getContenido() + "();");
                //match(Tipos.SNT);
                match(Tipos.ST);
            }
            else if (getClasificacion() == Tipos.ST)
            {
                lenguaje.WriteLine("\t\t\tmatch(\"" + getContenido() + "\");");
                match(Tipos.ST);
            }
            if (getClasificacion() != Tipos.finProduccion && getContenido() != ")")
            {
                simbolos();
            }
        }
        private bool esTipo(string clasificacion)
        {
            switch (clasificacion)
            {
                case "Identificador":
                case "Numero":
                case "Caracter":
                case "Asignacion":
                case "Inicializacion":
                case "OperadorLogico":
                case "OperadorRelacional":
                case "OperadorTernario":
                case "OperadorTermino":
                case "OperadorFactor":
                case "IncrementoTermino":
                case "IncrementoFactor":
                case "FinSentencia":
                case "Cadena":
                case "TipoDato":
                case "ZonaCondicion":
                case "Ciclo":
                    return true;
            }
            return false;
        }
    }
}