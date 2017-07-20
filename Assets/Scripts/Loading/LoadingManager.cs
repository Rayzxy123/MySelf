using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;


//主要做的就是加载玩家初始操作匹配界面(主要加载那些需要从服务器上拉下的信息)
public class LoadingManager {

    public static List<Action> loadingFuncDic;
    public static int progressVal;

    //初始化所有的loading信息
    public static void InitLoading()
    {

        //EventCenter.addEventListener();
        if(loadingFuncDic == null)
            loadingFuncDic = new List<Action>();

        loadingFuncDic.Add(ConfigData.InitConfigInfo);
        //在初始化完成后进行逐个函数的执行
        ExecuteLoadingFunc(0);
        Progress(100);
    }

    //执行加载信息函数
    public static void ExecuteLoadingFunc(int i)
    {

        //避免越界
        if (i == loadingFuncDic.Count)
            return;

        loadingFuncDic[i]();
        ExecuteLoadingFunc(++i);
    }

    //设置进度条跳转信息
    public static void SetProgressInfo(string info)
    {

        LoadingUI.loadingInfo.text = info;
    }

    //进度条成长
    public static void Progress(int val)
    {

        progressVal = val;
        LoadingUI.loadingProgress.fillAmount = val / 100.0f;
    }

    //初始化玩家基本信息
    public static void InitPlayerInfo()
    {

    }

    //初始化玩家所拥有兵卡信息
    public static void InitCardInfo()
    {

    }

    //初始化商店信息
    public static void InitShopInfo()
    {


    }

    //初始化主界面信息
    public static void InitMainUI()
    {

    }
}
