
public class LevelInfosExport
{
	#region Fields

	private int[,] _brickLevelArray;

	#endregion

	#region Properties

	public int[,] BrickLevelArray { get; set; }

	#endregion

	#region Constructor

	public LevelInfosExport(int[,] brickLevelArray)
	{
		_brickLevelArray = brickLevelArray;
	}

	#endregion
}