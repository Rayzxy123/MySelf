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

    //发送连接请求
    public void LoginIn(string accountText, string pwdText)
    {

        //如果不是处在非连接状态, 就不用connect了
        if (_connection.netWorkState != NetWorkState.DISCONNECTED)
        {
            //如果当前状态时处于连接状态
            if (_connection.netWorkState == NetWorkState.CONNECTED)
            {
                JsonObject msg = new JsonObject();
                msg["uid"] = accountText;
                msg["pwd"] = Translate.encodePwd(pwdText);
                _connection.request("gate.loginHandler.loginIn", msg, LoginLogic.ParseLoginInResponse);
            }
        }

        //如果当前处于非连接状态,先进行连接
        if (_connection.netWorkState == NetWorkState.DISCONNECTED)
        {
            //初始化连接
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

    //发送注册请求
    public void Register(string accountText, string pwdText)
    {
        //如果不是处在非连接状态
        if (_connection.netWorkState != NetWorkState.DISCONNECTED)
        {
            //如果当前正处在连接状态时
            if (_connection.netWorkState == NetWorkState.CONNECTED)
            {
                JsonObject msg = new JsonObject();
                msg["uid"] = accountText;
                msg["pwd"] = Translate.encodePwd(pwdText);
                //向gate服务器发送注册请求
                _connection.request("gate.loginHandler.register", msg, LoginLogic.ParseRegisterResponse);
            }
        }

        //如果当前处于非连接状态,先进行连接
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

