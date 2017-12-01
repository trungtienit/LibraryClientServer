using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Client.View
{
    public partial class ChangeBookForm : Form
    {
        public ChangeBookForm()
        {
            InitializeComponent();
        }
        public String nameCurrent;
        public int a=0;

        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                String name = openFileDialog1.FileName;
                textBox1.Text= name ;
                btnAccept.Enabled = true;
            }
        }

        private void ChangeBookForm_Load(object sender, EventArgs e)
        {
            btnAccept.Enabled = false;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            nameCurrent = textBox1.Text.ToString();
        }
    }
}
