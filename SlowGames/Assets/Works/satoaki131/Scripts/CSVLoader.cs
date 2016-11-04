using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class CSVData
{
    //新しい要素を追加したら、コンストラクタで代入すること
    public CSVData(string name, int score, float test)
    {
        _score = score;
        _name = name;
        _test = test;
    }

    //必要な要素
    public int _score;
    public string _name;
    public float _test;
}

/// <summary>
/// CSV読み込み用
/// Loadは、Score用
/// </summary>
public class CSVLoader : MonoBehaviour {


    /// <summary>
    /// CSVデータを読み込む
    /// Score用
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static List<CSVData> ScoreDataLoad(string path)
    {
        List<CSVData> data = new List<CSVData>();

        StreamReader sr = new StreamReader(Application.dataPath + path);

        // 読み込んだデータを文字列に変換
        string strStream = sr.ReadToEnd();

        // 文字列カンマとカンマの間に何もなかったら無視する設定
        StringSplitOptions option = StringSplitOptions.RemoveEmptyEntries;

        // 行に分ける
        string[] lines = strStream.Split(new char[] { '\n', '\n' }, option);

        // 行
        int heigth = lines.Length;

        // リストを生成
        for (int y = 0; y < heigth; ++y)
        {
            string[] elements = lines[y].Split(new char[] { ',', ',' }, option);
            data.Add(new CSVData(elements[0], int.Parse(elements[1]), float.Parse(elements[2])));
        }

        return data;
    }

    /// <summary>
    /// intのCSVデータを読み込む
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static List<List<int>> intLoad(string path)
    {
        List<List<int>> date = new List<List<int>>();
        
        // CSV を読み込む
        StreamReader sr = new StreamReader(Application.dataPath + path);

        // 読み込んだデータを文字列に変換
        string strStream = sr.ReadToEnd();

        // 文字列カンマとカンマの間に何もなかったら無視する設定
        StringSplitOptions option = StringSplitOptions.RemoveEmptyEntries;

        // 行に分ける
        string[] lines = strStream.Split(new char[] { '\n', '\n' }, option);

        // カンマで分けるのに使う
        char[] spliter = new char[1] { ',' };

        // 行
        int heigth = lines.Length;
        // 列
        int width = lines[0].Split(spliter, option).Length;

        // リストを生成
        for (int y = 0; y < heigth; ++y)
        {
            date.Add(new List<int>());
            for (int x = 0; x < width; ++x)
            {             
                // カンマで分ける
                string[] readStrData = lines[y].Split(spliter, option);

                // int に直す
                date[y].Add(int.Parse(readStrData[x]));
            }
        }

        return date;
    }

    //デバッグ用
    void Start()
    {
        Debug.Log(Application.dataPath);
        var data = ScoreDataLoad("/Resources/CSV/test.csv");
        string s = "";
        foreach (var a in data)
        {
            s += a._score.ToString() + ", " + a._name.ToString() + a._test.ToString();

        }
        Debug.Log(s);
    }
}
