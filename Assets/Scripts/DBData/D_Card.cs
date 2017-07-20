using UnityEngine;
using System.Collections;

public class D_Card {

    public int id;                      //一个兵种对应一个Id
    public string name;             //该兵种的名字信息(即卡片的标题信息)
    public string info;             //该兵种的介绍
    public int color;               //该兵种的颜色等级
    public int atk1;                //物理攻击力
    public int atk2;                //魔法攻击力
    public int def;                 //防御力
    public int hp;              //血量
    public int mp;              //魔量
    public int moveDis;         //移动距离
    public int atkArea;         //攻击范围 1:默认 
    public int atkDis;          //攻击距离 1:一格 2:二格 3:4格 
    public int atkNum;          //可攻击数目: 1:可攻击一个 2:可攻击二个
    public int energy;      //召唤该兵种所需的能量
    public string price;    //金币|宝石 按照这样的格式
}
