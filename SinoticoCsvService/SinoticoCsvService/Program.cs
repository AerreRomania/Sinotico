using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SinoticoCsvService
    {
    static class Program
        {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        static void Main()
            {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new CsvImporter()
            };
            ServiceBase.Run(ServicesToRun);
            }
        }
    }
