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
		private readonly Chamber[] chambers;
		/// <summary>
		/// Количество баков от 0 до 6
		/// </summary>
		private readonly int numOfTanks;

		public Car(Chamber[] chambers, int numOfTanks)
		{
			this.chambers = chambers;
			this.numOfTanks = numOfTanks;
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

	}
}