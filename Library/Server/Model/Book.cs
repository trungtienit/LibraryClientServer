using Client.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public class Book
    {
        String id;
        String name;
        String type;
        double price;
        String size;
        String path;
        Boolean onDrive;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        public double Price
        {
            get { return price; }
            set { price = value; }
        }
        public string Size
        {
            get { return size; }
            set { size = value; }
        }
        public string Path
        {
            get { return path; }
            set { path = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public Book()
        {
        }
        public Book(Book b)
        {
            this.id = b.id;
            this.name = b.name;
            this.type = b.type;
            this.price = b.price;
            this.size = b.size;
            this.path = b.path;
            this.onDrive = b.onDrive;
        }
        public Book(Builder builder)
        {
            this.id = builder.id;
            this.name = builder.name;
            this.type = builder.type;
            this.price = builder.price;
            this.size = builder.size;
            this.path = builder.path;
            this.onDrive = builder.onDrive;
        }
        override
        public string ToString()
        {
            // char sign = ServerManager.SIGN;
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
            public Boolean onDrive;
            public Builder()
            {
                id = "";
                name = "";
                type = "";
                price = 0.0f;
                size = "";
                path = "";
                onDrive = false;
            }
            public Builder Id(String id)
            {
                this.id = id;
                return this;
            }
            public Builder OnDrive()
            {
                this.onDrive = true;
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
