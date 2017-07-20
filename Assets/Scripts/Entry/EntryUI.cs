using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Object = UnityEngine.Object;

public class EntryUI : MonoBehaviour{

    //关于保存EntryUI的实例
    public GameObject entryUIPrefab;
    //保存的回调函数
    public Action<bool> callback;
    //EntryUI提示文字闪烁动画

    //清除掉EntryUI可能会用到的所有资源
    public void Clear()
    {

        //记录下遍历的次数
        Transform tf = GameEntry.canvasRoot.transform;

        for (int childIndex = 0; childIndex < tf.childCount; ++childIndex)
        {
            Transform entryTf = tf.GetChild(childIndex);
            //获得每个Child的名字
            string name = entryTf.name;
            if (name == Constants.entryName)
            {

                Object.Destroy(entryTf.gameObject);
                break;
            }
        }
    }

    //游戏进入状态
    //包括进入界面的声音资源, 动画资源, 图片资源
    public void InitEntry(Action<bool> callback0)
    {

        InitEntryUI();
        InitEntryAnimation();
        InitEntrySound(()=> {
            //播放音频
            GameEntry.audioRoot.Play();
        });

        //保存回调函数
        callback = callback0;
    }

    //进行UI界面的初始化
    public void InitEntryUI()
    {
        //初始化entry根对象的位置
        transform.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
        //进行实质化
        GameObject entryUI = (GameObject)Object.Instantiate(entryUIPrefab, transform);
        //更新下名字,使其不带有Clone字段
        entryUI.name = entryUIPrefab.name;
        RectTransform rt = entryUI.GetComponent<RectTransform>();
        rt.localPosition = Vector3.zero;
    }

    //初始化进场动画
    public void InitEntryAnimation()
    {

    }

    //初始化进场音乐
    public void InitEntrySound(Action callback)
    {

        string entryBgmPath = Constants.entryPath + '/' + Constants.entryBgm;
        AudioClip ac = Resources.Load<AudioClip>(entryBgmPath);
        if (ac != null)
        {
            GameEntry.audioRoot.clip = ac;
            if(callback != null)
                callback();
        }
    }

    //加入点击事件逻辑s
    public void ClickListener(Action callback)
    {
        //如果当前处在entry界面
        if (UIManager.uiDic[Constants.entryName].activeSelf)
        {

            if (callback != null)
                callback();
        }
    }

    public void Awake()
    {

    }

    public void Start()
    {
        
        InitEntry(null);

    }

    public void Update()
    {

        if (Input.GetMouseButtonDown(0))
            ClickListener(() => {
                //状态切换到登陆状态
                GameEntry.gameState = GameState.GameState_LOGIN;
            });
    }
}
