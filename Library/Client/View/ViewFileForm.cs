using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Server
{
    public partial class ViewFile : Form
    {
        internal string bookCurrent=null;

        public ViewFile()
        {
            InitializeComponent();
        }

        private void ViewFile_Load(object sender, EventArgs e)
        {
          
        }
        public void UpdateBookCurrent()
        {
           webBrowser1.Navigate(bookCurrent);
            if (bookCurrent != null)
                btnReload.Visible = false;

        }
        private void btnReload_Click(object sender, EventArgs e)
        {
             webBrowser1.Navigate(bookCurrent);
            if (bookCurrent != null)
                btnReload.Visible = false;
        }
    }
}
