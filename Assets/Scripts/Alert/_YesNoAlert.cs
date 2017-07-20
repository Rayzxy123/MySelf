using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using UnityEngine.UI;

public class _YesNoAlert : MonoBehaviour {

    private Text alertInfo;          //提示信息(对应显示提示框的文本信息)
    private Button closeButton;      //关闭按钮
    private Button yesButton;        //确认按钮
    private Button noButton;         //取消信息

    public Action yesCallback;      //Yes按钮的回调函数
    public Action noCallback;       //No按钮的回调函数
    public Action closeCallback;    //关闭按钮的回调函数

    public void Awake()
    {

        //初始化界面信息
        yesButton = transform.GetChild(0).gameObject.GetComponent<Button>();
        noButton = transform.GetChild(1).gameObject.GetComponent<Button>();
        closeButton = transform.GetChild(2).gameObject.GetComponent<Button>();
        alertInfo = transform.GetChild(3).gameObject.GetComponent<Text>();
    }

    //设置提示信息
    public _YesNoAlert SetAlertInfo(string text)
    {

        alertInfo.text = text;
        return this;
    }

    //设置提示信息颜色
    public _YesNoAlert SetAlertInfoColor(Color c)
    {

        alertInfo.color = c;
        return this;
    }

    //设置提示信息大小
    public _YesNoAlert SetAlertInfoSize(int size)
    {

        alertInfo.fontSize = size;
        SetAlertInfoSizeDelta(new Vector2(400.0f, size * 3.0f));
        return this;
    }

    //设置提示信息文本框大小
    public _YesNoAlert SetAlertInfoSizeDelta(Vector2 sizeDelta)
    {

        alertInfo.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        return this;
    }
    
    //设置确认按钮的按钮文本
    public _YesNoAlert SetYesButtonText(string text)
    {

        yesButton.transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = text;
        return this;
    }

    //设置取消按钮的按钮文本
    public _YesNoAlert SetNoButtonText(string text)
    {

        noButton.transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = text;
        return this;
    }

    //设置Yes按钮的回调事件
    public _YesNoAlert SetYesButtonEvent(Action callback)
    {
        //确保最后写的时候不会忘记调用添加事件监听的函数
        if (callback != null)
            yesCallback = callback;

        if (yesCallback != null)
            yesButton.onClick.AddListener(YesEvent);

        return this;
    }

    //设置No按钮的回调函数
    public _YesNoAlert SetNoButtonEvent(Action callback)
    {

        if (callback != null)
            noCallback = callback;

        if (noCallback != null)
            noButton.onClick.AddListener(NoEvent);

        return this;
    }

    //设置close按钮的回调函数
    public _YesNoAlert SetCloseButtonEvent(Action callback)
    {
        
        if (callback != null)
            closeCallback = callback;

        if (closeCallback != null)
            closeButton.onClick.AddListener(CloseEvent);

        return this;
    }

    //点击确认按钮产生的事件
    public void YesEvent()
    {

        if (yesCallback != null)
            yesCallback();
    }

    //点击取消按钮的事件
    public void NoEvent()
    {

        if (noCallback != null)
            noCallback();
    }

    //点击关闭按钮的事件
    public void CloseEvent()
    {

        if (closeCallback != null)
            closeCallback();
    }

    void Start () {
	    
	}
	
	void Update () {
	
	}
}
