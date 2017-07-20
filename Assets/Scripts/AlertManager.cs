using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Object = UnityEngine.Object;

//管理所有的提示框
public class AlertManager {

    //提示框要挂载在此根上
    private static Transform _root;

    private static GameObject alerts;

    //展示提示框
    //带一个提示信息, 一个确认按钮, 一个取消按钮, 一个关闭按钮
    public static _YesNoAlert ShowYesNo()
    {

        if (_root == null)
            _root = GameObject.Find("AlertManager").transform;

        GameObject obj = (GameObject)Object.Instantiate(Resources.Load<GameObject>(Constants.alertYesNoPath), _root);
        obj.GetComponent<RectTransform>().localPosition = Vector3.zero;
        obj.name = "YesNo";
        //得到挂载的YesNoAlert脚本
        _YesNoAlert alert = obj.GetComponent<_YesNoAlert>();

        alerts = obj;
        return alert;
    }

    public static _YesAlert ShowYes()
    {

        if (_root == null)
            _root = GameObject.Find("AlertManager").transform;

        GameObject obj = (GameObject)Object.Instantiate(Resources.Load<GameObject>(Constants.alertYesPath), _root);
        obj.GetComponent<RectTransform>().localPosition = Vector3.zero;
        obj.name = "Yes";

        _YesAlert alert = obj.GetComponent<_YesAlert>();
        alerts = obj;
        return alert;
    }

    //释放掉当前的提示框所占内存
    public static void Destroy()
    {

        Object.Destroy(alerts);
    }

    //隐藏当前的提示框
    public static void Hide()
    {

        alerts.SetActive(false);
    }
}
