using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Object = UnityEngine.Object;

public enum Type
{
    None = -1,
    Message = 0, 
    TXT = 1,
    XML = 2
};

public class _Text : MonoBehaviour {

    private readonly Color _defaultColor = Color.black;              //文本的默认颜色信息
    private readonly int _defaultFontSize = 24;                      //文本的默认文字大小  
    //private string[] _words;                             //把当前Text的字拆解成string的串, 考虑到会出现中文字符
    private List<string[]> _wordArray;                     //保存文字数组的列表
    private Action _callback;                           //回调函数
    private Type _type = Type.None;                             //该文本类型
    private bool _coroutineStart = true;                 //协程是否开启
    private float _displaySpeed = 10;                    //文字的输出速率
    private bool _startRichTextOutput = false;           //是否开始富文本的输出
    private List<string> _richTextList;                  //富文本列表
    private int _sentenceMaxLength;                      //定义了一行所能出现的最长长度
    private int _id;                    //id属性标识唯一的文本串
    private bool _skipTextOpen = true;                      //是否跳过当前文字
    private bool _canSkipText = false;                       //是否在可以跳过当前文本的情况下马上跳过文本
    private bool _textOutputFinished = false;                //判断当前文本是否全部输出完毕
    private bool _originCoroutineStart = true;              //即一开始所赋值的协程开启状态
    private float _startTime = 0.0f;                            //是否是第一次初始化
    private bool _first = false;

    [HideInInspector]
    public bool displayTextOnScreen = false;                //是否把文本信息显示在屏幕上
    public Text textInfo;                       //文本信息
    [HideInInspector]
    public Transform root;                      //文字的根节点
    public string msg;                      //保存当text类型为Message时的信息

    public Type type { get { return _type; } set { _type = value; } }
    public List<string[]> wordArray { get { return _wordArray; } set { _wordArray = value; } }

    public int id { get { return _id; } set { _id = value; } }

    void Awake()
    {

        if (textInfo == null)
            textInfo = GetComponent<Text>();
        _wordArray = new List<string[]>();
        _sentenceMaxLength = _defaultFontSize * TextManager.maxRowWordNum;           //定义
        textInfo.color = _defaultColor;
        textInfo.fontSize = _defaultFontSize;

        _startTime = Time.realtimeSinceStartup;
    }

    //设置文本输出速率
    public _Text SetDisplaySpeed(float speed)
    {

        _displaySpeed = speed;
        return this;
    }

    //执行回调函数
    public void Callback()
    {

        if (_callback != null)
            _callback();
    }

    //重新调整文本域大小
    public _Text Resize(int width, int height)
    {

        Vector2 vec = new Vector2(width, height);
        SetSize(vec);
        return this;
    }

    //设置文本类型
    public _Text SetType(Type type)
    {

        _type = type;
        return this;
    }

    //设置当前文本是否可以被一键跳过
    public bool SetCanSkip(bool skip)
    {

        _skipTextOpen = skip;
        return _skipTextOpen;
    }

    //设置文本域的大小
    public _Text SetSize(Vector2 size)
    {

        textInfo.GetComponent<RectTransform>().sizeDelta = size;
        return this;
    }

    //设置文本位置信息
    public _Text SetPosition(Vector3 pos)
    {

        textInfo.GetComponent<RectTransform>().localPosition = pos;
        return this;
    }

    //设置当前字体回调函数
    public _Text SetCallback(Action callback)
    {

        _callback = callback;
        return this;
    }

    //设置字体颜色
    public _Text SetColor(Color c)
    {

        textInfo.color = c;
        return this;
    }

    //设置字体大小
    public _Text SetFontSize(int fontSize)
    {

        textInfo.fontSize = fontSize;
        _sentenceMaxLength = fontSize * TextManager.maxRowWordNum;           //定义
        return this;
    }

    //设置是否开启协程
    public _Text SetCoroutineStart(bool open)
    {

        _coroutineStart = open;
        _originCoroutineStart = _coroutineStart;
        return this;
    }

    //设置是否打开文字移动
    public _Text SetMoveOpen(bool open)
    {
        _first = open;
        return this;
    }

    //这里得到的index是正常的字符串下对应字符的下标,即没有前面空白字符的
    public int FindWords(string text, int lIndex, int num)
    {

        int startIndex = TextManager.richTextList[_id][lIndex];
        //Debug.Log(startIndex);
        int anotherIndex = text.IndexOf('/', startIndex);
        int richTextFrontTagIndex = text.LastIndexOf('>', anotherIndex);

        string label = text.Substring(startIndex, richTextFrontTagIndex - startIndex + 1);
        string richText = text.Substring(richTextFrontTagIndex + 1, anotherIndex - richTextFrontTagIndex - 2);
        int i = anotherIndex, index = 0;

        while (text.IndexOf('/', i + 1) != -1 && (index = text.IndexOf('/', i + 1)) - i < 8)
        {
            //..判定其为一组必标签
            i = index;
            continue;
        }

        int closeTagIndex = text.IndexOf('>', i);
        string anotherLabel = text.Substring(anotherIndex - 1, closeTagIndex - anotherIndex + 2);

        //初始化富文本列表(richTextList)
        if (_richTextList == null)
            _richTextList = new List<string>();

        //遍历当前富文本,为每个字加上富文本效果标签
        for (int wordIndex = 0; wordIndex < richText.Length; ++wordIndex)
            _richTextList.Add(string.Concat(label, richText[wordIndex], anotherLabel));

        //出现了几个特效标签,记录其个数
        while (TextManager.richTextList[_id][lIndex++] <= richTextFrontTagIndex)
            ++num;

        return num;
    }

    //进行文本输出速率的抽取
    public int FindWords(string text, int speedTagIndex)
    {

        int index0 = text.IndexOf('=', speedTagIndex);
        int index1 = text.IndexOf('>', index0);
        int speed = int.Parse(text.Substring(index0 + 1, index1 - index0 - 1));

        return speed;
    }

    //清空富文本列表
    public void ClearRichTextList()
    {

        _richTextList.Clear();
    }

    //逐字展示文本的协程
    IEnumerator DisplayTextW2W()
    {

        _coroutineStart = false;    //确保协程只被启动一次
        //检查该文本是否为空 --可能前面有人忘记初始化文本信息了
        if (textInfo.text != null)
            textInfo.text = "";

        int lIndex = -1, rIndex = -1, richTextIndex = 0;             //lIndex索引第几个<开头
        int totalLength = 0;
        int fontSize = textInfo.fontSize;
        int speedLeftIndex = -1, speedRightIndex = -1;                    //标记速度标签的下标索引
        int num = 0;                                    //记录左<的个数
        string text = msg;
        float speed = _displaySpeed;

        for (int wArrayIndex = 0; wArrayIndex < _wordArray.Count; ++wArrayIndex)
        {

            string[] words = _wordArray[wArrayIndex];
            for (int wordIndex = 0; wordIndex < words.Length; ++wordIndex)
            {

                if (_canSkipText)
                    break;

                //如果达到满足一行20个字,添加一个换行符
                if (totalLength >= _sentenceMaxLength)
                {
                    Resize(totalLength, _wordArray.Count * (textInfo.fontSize + 5));
                    textInfo.text += '\n';
                    totalLength = 0;            //重新进行单行的总长度计算
                }

                //开启速度标签读取模式
                if (speedRightIndex == speedLeftIndex - 1)
                {
                    if (words[wordIndex] == ">")
                    {
                        if (speedRightIndex == speedLeftIndex - 1)
                            speedRightIndex++;
                    }
                    continue;
                }

                if (words[wordIndex] == "<")
                {
                    //判断是否超出了长度范围
                    if (wordIndex + 2 < words.Length)
                    {
                        if (words[wordIndex + 1] == "s" && words[wordIndex + 2] == "p")
                        {

                            speedLeftIndex++;
                            speed = FindWords(text, TextManager.textOutputSpeedList[_id][speedLeftIndex]);
                            continue;
                        }
                    }
                    //如果超出了制定范围长度
                    else if (wordIndex + 2 > words.Length)
                    {
                        //如果当前获得的<字符处在分割数组的最后一位
                        if (wordIndex + 1 == words.Length)
                        {
                            string[] newWords = _wordArray[wArrayIndex + 1];
                            if (newWords[0] == "s" && newWords[1] == "p")
                            {
                                Debug.Log("2");
                                speedLeftIndex++;
                                speed = FindWords(text, TextManager.textOutputSpeedList[_id][speedLeftIndex]);
                                continue;
                            }
                        }
                        //如果当前获得的<字符处在分割数组的倒数第二位
                        else if (wordIndex + 2 == words.Length)
                        {
                            Debug.Log("2");
                            string[] newWords = _wordArray[wArrayIndex + 1];
                            if (words[wordIndex + 1] == "s" && newWords[0] == "p")
                            {

                                speedLeftIndex++;
                                speed = FindWords(text, TextManager.textOutputSpeedList[_id][speedLeftIndex]);
                                continue;
                            }
                        }
                    }
                }

                if (words[wordIndex] == "<")
                {

                    //当出现<标签起始符号时, 进行预测下一位 判断是否是'size'标签
                    if (text[TextManager.richTextList[_id][lIndex + 1] + 1] == 's')
                    {

                        int anotherIndex = text.IndexOf('>', TextManager.richTextList[_id][lIndex + 1]);
                        fontSize = int.Parse(text.Substring(TextManager.richTextList[_id][lIndex + 1] + 6, anotherIndex - TextManager.richTextList[_id][lIndex + 1] - 6));
                    }

                    lIndex++;

                    //判断是否是富文本标签开头
                    if (lIndex == 0 || lIndex % (2 * num) == 0)
                        num = FindWords(text, lIndex, num);

                    _startRichTextOutput = true;        //开启富文本输出流
                }

                if (words[wordIndex] == ">")
                {

                    rIndex++;
                    //如果查询到结束富文本字符
                    if (rIndex != 0 && (rIndex + 1) % (2 * num) == 0)
                    {

                        fontSize = textInfo.fontSize;           //恢复正常文字大小
                        _startRichTextOutput = false;           //关闭富文本输出流
                        continue;
                    }

                }

                if (!_startRichTextOutput)
                    textInfo.text += words[wordIndex];
                else
                {
                    if (richTextIndex != _richTextList.Count)
                        textInfo.text += _richTextList[richTextIndex++];
                    else
                        continue;
                }

                totalLength += fontSize;            //记录当前每行总共长度
                if (!_canSkipText)
                    yield return new WaitForSeconds(1.0f / speed);      //等待s后进行输出
            }

            totalLength = 0;
        }

        _textOutputFinished = true;      //文本已经输出完毕了

    }

    //文字向上移动
    IEnumerator MoveUp()
    {

        while (Time.realtimeSinceStartup - _startTime < 1.0f)
        {

            GetComponent<RectTransform>().localPosition += 20.0f * CVector.vec3Y * Time.deltaTime;
            yield return true;
        }
    }

    //每一帧都进行更新信息
    void Update () {

        if (!TextManager.displayModeOpen)
        {
            if (_first)
            {
                _first = false;
                StartCoroutine(MoveUp());
            }

            if (Time.realtimeSinceStartup - _startTime > 1.0f)
            {
                if (_callback != null)
                    Callback();
            }
        }

        //如果开启了协程,开启了逐字读写模式
        if (_coroutineStart && TextManager.displayModeOpen && displayTextOnScreen)
            StartCoroutine(DisplayTextW2W());

        if (_callback != null && Input.GetMouseButtonDown(0) && _textOutputFinished)
            _callback();

        //如果开启了文字跳过模式,再点击左键的情况下
        if (!_originCoroutineStart || _skipTextOpen && Input.GetMouseButtonDown(0))
        {
            
            //str为初始化字符串
            string str = "";
            //index = 0, i = 0
            int index = 0, i = 0;

            int startIndex = 0;
            while ((index = msg.IndexOf("<speed", i)) != -1)
            {

                //连接字符串
                str = string.Concat(str, msg.Substring(startIndex, index - startIndex));
                startIndex = msg.IndexOf('>', index) + 1;
                i = index + 1;
            }
            str = string.Concat(str, msg.Substring(startIndex));
            msg = str;
            textInfo.text = msg;            //马上显示文本信息
            _canSkipText = true;             //即可以跳过该文本
            _textOutputFinished = true;      //文本已经输出完毕了
        }
    }
}
