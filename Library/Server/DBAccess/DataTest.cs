using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    class DataTest
    {
        public static List<Book> GetListBook()
        {
            List<Book> books = new List<Book>();
            for(int i = 0; i < 10; i++)
            {
                Book b = new Book.Builder()
                        .Id(i+"")
                        .Name("Book "+ i)
                        .Type("pdf")
                        .Price(0.0f)
                        .Size("128kb")
                        .Path("")
                        .Build();
                books.Add(b);
            }
            return books;
        }
    }
}
