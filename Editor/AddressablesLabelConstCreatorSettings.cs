using UnityEditor;
using UnityEngine;

namespace Kogane.Internal
{
	/// <summary>
	/// Preferences における設定を管理する ScriptableObject
	/// </summary>
	internal sealed class AddressablesLabelConstCreatorSettings
		: ScriptableObjectForProjectSettings<AddressablesLabelConstCreatorSettings>
	{
		//================================================================================
		// 定数
		//================================================================================
		public const string PACKAGE_NAME = "UniAddressablesLabelConstCreator";

		private const string DEFAULT_OUTPUT_ASSET_PATH = "Assets/AddressablesLabelConst.cs";

		private const string DEFAULT_CODE_TEMPLATE = @"using System.Collections.Generic;

namespace Kogane
{
    public static partial class AddressablesLabelConst
    {
        public const int LENGTH = #LENGTH#;

#VALUES#

        public static IEnumerable<string> GetValues()
        {
#GET_VALUES_CONTENTS#
        }
    }
}";

		//================================================================================
		// 変数(static)
		//================================================================================
		[SerializeField]                  private string m_outputAssetPath = DEFAULT_OUTPUT_ASSET_PATH;
		[SerializeField][Multiline( 16 )] private string m_codeTemplate    = DEFAULT_CODE_TEMPLATE;

		//================================================================================
		// プロパティ
		//================================================================================
		public string OutputAssetPath => m_outputAssetPath;
		public string CodeTemplate    => m_codeTemplate;

		//================================================================================
		// 関数(static)
		//================================================================================
		[SettingsProvider]
		private static SettingsProvider SettingsProvider()
		{
			return CreateSettingsProvider
			(
				settingsProviderPath: $"Kogane/{PACKAGE_NAME}",
				onGUIExtra: so =>
				{
					if ( GUILayout.Button( "Reset to Default" ) )
					{
						so.FindProperty( nameof( m_outputAssetPath ) ).stringValue = DEFAULT_OUTPUT_ASSET_PATH;
						so.FindProperty( nameof( m_codeTemplate ) ).stringValue    = DEFAULT_CODE_TEMPLATE;
					}

					if ( GUILayout.Button( "Create Code" ) )
					{
						AddressablesLabelConstCreator.Create();
					}
				}
			);
		}
	}
}