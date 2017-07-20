using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;
using Object = UnityEngine.Object;

public class TextManager {

    [SerializeField]
    private static Transform _root;                 //TextManager的根节点的transform
    [HideInInspector]
    public static int changeRowNums = 0;           //记录换过多少次行
    [HideInInspector]
    public const int maxRowWordNum = 20;            //文本域中一行最多输出20个字        split之后在一个字符串前后各自出现一个字符
    [HideInInspector]
    public static bool displayModeOpen = true;                    //默认开启逐字显示功能
    [HideInInspector]
    public static int textId = -1;                      //管理文本出现的Id属性
    [HideInInspector]
    public static Dictionary<int, List<int>> richTextList;                   //保存富文本信息的列表
    [HideInInspector]
    public static Dictionary<int, List<int>> textOutputSpeedList;            //保存播放字速度的列表

    //自动销毁文本信息
    public static void Destroy(_Text text)
    {
        textId--;           //减少一个文本的引用
        if (textId != -1)
            richTextList.Remove(text.id);
        else
            richTextList.Clear();

        Object.Destroy(_root.GetChild(text.id).gameObject);
        Object.Destroy(text);      
    }

    //设置是否启动逐字显示
    public static void SetDisplayMode(bool open)
    {

        displayModeOpen = open;
    }

    //通过WWW来读取文件
    static IEnumerator LoadFromWWW(_Text t, string fileName, Action callback)
    {

        //读取保存在streamingAssets下的txt文件
        string path = "file://" + Application.streamingAssetsPath + '/' + fileName;
        List<string[]> lines = new List<string[]>();
        WWW www = new WWW(path);
        yield return www;
        t.msg = www.text;
        t.msg = t.msg.Trim((char)65279);

        string[] arrs = t.msg.Split('\n');

        SplitString(lines, arrs);
        t.Resize(maxRowWordNum * t.textInfo.fontSize, lines.Count * (t.textInfo.fontSize + 5));           //重新调整文本域大小
        t.wordArray = lines;

        callback();
    }

    //分割所读取字符串
    public static void SplitString(List<string[]> lines, string []arrs)
    {
        //分割字符串
        for (int strIndex = 0; strIndex < arrs.Length; ++strIndex)
        {

            string[] str;
            //多分配一位的字符,留给'\n', 超出一行最大值是进行换行操作
            int len = arrs[strIndex].Length;

            int rowIndex = 0;

            while (len > maxRowWordNum)
            {

                changeRowNums++;
                int k = 0;
                str = new string[maxRowWordNum];

                //对当前字符串按字马上进行赋值操作
                for (int wordIndex = rowIndex * maxRowWordNum; wordIndex < (rowIndex + 1) * maxRowWordNum; ++wordIndex)
                    str[k++] = arrs[strIndex][wordIndex].ToString();

                k = 0;
                rowIndex++;
                len -= maxRowWordNum;

                lines.Add(str);
            }

            //如果截取20后的字符串仍有余 或者 字符串本身小于20
            if (len != 0)
            {

                str = new string[len + 1];
                int k = 0;                  //字符串字符迭代变量
                for (int wordIndex = rowIndex * maxRowWordNum; wordIndex < arrs[strIndex].Length; ++wordIndex)
                    str[k++] = arrs[strIndex][wordIndex].ToString();
                str[k] = "\n";
                lines.Add(str);
            }
        }
    }

    //简单分割字符串
    public static void SplitString(List<string[]> lines, string msg)
    {

        string[] words = new string[msg.Length + 1];
        int charIndex = 0;
        for (charIndex = 0; charIndex < msg.Length; ++charIndex)
        {

            words[charIndex] = msg[charIndex].ToString();
        }

        words[charIndex] = '\n'.ToString();
        lines.Add(words);
    }

    //可以直接是文本信息,或者是文本文件的名字(包含.txt)
    public static _Text Show(string msg)
    {

        if (_root == null)
            _root = GameEntry.textRoot.transform;

        GameObject obj = (GameObject)Object.Instantiate(Resources.Load<GameObject>("Text"), _root);
        obj.GetComponent<RectTransform>().localPosition = Vector3.zero;
        _Text t = obj.GetComponent<_Text>();
        t.id = ++textId;
        t.name = t.id.ToString();               //挂载在TextManager根节点下的文本名字即为其id

        if (richTextList == null)
            richTextList = new Dictionary<int, List<int>>();

        if (textOutputSpeedList == null)
            textOutputSpeedList = new Dictionary<int, List<int>>();

        List<int> tmpList = new List<int>();            //tmpList
        List<int> speedList = new List<int>();          //记录一句话中的速度点

        int i = 0, index = 0;

        //是否要展示文本文件中的信息
        if (msg.Contains(".txt"))
        {
            if (t.type == Type.None)
                t.type = Type.TXT;

            t.textInfo.text = msg;
            t.StartCoroutine(LoadFromWWW(t, msg, ()=>
            {

                //把富文本开始索引下标添加到richTextList中
                while ((index = t.msg.IndexOf('<', i)) != -1)
                {
                    //进行标签预测(如果出现speed)
                    if (t.msg[index + 1] == 's' && t.msg[index + 2] == 'p')
                        speedList.Add(index);       //如果出现这个情况, 基本上可以判定该标签为标记文本输出速率的标签
                    else
                        tmpList.Add(index);
                    i = index + 1;
                }


                textOutputSpeedList.Add(t.id, speedList);
                richTextList.Add(t.id, tmpList);

                t.displayTextOnScreen = true;
            }));
        }
        else
        {

            //设置成
            if (t.type == Type.None)
                t.type = Type.Message;
            
            List<string[]> lines = new List<string[]>();
            t.msg = msg;
            SplitString(lines, msg);
            t.wordArray = lines;

            //把富文本开始索引下标添加到richTextList中
            while ((index = t.msg.IndexOf('<', i)) != -1)
            {
                //进行标签预测(如果出现speed)
                if (t.msg[index + 1] == 's' && t.msg[index + 2] == 'p')
                {
                    //Debug.Log(speedList.Count);
                    speedList.Add(index);       //如果出现这个情况, 基本上可以判定该标签为标记文本输出速率的标签
                }
                else
                    tmpList.Add(index);
                i = index + 1;
            }

            textOutputSpeedList.Add(t.id, speedList);
            richTextList.Add(t.id, tmpList);

            t.displayTextOnScreen = true;           //开启将文本展示在屏幕上
            t.textInfo.text = msg;
        }

        return t;
    }
}
