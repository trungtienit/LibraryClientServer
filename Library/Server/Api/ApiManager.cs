﻿using Google.Apis.Auth.OAuth2;
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
using Google.Apis.Drive.v3.Data;
using Google.Apis.Download;
using System.Windows.Forms;

namespace Server.Api
{
    class ApiManager
    {
        private string[] Scopes = { DriveService.Scope.DriveReadonly };
        private string ApplicationName = "BookApp";
        DriveService service;
        UserCredential credential;
        ServerManager serverManager;
        public ApiManager()
        {
            credential = GetUserCredential();
            serverManager = new ServerManager();
            service = GetDriceService(credential);
        }
        internal List<string> FindBookOnDrive(string v1, string v2)
        {
            byte mType = ServerManager.TYPE_FILE;
            if (v2.ToUpper().Equals("WORD"))
                mType = ServerManager.TYPE_FILE_MSWORD;
            if (v2.ToUpper().Equals("PDF"))
                mType = ServerManager.TYPE_FILE_PDF;
            if (v2.ToUpper().Equals("EXCEL"))
                mType = ServerManager.TYPE_FILE_EXCEL;
            if (v2.ToUpper().Equals("TEXT"))
                mType = ServerManager.TYPE_FILE_PLAIN_TEXT;
            if (v2.ToUpper().Equals("POWERPOINT"))
                mType = ServerManager.TYPE_FILE_POWERPOINT;
            if (v2.ToUpper().Equals("ALL"))
                mType = ServerManager.TYPE_FILE_PLAIN_TEXT | ServerManager.TYPE_FILE_EXCEL | ServerManager.TYPE_FILE_MSWORD | ServerManager.TYPE_FILE_PDF | ServerManager.TYPE_FILE_POWERPOINT;
            return FindBookByTitleAndTypeOnDrive(v1, mType);
        }
        public List<String> FindBookByTitleAndTypeOnDrive(String title, byte types)
        {
            #region Search
            List<String> results = new List<string>();
            string pageToken = null;
            String mTypes = "";
            if (types != 0)
            {
                if ((types & ServerManager.TYPE_FILE_MSWORD) == ServerManager.TYPE_FILE_MSWORD)

                    mTypes = mTypes + " or " + ServerManager.MSWORD + " or " + ServerManager.OFFICE_DOC+ " or " +ServerManager.DOC_GOOGLE;
                if ((types & ServerManager.TYPE_FILE_PDF) == ServerManager.TYPE_FILE_PDF)
                    mTypes = mTypes + " or " + ServerManager.PDF;
                if ((types & ServerManager.TYPE_FILE_EXCEL) == ServerManager.TYPE_FILE_EXCEL)
                    mTypes = mTypes + " or " + ServerManager.EXCEL;
                if ((types & ServerManager.TYPE_FILE_PLAIN_TEXT) == ServerManager.TYPE_FILE_PLAIN_TEXT)
                    mTypes = mTypes + " or " + ServerManager.PLAIN_TEXT;
                if ((types & ServerManager.TYPE_FILE_POWERPOINT) == ServerManager.TYPE_FILE_POWERPOINT)
                    mTypes = mTypes + " or " + ServerManager.OFFICE_POWERPOINT;
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
                   + "' and ( " + mTypes + " )";
                }
                else
                    request.Q = "name contains '" + title + "'"; 

                Random r = new Random();
                request.Spaces = "drive";
                request.Fields = "nextPageToken, files(id, name, mimeType, size, createdTime, modifiedTime,fullFileExtension)";
                request.PageToken = pageToken;
                FileList result = null;
                try
                {
                    result = request.Execute();
                }
                catch (Exception E)
                {
                    Console.Write("ERROR Network: " + E.StackTrace);
                }
                if (result == null)
                    return results;
                foreach (var file in result.Files)
                {
                    int p = 0;
                    if (!file.FullFileExtension.ToUpper().Contains("TXT")
                   && !file.FullFileExtension.ToUpper().Contains("XLS")
                    &&!file.FullFileExtension.ToUpper().Contains("PPT"))
                        p = (r.Next(1, 99) * 100);
                    Book b = new Book.Builder()
                     .Id(CreateId(file))
                     .Name(file.Name)
                     .Type("." + file.FullFileExtension)
                     .Price(p)
                     .Size(serverManager.CustomSize((long)file.Size))
                     .Path(file.Id)
                     .OnDrive()
                     .Build();
                    BookDB.AddNewBook(b);
                    results.Add(b.ToString());
                    Console.WriteLine(String.Format(
                            "Found file: {0} ({1})", file.Name, file.Id));
                }
                pageToken = result.NextPageToken;
            } while (pageToken != null);
            Console.WriteLine("Search done");

            return results;
            #endregion
        }
        private string CreateId(Google.Apis.Drive.v3.Data.File file)
        {
            DateTime dateTime = new DateTime();
            if (file.CreatedTime != null)
                dateTime = (DateTime)file.CreatedTime;
            else if (file.ModifiedTime != null)
                dateTime = (DateTime)file.ModifiedTime;

            String id = "GD" + dateTime.Second
                 + dateTime.Minute
                 + dateTime.Hour
                 + file.Size.ToString()
                 + dateTime.Day
                 + dateTime.Month
                 + dateTime.Year;

            return id.Substring(0, 10);
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

        public string DownloadGoogleFile(string fileId)
        {
            FilesResource.GetRequest request = service.Files.Get(fileId);

            string fileName = request.Execute().Name;
            String filePath = BookDB.PATH_DB_DIR + "/" + fileName;
            if (System.IO.File.Exists(filePath))
            {
                return filePath;
            }
            MemoryStream stream1 = new MemoryStream();

            request.MediaDownloader.ProgressChanged += (Google.Apis.Download.IDownloadProgress progress) =>
            {
                switch (progress.Status)
                {
                    case DownloadStatus.Downloading:
                        {
                            //Console.WriteLine(progress.BytesDownloaded);
                            break;
                        }
                    case DownloadStatus.Completed:
                        {
                            //Console.WriteLine("Download complete.");
                            if (!System.IO.File.Exists(filePath))
                                SaveStream(stream1, filePath);
                            break;
                        }
                    case DownloadStatus.Failed:
                        {
                            //Console.WriteLine("Download failed.");
                            break;
                        }
                }
            };
            request.Download(stream1);
            return filePath;
        }
        private static void SaveStream(MemoryStream stream, string FilePath)
        {
            using (System.IO.FileStream file = new FileStream(FilePath, FileMode.Create, FileAccess.ReadWrite))
            {
                stream.WriteTo(file);
            }
        }
        public UserCredential GetUserCredential()
        {
            using (var stream = new FileStream(Application.StartupPath + "\\client_secret.json", FileMode.Open, FileAccess.Read))
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
