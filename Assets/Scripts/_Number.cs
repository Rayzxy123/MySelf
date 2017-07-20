using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;
using Object = UnityEngine.Object;

public class _Number : MonoBehaviour {

    [SerializeField]
    private readonly int _defaultSize = 20;   //该Number中默认单个元素的大小
    [SerializeField]
    private string _content;        //该number的内容
    private int preNowBitDis = 999;           //记录改变前数字和改变后数字的位数差别
    private float _firstCharPosX = 0;           //记录第一个字符的中心坐标
    private float _lastCharPosX = 0;         //记录最后一个字符的中心位置
    [SerializeField]
    private Vector3 _pos = new Vector3(0.0f, 0.0f, 0.0f);           //数字出现的位置
    private List<Image> _pics;           //该number所对应的图片资源
    [SerializeField]
    private int _size = 0;          //该number手动设置的每个元素的大小
    [SerializeField]
    private readonly float _defaultRollInterval = 0.05f;         //默认滚动间隔
    [SerializeField]
    private readonly float _defaultRollTime = 1.0f;              //默认滚动时长
    [SerializeField]
    private float _rollInterval = 0.0f;         //数字滚动间隔
    [SerializeField]
    private float _rollTime = 0.0f;              //数字滚动时长
    [SerializeField]
    private bool isInited = false;      //判断是否是第一次初始化
    [SerializeField]
    private bool _changeModeOpen = true;    //是否开启特效模式
    [SerializeField]
    private bool _canChange = false;     //是否开始改变
    [SerializeField]
    private float _tickStartTime = 0.0f;   //程序开始时立即开始计时 用于估计60秒的间隔时间
    [SerializeField]
    private bool _setContentOpen = false;   //是否设置数字
    [SerializeField]
    private float _triggerStartTime = 0.0f;   //触发数字滚动特效的开始时间(除了onTick方法触发方式的所有其他触发方式)
    [SerializeField]
    private int changeNum;          //改变的数字差值大小
    [SerializeField]
    private bool isTriggered = false;        //是否在触发状态下
    [SerializeField]
    private bool isAdd = true;              //是否是处于加资源的状态
    [HideInInspector]
    public Action callback;            //回调函数
    [HideInInspector]
    public Transform root;          //该类挂载的根节点

    public float rollInterval { set { _rollInterval = value; } get { return _rollInterval; } }
    public float rollTime { set { _rollTime = value; } get { return _rollTime; } }

    // Use this for initialization
    void Start () {

        //初始化List存放纹理的List
        if (_pics == null)
            _pics = new List<Image>();

        _lastCharPosX = GetComponent<RectTransform>().sizeDelta.y - 5.0f;
    }

    //清空图片列表
    public void ClearPics()
    {

        //如果纹理资源列表不为空的话
        if(_pics!=null && _pics.Count > 0)
            _pics.Clear();
    }

    //把转换进来的字符串转换成纹理贴图
    public void ConvertToSprites(string str)
    {

        //匹配所有字符都是数字的字符串
        Regex rg = new Regex(@"^\d*$");
        bool isMatch = true;

        //前提传入的字符串不为空
        if (str != null)
            isMatch = rg.Match(str).Success;
        else
            throw new Exception("输入字符串不能为空,我的哥");

        //如果模式不匹配的话
        if (!isMatch)
            throw new Exception("请输入纯数字, Thank U");
        else
        {
            //检查下List是否被初始化
            if (_pics == null)
                _pics = new List<Image>();

            AdjustPosition(str);
        }
    }

    //重新调整纹理所在的位置
    public void AdjustPosition(string str)
    {
        //即还没有进行初始化操作
        if (_lastCharPosX == 0)
            _lastCharPosX = GetComponent<RectTransform>().sizeDelta.y - 5.0f;

        //判断是否已经初始化过
        if (!isInited)
        {

            //遍历字符串
            for (int strIndex = 0; strIndex < str.Length; ++strIndex)
            {


                Image img = ((GameObject)Object.Instantiate(Resources.Load<GameObject>("Image"), transform)).GetComponent<Image>();
                //获得每个纹理的RectTransform
                RectTransform rt = img.GetComponent<RectTransform>();
                rt.localPosition = Vector3.zero;
                if (_size != 0)
                {
                    rt.sizeDelta = new Vector2(_size, _size);
                    rt.localPosition = new Vector3(_lastCharPosX - (str.Length - strIndex - 1)* (_size - _size / 5), 0.0f, 0.0f);
                }
                else
                {
                    rt.sizeDelta = new Vector2(_defaultSize, _defaultSize);
                    rt.localPosition = new Vector3(_lastCharPosX - (str.Length - strIndex - 1) * (_defaultSize - _defaultSize / 5), 0.0f, 0.0f);
                }
                //加入图片资源
                img.sprite = NumberManager.numSprites[int.Parse(str[strIndex].ToString())];
                _pics.Add(img);
            }

            isInited = true;
        }
        else
        {

            if (!isAdd)
                RemoveAtLast(preNowBitDis);

            //遍历字符串
            for (int strIndex = 0; strIndex < str.Length; ++strIndex)
            {
                //得到字符串的单个纹理图片对象
                Image img = _pics[strIndex];
                img.name = str[strIndex].ToString();
                //更换纹理资源
                img.sprite = NumberManager.numSprites[int.Parse(str[strIndex].ToString())];
                RectTransform rt = img.GetComponent<RectTransform>();
                if (_size != 0)
                {
                    rt.sizeDelta = new Vector2(_size, _size);
                    rt.localPosition = new Vector3(_lastCharPosX - (str.Length - strIndex - 1) * (_size - _size / 5), 0.0f, 0.0f);
                }
                else
                {
                    rt.sizeDelta = new Vector2(_defaultSize, _defaultSize);
                    //Debug.Log(_lastCharPosX - (str.Length - strIndex - 1) * (_defaultSize - _defaultSize / 5));
                    rt.localPosition = new Vector3(_lastCharPosX - (str.Length - strIndex - 1) * (_defaultSize - _defaultSize / 5), 0.0f, 0.0f);
                }
            }

            _canChange = false;
        }

        if (_size != 0)
            _firstCharPosX = _lastCharPosX - (str.Length - 1) * (_size - _size / 5);
        else
            _firstCharPosX = _lastCharPosX - (str.Length - 1) * (_defaultSize - _defaultSize / 5);
    }

    //执行数字发生改变后的逻辑
    public void onChange(int val)
    {

        isAdd = val > 0 ? true : false;

        changeNum = val;
        //如果开启了滚动特效的话
        if (_changeModeOpen)
        {
            _triggerStartTime = Time.realtimeSinceStartup;
            isTriggered = true;
        }
        _canChange = true;
    }

    //进行数字对应纹理的滚动
    IEnumerator Roll(string rollNumbers, float startTime, int index, bool up = true, float interval = 0.05f, float rollTime = 2.0f, Action callback = null, bool deleteNum = false)
    {

        bool first = true;
        List<Sprite> sprites = NumberManager.numSprites;
        //必须格外注意字符与整数型之间的相互转换
        int spriteIndex = rollNumbers[index] - '0';

        while (Time.realtimeSinceStartup - startTime < rollTime)
        {
            if (Time.realtimeSinceStartup - startTime > rollTime / 2.0f && first)
            {
                first = false;
                if (!deleteNum)
                {
                    if (callback != null)
                        callback();
                }
            }

            //如果到达滚动上限
            if (_pics[index].sprite.name == sprites[sprites.Count - 1].name && up)
                _pics[index].sprite = sprites[0];

            //如果到达滚动下限
            if (_pics[index].sprite.name == sprites[0].name && !up)
                _pics[index].sprite = sprites[sprites.Count - 1];

            if (up)
            {
                _pics[index].sprite = sprites[spriteIndex++];
                spriteIndex = spriteIndex > 9 ? 0 : spriteIndex;
            }
            else
            {
                _pics[index].sprite = sprites[spriteIndex--];
                spriteIndex = spriteIndex < 0 ? 9 : spriteIndex;
            }
            yield return new WaitForSeconds(interval);
        }

        //当确保是删除数字的滚动操作后 才调用回调函数
        if (deleteNum)
        {
            //完成连串的滚动动作后
            if (callback != null)
                callback();
        }
    }

    //处理数字的滚动逻辑
    //包括startIndex, endIndex所指向的字符
    public void RollNumber(string rollNumbers, int startIndex, int endIndex, bool up = true, float interval = 0.05f, float rollTime = 2.0f, 
                    Action callback = null, bool isNewNum = false, bool deleteNum = false)
    {

        //如果不是新数字
        if (!isNewNum)
        {

        }
        //如果是新插入的数字的话
        else
            insertNumInitInfo(startIndex);

        while (startIndex <= endIndex)
            StartCoroutine(Roll(rollNumbers, Time.realtimeSinceStartup, startIndex++, up, interval, rollTime, callback, deleteNum));
    }

    //插入数字信息在屏幕上显示出来
    public void insertNumInitInfo(int startIndex)
    {

        _pics[startIndex].sprite = NumberManager.numSprites[startIndex + 1];
        RectTransform rt = _pics[startIndex].GetComponent<RectTransform>();

        //进行插入数字的重新定位
        if (_size != 0)
        {
            rt.sizeDelta = new Vector2(_size, _size);
            rt.localPosition = new Vector3(_firstCharPosX - (_size - _size / 5), 0.0f, 0.0f);
            _firstCharPosX -= (_size - _size / 5);
        }
        else
        {
            rt.sizeDelta = new Vector2(_defaultSize, _defaultSize);
            rt.localPosition = new Vector3(_firstCharPosX - (_defaultSize - _defaultSize / 5), 0.0f, 0.0f);
            _firstCharPosX -= (_defaultSize - _defaultSize / 5);
        }
    }

    //用来处理滚动新数字的协程
    IEnumerator RollNewNum(int dis, float localRollTime, string newNum)
    {

        int startIndex = dis - 1;

        float rollTime1 = localRollTime / 2.0f;
        //index的作用是为了只进入一次滚动函数,否则会进行无限滚动下去
        int index = 1000000000;
        //当滚动到一定程度时, 插入第一个数字, 并根据需要进行滚动
        while (startIndex >= 0)
        {
            if (index != startIndex)
            {

                float interval = 0.0f;

                index = startIndex;

                //如果不是第一位数字的话,就进行任意的滚动
                if (startIndex != 0)
                {
                    if (newNum[startIndex] != '1' && newNum[startIndex] != '0')
                        interval = rollTime1 / (newNum[startIndex] - '1' + 1);
                    else
                    {
                        if (_rollInterval - 0.0f > 0.00001f)
                            interval = _rollInterval;
                        else
                            interval = _defaultRollInterval;
                    };

                    RollNumber(newNum, startIndex, startIndex, true, interval, rollTime1, () =>
                    {
                        rollTime1 /= 2.0f;
                        startIndex--;
                    }, true, false);
                }
                //如果是第一位的话
                else
                {
                    if (newNum[startIndex] != '1' && newNum[startIndex] != '0')
                    {
                        interval = rollTime1 / (newNum[startIndex] - '1' + 1);
                        RollNumber(newNum, startIndex, startIndex, true, interval, rollTime1, () =>
                        {
                            rollTime1 /= 2.0f;
                            startIndex--;
                        }, true, false);
                    }
                    else
                        insertNumInitInfo(startIndex--);
                }
            }

            yield return true;
        }
    }

    //滚动那些即将要被删除掉数字的协程
    IEnumerator RollDeleteNum(int startIndex, int dis, float localRollTime, string content)
    {
        //逻辑结束时的索引
        int endIndex = dis - 1;

        //高位数字滚动间隔
        float interval = 0.0f;
        //高位数字总的滚动时间
        float rollTime = localRollTime / 2.0f;

        bool first = true;
        int index = 10000;

        while (startIndex <= endIndex)
        {
            //如果是第一次加载的话
            if (first && (index != startIndex))
            {

                index = startIndex;
                first = false;
                interval = rollTime / (content[startIndex] - '0' + 1);
                //滚动数字
                RollNumber(content, startIndex, startIndex, false, interval, rollTime, () =>
                {
                    MoveSpritePos(startIndex);
                    rollTime /= 2.0f;
                    startIndex++;
                }, false, true);

                continue;
            }

            if (index != startIndex)
            {
                index = startIndex;
                //如果不是1的话
                if (content[startIndex] != '1')
                    interval = rollTime / (content[startIndex] - '0' + 1);
                else
                    interval = rollTime / (content[startIndex] - '0' + 1);

                //滚动数字
                RollNumber(content, startIndex, startIndex, false, interval, rollTime, () =>
                {
                    MoveSpritePos(startIndex);
                    rollTime /= 2.0f;
                    startIndex++;
                }, false, true);
            }
            yield return true;
        }
    }

    //doChange的操作
    //@param val的值为我们希望其改变的值
    public void doChange(int val)
    {

        //把_content内容翻译到整形
        int past = int.Parse(_content);
        string newNum = (past + val).ToString();
        //位差
        preNowBitDis = Mathf.Abs(newNum.Length - _content.Length);

        if (_changeModeOpen)
        {

            bool first = true;

            //如果没有定义_rollInterval, 就使用默认滚动间隔
            if ((rollInterval - 0.0f) < 0.0001f)
                rollInterval = _defaultRollInterval;

            if ((rollTime - 0.0f) < 0.0001f)
                rollTime = _defaultRollTime;

            //如果位数发生改变的话
            if (newNum.Length != _content.Length)
            {
                //插入若干位
                if (newNum.Length > _content.Length)
                {
                    //dis表示插入的位数
                    int dis = newNum.Length - _content.Length;
                    InsertAtFirst(dis);
                    //滚动数字
                    RollNumber(newNum, dis, _pics.Count - 1, true, rollInterval, rollTime, () =>
                    {
                        if (first)
                        {
                            first = false;
                            StartCoroutine(RollNewNum(dis, rollTime, newNum));
                        }
                    });
                }
                //删除若干位
                else
                {

                    //删除了多少位
                    int dis = _content.Length - newNum.Length;

                    int startRollIndex = 1;

                    if (_content[0] == '1')
                    {
                        MoveSpritePos(0);
                        startRollIndex = 2;
                    }

                    //即将要被删除的那几位
                    StartCoroutine(RollDeleteNum(startRollIndex - 1, dis, rollTime, _content));

                    //向下从dis位开始的滚动数字
                    RollNumber(_content, startRollIndex, _pics.Count - 1, false, rollInterval, rollTime);
                }
            }
            else
            {

                //滚动距离
                int dis = newNum[0] - _content[0];
                float interval;
                if (dis != 0)
                    interval = (rollTime / 2.0f) / Mathf.Abs(dis);
                else
                {

                    rollTime = 0.5f;
                    rollInterval = 0.05f;
                    interval = rollTime / 2.0f;
                }

                //保存_content内容的临时副本
                string content = _content;

                if (_content.Length > 1)
                {

                    //首先滚动除第一个外所有数字
                    if (int.Parse(newNum) > past)
                        RollNumber(_content, 1, _pics.Count - 1, true, rollInterval, rollTime, () =>
                        {
                            if (newNum[0] != content[0])
                                //滚动第一个数字
                                RollNumber(content, 0, 0, true, interval, rollTime / 2.0f);
                        });
                    else
                        RollNumber(_content, 1, _pics.Count - 1, false, rollInterval, rollTime, () =>
                        {
                            if (newNum[0] != content[0])
                                //滚动第一个数字
                                RollNumber(content, 0, 0, false, interval, rollTime / 2.0f);
                        });
                }
                else
                {

                    //重新定义滚动间隔
                    interval = rollTime / Mathf.Abs(dis); 
                    //进行低位慢速滚动
                    RollNumber(_content, 0, 0, true, interval, rollTime);
                }
            }

            //关闭掉可以改变的姿势
            _canChange = false;
            _content = newNum;
        }
        else
        {

            //如果位数发生改变的话
            if (newNum.Length != _content.Length)
            {
                //插入若干位
                if (newNum.Length > _content.Length)
                    InsertAtFirst(newNum.Length - _content.Length);
                //删除若干位
                else
                    RemoveAtLast(_content.Length - newNum.Length);
            }

            SetContent(newNum);
        }
    }

    //在纹理列表最前面插入若干位
    public void InsertAtFirst(int count)
    {

        for (int index = 0; index < count; ++index)
        {
            Image img = ((GameObject)GameObject.Instantiate(Resources.Load<GameObject>("Image"), transform)).GetComponent<Image>();
            img.GetComponent<RectTransform>().localPosition = new Vector3(-1000.0f, 0.0f, 0.0f);
            _pics.Insert(0, img);
        }
    }

    //移动纹理在屏幕上的位置,再统一删除
    public void MoveSpritePos(int index)
    {

        _pics[index].GetComponent<RectTransform>().localPosition = new Vector3(1000.0f, 0.0f, 0.0f);
        _pics[index].sprite.name = "0";
    }

    //移除掉首个元素
    public void RemoveAtFirst()
    {
        _pics.RemoveAt(0);
    }

    //移除掉纹理列表的最后若干位
    public void RemoveAtLast(int count)
    {
        Transform tf = GameEntry.numberRoot.transform.GetChild(0);
        for (int index = 0; index < count; ++index)
        {

            Destroy(tf.GetChild(index).gameObject);
            _pics.RemoveAt(0);
        }
    }

    //获取当前_Number中管理的数字内容
    public string GetContent()
    {

        return _content;
    }

    //设置输出内容
    public _Number SetContent(string content)
    {
        //初始化内容
        _content = content;
        //把字符串转换成图片资源
        ConvertToSprites(content);

        return this;
    }

    //设置单个纹理的大小
    public _Number SetSize(int size)
    {
        _size = size;

        AdjustPosition(_content);
        return this;
    }

    //设置位置
    public _Number SetPosition(Vector3 pos)
    {
        _pos = pos;
        transform.localPosition = _pos;
        return this;
    }

    //设置当前模式
    public _Number SetModeOpen(bool open)
    {

        _changeModeOpen = open;
        return this;
    }

    //设置回调函数
    public _Number SetCallback(Action callback)
    {
        if(callback != null)
            this.callback = callback;
        return this;
    }

    //每隔60秒发送以下修改数字的请求
    //可以被提前调用
    public void onTick()
    {

        SetContent(_content);
    }

    // Update is called once per frame
    void Update() {
        //只执行一次
        if (_canChange)
            doChange(changeNum);

        //每隔60秒调用一次onTick函数
        if ((Time.realtimeSinceStartup - _tickStartTime > 60.0f) || _setContentOpen)
        {

            Debug.Log("onTick.........................................");
            _tickStartTime = Time.realtimeSinceStartup;
            _setContentOpen = false;
            isTriggered = false;
            onTick();
        }

        //如果当前处于可出发状态
        if (isTriggered)
        {

            //rollTime + 0.22秒的原因是要解决数字滚动溢出的问题
            //不想解决根本问题,干脆上调一下调整时间
            if (Time.realtimeSinceStartup - _triggerStartTime > (rollTime + (preNowBitDis / 40.1f + 0.02f)))
                _setContentOpen = true;
        }
	}
}
