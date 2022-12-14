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
        string primerap;
        int tp, tl;
        List<string> listaSNT;
        public Lenguaje(string nombre) : base(nombre)
        {
            primerap = "";
            tp = tl = 0;
            listaSNT = new List<string>();
        }
        public Lenguaje()
        {
            primerap = "";
            tp = tl = 0;
            listaSNT = new List<string>();
        }
        public void Dispose()
        {

            cerrar();
        }
        private bool esSNT(string contenido)
        {
            return listaSNT.Contains(contenido);
            //return true;
        }
        private void agregarSNT(string contenido)
        {
            string[] lineaactual = System.IO.File.ReadAllLines("C:\\Users\\aaron\\Documents\\Lenguajes y automatas II\\Unidad 4\\Generador\\c2.gram");
            foreach (string linea in lineaactual)
            {
                string sinespacio = linea.Replace(" ", "");
                int valor = sinespacio.IndexOf("-");
                if (valor > 0)
                {
                    string snt = sinespacio.Substring(0, valor);
                    if (!listaSNT.Contains(snt))
                        listaSNT.Add(snt);
                }
            }
        }
        private void Programa(string produccionPrincipal)
        {
            agregarSNT(getContenido());
            identarPrograma("using System;");
            identarPrograma("using System.IO;");
            identarPrograma("using System.Collections.Generic;");
            identarPrograma("");
            identarPrograma("namespace Generico");
            identarPrograma("{");
            identarPrograma("public class Program");
            identarPrograma("{");
            identarPrograma("static void Main(string[] args)");
            identarPrograma("{");
            identarPrograma("try");
            identarPrograma("{");
            identarPrograma("using (Lenguaje a = new Lenguaje())");
            identarPrograma("{");
            identarPrograma("a." + produccionPrincipal + "();");
            identarPrograma("}");
            identarPrograma("}");
            identarPrograma("catch (Exception e)");
            identarPrograma("{");
            identarPrograma("Console.WriteLine(e.Message);");
            identarPrograma("}");
            identarPrograma("}");
            identarPrograma("}");
            identarPrograma("}");
        }
        public void gramatica()
        {
            cabecera();
            primerap = getContenido();
            Programa(primerap);
            cabeceraLenguaje();
            listaProducciones(true);
            identarlenguaje("}");
            identarlenguaje("}");
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
            identarlenguaje("using System;");
            identarlenguaje("using System.Collections.Generic;");
            identarlenguaje("namespace Generico");
            identarlenguaje("{");
            identarlenguaje("public class Lenguaje : Sintaxis, IDisposable");
            identarlenguaje("{");
            identarlenguaje("public Lenguaje(string nombre) : base(nombre)");
            identarlenguaje("{");
            identarlenguaje("}");
            identarlenguaje("public Lenguaje()");
            identarlenguaje("{");
            identarlenguaje("}");
            identarlenguaje("public void Dispose()");
            identarlenguaje("{");
            identarlenguaje("cerrar();");
            identarlenguaje("}");
        }
        private void listaProducciones(bool p)
        {
            if (p)
            {
                identarlenguaje("public void " + getContenido() + "()");
                p = false;
            }
            else
            {
                identarlenguaje("private void " + getContenido() + "()");
            }
            identarlenguaje("{");
            match(Tipos.ST);
            match(Tipos.Produce);
            simbolos();
            match(Tipos.finProduccion);
            identarlenguaje("}");
            if (!FinArchivo())
            {
                listaProducciones(p);
            }
        }
        private void simbolos()
        {
            if (getContenido() == "\\(")
            {
                match("\\(");
                if (esSNT(getContenido()))
                {
                    throw new Error("Error: se espera un ST ", log);
                }
                else if (esTipo(getContenido()))
                {
                    identarlenguaje("if (getClasificacion() == Tipos." + getContenido() + ")");
                }
                else
                {
                    identarlenguaje("if (getContenido() == \"" + getContenido() + "\")");
                }
                identarlenguaje("{");
                simbolos();
                match("\\)");
                identarlenguaje("}");
            }
            else if (esTipo(getContenido()))
            {
                identarlenguaje("match(Tipos." + getContenido() + ");");
                match(Tipos.ST);
            }
            else if (esSNT(getContenido()))
            {
                identarlenguaje("" + getContenido() + "();");
                //match(Tipos.SNT);
                match(Tipos.ST);
            }
            else if (getClasificacion() == Tipos.ST)
            {
                identarlenguaje("match(\"" + getContenido() + "\");");
                match(Tipos.ST);
            }
            if (getClasificacion() != Tipos.finProduccion && getContenido() != "\\)")
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
                case "Zona":
                case "Condicion":
                case "Ciclo":
                    return true;
            }
            return false;
        }
        private void identarPrograma(string cadena)
        {
            if (cadena == ("}"))
            {
                tp--;
            }
            for (int i = 0; i < tp; i++)
            {
                programa.Write("\t");
            }
            if (cadena == "{")
            {
                tp++;
            }
            programa.WriteLine(cadena);
        }
        private void identarlenguaje(string cadena)
        {
            if (cadena == ("}"))
            {
                tl--;
            }
            for (int i = 0; i < tl; i++)
            {
                lenguaje.Write("\t");
            }
            if (cadena == "{")
            {
                tl++;
            }
            lenguaje.WriteLine(cadena);
        }
    }
}
