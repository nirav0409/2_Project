using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    class Constants
    {

        /// <summary>
        /// Media Constants
        /// </summary>
        public static String path = System.AppDomain.CurrentDomain.BaseDirectory+"config" +"\\stateFile.txt";
        public static String pathConfig = System.AppDomain.CurrentDomain.BaseDirectory + "config" + "\\.config";
        public static String pathUno = System.AppDomain.CurrentDomain.BaseDirectory +"Receiver" +"\\Conf.ino";
        public static String ardiunoDir;
        public static String defaultardiunoDir = System.AppDomain.CurrentDomain.BaseDirectory + "arduino";
            //"C:\\Users\\Lenovo\\Downloads\\Arduino\\Arduino\\Arduino1.8.8\\arduino-1.8.8";

    }
}
