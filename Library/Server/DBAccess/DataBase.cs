using Server;
using Server.Api;
using Server.Common;
using Server.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Server
{
    class DataBase
    {
        public const String PATH_CONFIG = "..\\..\\Config";
        public const String PATH_DB_XML = PATH_CONFIG + "\\BooksDB.xml";
        public const String PATH_DB_DIR = PATH_CONFIG + "\\Data";
        public const String PATH_CACHE = PATH_CONFIG + "\\Preview";
        public const String PATH_DB_USER_XML = PATH_CONFIG + "\\UsersDB.xml";
        public static string[] EXTENSIONS = new[] { ".doc", ".pdf", ".docx", ".xls", ".xlsm", ".txt",".ppt",".pptx" };
        public static List<Book> books;
        public DataBase() { }
        public static ApiManager apiManager = new ApiManager();
        public static List<Book> GetListBook()
        {

            if (books == null)
            {
                if (!File.Exists(PATH_DB_XML))
                    WriteNewDB();
                else
                    LoadDB();
            }

            return books;
        }

        //TODO Load database from xml
        public static void LoadDB()
        {
            books = new List<Book>();
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Book>));
                using (FileStream fs = new FileStream(PATH_DB_XML, FileMode.Open, FileAccess.Read))
                {
                    books = serializer.Deserialize(fs) as List<Book>;
                    Console.Write("Database load");
                }

            }
            catch (Exception E)
            {
                Console.Write("DB ERROR :" + E.StackTrace);
            }
        }

        internal static void AddRangeNewBook(List<Book> list)
        {
            foreach (Book b in list)
                AddNewBook(b);
        }

        //TODO Write listbooks --> file xml
        public static void WriteNewDB()
        {
            if (books == null)
                books = new List<Book>();
            else
                books.Clear();
            ServerManager serverManager = new ServerManager();
            books = serverManager.GetAllDataLocal(DataBase.PATH_DB_DIR, EXTENSIONS);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Book>));
                using (FileStream fs = new FileStream(PATH_DB_XML, FileMode.Create, FileAccess.Write))
                {
                    serializer.Serialize(fs, books);
                    Console.Write("Database updated");
                }

            }
            catch (Exception E)
            {
                Console.Write("DB Write ERROR" + E.StackTrace);
            }
        }
        public static void AddNewBook(Book b)
        {
            //Save to xml
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(PATH_DB_XML);
            }
            catch (Exception E)
            {
                DataBase.WriteNewDB();
                doc.Load(PATH_DB_XML);
            }
            XmlNode book = doc.CreateElement("Book");
            XmlNode id = doc.CreateElement("Id");
            id.InnerText = b.Id;
            XmlNode name = doc.CreateElement("Name");
            name.InnerText = b.Name;
            XmlNode type = doc.CreateElement("Type");
            type.InnerText = b.Type;
            XmlNode price = doc.CreateElement("Price");
            price.InnerText = b.Price.ToString();
            XmlNode size = doc.CreateElement("Size");
            size.InnerText = b.Size;
            XmlNode path = doc.CreateElement("Path");
            path.InnerText = b.Path;

            book.AppendChild(id);
            book.AppendChild(name);
            book.AppendChild(type);
            book.AppendChild(price);
            book.AppendChild(size);
            book.AppendChild(path);

            doc.DocumentElement.AppendChild(book);
            doc.Save(PATH_DB_XML);

            //Save to listbooks
            books.Add(b);
        }

        internal static Book GetFilePreview(string id)
        {
            Book b = GetFile(id);

            if (b == null)
                return null;
            Book tmp = new Book(b);
            if (tmp.Id.StartsWith("GD") )
            {
                apiManager = new ApiManager();
                tmp.Path = apiManager.DownloadGoogleFile(tmp.Path);
                tmp.OnDrive = false;
            }
            FileUtils f = new FileUtils();
            String rs = f.ConvertPdfPreview(tmp.Path);
            tmp.Path = rs;
            return tmp;
        }

        internal static Book GetFile(string id)
        {
            foreach (Book b in books)
                if (id.Equals(b.Id))
                    return b;
            return null;
        }

        public static void DeleteBook(Book b)
        {
            //Remove book in xml
            XmlDocument doc = new XmlDocument();
            doc.Load(PATH_DB_XML);
            foreach (XmlNode xNode in doc.SelectNodes("ArrayOfBook/Book"))
                if (xNode.SelectSingleNode("Id").InnerText == b.Id)
                    xNode.ParentNode.RemoveChild(xNode);
            doc.Save(PATH_DB_XML);

            //Remove book in listbooks
            books.Remove(b);
        }
    }

}
