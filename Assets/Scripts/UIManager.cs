using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;

public class UIManager {

    //存取UI的字典
    //存放所有ui挂载的根对象实例
    public static Dictionary<string, GameObject> uiDic;

    //清空所有的元素
    public static void Clear()
    {

        uiDic.Clear();
    }

    //初始化UI字典
    public static void InitUIDic()
    {
        if (uiDic == null)
            uiDic = new Dictionary<string, GameObject>();
    }

    //往字典中加入元素
    public static void Add(GameObject ui)
    {

        if (uiDic == null)
            uiDic = new Dictionary<string, GameObject>();
        uiDic.Add(ui.name, ui);
    }

    //显示当前UI
    public static void ShowUI(string uiName, bool hideOthers = true)
    {

        if (hideOthers)
        {
            //所有ui设置成不可见
            foreach (KeyValuePair<string, GameObject> kv in uiDic)
            {
                //kv.Value.transform
            }
        }

        //设置成uiName界面为可见
        uiDic[uiName].SetActive(true);
    }

    //隐藏掉当前UI
    public static void HideUI(string uiName)
    {

        //设置成uiName界面为可见
        uiDic[uiName].SetActive(false);
    }

    //释放掉当前UI所占的内存
    public static void DestroyUI(string uiName)
    {

        Object.Destroy(uiDic[uiName]);
        uiDic.Remove(uiName);
    }
}
