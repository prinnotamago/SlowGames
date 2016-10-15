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
/// タグネームの定数を生成する機能
/// </summary>
public class TagNameCreater
{

    const string ITEM_NAME = "Utility/TagName";  // コマンド名
    const string HASH_TAG_NAME_PATH = "Assets/Scripts/Utility/HashTagName.cs";      // ファイルパス
    const string TAG_NAME_PATH = "Assets/Scripts/Utility/TagName.cs";      // ファイルパス

    static readonly string HASH_TAG_NAME_FILENAME = Path.GetFileName(HASH_TAG_NAME_PATH);                   // ファイル名(拡張子あり)
    static readonly string HASH_TAG_NAME_FILENAME_WITHOUT_EXTENSION = Path.GetFileNameWithoutExtension(HASH_TAG_NAME_PATH);   // ファイル名(拡張子なし)

    static readonly string TAG_NAME_FILENAME = Path.GetFileName(TAG_NAME_PATH);                   // ファイル名(拡張子あり)
    static readonly string TAG_NAME_FILENAME_WITHOUT_EXTENSION = Path.GetFileNameWithoutExtension(TAG_NAME_PATH);   // ファイル名(拡張子なし)


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

        HashTagNameCreateSctipt();
        TagNameCreateSctipt();

        EditorUtility.DisplayDialog(HASH_TAG_NAME_FILENAME, "作成が完了しました", "OK");
        EditorUtility.DisplayDialog(TAG_NAME_FILENAME, "作成が完了しました", "OK");
    }

    /// <summary>
    /// スクリプトを作成
    /// </summary>
    public static void HashTagNameCreateSctipt()
    {
        var builder = new StringBuilder();
        builder.AppendLine("/// <summary>");
        builder.AppendLine("/// タグ名を定数で管理するクラス");
        builder.AppendLine("/// </summary>");
        builder.AppendFormat("public static class {0}", HASH_TAG_NAME_FILENAME_WITHOUT_EXTENSION).AppendLine();
        builder.AppendLine("{");

        foreach (var n in UnityEditorInternal.InternalEditorUtility.tags.Select(c => new { var = RemoveInvalidChars(c), val = c }))
        {
            builder.Append("\t").AppendFormat(@"public static readonly int {0} = ""{1}"".GetHashCode();", n.var, n.val).AppendLine();
        }
        builder.AppendLine("}");

        var directoryName = Path.GetDirectoryName(HASH_TAG_NAME_PATH);
        if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);

        File.WriteAllText(HASH_TAG_NAME_PATH, builder.ToString(), Encoding.UTF8);
        AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
    }

    /// <summary>
    /// スクリプトを作成
    /// </summary>
    public static void TagNameCreateSctipt()
    {
        var builder = new StringBuilder();
        builder.AppendLine("/// <summary>");
        builder.AppendLine("/// タグ名を定数で管理するクラス");
        builder.AppendLine("/// </summary>");
        builder.AppendFormat("public static class {0}", TAG_NAME_FILENAME_WITHOUT_EXTENSION).AppendLine();
        builder.AppendLine("{");

        foreach (var n in UnityEditorInternal.InternalEditorUtility.tags.Select(c => new { var = RemoveInvalidChars(c), val = c }))
        {
            builder.Append("\t").AppendFormat(@"public const string {0} = ""{1}"";", n.var, n.val).AppendLine();
        }
        builder.AppendLine("}");

        var directoryName = Path.GetDirectoryName(TAG_NAME_PATH);
        if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);

        File.WriteAllText(TAG_NAME_PATH, builder.ToString(), Encoding.UTF8);
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
