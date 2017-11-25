
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Server.Api;
using Server.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Server
{

    internal sealed class Program
    {

        [STAThread]
        private static void Main(string[] args)
        {

            ApiManager apiManager = new ApiManager();
            apiManager.Search("chuong", ServerManager.TYPE_FILE_PDF|ServerManager.TYPE_FILE_MSWORD| ServerManager.TYPE_FILE_EXCEL);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ServerForm());

        }

    }
}
