﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Client.View
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
            textBox1.Text = book.Name;
            textBox2.Text = book.Size;
            textBox3.Text = book.Price+"";
        }
    }
}