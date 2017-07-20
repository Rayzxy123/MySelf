using UnityEngine;
using System.Collections;
using System;
using DG.Tweening;
using Object = UnityEngine.Object;

public enum GameState
{
    //游戏进入状态
    GameState_ENTRY,
    //游戏登录状态
    GameState_LOGIN,
    //游戏加载状态
    GameState_LOADING,
    //游戏进入状态
    GameState_INGAME
};

//游戏主入口
//因为继承了MonoBehaviour的类都是会被Editor进行序列化的
//所以静态字段也可以在非静态方法里使用
public class GameEntry : MonoBehaviour {

    //根画布(挂载UI信息)
    public static GameObject canvasRoot;
    //挂载所有文字信息的根对象
    public static GameObject textRoot;
    //挂载所有数字信息的根对象
    public static GameObject numberRoot;
    //挂载所有提示框信息的根对象
    public static GameObject alertRoot;
    //连接中心
    public static GameObject connectionRoot;
    //控制背景UI播放的声源
    public static AudioSource audioRoot;
    //保存游戏状态的切换
    public static GameState gameState;
    //是否加载过进入时所需的必要资源
    private bool entryResourceLoaded = false;
    //是否加载过登陆时所需的必要资源
    private bool loginResourceLoaded = false;
    //是否加载过游戏所必要的资源
    private bool gameResourceLoaded = false;

    void Start () {

        canvasRoot = GameObject.Find("Root");
        connectionRoot = GameObject.Find("TestConnection");

        textRoot = GameObject.Find("TextManager");
        numberRoot = GameObject.Find("NumberManager");
        alertRoot = GameObject.Find("AlertManager");
        audioRoot = canvasRoot.GetComponent<AudioSource>();

        DOTween.Init();             //简单初始化DOTween
        UIManager.InitUIDic();                 //初始化保存UI的数据字典
        NumberManager.InitNumberResource();             //初始化NumberManager所需要的纹理资源
        CVector.InitVector3();                  //初始化Vector3配置信息

        gameState = GameState.GameState_ENTRY;     
    }
	
    //游戏主循环
	void Update () {

        //游戏状态切换处理对应逻辑
        switch (gameState)
        {
            //刚刚进入游戏
            case GameState.GameState_ENTRY:
                //前提是满足所有条件的情况下
                //如果点击了鼠标, 就可以进入Login界面了
                if (!entryResourceLoaded)
                {

                    string uiPath = Constants.entryPath + '/' + Constants.entryName;
                    entryResourceLoaded = true;
                    //调用ui初始化方法
                    GameObject entry = (GameObject)Object.Instantiate(Resources.Load<GameObject>(uiPath), canvasRoot.transform);
                    entry.name = Constants.entryName;
                    UIManager.Add(entry);
                }
                break;
            //进入登录界面
            case GameState.GameState_LOGIN:
                //如果仍然没有加载登陆资源
                if (!loginResourceLoaded)
                {

                    loginResourceLoaded = true;
                    string loginPath = Constants.loginPath + '/' + Constants.loginName;
                    //卸载掉 进入界面的资源
                    UIManager.uiDic[Constants.entryName].GetComponent<EntryUI>().Clear();
                        GameObject login = (GameObject)Object.Instantiate(Resources.Load<GameObject>(loginPath), canvasRoot.transform);
                    login.name = Constants.loginName;
                    UIManager.Add(login);
                }
                break;
            case GameState.GameState_LOADING:

                //确保只初始化一次资源
                if (!gameResourceLoaded)
                {

                    //释放掉Login界面所占用的内存
                    UIManager.uiDic[Constants.loginName].GetComponent<LoginUI>().Clear();
                    gameResourceLoaded = true;
                    //加载loadingUI
                    string uiPath = Constants.loadingPath + '/' + Constants.loadingName;
                    GameObject loading = (GameObject)Object.Instantiate(Resources.Load<GameObject>(uiPath), GameEntry.canvasRoot.transform);
                    loading.GetComponent<RectTransform>().localPosition = Vector3.zero;
                    loading.name = Constants.loadingName;
                    UIManager.Add(loading);
                }
                break;
            case GameState.GameState_INGAME:
                break;
        }
	}

}
