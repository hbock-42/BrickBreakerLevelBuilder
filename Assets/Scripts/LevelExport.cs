﻿using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

public static class LevelExport
{
	private static string _fileName = "LevelTest.xml";

	/// <summary>
	/// Convert level informations to an Xml file
	/// </summary>
	public static void ToXml(LevelInfosExport levelInfosExport)
	{
		var xDoc = FormatXml(levelInfosExport);
		xDoc.Save(Path.Combine(SetupStrings.LevelSavePath, _fileName));
	}

	private static XDocument FormatXml(LevelInfosExport levelInfosExport)
	{
		var xDoc = new XDocument();

		xDoc.Add(new XElement(
			"Level",
			new XAttribute("Width", levelInfosExport.BrickLevelArray.GetLength(1)),
			new XAttribute("Height", levelInfosExport.BrickLevelArray.GetLength(0))
		));
		var levelElem = xDoc.Element("Level");
		// ReSharper disable once PossibleNullReferenceException
		levelElem.Value = Int2X2ToCsv(levelInfosExport.BrickLevelArray);
		return xDoc;
	}

	/// <summary>
	/// Convert a 2 x 2 int array in csv
	/// </summary>
	/// <param name="intMatrix"></param>
	/// <returns></returns>
	private static string Int2X2ToCsv(int[,] intMatrix)
	{
		var csvSb = new StringBuilder();

		var yMax = intMatrix.GetLength(0);
		var xMax = intMatrix.GetLength(1);
		for (var y = 0; y < yMax; y++)
		{
			for (var x = 0; x < xMax; x++)
			{
				csvSb.Append(intMatrix[y, x]);
				if (x == xMax - 1) continue;
				csvSb.Append(",");
			}
			csvSb.Append(Environment.NewLine);
		}

		return csvSb.ToString();
	}
}