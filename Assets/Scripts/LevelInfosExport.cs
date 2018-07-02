
public class LevelInfosExport
{
	#region Fields

	#endregion

	#region Properties

	public int[,] BrickLevelArray { get; private set; }

	#endregion

	#region Constructor

	public LevelInfosExport(int[,] brickLevelArray)
	{
		BrickLevelArray = brickLevelArray;
	}

	#endregion
}