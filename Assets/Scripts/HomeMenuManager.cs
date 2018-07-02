using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeMenuManager : MonoBehaviour
{

	private const string LevelEditorSceneName = "LevelEditor";

	public void OnWidthInputFieldValueChanged(string value)
	{
		int width;

		if (int.TryParse(value, out width) && width >= 0)
		{
			LevelInfos.GridWidth = width;
		}
	}

	public void OnHeightInputFieldValueChanged(string value)
	{
		int height;

		if (int.TryParse(value, out height) && height >= 0)
		{
			LevelInfos.GridHeight = height;
		}
	}

	public void OnValidateButtonClick()
	{
		LoadLevelEditorScene();
	}

	private static void LoadLevelEditorScene()
	{
		SceneManager.LoadScene(LevelEditorSceneName);
	}
}
