using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WebClient
{
	class Program
	{
		static void Main(string[] args)
		{
			var c = new WebClient();
			c.GetCarsList();
		}
	}
}
