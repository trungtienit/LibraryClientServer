using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Server.Common
{
    public class ClientManager
    {
        public static String FOLDER_CONFIG = Application.StartupPath + "\\Config";
        public static String FOLDER_DOWLOAD = FOLDER_CONFIG+ "\\Download";
        public static String FOLDER_PREVIEW = FOLDER_CONFIG+"\\Preview";

        public const char SIGN = '#';
        public const byte TYPE = 1;
        public const byte TYPE_SEARCH = TYPE << 1;
        public const byte TYPE_DOWNLOAD = TYPE << 2;
        public const byte TYPE_PREVIEW= TYPE << 3;
        public const byte TYPE_VIEW_FULL = TYPE << 4;
        public const byte TYPE_CHANGE = TYPE << 5;
        public const byte TYPE_LOGIN = TYPE << 6;
        public const int BUFFER_SIZE = 1024;

        public static int myWallet ;
    }
}
