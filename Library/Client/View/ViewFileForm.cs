using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class ViewFile : Form
    {
        public ViewFile()
        {
            InitializeComponent();
        }

        private void ViewFile_Load(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            String a = openFileDialog1.FileName;
            webBrowser1.Navigate(a);
        }
    }
}
