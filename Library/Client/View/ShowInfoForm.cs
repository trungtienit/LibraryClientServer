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
    public partial class ShowInfoForm : Form
    {
        public ShowInfoForm()
        {
            InitializeComponent();
        }
         public Book book;
        private void ShowInfoForm_Load(object sender, EventArgs e)
        {
            if (book.Name.ToUpper().EndsWith("TXT") || book.Name.ToUpper().EndsWith("XLS") || book.Name.ToUpper().EndsWith("PPT"))
                btnPreview.Enabled = false;
            else btnPreview.Enabled = true;

            textBox1.Text = book.Name;
            textBox2.Text = book.Size;
            textBox3.Text = book.Price+"";
           
        }

        private void btnBuy_Click(object sender, EventArgs e)
        {

        }

        private void btnPreview_Click(object sender, EventArgs e)
        {

        }
    }
}
