using UnityEngine;
using System.Collections;
using System;
using SimpleJson;

public class Rpc {

    //向服务器 发送远程调用事件
    public static void dispatchEvent(int eventName, Action<JsonObject> callback)
    {
        JsonObject obj = new JsonObject();
        if(callback != null)
            callback(obj);
    }


}
