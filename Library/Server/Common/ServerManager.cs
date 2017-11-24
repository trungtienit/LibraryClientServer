using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Server.Common
{
    public class ServerManager
    {
        public const char SIGN = '#';
        public const byte TYPE = 1;
        public const byte TYPE_SEARCH = TYPE << 1;
        public const byte TYPE_DOWNLOAD = TYPE << 2;
        public const byte TYPE_PREVIEW = TYPE << 3;
        public const byte TYPE_VIEW_FULL = TYPE << 4;
        public const byte TYPE_CHANGE = TYPE << 5;


        public static List<Book> GetAllDataLocal(string pathFolder, string[] fileTypes)
        {

            List<Book> mList = new List<Book>();
            DirectoryInfo dinfo = new DirectoryInfo(pathFolder);

            FileInfo[] files =
                dinfo.GetFiles()
                     .Where(f => fileTypes.Contains(f.Extension.ToLower()))
                     .ToArray();
            string[] dl = Directory.GetDirectories(pathFolder);

            foreach (string s in dl)
            {
                mList.AddRange(GetAllDataLocal(s, fileTypes));
            }
            Random r = new Random();
            foreach (FileInfo file in files)
            {

                double p = (r.Next(1, 99) * 100000);
                String t = " bytes";
                String l = file.Length.ToString();
                if (file.Length > 1000000)
                {
                    l = (file.Length / 1000000).ToString();
                    t = " MB";
                }
                else if (file.Length > 1000)
                {
                    l = (file.Length / 1000).ToString();
                    t = " KB";
                }

                String nameEdit = file.Name;
                if (nameEdit.Contains(ServerManager.SIGN))
                    nameEdit.Replace(ServerManager.SIGN, '~');
                Book b = new Book.Builder()
                      .Id(CreateId(file))
                      .Name(nameEdit)
                      .Type(file.Extension)
                      .Price(p)
                      .Size(l + t)
                      .Path(file.FullName)
                      .Build();
                mList.Add(b);
            }
            return mList;
        }

        private static string CreateId(FileInfo file)
        {
            DateTime dateTime = new DateTime();
            if (file.CreationTime != null)
                dateTime = file.CreationTime;
            else if (file.LastAccessTime != null)
                dateTime = file.LastAccessTime;

            String id = file.CreationTime.Second
                + dateTime.Minute
                + dateTime.Hour
                + file.Length.ToString()
                + dateTime.Day
                + dateTime.Month
                + dateTime.Year;

            return id.Substring(0, 10);
        }
    }
}
