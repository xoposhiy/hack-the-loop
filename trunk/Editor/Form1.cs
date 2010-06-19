using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CircuitCalc.CircuitBuilding;

namespace Editor
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			string input = textBox1.Text;
			textBox2.Text = BuildFactory(input);
		}

		private string BuildFactory(string input)
		{
			var bytes = new Builder("01202101210201202" + new string('0', input.Length), "11021210112101221" + input).Build();
			var factory = new CircuitSerializer().Serialize(bytes);
			Clipboard.SetText(factory);
			return factory;
		}
	}
}
