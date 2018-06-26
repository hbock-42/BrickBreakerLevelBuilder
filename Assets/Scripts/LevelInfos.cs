using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelInfos
{
	#region Properties

	public static int GridWidth { get; set; }
	public static int GridHeight { get; set; }

	#endregion

	#region Static Constructor

	/// <summary>
	/// This is called whenever we want to use unset values
	/// This allow us to test without manualy set this values in the HomeMenu scene
	/// </summary>
	static LevelInfos()
	{
		GridWidth = 10;
		GridHeight = 40;
	}

	#endregion

	#region Methods

	//public static void Save()
	//{
	//	//System.Xml.Linq
	//}

	#endregion
}
