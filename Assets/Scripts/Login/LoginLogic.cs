using UnityEngine;
using System.Collections;
using SimpleJson;
using Pomelo.DotNetClient;
using DG.Tweening;
using UnityEngine.UI;

public class LoginLogic  {

    //解析注册响应
    public static void ParseRegisterResponse(Message Info)
    {

        bool status = (bool)Info.jsonObj["status"];

        if (status)
        {

            //弹出注册成功的提示框
            _YesAlert alert = AlertManager.ShowYes();
            alert.SetAlertInfo("注册成功").SetYesButtonText("确认")
                .SetYesButtonEvent(() => {

                    //alert.autoClose(Time.realtimeSinceStartup);                  //自动关闭事件
                    AlertManager.Destroy();
                })
                .SetAutoCloseEvent(null);
            Debug.Log("Succeed");
        }
        else
        {


            //弹出注册失败,账号已经存在的提示框
            AlertManager.ShowYes().SetAlertInfo("该账号已被注册").SetYesButtonText("确认")
                .SetYesButtonEvent(() => {

                    AlertManager.Destroy();
                });
            Debug.Log("failed");
        }
    }

    //解析登陆响应
    public static void ParseLoginInResponse(Message Info)
    {

        object obj = Info.jsonObj["status"];
        int status = int.Parse(obj.ToString());

        switch (status)
        {
            //登陆成功状态码
            case 200:

                TextManager.displayModeOpen = false;

                _Text t = TextManager.Show("登陆成功");
                t.SetMoveOpen(true).SetColor(Color.black)
                    .SetCallback(() => {
                        TextManager.Destroy(t);
                        GameEntry.gameState = GameState.GameState_LOADING;
                    });
                Debug.Log("登陆成功");
                break;
            //用户名不存在
            case 301:

                AlertManager.ShowYes().SetYesButtonText("确认").SetAlertInfo("该用户名不存在")
                    .SetYesButtonEvent(() => {
                        AlertManager.Destroy();
                    });
                Debug.Log("用户名不存在");
                break;
            //登陆密码错误
            case 302:
                AlertManager.ShowYes().SetYesButtonText("确认").SetAlertInfo("登录密码错误")
                    .SetYesButtonEvent(() => {
                        AlertManager.Destroy();
                    });
                Debug.Log("登陆密码错误");
                break;
        }
    }
}
