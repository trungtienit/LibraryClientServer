using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Client.Common
{

    public class ServerManager
    {
        public const String PDF = "mimeType = 'application/pdf'";
        public const String PLAIN_TEXT = "mimeType = 'text/plain'";
        public const String RICH_TEXT = "mimeType = 'application/rtf'";
        public const String EXCEL = "mimeType = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'";
        public const String MSWORD = "mimeType = 'application/vnd.openxmlformats-officedocument.wordprocessingml.document'";
        public const String OFFICE_DOC = "mimeType = 'application/vnd.oasis.opendocument.text'";

        public const char SIGN = '#';
        public const byte TYPE = 1;
        public const byte TYPE_SEARCH = TYPE << 1;
        public const byte TYPE_DOWNLOAD = TYPE << 2;
        public const byte TYPE_PREVIEW = TYPE << 3;
        public const byte TYPE_VIEW_FULL = TYPE << 4;
        public const byte TYPE_CHANGE = TYPE << 5;

        public const byte TYPE_FILE = 1;
        public const byte TYPE_FILE_MSWORD = TYPE_FILE<< 1;
        public const byte TYPE_FILE_PLAIN_TEXT = TYPE_FILE << 2;
        public const byte TYPE_FILE_PDF = TYPE_FILE << 3;
        public const byte TYPE_FILE_RICH_TEXT = TYPE_FILE << 4;
        public const byte TYPE_FILE_EXCEL = TYPE_FILE << 5;
        public const byte TYPE_FILE_OFFICE_DOC = TYPE_FILE << 6;

        public static int BUFFER_SIZE = 1024 ;

        public List<Book> GetAllDataLocal(string pathFolder, string[] fileTypes)
        {

            List<Book> mList = new List<Book>();
            if (!System.IO.Directory.Exists(DataBase.PATH_DB))
                System.IO.Directory.CreateDirectory(DataBase.PATH_DB);
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

                double p = (r.Next(1, 99) * 10000);
                
               
                Book b = new Book.Builder()
                      .Id(CreateId(file))
                      .Name(CustomName(file.Name))
                      .Type(file.Extension)
                      .Price(p)
                      .Size(CustomSize(file.Length))
                      .Path(file.FullName)
                      .Build();
                mList.Add(b);
            }
            return mList;
        }
        public List<string> FindBook(string name, string type)
        {
            List<String> l = new List<string>();
            if (type.Equals("All"))
            {
                foreach (Book b in DataBase.GetListBook())
                {
                    if (b.Name.ToUpper().Contains(name.ToUpper()))
                        l.Add(b.ToString());
                }
            }
            else
            {

                if (type.Equals("Word"))
                {
                    foreach (Book b in DataBase.GetListBook())
                    {
                        if (b.Name.ToUpper().Contains(name.ToUpper())
                                  && (b.Type.ToUpper().Equals(".DOC")
                                  || b.Type.ToUpper().Equals(".DOCX")))
                            l.Add(b.ToString());
                    }
                }
                if (type.Equals("Exel"))
                {
                    foreach (Book b in DataBase.GetListBook())
                    {
                        if (b.Name.ToUpper().Contains(name.ToUpper())
                            && (b.Type.ToUpper().Equals(".XLSX")
                            || b.Type.ToUpper().Equals(".XLSM")
                            || b.Type.ToUpper().Equals(".XLS")))
                            l.Add(b.ToString());
                    }
                }
                if (type.Equals("Pdf"))
                {
                    foreach (Book b in DataBase.GetListBook())
                    {
                        if (b.Name.ToUpper().Contains(name.ToUpper())
                             && b.Type.ToUpper().Equals(".PDF"))
                            l.Add(b.ToString());
                    }
                }
                if (type.Equals("Text"))
                {
                    foreach (Book b in DataBase.GetListBook())
                    {
                        if (b.Name.ToUpper().Contains(name.ToUpper())
                           && b.Type.ToUpper().Equals(".TXT"))
                            l.Add(b.ToString());
                    }
                }

            }


            return l;
        }
        public string CustomName(string name)
        {
            String nameEdit = name;
            if (nameEdit.Contains(ServerManager.SIGN))
                nameEdit.Replace(ServerManager.SIGN, '~');
            return nameEdit;
        }

        public string CustomSize(long length)
        {
            String t = " bytes";
            String l = length.ToString();
            if (length > 1000000)
            {
                l = (length / 1000000).ToString();
                t = " MB";
            }
            else if (length > 1000)
            {
                l = (length / 1000).ToString();
                t = " KB";
            }
            return l + t;
        }

        public string CreateId(FileInfo file)
        {
            DateTime dateTime = new DateTime();
            if (file.CreationTime != null)
                dateTime = file.CreationTime;
            else if (file.LastAccessTime != null)
                dateTime = file.LastAccessTime;

            String id = dateTime.Second
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
