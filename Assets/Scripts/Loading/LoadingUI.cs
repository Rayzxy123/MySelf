using UnityEngine;
using System.Collections;
using System;
using Objct = UnityEngine.Object;
using UnityEngine.UI;
using DG.Tweening;

//loadingUI挂载在loading根对象上
public class LoadingUI : MonoBehaviour{

    public GameObject loadingPrefab;
    //loadingUI为挂载在loading下显示在屏幕上的对象实例
    public static GameObject loadingUI;
    //回调函数
    public Action<bool> callback;
    //loadingUI中的提示标签
    [HideInInspector]
    public static Text loadingInfo;
    //进度条信息
    [HideInInspector]
    public static Image loadingProgress;

    //释放内存
    public void Clear()
    {

        GameObject.Destroy(transform.gameObject);
    }

    public void Awake()
    {

        InitLoading(null);
    }

    public void Start()
    {

        LoadingManager.InitLoading();
    }

    //初始化Loading相关资源
    public void InitLoading(Action<bool> callback0)
    {

        InitLoadingUI();

        if (callback0 != null)
            callback = callback0;
    }

    //初始化UI
    public void InitLoadingUI()
    {

        //实质化UI界面
        loadingUI = (GameObject)GameObject.Instantiate(loadingPrefab, transform);
        //设置成中心在锚点的位置
        loadingUI.GetComponent<RectTransform>().localPosition = Vector3.zero;
        loadingUI.name = Constants.loadingUIName;

        loadingInfo = loadingUI.transform.GetChild(0).gameObject.GetComponent<Text>();
        loadingProgress = loadingUI.transform.GetChild(2).gameObject.GetComponent<Image>();

        loadingInfo.text = "正在加载资源信息";
    }

    //初始化所有的游戏资源信息
    public void InitGameResources()
    {
        
    }
}
