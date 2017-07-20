using UnityEngine;
using System.Collections;
using SimpleJson;

public class ChatLogic {

    //解析响应
    public static void ParseResponse(JsonObject Info)
    {

        foreach (string key in Info.Keys)
        {

            //string val = Info[key].ToString();

            switch (key)
            {
                case "status":
                    //如果状态码是200
                    switch (Info[key].ToString())
                    {
                        case "200":
                            //...账号登录成功
                            Debug.Log("状态码200");
                            break;
                        case "301":
                            //...账户名错误
                            break;
                        case "302":
                            //...密码错误
                            break;
                        case "500":
                            //...客户端被强制踢下线
                            break;     
                    }
                    break;
            }
        }
    }
}
