using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.DBAccess
{
   public class User
    {
        public String name;
        public String pass;
        public double wallet;
        public Builder builder;

        public User()
        {
        }

        public User(Builder builder)
        {
            this.name = builder.name;
            this.pass = builder.pass;
            this.wallet = builder.wallet;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Pass
        {
            get { return pass; }
            set { pass = value; }
        }
        public double Wallet
        {
            get { return wallet; }
            set { wallet = value; }
        }
        public class Builder
        {
            public String name;
            public String pass;
            public double wallet;

            public Builder()
            {
                this.name = "";
                this.pass = "";
                this.wallet = 0.0;
            }

            public Builder Name(string name)
            {
                this.name = name;
                return this;
            }
            public Builder Pass(string pass)
            {
                this.pass = pass;
                return this;
            }
            public Builder Wallet(double wallet)
            {
                this.wallet = wallet;
                return this;
            }
            public User Build()
            {
                return new User(this);
            }
        }
    }
}
