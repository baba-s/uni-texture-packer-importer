using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UniTexturePackerImporterEditor
{
	/// <summary>
	/// TexturePacker から出力されたテクスチャを .json に従って分割するクラス
	/// </summary>
	public sealed class AtlasData
	{
		//==============================================================================
		// 変数(readonly)
		//==============================================================================
		public readonly string m_path;

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public AtlasData( string path ) => m_path = path;

		/// <summary>
		/// 分割します
		/// </summary>
		public void Split()
		{
			var textAsset	= AssetDatabase.LoadAssetAtPath<TextAsset>( m_path );
			var atlasJson	= JsonUtility.FromJson<AtlasJson>( textAsset.text );

			if ( atlasJson == null ) return;

			var meta		= atlasJson.meta;
			var frames		= atlasJson.frames;
			var dir			= Path.GetDirectoryName( m_path );
			var texturePath	= dir + "/" + meta.image;
			var importer	= AssetImporter.GetAtPath( texturePath ) as TextureImporter;

			importer.textureType		= TextureImporterType.Sprite;
			importer.spriteImportMode	= SpriteImportMode.Multiple;

			var spritesheet = new List<SpriteMetaData>();

			for ( int i = 0; i < frames.Length; i++ )
			{
				var frame	= frames[ i ];
				var name	= Path.GetFileNameWithoutExtension( frame.filename );
				var rect	= frame.frame.ToRect();
				var pivot	= frame.pivot.ToVector2();

				// TexturePacker と Unity で座標の原点が違うのでここで補正
				rect.y = meta.size.h - rect.y - rect.height;

				var spriteMetaData = new SpriteMetaData
				{
					name		= name	,
					rect		= rect	,
					alignment	= 0		,
					pivot		= pivot	,
				};
				spritesheet.Add( spriteMetaData );
			}

			importer.spritesheet = spritesheet.ToArray();

			EditorUtility.SetDirty( importer );
			AssetDatabase.ImportAsset( texturePath );
		}
	}

	/// <summary>
	/// TexturePacker から出力された .json に対応するクラス
	/// </summary>
	[Serializable]
	public sealed class AtlasJson
	{
		public AtlasFrame[]	frames	;
		public AtlasMeta	meta	;
	}

	/// <summary>
	/// 分割されたテクスチャの情報を管理するクラス
	/// </summary>
	[Serializable]
	public sealed class AtlasFrame
	{
		public string		filename			;
		public AtlasRect	frame				;
		public bool			rotated				;
		public bool			trimmed				;
		public AtlasRect	spriteSourceSize	;
		public AtlasSize	sourceSize			;
		public AtlasVector2	pivot				;
	}

	/// <summary>
	/// 矩形情報を管理するクラス
	/// </summary>
	[Serializable]
	public sealed class AtlasRect
	{
		public int x;
		public int y;
		public int w;
		public int h;

		public Rect ToRect() => new Rect( x, y, w, h );
	}

	/// <summary>
	/// サイズ情報を管理するクラス
	/// </summary>
	[Serializable]
	public sealed class AtlasSize
	{
		public int w;
		public int h;
	}

	/// <summary>
	/// Vector2 の情報を管理するクラス
	/// </summary>
	[Serializable]
	public sealed class AtlasVector2
	{
		public float x;
		public float y;

		public Vector2 ToVector2() => new Vector2( x, y );
	}

	/// <summary>
	/// TexturePacker でパッキングされた画像のメタデータを管理するクラス
	/// </summary>
	[Serializable]
	public sealed class AtlasMeta
	{
		public string		app			;
		public string		version		;
		public string		image		;
		public string		format		;
		public AtlasSize	size		;
		public int			scale		;
		public string		smartupdate	;
	}

	/// <summary>
	/// TexturePacker から出力された .json に変更があったらテクスチャを自動で分割するクラス
	/// </summary>
	public sealed class UniTexturePackerImporter : AssetPostprocessor
	{
		//==============================================================================
		// 変数(static)
		//==============================================================================
		private static UniTexturePackerImporterSettings m_settings;

		//==============================================================================
		// プロパティ(static)
		//==============================================================================
		private static UniTexturePackerImporterSettings settings
		{
			get
			{
				if ( m_settings == null )
				{
					m_settings = AssetDatabase
						.FindAssets( "t:UniTexturePackerImporterSettings" )
						.Select( AssetDatabase.GUIDToAssetPath )
						.Select( c => AssetDatabase.LoadAssetAtPath<UniTexturePackerImporterSettings>( c ) )
						.FirstOrDefault()
					;
				}
				return m_settings;
			}
		}

		//==============================================================================
		// 関数(static)
		//==============================================================================
		/// <summary>
		/// アセットが読み込まれた時に呼び出されます
		/// </summary>
		private static void OnPostprocessAllAssets
		(
			string[] importedAssets			,
			string[] deletedAssets			,
			string[] movedAssets			,
			string[] movedFromAssetPaths
		)
		{
			if ( settings == null )
			{
				Debug.LogWarning( "UniTexturePackerImporterSettings.asset が存在しません" );
				return;
			}

			var pathList = settings.PathList;

			if ( pathList == null || pathList.Length <= 0 )
			{
				Debug.LogWarning( "UniTexturePackerImporterSettings.asset の Path List が設定されていません", settings );
				return;
			}

			var atlasDataList = importedAssets
				.Where( c => pathList.Any( p => c.Contains( p ) ) )
				.Where( c => c.EndsWith( ".json" ) )
				.Select( c => new AtlasData( c ) )
				.ToArray()
			;

			for ( int i = 0; i < atlasDataList.Length; i++ )
			{
				var atlas = atlasDataList[ i ];
				atlas.Split();
			}
		}
	}
}