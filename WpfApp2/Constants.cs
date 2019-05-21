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
        public static String path = System.AppDomain.CurrentDomain.BaseDirectory +"\\stateFile.txt";
        public static String pathOut = System.AppDomain.CurrentDomain.BaseDirectory +"\\stateFile.txt";
        public static String pathUno= System.AppDomain.CurrentDomain.BaseDirectory+ "\\Conf.ino";
        public static String ardiunoDir;
            //"C:\\Users\\Lenovo\\Downloads\\Arduino\\Arduino\\Arduino1.8.8\\arduino-1.8.8";
        public static int MEDIA_FAST_FORWARD = 0xB3;
        public static int MEDIA_REWIND = 0xB4;
        public static int MEDIA_NEXT = 0xB5;
        public static int MEDIA_PREVIOUS = 0xB6;
        public static int MEDIA_STOP = 0xB7;
        public static int MEDIA_PLAY_PAUSE = 0xCD;
        public static int MEDIA_VOLUME_MUTE = 0xE2;
        public static int MEDIA_VOLUME_UP = 0xE9;
        public static int MEDIA_VOLUME_DOWN = 0xEA;


        // public static int KEYshortcut


        public static int KEY_LEFT_CTRL = 0x80;
        public static int KEY_LEFT_SHIFT = 0x81;
        public static int KEY_LEFT_ALT = 0x82;
        public static int KEY_LEFT_GUI = 0x83;
        public static int KEY_RIGHT_CTRL = 0x84;
        public static int KEY_RIGHT_SHIFT = 0x85;
        public static int KEY_RIGHT_ALT = 0x86;
        public static int KEY_RIGHT_GUI = 0x87;
        public static int KEY_UP_ARROW = 0xDA;
        public static int KEY_DOWN_ARROW = 0xD9;
        public static int KEY_LEFT_ARROW = 0xD8;
        public static int KEY_RIGHT_ARROW = 0xD7;
        public static int KEY_BACKSPACE = 0xB2;
        public static int KEY_TAB = 0xB3;
        public static int KEY_RETURN = 0xB0;
        public static int KEY_ESC = 0xB1;
        public static int KEY_INSERT = 0xD1;
        public static int KEY_DELETE = 0xD4;
        public static int KEY_PAGE_UP = 0xD3;
        public static int KEY_PAGE_DOWN = 0xD6;
        public static int KEY_HOME = 0xD2;
        public static int KEY_END = 0xD5;
        public static int KEY_CAPS_LOCK = 0xC1;
        public static int KEY_F1 = 0xC2;
        public static int KEY_F2 = 0xC3;
        public static int KEY_F3 = 0xC4;
        public static int KEY_F4 = 0xC5;
        public static int KEY_F5 = 0xC6;
        public static int KEY_F6 = 0xC7;
        public static int KEY_F7 = 0xC8;
        public static int KEY_F8 = 0xC9;
        public static int KEY_F9 = 0xCA;
        public static int KEY_F10 = 0xCB;
        public static int KEY_F11 = 0xCC;
        public static int KEY_F12 = 0xCD;
    }
}
