using Library;
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

namespace Library
{
    class DataBase
    {
        private const String PATH = "..\\..\\..\\..\\TestData.xml";
        public static List<Book> books;
        public DataBase() { }
        public static List<Book> GetListBook()
        {
            if (books == null)
                LoadDB();
            return books;
        }
        public static void LoadDB()
        {

            try
            {
                books = new List<Book>();

                XmlSerializer serializer = new XmlSerializer(typeof(List<Book>));
                using (FileStream fs = new FileStream(PATH, FileMode.Open, FileAccess.Read))
                {
                    books = serializer.Deserialize(fs) as List<Book>;
                    Console.Write("Database Created");
                }

            }
            catch (Exception E)
            {
                Console.Write("DB ERROR :" + E.StackTrace);
            }
        }
        public static void InitDB(List<Book> books)
        {
            try
            {
           
                XmlSerializer serializer = new XmlSerializer(typeof(List<Book>));
                using (FileStream fs = new FileStream(PATH, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    serializer.Serialize(fs, books);
                    Console.Write("Database updated");
                }
                AddNewBook(DataTest.GetListBook()[9]);

            }
            catch (Exception E)
            {
                Console.Write("DB ERROR");
            }
        }
        public static void AddNewBook(Book b)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(PATH);
            XmlNode book = doc.CreateElement("Book");
            XmlNode id = doc.CreateElement("Id");
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
            doc.Save(PATH);
        }
        public static void DeleteBook(Book b)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(PATH);
            foreach (XmlNode xNode in doc.SelectNodes("ArrayOfBook/Book"))
                if (xNode.SelectSingleNode("Id").InnerText == b.Id)
                    xNode.ParentNode.RemoveChild(xNode);
            doc.Save(PATH);
        }
    }

}
