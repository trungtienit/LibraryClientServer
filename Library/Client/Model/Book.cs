using Server.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public class Book
    {
        String id;
        String name;
        String type;
        double price;
        String size;
        String path;

        public string Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Type { get => type; set => type = value; }
        public double Price { get => price; set => price = value; }
        public string Size { get => size; set => size = value; }
        public string Path { get => path; set => path = value; }
  
        public Book()
        {
        }

        public Book(Builder builder)
        {
            this.id = builder.id;
            this.name = builder.name;
            this.type = builder.type;
            this.price = builder.price;
            this.size = builder.size;
            this.path = builder.path;
        }
        public string ToString()
        {
            return String.Format("{0}#{1}#{2}#{3}#{4}", id, name, type, price, size);
        }
        public class Builder
        {
            public String id;
            public String name;
            public String type;
            public double price;
            public String size;
            public String path;
            public Builder()
            {
                id = "";
                name = "";
                type = "";
                price = 0.0f;
                size = "";
                path = "";
            }
            public Builder Id(String id)
            {
                this.id = id;
                return this;
            }
            public Builder Name(String name)
            {
                this.name = name;
                return this;
            }
            public Builder Type(String type)
            {
                this.type = type;
                return this;
            }
            public Builder Price(double price)
            {
                this.price = price;
                return this;
            }
            public Builder Size(String size)
            {
                this.size = size;
                return this;
            }
            public Builder Path(String path)
            {
                this.path = path;
                return this;
            }
            public Book Build()
            {
                return new Book(this);
            }

   
      

        }


    }
}
