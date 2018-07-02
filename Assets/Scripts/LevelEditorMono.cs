using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pair
{
	public int X;
	public int Y;
}

public class LevelEditorMono : MonoBehaviour
{
	

	#region Serialized Fields

	[SerializeField] [Tooltip("Color of the brick with the lowest level")] private Color _minColor;
	[SerializeField] [Tooltip("Color of the brick with the highest level")] private Color _maxColor;

	[SerializeField] private GameObject _buttonPrefab;
	[SerializeField] private RectTransform _gridCanvasRectTransform;
	[SerializeField] private GameObject _gridCellsGameObject;
	[SerializeField] private GameObject _menuPanel;

	#endregion

	#region Fields

	private GridLayoutGroup _gridCellsGridLayoutGroup;

	#region Button

	private GameObject[,] _buttonsGameObjectsArray;
	private Button[,] _buttonsArray;
	private Image[,] _buttonsImageArray;
	private GameObject[,] _buttonsFrameGameObjectsArray;
	private Text[,] _buttonsTextArray;
	private int[,] _brickLevelArray;

	#endregion


	/// <summary>
	/// x value of the currently selected button
	/// </summary>
	private int _xCurrent;
	/// <summary>
	/// y value of the currently selected button
	/// </summary>
	private int _yCurrent;

	#endregion

	#region Properties

	private float CellWidth { get; set; }
	private float CellHeight { get; set; }

	private float GridCanvasWidth { get { return _gridCanvasRectTransform.rect.width; } }
	private float GridCanvasHeight { get { return _gridCanvasRectTransform.rect.height; } }

	private GridLayoutGroup GridCellsGridLayoutGroup
	{
		get
		{
			if (_gridCellsGridLayoutGroup == null)
			{
				_gridCellsGridLayoutGroup = _gridCellsGameObject.GetComponent<GridLayoutGroup>();
			}

			return _gridCellsGridLayoutGroup;
		}
	}

	public List<Pair> Selected { get; set; }

	private bool MultiSelection { get; set; }

	#endregion

	#region Monobehaviour

	private void Start()
	{
		// Calculate width and heigh of a cell
		CellWidth = GridCanvasWidth / LevelInfos.GridWidth;
		CellHeight = GridCanvasHeight / LevelInfos.GridHeight;

		// Set width and heigh calculated previously in the GridLayoutGroup
		GridCellsGridLayoutGroup.cellSize = new Vector2(CellWidth, CellHeight);

		_brickLevelArray = new int[LevelInfos.GridHeight, LevelInfos.GridWidth];

		InstantiateCellButtons();

		Selected = new List<Pair>();
	}

	private void Update()
	{
		ManageInputs();
	}

	#endregion

	#region Methods

	private void InstantiateCellButtons()
	{
		_buttonsGameObjectsArray = new GameObject[LevelInfos.GridHeight, LevelInfos.GridWidth];
		_buttonsArray = new Button[LevelInfos.GridHeight, LevelInfos.GridWidth];
		_buttonsImageArray = new Image[LevelInfos.GridHeight, LevelInfos.GridWidth];
		_buttonsFrameGameObjectsArray = new GameObject[LevelInfos.GridHeight, LevelInfos.GridWidth];
		_buttonsTextArray = new Text[LevelInfos.GridHeight, LevelInfos.GridWidth];

		for (var y = 0; y < LevelInfos.GridHeight; y++)
		{
			var tmpY = y;
			for (var x = 0; x < LevelInfos.GridWidth; x++)
			{
				_buttonsGameObjectsArray[y, x] = Instantiate(_buttonPrefab, _gridCellsGameObject.transform);
				_buttonsGameObjectsArray[y, x].name = "Cell(" + x + ", " + y + ")";
				_buttonsImageArray[y, x] = _buttonsGameObjectsArray[y, x].GetComponent<Image>();
				_buttonsFrameGameObjectsArray[y, x] = _buttonsGameObjectsArray[y, x].transform.Find("Frame").gameObject;
				_buttonsTextArray[y, x] = _buttonsGameObjectsArray[y, x].transform.Find("Text").gameObject.GetComponent<Text>();

				_buttonsArray[y, x] = _buttonsGameObjectsArray[y, x].GetComponent<Button>();
				var tmpX = x;
				_buttonsArray[y, x].onClick.AddListener(delegate { OnButtonClick(tmpX, tmpY); });
			}
		}
	}

	private void OnButtonClick(int x, int y)
	{
		ClearSelected();

		if (CheckMultiSelection(x, y)) return;

		// Hide the frame of the previously selected button
		_buttonsFrameGameObjectsArray[_yCurrent, _xCurrent].SetActive(false);

		_xCurrent = x;
		_yCurrent = y;
		// Show the frame of the newly selected button
		_buttonsFrameGameObjectsArray[y, x].SetActive(true);
	}

	private bool CheckMultiSelection(int xNew, int yNew)
	{
		if (!(MultiSelection = Input.GetKey(KeyCode.LeftShift))) return false;

		var xDelta = xNew - _xCurrent;
		var yDelta = yNew - _yCurrent;

		var signX = Math.Sign(xDelta);
		var signY = Math.Sign(yDelta);

		for (var y = 0; y <= Math.Abs(yDelta); y++)
		{
			for (var x = 0; x <= Math.Abs(xDelta); x++)
			{
				Selected.Add(new Pair
				{
					X = _xCurrent + x * signX,
					Y = _yCurrent + y * signY
				});
				_buttonsFrameGameObjectsArray[_yCurrent + y * signY, _xCurrent + x * signX].SetActive(true);
			}
		}

		return true;
	}

	private void ClearSelected()
	{
		foreach (var pair in Selected)
		{
			_buttonsFrameGameObjectsArray[pair.Y, pair.X].SetActive(false);
		}
		Selected.Clear();
	}

	private void ManageInputs()
	{
		if (Math.Abs(Input.mouseScrollDelta.y) > Mathf.Epsilon)
		{
			OnMouseScrolled((int)Input.mouseScrollDelta.y);
		}

		if (Input.GetKeyUp(KeyCode.Escape))
		{
			_menuPanel.SetActive(!_menuPanel.activeSelf);
		}
	}

	private void OnMouseScrolled(int deltaValue)
	{
		if (MultiSelection)
		{
			foreach (var pair in Selected)
			{
				SetBrickInfo(pair.X, pair.Y, deltaValue);
			}
		}
		else
		{
			SetBrickInfo(_xCurrent, _yCurrent, deltaValue);
		}

		UpdateButtonColor();
	}

	private void SetBrickInfo(int x, int y, int deltaValue)
	{
		_brickLevelArray[y, x] += deltaValue;
		if (_brickLevelArray[y, x] < 0) _brickLevelArray[y, x] = 0;
		_buttonsTextArray[y, x].text = _brickLevelArray[y, x].ToString();
	}

	private void UpdateButtonColor()
	{
		float hMin, sMin, vMin, hMax, sMax, vMax;
		Color.RGBToHSV(_minColor, out hMin, out sMin, out vMin);
		Color.RGBToHSV(_maxColor, out hMax, out sMax, out vMax);

		var min = int.MaxValue;
		var max = 0;

		foreach (var levelValue in _brickLevelArray)
		{
			if (levelValue != 0 && levelValue < min) min = levelValue;
			if (levelValue > max) max = levelValue;
		}

		var deltaLvl = max - min;
		var deltaHue = hMax - hMin;
		var deltaSaturation = sMax - sMin;
		var deltaValue = vMax - vMin;

		for (var y = 0; y < LevelInfos.GridHeight; y++)
		{
			for (var x = 0; x < LevelInfos.GridWidth; x++)
			{
				var lvlPercent = (_brickLevelArray[y, x] - min) / (float)deltaLvl;
				var color = _brickLevelArray[y, x] == 0 ? Color.white : Color.HSVToRGB(hMin + (deltaHue * lvlPercent), sMin + (deltaSaturation * lvlPercent), vMin + (deltaValue * lvlPercent));
				_buttonsImageArray[y, x].color = color;
			}
		}

	}

	public void OnExportButtonClicked()
	{
		var levelInfosExport = new LevelInfosExport(_brickLevelArray);
		LevelExport.ToXml(levelInfosExport);
	}

	#endregion
}
