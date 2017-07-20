using UnityEngine;
using System.Collections;

enum Status
{
    Connected,
    DisConnected
};

//一方面用来确认客户端或者服务器端 有没有宕机
//另一方面也可以用来确认当前的网络通信状态
public class Heartbeat  {

    private float hbStartTime;               //记录下心跳发送的起始时间
    private float hbInterval = 60.0f;               //心跳的间隔时间
    private Status status;                  //当前的连接状态
    private float hbReceivedTime;               //记录下从服务器端发送回来的时间
    private float hbWaitTime = 30.0f;       //在发送心跳数据之后, 等待服务器返回数据的时间,否则判定为服务器宕机,或者网络状况不好

    //发送心跳请求
    public static void SendHeartbeat()
    {

    }
}
