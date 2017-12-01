using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Common
{
    public class ClientManager
    {
        public const String FOLDER_DOWLOAD = "..\\Download";

        public const char SIGN = '#';
        public const byte TYPE = 1;
        public const byte TYPE_SEARCH = TYPE << 1;
        public const byte TYPE_DOWNLOAD = TYPE << 2;
        public const byte TYPE_PREVIEW= TYPE << 3;
        public const byte TYPE_VIEW_FULL = TYPE << 4;
        public const byte TYPE_CHANGE = TYPE << 5;

        public const int BUFFER_SIZE = 1024;

        public static int myWallet = 5000000;
    }
}
