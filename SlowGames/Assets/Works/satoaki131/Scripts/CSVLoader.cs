using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// CSV読み込み用
/// Loadは、Score用
/// </summary>
public class CSVLoader : MonoBehaviour {

    public class CSVData{
        public CSVData(string name, int score) {
            this.score = score;
            this.name = name;
        }

        public int score;
        public string name;
    }

    /// <summary>
    /// CSVデータを読み込む
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    static List<CSVData> Load(string path)
    {
        List<CSVData> data = new List<CSVData>();

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

        // リストを生成
        for (int y = 0; y < heigth; ++y)
        {
            string[] test = lines[y].Split(new char[] { ',', ',' }, option);
            data.Add(new CSVData(test[0], int.Parse(test[1])));
        }

        return data;
    }

    /// <summary>
    /// intのCSVデータを読み込む
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    static List<List<int>> intLoad(string path)
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
        var data = Load("/Resources/CSV/test.csv");
        string s = "";
        foreach (var a in data)
        {
            s += a.score.ToString() + ", " + a.name.ToString();

        }
        Debug.Log(s);
    }
}
