using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class _YesAlert : MonoBehaviour {

    private Text alertInfo;          //提示信息
    private Button yesButton;        //确认按钮
    private Button closeButton;      //关闭按钮

    public Action callback;         //处理包括点击确认和关闭按钮的事件
    public Action<float> autoClose;        //处理自动关闭的逻辑

    public void Awake()
    {
        //初始化
        yesButton = transform.GetChild(0).gameObject.GetComponent<Button>();
        closeButton = transform.GetChild(1).gameObject.GetComponent<Button>();
        alertInfo = transform.GetChild(2).gameObject.GetComponent<Text>();
    }

    //设置提示信息
    public _YesAlert SetAlertInfo(string text)
    {

        alertInfo.text = text;
        return this;
    }

    //设置提示信息颜色
    public _YesAlert SetAlertInfoColor(Color c)
    {

        alertInfo.color = c;
        return this;
    }

    //设置提示信息大小
    public _YesAlert SetAlertInfoSize(int size)
    {

        alertInfo.fontSize = size;
        SetAlertInfoSizeDelta(new Vector2(400.0f, size * 3.0f));
        return this;
    }

    //设置提示信息文本框大小
    public _YesAlert SetAlertInfoSizeDelta(Vector2 sizeDelta)
    {

        alertInfo.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        return this;
    }

    //设置确认按钮的按钮文本
    public _YesAlert SetYesButtonText(string text)
    {

        yesButton.transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = text;
        return this;
    }

    //设置Yes按钮的回调事件
    public _YesAlert SetYesButtonEvent(Action callback0)
    {

        if (callback0 != null)
            callback = callback0;

        if (callback != null)
        {
            yesButton.onClick.AddListener(YesEvent);
            closeButton.onClick.AddListener(CloseEvent);
        }

        return this;
    }

    //设置自动关闭事件的逻辑
    public _YesAlert SetAutoCloseEvent(Action<float> callback)
    {

        if (callback != null)
            autoClose = callback;

        return this;
    }

    //点击确认按钮产生的事件
    public void YesEvent()
    {

        if (callback != null)
            callback();
    }

    //点击关闭按钮的事件
    public void CloseEvent()
    {

        if (callback != null)
            callback();
    }

    void Start () {
	
	}
	
	void Update () {
	
	}


}
