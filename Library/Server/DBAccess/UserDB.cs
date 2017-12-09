using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Server.DBAccess
{
    class UserDB
    {


        public static List<User> users;
        public static void LoadUsersDB()
        {

            users = new List<User>();
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
                using (FileStream fs = new FileStream(DataBase.PATH_DB_USER_XML, FileMode.Open, FileAccess.Read))
                {
                    users = serializer.Deserialize(fs) as List<User>;
                    Console.Write("Users load");
                }

            }
            catch (Exception E)
            {
                Console.Write("DB User ERROR :" + E.StackTrace);
            }
        }
        public static void WriteNewUserDB()
        {
            if (users == null)
                users = new List<User>();
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
                using (FileStream fs = new FileStream(DataBase.PATH_DB_USER_XML, FileMode.Create, FileAccess.Write))
                {
                    serializer.Serialize(fs, users);
                    Console.Write("Users updated");
                }
            }
            catch (Exception E)
            {
                Console.Write("DB Write ERROR" + E.StackTrace);
            }
        }
        public static void AddNewUser(User u)
        {
            //Save to xml
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(DataBase.PATH_DB_USER_XML);
            }
            catch (Exception E)
            {
                WriteNewUserDB();
                doc.Load(DataBase.PATH_DB_USER_XML);
            }
            XmlNode user = doc.CreateElement("User");
            XmlNode name = doc.CreateElement("Name");
            name.InnerText = u.Name;
            XmlNode pass = doc.CreateElement("Pass");
            pass.InnerText = u.Pass;
      
            XmlNode wallet = doc.CreateElement("Wallet");
            wallet.InnerText = u.Wallet.ToString();

            user.AppendChild(name);
            user.AppendChild(pass);
            user.AppendChild(wallet);

            doc.DocumentElement.AppendChild(user);
            doc.Save(DataBase.PATH_DB_USER_XML);

            //Save to listbooks
            if (users == null)
                users = new List<User>();
            users.Add(u);
        }
        public static User Login(String name, String pass)
        {
            if (users == null)
                WriteNewUserDB();
            foreach(User u in users)
            {
                if (u.Name.Equals(name))
                    if (u.Pass.Equals(pass))
                        return u;
            }
            return null;
        }

        internal static bool Exist(string name)
        {
            if (users == null)
                return false;
            foreach (User u in users)
            {
                if (u.Name.Equals(name))
                    return true;
            }
            return false;
        }
    }
}
