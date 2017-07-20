using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

//初始化ConfigData,从本地读取一些必要配置信息
public class ConfigData {

    private static string url = Application.streamingAssetsPath + '/' + Constants.ConfigPath + 
                    '/' + "dbTableName.txt";
    //初始化数据库所需要加载的表名
    public static List<string> dbTableNameLists;
    public static Dictionary<int, D_Lv> d_lvDic;                //保存数据库中的玩家升级信息
    public static Dictionary<int, D_Card> d_cardDic;            //保存卡片对应信息

    //主要做一些数据结构的初始化工作
    public static void InitConfigInfo()
    {

        InitDBTableName();
        InitLists();
        InitConfigData();
    }

    public static void InitLists()
    {

        if (d_lvDic == null)
            d_lvDic = new Dictionary<int, D_Lv>();

        if (d_cardDic == null)
            d_cardDic = new Dictionary<int, D_Card>();
    }

    public static void InitDBTableName()
    {
        string str = "";
        if (dbTableNameLists == null)
            dbTableNameLists = new List<string>();

        //读取数据库中表信息
        StreamReader reader = new StreamReader(url);
        while ((str = reader.ReadLine()) != null)
            dbTableNameLists.Add(str);

        //进行数据库的连接
        Database.Connect();
    }

    //初始化必要信息
    public static void InitConfigData()
    {

        for (int tableIndex = 0; tableIndex < dbTableNameLists.Count; ++tableIndex)
        {
            string querySql = "";
            MySqlDataReader reader = null;
            switch (dbTableNameLists[tableIndex])
            {

                case "d_lv":

                    if (d_lvDic == null)
                        d_lvDic = new Dictionary<int, D_Lv>();

                    LoadingManager.SetProgressInfo("正在加载配置信息");
                    querySql = string.Format("select *from {0}", dbTableNameLists[tableIndex]);
                    reader = Database.Query(querySql);
                    //进行数据表的遍历
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {

                            D_Lv lv = new D_Lv();
                            lv.lv = reader.GetInt32(0);
                            lv.exp = reader.GetString(1);
                            lv.reward = reader.GetString(2);
                            d_lvDic.Add(lv.lv, lv);
                        }
                    }

                    reader.Close();
                    LoadingManager.Progress(5);
                    break;
                case "d_card":

                    if (d_cardDic == null)
                        d_cardDic = new Dictionary<int, D_Card>();

                    LoadingManager.SetProgressInfo("正在加载卡片信息");
                    querySql = string.Format("select *from {0}", dbTableNameLists[tableIndex]);
                    reader = Database.Query(querySql);
                    //进行数据表的遍历
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {

                            D_Card card = new D_Card();
                            card.id = reader.GetInt32(0);
                            card.name = reader.GetString(1);
                            card.info = reader.GetString(2);
                            card.color = reader.GetInt32(3);
                            card.atk1 = reader.GetInt32(4);
                            card.atk2 = reader.GetInt32(5);
                            card.def = reader.GetInt32(6);
                            card.hp = reader.GetInt32(7);
                            card.mp = reader.GetInt32(8);
                            card.moveDis = reader.GetInt32(9);
                            card.atkArea = reader.GetInt32(10);
                            card.atkDis = reader.GetInt32(11);
                            card.atkNum = reader.GetInt32(12);
                            card.energy = reader.GetInt32(13);
                            card.price = reader.GetString(14);
                            d_cardDic.Add(card.id, card);
                        }
                    }

                    reader.Close();
                    LoadingManager.Progress(15);
                    break;
            }
        }
    }
}
