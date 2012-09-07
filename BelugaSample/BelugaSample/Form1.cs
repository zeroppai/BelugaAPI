using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BelugaSample
{
    public partial class Form1 : Form
    {
		Beluga belga = new Beluga();
		const String moja_room = "11eJDfcF96UIc";

        public Form1()
        {
            InitializeComponent();
			Update();
        }

		private void Update()
		{
			var timeline = belga.getRoom(moja_room);
			foreach (var item in timeline)
			{
				richTextBox1.Text += item.id + "-------------------------------\n";
				richTextBox1.Text += item.user + "\n";
				richTextBox1.Text += item.text + "\n";
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (textBox1.Text != "")
				belga.postText(textBox1.Text, moja_room);
			Update();
		}
    }
}
