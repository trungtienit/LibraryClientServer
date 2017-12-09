using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Server.View
{
    public partial class AddUserForm : Form
    {
        public AddUserForm()
        {
            InitializeComponent();
        }
        public String name;
        public String pass;

        private void btnLogin_Click(object sender, EventArgs e)
        {
            name = tbUserName.Text;
            pass = tbPassword.Text;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
