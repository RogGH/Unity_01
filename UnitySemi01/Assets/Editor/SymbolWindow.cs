using UnityEngine;
using UnityEditor;

public class SymbolWindow : EditorWindow
{
	private class SymbolData
	{
		public string name;
		public string commment;
		public bool isEnable;
	}

	private static SymbolData mSymbolData;

	[MenuItem("Editor/Symbol")]
	private static void Create()
	{
		// 生成
		var wnd = GetWindow<SymbolWindow>();
		// ウィンドウタイトルの設定
		wnd.titleContent = new GUIContent("SymbolWindow");

		mSymbolData.name = "DEBUG";
	}

	private void OnGUI()
	{
		// ボタン表示
		if (GUILayout.Button("ボタン"))
		{
			// プレイヤーセッティングから設定
			//PlayerSettings.SetScriptingDefineSymbolsForGroup(
			//	EditorUserBuildSettings.selectedBuildTargetGroup,
			//	string.Join(";", mSymbolData)
			//);
			// ボタンが押された場合、コンソールにログを表示
			Debug.Log("ボタン押されたよ");
		}
	}
}