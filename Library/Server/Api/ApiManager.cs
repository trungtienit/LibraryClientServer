using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Server.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Server.Api
{

    class ApiManager
    {
        private string[] Scopes = { DriveService.Scope.DriveReadonly };
        private string ApplicationName = "BookApp";
        DriveService service;
        UserCredential credential;

        List<Book> mList;
        public ApiManager()
        {
            credential = GetUserCredential();

            service = GetDriceService(credential);
        }
        public List<Book> Search(String title,byte types)
        {
            #region Search
            if (mList == null)
                mList = new List<Book>();
            else mList.Clear();

            string pageToken = null;
            String mTypes = "";
            if (types != 0)
            {
                if ((types & ServerManager.TYPE_FILE_MSWORD) == ServerManager.TYPE_FILE_MSWORD)
                    mTypes = mTypes + " or " + ServerManager.MSWORD + " or " + ServerManager.OFFICE_DOC;
                if ((types & ServerManager.TYPE_FILE_PDF) == ServerManager.TYPE_FILE_PDF)
                    mTypes = mTypes + " or " + ServerManager.PDF;
                if ((types & ServerManager.TYPE_FILE_EXCEL) == ServerManager.TYPE_FILE_EXCEL)
                    mTypes = mTypes + " or " + ServerManager.EXCEL;
            }
          
            Console.WriteLine("Start Search");
            do
            {
                var request = service.Files.List();
                if (mTypes.Length > 0)
                {
                    mTypes = mTypes.Substring(3);
                    request.Q = "name contains '"
                   + title
                   + "' and ( "+mTypes+" )";
                }
                   
                request.Spaces = "drive";
                request.Fields = "nextPageToken, files(id, name, mimeType, size, createdTime, modifiedTime,fullFileExtension)";
                request.PageToken = pageToken;
                var result = request.Execute();
                foreach (var file in result.Files)
                {

                    Book b = new Book.Builder()
                     .Id("")
                     .Name(file.Name)
                     .Type("."+file.FullFileExtension)
                     .Price(0)
                     .Size(ServerManager.CustomSize((long) file.Size))
                     .Path("")
                     .OnDrive()
                     .Build();
                    mList.Add(b);
                    Console.WriteLine(String.Format(
                            "Found file: {0} ({1})", file.Name, file.Id));
                }
                pageToken = result.NextPageToken;
            } while (pageToken != null);
            Console.WriteLine("Search done");
            return mList;
            #endregion
        }

        public void GetFileMetaData()
        {
            #region Get MetaData
            IList<Google.Apis.Drive.v3.Data.File> files = service.Files.List().Execute().Files;
            Console.WriteLine("Start Load");
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    Console.WriteLine("{0} ({1})", file.Name, file.Id);
                }
            }
            else
            {
                Console.WriteLine("No files found.");
            }

            Console.WriteLine("Done!");
            #endregion
        }

        public UserCredential GetUserCredential()
        {
            using (var stream = new FileStream("..\\..\\client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/BookApp.json");

                return GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                // Console.WriteLine("Credential file saved to: " + credPath);

            }
        }
        public DriveService GetDriceService(UserCredential credential)
        {
            return new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }
    }
}
