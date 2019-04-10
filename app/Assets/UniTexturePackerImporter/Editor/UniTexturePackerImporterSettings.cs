using UnityEngine;

namespace UniTexturePackerImporterEditor
{
	/// <summary>
	/// UniTexturePackerImporter の設定を管理するクラス
	/// </summary>
	public sealed class UniTexturePackerImporterSettings : ScriptableObject
	{
		//==============================================================================
		// 変数(SerializeField)
		//==============================================================================
		[SerializeField] private string[] m_pathList = null;	// 対象のフォルダのパスのリスト

		//==============================================================================
		// プロパティ
		//==============================================================================
		public string[] PathList => m_pathList;
	}
}