using System;
using System.Windows.Forms;
using CircuitCalc;
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
            try
            {
                var buildFactory = BuildFactory(input).Trim();
                textBox2.Text = buildFactory;
                var output =
                    new Calculator(buildFactory.Split(new[] {Environment.NewLine}, StringSplitOptions.None)).
                PushString(MakeInput(input));
                Text = output;
            }
            catch
            {
                textBox2.Text = "";
            }
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

        private int countGroups(string text)
        {
            if (string.IsNullOrEmpty(text)) return -1;
            if (text[0] != '(') return text.Split(',').Length;
            int brace = 0;
            int count = 0;
            for (int i = 0; i < text.Length; ++i)
            {
                if (text[i] == '(') ++brace;
                if (text[i] == ')')
                {
                    --brace;
                    if (brace == 0) ++count;
                }
            }
            if (brace != 0) return -1;
            return count;
        }

        private string GetPrefix(int number)
        {
            string[] res = new[] {"0", "1", "220", "2210", "2211", "2212", "2222000"};
            return (number >= 0 && number < res.Length) ? res[number] : "<bad>";
        }

        private string Conv3(int number, int pad)
        {
            string result = "";
            while (number > 0)
            {
                result = (number % 3) + result;
                number /= 3;
            }
            return result.PadLeft(pad, '0');
        }

	    private string GetNumber(int number)
        {
            if (number >= 4)
            {
                if (number <= 12)
                {
                    return "220" + Conv3(number - 4, 2);
                }
                else if (number <= 39)
                {
                    return "2210" + Conv3(number - 13, 3);
                }
                else if (number <= 120)
                {
                    return "2211" + Conv3(number - 40, 4);
                }
                else if (number <= 363)
                {
                    return "2212" + Conv3(number - 121, 5);
                }
                else if (number <= 1092)
                    return "2222000" + Conv3(number - 364, 6);
                else
                    return "<unimpl>";
            }
            else if (number >= 0)
            {
                string[] res1 = new[] { "0", "10", "11", "12" };
                return res1[number];
            }
            else 
                return "<undef>";
        }

	    private string ParseMatrix(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            if (text[0] == '(')
            {
                int count = countGroups(text);
                if (count == 1)
                {
                    string inner = text.Substring(1, text.Length - 2);
                    return GetPrefix(countGroups(inner)) + ParseMatrix(inner);
                }
                else if (count >= 2)
                {
                    int pos = 0, brace = 0;
                    string result = "";
                    for(int i = 0; i < text.Length; ++i)
                    {
                        if (text[i] == '(') ++brace;
                        if (text[i] == ')')
                        {
                            --brace;
                            if (brace == 0)
                            {
                                result += ParseMatrix(text.Substring(pos, i - pos + 1));
                                pos = i + 1;
                            }
                        }
                    }
                    return result;
                }
                else
                {
                    return "<error>";
                }
            }
            else // парсим список
            {
                string[] list = text.Split(',');
                string result = "";
                for(int i = 0; i < list.Length; ++i)
                {
                    try
                    {
                        int num = int.Parse(list[i]);
                        result += GetNumber(num);
                    }
                    catch
                    {
                        result += "<err>";
                    }
                }
                return result;
            }
        }

	    private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = ParseMatrix(textBox3.Text.Replace("\r", "").Replace("\n", "").Replace(" ", ""));
        }
	}
}
