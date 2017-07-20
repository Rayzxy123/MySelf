using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class NumberManager {

    public static Transform root;               //NumberManager在scene上的根节点
    public static List<Sprite> numSprites;   //所有数字共享的纹理资源
    public static List<_Number> numbers;        //管理所有在屏幕上显示的数字

    //初始化数字所需纹理资源
    public static void InitNumberResource()
    {

        root = GameEntry.numberRoot.transform;

        //初始化纹理书资料表
        if (numSprites == null)
            numSprites = new List<Sprite>();

        //初始化管理数字列表
        if (numbers == null)
            numbers = new List<_Number>();

        Sprite[] sprites = Resources.LoadAll<Sprite>("Numbers/");

        for (int spriteIndex = 0; spriteIndex < sprites.Length; ++spriteIndex)
            numSprites.Add(sprites[spriteIndex]);
    }

    //Show所做的操作就是返回挂载的_Number对象
    public static _Number Show(string content)
    {

        GameObject number = (GameObject)Object.Instantiate(Resources.Load<GameObject>("Number"), root);
        number.GetComponent<RectTransform>().localPosition = Vector3.zero;

        _Number num = number.GetComponent<_Number>();
        number.GetComponent<RectTransform>().localPosition = Vector3.zero;

        if (content == null || content.Length == 0
                        || content.Trim().Length == 0)
        {
            throw new System.Exception("数字不可为空");
        }
        else 
            num.SetContent(content);

        if (numbers == null)
            numbers = new List<_Number>();

        //把当前数字加入管理列表中
        numbers.Add(num);

        return num;
    }



    /// <summary>
    /// 资源删减后的回调函数
    /// </summary>
    /// <param name="resId">对应的资源Id</param>
    /// <param name="val">改变的资源值</param>
    public static void onResourceChange(int resId, int val)
    {
        //传递到下一层
        numbers[resId].onChange(val);
    }
}
