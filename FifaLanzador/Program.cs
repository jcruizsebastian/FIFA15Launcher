using System;
using System.IO;
using System.Reflection;

namespace FifaLanzador
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Asegurate de tener instalado Python en versión >= 3.7 y los componentes wmi y pypiwin32...");
            Console.WriteLine("En caso de fallo, lanzar directamente desde CMD el fichero que se autogenera llamado main.py a través del comando py main.py y analizar la salida.");

            if (!File.Exists("fifa15.exe"))
            {
                Console.WriteLine("No se encuentra el fichero fifa15.exe. Situar este ejecutable en la ruta en la que se encuentre el fifa15.exe.");
                Console.WriteLine("Pulsar tecla para cerrar.");
                Console.ReadKey();

                return;
            }

            if (!File.Exists("main.py"))
            {
                CreatePythonLauncherScriptFromResource();
            }

            RunCMDCommand("py main.py");
        }

        private static void CreatePythonLauncherScriptFromResource()
        {
            string resourceName = "FifaLanzador.main.py";

            if (!File.Exists("main.py"))
            {

                var assembly = Assembly.GetExecutingAssembly();
                using (Stream input = assembly.GetManifestResourceStream(resourceName))
                using (Stream output = File.Create("main.py"))
                {
                    byte[] buffer = new byte[8192];

                    int bytesRead;
                    while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        output.Write(buffer, 0, bytesRead);
                    }
                }
            }
        }

        public static String RunCMDCommand(string comando)
        {
            //Indicamos que deseamos inicializar el proceso cmd.exe junto a un comando de arranque. 
            //(/C, le indicamos al proceso cmd que deseamos que cuando termine la tarea asignada se cierre el proceso).
            //Para mas informacion consulte la ayuda de la consola con cmd.exe /? 
            System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + comando);


            // Indicamos que la salida del proceso se redireccione en un Stream
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            //Indica que el proceso no despliegue una pantalla negra (El proceso se ejecuta en background)
            procStartInfo.CreateNoWindow = true;
            //Esconder la ventana
            //procStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //Inicializa el proceso
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = procStartInfo;
            proc.Start();
            //Consigue la salida de la Consola(Stream) y devuelve una cadena de texto
            string result = proc.StandardOutput.ReadToEnd();

            //Muestra en pantalla la salida del Comando
            return result;
        }
    }
}
