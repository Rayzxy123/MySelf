using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

//进行解密,加密
public class Translate {

    //密码表
    public static Dictionary<char, char> codeTable;

    //初始化加密码表
    public static void InitCodeTable(string url)
    {
        if (codeTable == null)
            codeTable = new Dictionary<char, char>();

        //从本地读取密码表
        StreamReader reader = new StreamReader(url);
        string str = null;
        //当行读取为空时
        while ((str = reader.ReadLine()) != null)
        {
            //进行每行的数据解析
            char []c = ParseLine(str);
            codeTable.Add(c[0], c[1]);
        }
    }

    //进行数据的解析
    public static char[] ParseLine(string line)
    {
        char[] c = new char[2];
        int index = line.IndexOf(':');

        c[0] = char.Parse(line.Substring(0, index));
        c[1] = char.Parse(line.Substring(index + 1));
        return c;
    }

    //对密码进行加密
    public static string encodePwd(string pwd)
    {

        StringBuilder sb = new StringBuilder();
        //简单加密
        for (int pwdIndex = 0; pwdIndex < pwd.Length; ++pwdIndex)
        {
            sb.Append(codeTable[pwd[pwdIndex]]);
        }


        return sb.ToString();
    }

    public static void decodePwd()
    {

    }
}
