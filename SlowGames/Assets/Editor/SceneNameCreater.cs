/// <author>
/// 新井大一
/// </author>


using System;
using System.Text;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;

/// <summary>
/// シーンネームの定数を生成する機能
/// </summary>
public class SceneNameCreater
{

    const string ITEM_NAME = "Utility/SceneName";  // コマンド名
    const string PATH = "Assets/Scripts/Utility/SceneName.cs";      // ファイルパス

    static readonly string FILENAME = Path.GetFileName(PATH);                   // ファイル名(拡張子あり)
    static readonly string FILENAME_WITHOUT_EXTENSION = Path.GetFileNameWithoutExtension(PATH);   // ファイル名(拡張子なし)


    // 無効な文字を管理する配列
    static readonly string[] INVALUD_CHARS =
    {
        " ", "!", "\"", "#", "$",
        "%", "&", "\'", "(", ")",
        "-", "=", "^",  "~", "\\",
        "|", "[", "{",  "@", "`",
        "]", "}", ":",  "*", ";",
        "+", "/", "?",  ".", ">",
        ",", "<"
    };

    [MenuItem(ITEM_NAME)]
    public static void Create()
    {
        if (!CanCreate()) return;

        CreateSctipt();

        EditorUtility.DisplayDialog(FILENAME, "作成が完了しました", "OK");
    }

    /// <summary>
    /// スクリプトを作成
    /// </summary>
    public static void CreateSctipt()
    {
        var builder = new StringBuilder();
        builder.AppendLine("/// <summary>");
        builder.AppendLine("/// シーン名を定数で管理するクラス");
        builder.AppendLine("/// </summary>");
        builder.AppendFormat("public static class {0}", FILENAME_WITHOUT_EXTENSION).AppendLine();
        builder.AppendLine("{");
        builder.AppendLine("public enum Name");
        builder.AppendLine("{");

        foreach (var n in EditorBuildSettings.scenes.Select(c => Path.GetFileNameWithoutExtension(c.path)).Distinct().Select(c => new { var = RemoveInvalidChars(c), val = c}))
        {
            builder.Append("\t").AppendFormat("{0},", n.var, n.val).AppendLine();
        }
        builder.AppendLine("}");

        foreach (var n in EditorBuildSettings.scenes.Select(c => Path.GetFileNameWithoutExtension(c.path)).Distinct().Select(c => new { var = RemoveInvalidChars(c), val = c }))
        {
            builder.Append("\t").AppendFormat(@"public const string {0} = ""{1}"";", n.var, n.val).AppendLine();
        }

        builder.AppendLine("}");

        var directoryName = Path.GetDirectoryName(PATH);
        if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);

        File.WriteAllText(PATH, builder.ToString(), Encoding.UTF8);
        AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
    }

    /// <summary>
    /// タグ名を定数で管理するクラスを作成できるかどうかを取得
    /// </summary>
    /// <returns></returns>
    [MenuItem(ITEM_NAME, true)]
    public static bool CanCreate()
    {
        return !EditorApplication.isPlaying && !Application.isPlaying && !EditorApplication.isCompiling;
    }

    /// <summary>
    /// 無効な文字を削除
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string RemoveInvalidChars(string str)
    {
        Array.ForEach(INVALUD_CHARS, c => str = str.Replace(c, string.Empty));
        return str;
    }
}
