namespace Data
{
	// Todo: Avoid class duplication by using a common project for shared classes
	public class Level
	{
		#region Properties

		public int[,] BrickLevelArray { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }

		#endregion

		#region Constructor

		public Level(int[,] brickLevelArray)
		{
			BrickLevelArray = brickLevelArray;
			Width = BrickLevelArray.GetLength(1);
			Height = BrickLevelArray.GetLength(0);
		}

		#endregion
	}
}