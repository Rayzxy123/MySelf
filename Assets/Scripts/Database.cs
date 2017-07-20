using UnityEngine;
using System.Collections;
using MySql.Data.MySqlClient;
using System.Data;

//数据库管理类
public class Database {

    public const string host = "127.0.0.1";
    public const string database = "data";
    public const string id = "root";
    public const string pwd = "zxy";
    public static MySqlConnection myConn;
    public static MySqlCommand mc;

    //创建连接
    public static void Connect()
    {

        string connectionString = string.Format("Server = {0};port={4};Database = {1}; User ID = {2}; Password = {3};"
            , host, database, id, pwd, "3306");

        //打开连接
        myConn = new MySqlConnection(connectionString);
        myConn.Open();
    }

    //进行数据查询
    public static MySqlDataReader Query(string querySql)
    {
        if (mc == null)
            mc = new MySqlCommand(querySql, myConn);
        else
            mc.CommandText = querySql;
        
        
        return mc.ExecuteReader();
    }

    //进行数据插入
    public static void Insert()
    {

    }

    //进行数据删除
    public static void Delete()
    {

    }
}
