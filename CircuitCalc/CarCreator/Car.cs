using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CircuitCalc.TParsing;

namespace CircuitCalc.CarCreator
{
	/// <summary>
	///  Класс описывающи отдельную машинку
	/// Подразумевается что у нас не более 6 баков и они пронумерованны от 0 до 5
	/// </summary>

	class Car
	{
		private Chamber[] chambers;
		/// <summary>
		/// Количество баков от 0 до 6
		/// </summary>
		private readonly int numOfTanks;

		private Matrix[] fuel;
		private static readonly TEncoder encoder = new TEncoder();
		private Comparison<Chamber> sortByChambers =
			(a, b) => String.Compare(encoder.EncodeChamber(a), encoder.EncodeChamber(b));

		private static int[][] permutations = new int[720][];
		private static int[] next(int [] arr)
		{
			var res = (int[]) arr.Clone();
			int i = arr.Length - 1;
			for (i = arr.Length - 2; i >= 0 && res[i]>res[i+1]; i--) ;
			if(i>=0)
			{
				var j = i + 1;
				while (j < res.Length-1 && res[j+1] > res[j])
				{
					++j;
				}
				var tmp = res[i];
				res[i] = res[j];
				res[j] = tmp;

				Array.Reverse(res,j+1, res.Length - j-1);
 			}
			return res;
		}
		static Car() 
		{
			permutations[0] = new int[]{0,1,2,3,4,5};
			for (var i = 1; i < permutations.Length; i++)
			{
				permutations[i] = next((int[])(permutations[i-1].Clone()));
			}
		}

		public Car(Chamber[] chambers, int numOfTanks)
		{
			this.chambers = chambers;
			this.numOfTanks = numOfTanks;
		}
		public Car(Chamber[] chambers, int numOfTanks, Matrix[] fuel)
		{
			this.chambers = chambers;
			this.numOfTanks = numOfTanks;
			this.fuel = fuel;
		}

		public Chamber[] GetChambers()
		{
			return chambers;
		}
		public Matrix[] GetFuel()
		{
			return fuel;
		}
		public bool IsConnected()
		{
			// Создаём пустую матрицу связности
			var connected = new bool[numOfTanks,numOfTanks];
			for (var i = 0; i < numOfTanks; ++i )
			{
				for (var j = 0; j < numOfTanks; ++j )
				{
					connected[i, j] = false;
				}
			}
			// Инициализируем отношение непосредственной связности
			// Все баки в данном Chamber, которые подключены к верхней, связанны непосредственно
			// с теми что подключены к нижней
			foreach (var chamber in chambers)
			{
				foreach (var upTrank in chamber.upper)
				{
					foreach (var downTrank in chamber.lower)
					{
						connected[upTrank, downTrank] = true;
					}
				}
			}

			// Не непосредственная связность. Тут можно сделать в точности 6 шагов.

			for (int i = 0; i < 6; ++i )
			{
				for(int tank = 0; tank < numOfTanks; ++tank)
				{
					for (int connectedTank = 0; connectedTank < numOfTanks; connectedTank++)
					{
						if (connected[tank, connectedTank])
						{
							for (int k = 0; k < numOfTanks; k++)
							{
								connected[tank, k] |= connected[connectedTank, k];
							}
						}
					}
				}
			}

			for (int i = 0; i < numOfTanks; i++)
			{
				for (int j = 0; j < numOfTanks; j++)
				{
					if (!connected[i, j])
					{
						return false;
					}
				}
			}

			return true;
		}
		public void Normalize()
		{

			// попробуем ещё по пребирать перестановки Чамберов
			// Инициализируем тождественную
			var per2 = new int[chambers.Length];
			for (int i = 0; i < per2.Length; i++)
			{
				per2[i] = i;
			}
			var best = DeepCopyChambers(chambers);
			var origBest = DeepCopyChambers(best);
			for (int i = 0; i < 10000; i++)
			{
				var nextPer = new int[per2.Length];
				Array.Copy(per2, nextPer, nextPer.Length);
				nextPer = next(nextPer);
				var nextChmp = new Chamber[origBest.Length];
				var tmp = DeepCopyChambers(origBest);
				for (int j = 0; j < origBest.Length; j++)
				{
					nextChmp[j] = tmp[nextPer[j]];
				}
				nextChmp = OptimizeByTanks(nextChmp);
				var bestCode = encoder.EncodeCar(best);
				var nextCode = encoder.EncodeCar(nextChmp);
				if (nextCode.Length < bestCode.Length)
				{
					best = nextChmp;
					continue;
				}
				if (nextCode.Length > bestCode.Length)
				{
					continue;
				}
				if (String.CompareOrdinal(bestCode, nextCode) > 0)
				{
					Console.WriteLine("b!:" + bestCode);
					Console.WriteLine("n!:" + nextCode);
					best = nextChmp;
				}

			}

			chambers = best;
		}

		public Chamber[] OptimizeByTanks(Chamber[] orig)
		{
			var best = DeepCopyChambers(orig);

			for (int i = 1; i < 720; i++)
			{
				var next = DeepCopyChambers(orig);
				ApplyPermutation(next, permutations[i]);
				var bestCode = encoder.EncodeCar(best);
				var nextCode = encoder.EncodeCar(next);
				if (nextCode.Length < bestCode.Length)
				{
					best = next;
					continue;
				}
				if (nextCode.Length > bestCode.Length)
				{
					continue;
				}
				if (String.CompareOrdinal(bestCode, nextCode) > 0)
				{
					Console.WriteLine("b:" + bestCode);
					Console.WriteLine("n:" + nextCode);
					best = next;
				}
			}
			return best;
		}

		private Chamber[] DeepCopyChambers(Chamber[] from)
		{
			Chamber[] res = new Chamber[from.Length];
			for (int i = 0; i < from.Length; i++)
			{
				res[i] = new Chamber();
				res[i].isMaster = from[i].isMaster;
				res[i].lower = new int[from[i].lower.Length];
				res[i].upper = new int[from[i].upper.Length];

				Array.Copy(from[i].lower, res[i].lower, from[i].lower.Length);
				Array.Copy(from[i].upper, res[i].upper, from[i].upper.Length);
			}
			return res;
		}
		private void ApplyPermutation(Chamber[] chmbs, int[] permutation)
		{
			foreach (var chamber in chmbs)
			{
				for(int i = 0; i < chamber.lower.Length; ++i)
				{
					chamber.lower[i] = permutation[chamber.lower[i]];
				}
				for (int i = 0; i < chamber.upper.Length; ++i)
				{
					chamber.upper[i] = permutation[chamber.upper[i]];
				}
			}
		}


		public override string ToString()
		{
			var buld = new StringBuilder();
			buld.Append("Chambers:\n");
			foreach (var chamber in chambers)
			{
				buld.Append(chamber.ToString());
				buld.Append("\n");
			}
			buld.Append("Fuel\n");
			foreach (var matrix in fuel)
			{
				buld.Append(matrix.ToString());
				buld.Append("\n");
			}
			return buld.ToString();
		}

	}
}