using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Pomelo.DotNetClient;
using System;
using Object = UnityEngine.Object;

public class LoginUI : MonoBehaviour{

    //保存回调函数
    public Action<bool> callback;

    //loginUI和registerUI的模板
    public GameObject loginPrefab;
    public GameObject registerPrefab;

    //保存的loginUI对象实例
    public static GameObject loginUI;
    //保存的RegisterUI对象实例
    public static GameObject registerUI;

    public void Awake()
    {

    }

    public void Start()
    {

        InitLogin(null);
    }

    public void Update()
    {

    }

    //初始化Login界面 即对应先关资源
    public void InitLogin(Action<bool> callback0)
    {

        InitUI();

        if (callback0 != null)
            callback = callback0;
    }

    //释放掉UI所占的内存
    public void Clear()
    {

        ClearAllText();
        Object.Destroy(transform.gameObject);
    }

    //初始化UI界面
    public void InitUI()
    {

        //初始化位置
        transform.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;

        GameObject loginClone = (GameObject)GameObject.Instantiate(loginPrefab, transform);
        GameObject registerClone = (GameObject)GameObject.Instantiate(registerPrefab, transform);

        loginClone.GetComponent<RectTransform>().localPosition = Vector3.zero;
        registerClone.GetComponent<RectTransform>().localPosition = Vector3.zero;

        loginClone.SetActive(true);
        registerClone.SetActive(false);

        loginUI = loginClone;
        registerUI = registerClone;

        loginUI.name = loginPrefab.name;
        registerUI.name = registerPrefab.name;

        //加入按钮点击事件
        AddButtonClickEvent(loginUI);
        AddButtonClickEvent(registerUI);
    }
    
    //为该界面的按钮添加点击事件
    public void AddButtonClickEvent(GameObject ui)
    {
        switch (ui.name)
        {
                
            case Constants.registerUIName:
                ui.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(Register);
                ui.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(CloseRegister);
                break;
            case Constants.loginUIName:
                ui.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(LogIn);
                ui.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(OpenRegister);
                break;
        }
    }

    //注册事件
    public void Register()
    {

        AlertManager.ShowYesNo().SetAlertInfo("你确定要用这个账号进行注册码?")
            .SetYesButtonText("确认").SetNoButtonText("取消")
            .SetYesButtonEvent(() =>
            {

                InputField[] iField = registerUI.GetComponentsInChildren<InputField>();
                string accountText = iField[0].text;
                string pwdText = iField[1].text;
                GameEntry.connectionRoot.GetComponent<_Connection>().Register(accountText, pwdText);

                AlertManager.Destroy();
            })
            .SetNoButtonEvent(() =>
            {
                AlertManager.Destroy();
            })
            .SetCloseButtonEvent(() =>
            {
                AlertManager.Destroy();
            });
    }

    //登陆事件
    public void LogIn()
    {
        InputField[] iField = loginUI.GetComponentsInChildren<InputField>();
        string accountText = iField[0].text;
        string pwdText = iField[1].text;
        GameEntry.connectionRoot.GetComponent<_Connection>().LoginIn(accountText, pwdText);
    }

    //清空所有的输入框中的文本信息
    public void ClearAllText()
    {

        InputField[] iField = registerUI.GetComponentsInChildren<InputField>();
        for (int i = 0; i < iField.Length; ++i)
        {
            iField[i].text = "";
        }

        iField = loginUI.GetComponentsInChildren<InputField>();
        for (int i = 0; i < iField.Length; ++i)
        {
            iField[i].text = "";
        }
    }

    //关闭掉Register界面
    public void CloseRegister()
    {

        ClearAllText();
        registerUI.SetActive(false);
        loginUI.SetActive(true);
    }

    //打开Register界面
    public void OpenRegister()
    {

        ClearAllText();
        registerUI.SetActive(true);
        loginUI.SetActive(false);
    }
}
