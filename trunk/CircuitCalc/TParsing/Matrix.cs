using System;
using System.Text;

namespace CircuitCalc.TParsing
{
	public class Matrix
	{
		public int height, width;
		public readonly int[][] items;

		public Matrix(int[][] items)
		{
			this.items = items;
			height = items.Length;
			if(height == 0) width = 0;
			else width = items[0].Length;
		}

		public Matrix(int height, int width)
		{
			this.height = height;
			this.width = width;
			items = new int[height][];
			for(int y=0; y<height; y++)
				items[y] = new int[width];
		}

		public Matrix Sub(Matrix m)
		{
			var r = new Matrix(height, width);
			for(int y = 0; y < height; y++)
				for(int x = 0; x < width; x++)
					r.items[y][x] = items[y][x] - m.items[y][x];
			return r;
		}

		public bool IsNonNegative()
		{
			for(int y = 0; y < height; y++)
				for(int x = 0; x < width; x++)
					if(items[y][x] < 0) return false;
			return true;
		}

		public Matrix Mult(Matrix b)
		{
			var a = this;
			var c = new Matrix(a.height, b.width);
			for(int y = 0; y < c.height; y++)
				for(int x = 0; x < c.width; x++)
					for(int k = 0; k < a.width; k++)
						c.items[y][x] += a.items[y][k] * b.items[k][x];
			return c;
		}


		private static readonly Random rnd = new Random((int) DateTime.Now.Ticks);
		public static Matrix Random(int size)
		{
			var m = new Matrix(size, size);
			for(int y = 0; y < size; y++)
				for(int x = 0; x < size; x++)
					m.items[y][x] = rnd.Next(5)+1;
			return m;
		}

		public override string ToString()
		{
			var b = new StringBuilder();
			for(int y=0; y<height; y++)
			{
				for(int x = 0; x < width; x++)
				{
					b.Append(" " + items[y][x]);
				}
				b.AppendLine();
			}
			return b.ToString();
		}

		public bool NonZero()
		{
			for(int y = 0; y < height; y++)
				for(int x = 0; x < width; x++)
					if(items[y][x] != 0) return true;
			return false;
			
		}

		public static Matrix Simple(int v)
		{
			var r = new Matrix(1,1);
			r.items[0][0] = v;
			return r;
		}
	}
}