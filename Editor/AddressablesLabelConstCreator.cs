using Kogane.Internal;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;

namespace Kogane
{
	/// <summary>
	/// Addressables のラベルを文字列の定数で管理するクラスを生成するエディタ拡張
	/// </summary>
	public static class AddressablesLabelConstCreator
	{
		//================================================================================
		// 関数(static)
		//================================================================================
		/// <summary>
		/// コードを生成します
		/// </summary>
		[MenuItem( "Edit/" + AddressablesLabelConstCreatorSettings.PACKAGE_NAME + "/コード生成" )]
		public static void Create()
		{
			var settings = AddressablesLabelConstCreatorSettings.GetInstance();

			Create
			(
				codeTemplate: settings.CodeTemplate,
				outputAssetPath: settings.OutputAssetPath
			);
		}

		/// <summary>
		/// コードを生成します
		/// </summary>
		public static void Create( string codeTemplate, string outputAssetPath )
		{
			var labels = AddressableAssetSettingsDefaultObject.Settings.GetLabels();

			var values = labels
					.Where( x => !string.IsNullOrWhiteSpace( x ) )
					.Select
					(
						x => new ConstStringCodeGeneratorOptions.Element
						{
							Name    = x.ToUpper(),
							Comment = x,
							Value   = x,
						}
					)
					.ToArray()
				;

			var options = new ConstStringCodeGeneratorOptions
			{
				Template = codeTemplate,
				Elements = values,
			};

			var code = ConstStringCodeGenerator
					.Generate( options )
					.Replace( "\t", "    " )
					.Replace( "\r\n", "#NEW_LINE#" )
					.Replace( "\r", "#NEW_LINE#" )
					.Replace( "\n", "#NEW_LINE#" )
					.Replace( "#NEW_LINE#", "\r\n" )
				;

			ConstStringCodeGenerator.Write( outputAssetPath, code );
			AssetDatabase.Refresh();

			var filename = Path.GetFileName( outputAssetPath );
			var asset    = AssetDatabase.LoadAssetAtPath<TextAsset>( outputAssetPath );

			EditorUtility.DisplayDialog
			(
				AddressablesLabelConstCreatorSettings.PACKAGE_NAME,
				$"{filename} の作成が完了しました。",
				"OK"
			);

			EditorGUIUtility.PingObject( asset );
		}
	}
}