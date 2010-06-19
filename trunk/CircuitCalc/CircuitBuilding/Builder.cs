using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CircuitCalc.CircuitBuilding
{
	class Builder
	{
		private readonly string input;
		private readonly string wantedOutput;

		public Builder(string input, string wantedOutput)
		{
			this.input = input;
			this.wantedOutput = wantedOutput;
		}
		
		/// <returns>i-ый элемент - это out(i, 0) - 0, 1 или 2</returns>
		public byte[] Build()
		{
			//TODO
			return new byte[0];
		}
	}
}
