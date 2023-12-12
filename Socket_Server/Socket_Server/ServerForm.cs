﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Socket_Server.ServerForm;

namespace Socket_Server
    
{
    public partial class ServerForm : Form
    {
        /*public class ClientName
        {
            public static string name;

        }*/
        List<Socket> ClientSocketList = new List<Socket>();
        //Dictionary<string, string> dicSocket = new Dictionary<string, string>();
    
        
        public ServerForm()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //1. 创建一个负责监听的Socekt
            Socket socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //绑定端口IP（服务器内网）
            //创建IP地址和端口号对象
            IPAddress ip = IPAddress.Parse(txtIP.Text);
            IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32(txtPort.Text));
            //让负责监听的Socekt绑定IP地址和端口号
            try
            {
                socketWatch.Bind(point);
            }
            catch (Exception ex)
            {
                MessageBox.Show("无法启动服务器：" + ex.Message);
                return;
            }

            btnStart.Enabled = false;

            //设置监听队列
            socketWatch.Listen(20);

            //创建一个新线程执行监听程序
            Thread th = new Thread(Listen);
            th.IsBackground = true;
            th.Start(socketWatch);
        }

        private void ShowMsg(string str)
        {
            txtLog.AppendText(str);
            txtLog.AppendText("\r\n");
        }

        //这个Listen是自定义的方法
        #region 监听线程
        private void Listen(object o)
        {
            Socket socketWatch = o as Socket;
            ShowMsg("服务器端开始接收客户端的连接！");
            while (true)
            {
                Socket proxSocket = socketWatch.Accept();   //阻塞进程直到有客户端连接
                ShowMsg(string.Format("客户端：{0}上线了！", proxSocket.RemoteEndPoint.ToString()));

                //开启一个不断接收客户端的新线程
                Thread th = new Thread(ReceiveData);
                th.IsBackground = true;
                th.Start(proxSocket);

            }
        }
        #endregion
        
        //服务器端不停地接收客户端发送过来的消息
        private void ReceiveData(object o)
        {

            Socket proxsocket = o as Socket;
            SendMsgForAll(string.Format("客户端：{0}上线了！", proxsocket.RemoteEndPoint.ToString()));
            ClientSocketList.Add(proxsocket);
            while (true)
            {
                byte[] data = new byte[1024 * 1024];

                //当客户端连接成功后，服务器应该接收客户端发来的消息

                //获取收到的数据的字节数
                int len = 0;
                try
                {
                    len = proxsocket.Receive(data, 0, data.Length, SocketFlags.None);
                }
                catch (Exception ex)
                {
                    if (ClientSocketList.Contains(proxsocket))
                    {
                        //异常退出
                        ClientSocketList.Remove(proxsocket);
                        ShowMsg(string.Format("客户端：{0}非正常退出！", proxsocket.RemoteEndPoint.ToString()));
                        SendMsgForAll(string.Format("客户端：{0}非正常退出！", proxsocket.RemoteEndPoint.ToString()));
                        return;
                    }
                }

                //客户端正常退出
                if (len <= 0)
                {
                    if (ClientSocketList.Contains(proxsocket))
                    {
                        ClientSocketList.Remove(proxsocket);
                        ShowMsg(string.Format("客户端[{0}]正常退出！", proxsocket.RemoteEndPoint.ToString()));
                        SendMsgForAll(string.Format("客户端[{0}]正常退出！", proxsocket.RemoteEndPoint.ToString()));
                    }
                    return;
                }
                #region 接收到的是字符串
                if (data[0] == 1)
                {
                    //开启字符串转发线程
                    /*objClass stringobj = new objClass();
                    stringobj.objSocket = proxsocket;
                    stringobj.objData = data;
                    Thread stringth = new Thread(new ParameterizedThreadStart(TransReceiveStringAll));
                    stringth.Start(stringobj);*/
                    string strTmp = ProcessReceiveString(data);
                    /*foreach (KeyValuePair<string, string> kvp in dicSocket) {
                        if (kvp.Key.Equals(proxSocket.RemoteEndPoint.ToString())) {
                            ClientName.name = kvp.Value;
                        }
                    }*/
                    //strTmp = string.Format("客户端[" + proxSocket.RemoteEndPoint.ToString() +"]"+"："+ strTmp);
                    //strTmp = string.Format("客户端[{0}]：{1}", ClientName.name, strTmp);
                    strTmp = string.Format("客户端[{0}]：{1}", proxsocket.RemoteEndPoint.ToString(), strTmp);
                    ShowMsg(strTmp);
                    if (ClientSocketList.Contains(proxsocket))
                    {
                        foreach (Socket socketTmp in ClientSocketList)
                        {
                            if (socketTmp != proxsocket)
                            {
                                SendMsg(socketTmp, strTmp);
                            }
                        }
                    }
                }
                #endregion

                #region 接收到的是“戳一戳”
                else if (data[0] == 2)
                {
                    foreach (var proxSocket in ClientSocketList)
                    {
                        if (proxsocket.Connected&&proxSocket!=proxsocket)
                        {
                            proxSocket.Send(new byte[] { 2 }, SocketFlags.None);
                        }
                    }
                }
                #endregion
                /*#region 接收到的是昵称
                if (data[0] == 3)
                {
                    //存线程昵称
                    //dicSocket.Add(Encoding.Default.GetString(data, 1, data.Length - 1),proxsocket.RemoteEndPoint.ToString());
                    //dicSocket.Add(proxsocket.RemoteEndPoint.ToString(), ProcessReceiveString(data));
                    string name = Encoding.Default.GetString(data, 1, 1024);
                    string Name= name.Substring(0, 1024);
                    dicSocket.Add(proxsocket.RemoteEndPoint.ToString(), Name);
                }
                #endregion*/
            }
        }

        #region 转发接收到的字符串
        private void TransReceiveStringAll(object o)
        {
            objClass result = o as objClass;
            Socket proxSocket = result.objSocket;
            byte[] data = result.objData;
            string strTmp = ProcessReceiveString(data);
            /*foreach (KeyValuePair<string, string> kvp in dicSocket) {
                if (kvp.Key.Equals(proxSocket.RemoteEndPoint.ToString())) {
                    ClientName.name = kvp.Value;
                }
            }*/
            //strTmp = string.Format("客户端[" + proxSocket.RemoteEndPoint.ToString() +"]"+"："+ strTmp);
            //strTmp = string.Format("客户端[{0}]：{1}", ClientName.name, strTmp);
            strTmp = string.Format("客户端[{0}]：{1}", proxSocket.RemoteEndPoint.ToString(), strTmp);
            ShowMsg(strTmp);
            if (ClientSocketList.Contains(proxSocket))
            {
                foreach(Socket socketTmp in ClientSocketList)
                {
                    if(socketTmp != proxSocket)
                    {
                        SendMsg(socketTmp, strTmp);
                    }
                }
            }
        }

        #endregion

        #region 处理接收到的字符串
        private string ProcessReceiveString(byte[] data)
        {
            //把实际的字符串拿到
            string str = Encoding.Default.GetString(data,1,data.Length-1);
            return str;
        }
        #endregion

        #region 发送字符串消息
        private void SendMsg(Socket socketTmp, string Msg)
            {
                //原始字符串转成字节数组
                byte[] data = Encoding.Default.GetBytes(Msg);

                //对原始的数据数组加上协议的头部字节
                byte[] result = new byte[data.Length + 1];

                //设置当前的协议头部字节是1：代表字符串
                result[0] = 1;

                //把原始的数据放到最终的字节数组里去
                Buffer.BlockCopy(data, 0, result, 1, data.Length);
                socketTmp.Send(result, 0, result.Length, SocketFlags.None);
            }
            #endregion

            #region 给所有当前连接上的客户端发送字符串消息
            private void SendMsgForAll(string Msg)
            {
                foreach (var socketTmp in ClientSocketList)
                {
                    if (socketTmp.Connected)
                    {
                        SendMsg(socketTmp, Msg);
                    }
                }
            }

            #endregion

            #region 服务器端发送消息
            private void btnSendMsg_Click(object sender, EventArgs e)
            {
            ShowMsg("服务器端："+txtMsg.Text);
                SendMsgForAll("服务器端：" + txtMsg.Text);
                txtMsg.Clear();
                txtMsg.Focus();

            }
            #endregion

            #region 戳一戳
            private void btnShock_Click(object sender, EventArgs e)
            {
                foreach (var proxSocket in ClientSocketList)
                {
                    if (proxSocket.Connected)
                    {
                        proxSocket.Send(new byte[] { 2 }, SocketFlags.None);
                    }
                }
            }
            #endregion

            private void ServerForm_Load(object sender, EventArgs e)
            {
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
    }
class objClass
{
    public Socket objSocket;
    public byte[] objData;
}
