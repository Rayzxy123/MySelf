using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJson;
using Pomelo.DotNetClient;
using System.Threading;
using UnityEngine.UI;


public class TestConnection : MonoBehaviour
{
    
    public string ip = "localhost";
    public int port = 3014;
    public Connection _connection;

    //
    void Start()
    {
        string path = Application.streamingAssetsPath + '/' + Constants.ConfigPath + '/' + Constants.pwdFileName;
        Translate.InitCodeTable(path);

        _connection = new Connection();

        _connection.on("onTick", msg => {
            //LoginLogic.ParseLoginInResponse(msg);
        });

        _connection.on(Connection.DisconnectEvent, msg => {
            Debug.Log("disconnect in" + msg.jsonObj["reason"]);
        });

        _connection.on(Connection.ErrorEvent, msg => {
            Debug.Log("err in" + msg.jsonObj["reason"]);
        });
    }

    void Update()
    {
        _connection.Update();
    }

    //������������
    public void LoginIn(string accountText, string pwdText)
    {

        //������Ǵ��ڷ�����״̬, �Ͳ���connect��
        if (_connection.netWorkState != NetWorkState.DISCONNECTED)
        {
            //�����ǰ״̬ʱ��������״̬
            if (_connection.netWorkState == NetWorkState.CONNECTED)
            {
                JsonObject msg = new JsonObject();
                msg["uid"] = accountText;
                msg["pwd"] = Translate.encodePwd(pwdText);
                _connection.request("gate.loginHandler.loginIn", msg, LoginLogic.ParseLoginInResponse);
            }
        }

        //�����ǰ���ڷ�����״̬,�Ƚ�������
        if (_connection.netWorkState == NetWorkState.DISCONNECTED)
        {
            //��ʼ������
            _connection.InitClient(ip, port, msgObj =>
            {
                JsonObject user = new JsonObject();
                _connection.connect(user, data =>
                {

                    JsonObject msg = new JsonObject();
                    msg["uid"] = accountText;
                    msg["pwd"] = Translate.encodePwd(pwdText);
                    _connection.request("gate.loginHandler.loginIn", msg, LoginLogic.ParseLoginInResponse);
                });
            });
        }
    }

    //����ע������
    public void Register(string accountText, string pwdText)
    {
        //������Ǵ��ڷ�����״̬
        if (_connection.netWorkState != NetWorkState.DISCONNECTED)
        {
            //�����ǰ����������״̬ʱ
            if (_connection.netWorkState == NetWorkState.CONNECTED)
            {
                JsonObject msg = new JsonObject();
                msg["uid"] = accountText;
                msg["pwd"] = Translate.encodePwd(pwdText);
                //��gate����������ע������
                _connection.request("gate.loginHandler.register", msg, LoginLogic.ParseRegisterResponse);
            }
        }

        //�����ǰ���ڷ�����״̬,�Ƚ�������
        if (_connection.netWorkState == NetWorkState.DISCONNECTED)
        {
            _connection.InitClient(ip, port, msgObj => {
                JsonObject user = new JsonObject();
                _connection.connect(user, data => {
                    JsonObject msg = new JsonObject();
                    msg["uid"] = accountText;
                    msg["pwd"] = Translate.encodePwd(pwdText);
                    _connection.request("gate.loginHandler.register", msg, LoginLogic.ParseRegisterResponse);
                });
            });
        }
    }
}

