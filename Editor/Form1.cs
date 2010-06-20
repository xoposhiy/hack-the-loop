using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CircuitCalc.CircuitBuilding;
using CircuitCalc.PeCalc;

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
			var buildFactory = BuildFactory(input).Trim();
			textBox2.Text = buildFactory;
			var output = new Calculator(buildFactory.Split(new string[] {Environment.NewLine}, StringSplitOptions.None)).PushString(MakeInput(input));
			Text = output;
		}

		private string BuildFactory(string suffix)
		{
			var bytes = new Builder(MakeInput(suffix), Consts.keyPrefix + suffix).Build();
			var factory = new CircuitSerializer().Serialize(bytes);
			Clipboard.SetText(factory);
			return factory;
		}

		private string MakeInput(string input)
		{
			var part = Consts.serverInput;
			var s = part;
			while(s.Length <= part.Length + input.Length)
				s += part;
			return s;
		}
	}
}
