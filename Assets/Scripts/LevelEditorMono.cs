using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelEditorMono : MonoBehaviour
{
	#region Serialized Fields

	[SerializeField] private GameObject _buttonPrefab;
	[SerializeField] private RectTransform _gridCanvasRectTransform;
	[SerializeField] private GameObject _gridCellsGameObject;

	#endregion

	#region Fields

	private GridLayoutGroup _gridCellsGridLayoutGroup;

	#region Button

	private GameObject[,] _buttonsGameObjectsArray;
	private Button[,] _buttonsArray;
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
		_buttonsFrameGameObjectsArray = new GameObject[LevelInfos.GridHeight, LevelInfos.GridWidth];
		_buttonsTextArray = new Text[LevelInfos.GridHeight, LevelInfos.GridWidth];

		for (var y = 0; y < LevelInfos.GridHeight; y++)
		{
			var tmpY = y;
			for (var x = 0; x < LevelInfos.GridWidth; x++)
			{
				_buttonsGameObjectsArray[y, x] = Instantiate(_buttonPrefab, _gridCellsGameObject.transform);
				_buttonsGameObjectsArray[y, x].name = "Cell(" + x + ", " + y + ")";
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
		// Hide the frame of the previously selected button
		_buttonsFrameGameObjectsArray[_yCurrent, _xCurrent].SetActive(false);

		_xCurrent = x;
		_yCurrent = y;
		// Show the frame of the newly selected button
		_buttonsFrameGameObjectsArray[y, x].SetActive(true);
	}

	private void ManageInputs()
	{
		if (Math.Abs(Input.mouseScrollDelta.y) > Mathf.Epsilon)
		{
			OnMouseScrolled((int)Input.mouseScrollDelta.y);
		}

	}

	private void OnMouseScrolled(int deltaValue)
	{
		_brickLevelArray[_yCurrent, _xCurrent] += deltaValue;
		if (_brickLevelArray[_yCurrent, _xCurrent] < 0) _brickLevelArray[_yCurrent, _xCurrent] = 0;
		_buttonsTextArray[_yCurrent, _xCurrent].text = _brickLevelArray[_yCurrent, _xCurrent].ToString();
	}

	#endregion

}
