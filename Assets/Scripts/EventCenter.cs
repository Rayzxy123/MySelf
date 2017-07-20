using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SimpleJson;

//专门用来存储即将要被推入事件队列中的对,类似<参数, 回调函数>
public class EventStructure
{

    public JsonObject msg;
    public Action<JsonObject> callback;

    public EventStructure()
    {

    }

    public EventStructure(Action<JsonObject> callback0, JsonObject msg0 = null)
    {

        msg = msg0;
        callback = callback0;
    }
}

public class EventCenter  {

    //存储事件的字典
    public static Dictionary<int, Action<JsonObject>> eventDic;

    //当前正在执行的本地事件队列
    //最简单的先进先出原则
    public static Queue<EventStructure> eventQueue;
    //服务器事件队列
    public static Queue<EventStructure> serverEventQueue;

    //初始化字典
    public static void InitEventStructure()
    {

        if (eventDic == null)
            eventDic = new Dictionary<int, Action<JsonObject>>();

        if (eventQueue == null)
            eventQueue = new Queue<EventStructure>();
        if (serverEventQueue == null)
            serverEventQueue = new Queue<EventStructure>();
    }

    //加入事件监听器
    public static void addEventListener(int eventName, Action<JsonObject> callback)
    {

        if (eventDic == null)
            eventDic = new Dictionary<int, Action<JsonObject>>();

        //加入事件监听函数
        if (callback != null)
            eventDic.Add(eventName, callback);
    }

    //分派事件
    //@param eventName 事件名
    //@param src 事件源来自本地,还是服务器
    public static void dispatchEvent(int eventName, string src)
    {

        //判断是否是服务器端函数
        if (src == "sv")
        {
            //向服务器端派送事件, 服务器端调用回调函数, 返回msg Json对象
            Rpc.dispatchEvent(eventName, msg =>
            {
                //遍历事件字典
                foreach (KeyValuePair<int, Action<JsonObject>> kv in eventDic)
                {
                    //如果监听到了该事件
                    if (kv.Key == eventName)
                    {

                        Action<JsonObject> callback = kv.Value;
                        //比如向服务器发送请求,获得Json数据,并调用在客户端注册的监听函数
                        EventStructure es = new EventStructure(callback, msg);
                        eventQueue.Enqueue(es);
                        break;
                    }
                }
            });
        }
        //如果不是
        else if(src == "cli" || src == "" || src.Length == 0
                || src.Trim().Length == 0)
        {

            //遍历事件字典
            foreach (KeyValuePair<int, Action<JsonObject>> kv in eventDic)
            {
                //如果监听到了该事件
                if (kv.Key == eventName)
                {

                    Action<JsonObject> callback = kv.Value;
                    //比如向服务器发送请求,获得Json数据,并调用在客户端注册的监听函数
                    EventStructure es = new EventStructure(callback);
                    eventQueue.Enqueue(es);
                    break;
                }
            }
        }
    }
}
