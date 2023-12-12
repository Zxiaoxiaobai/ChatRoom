using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientForm

{
    public class MyName{
        public static String name;
        public static int n = 0;
    
    }
    public class YourName { 
        public static String name;
    }
    public partial class ClientForm : Form
    {
        public Socket ClientSocekt { get; set; }
        public ClientForm()
        {
            InitializeComponent();
            Form1_Load();
            ConnectInit();
        }

        public void ConnectInit()
        {
            //客户端链接服务器端
            //1. 创建Socekt对象
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ClientSocekt = socket;  
            //2. 链接服务器端
            try
            {
                //填写要连接服务器的公网ip和端口号
                //IPAddress iPAddress = IPAddress.Parse("8.137.97.47");
                //IPAddress iPAddress = IPAddress.Parse("192.168.137.1");
                //IPEndPoint point = new IPEndPoint(iPAddress, Convert.ToInt32(50002));
                //socket.Connect(iPAddress, 53817);
                //socket.Connect(point);
                IPAddress iPAddress = IPAddress.Parse("8.137.97.47");
                socket.Connect(iPAddress, 50000);
            }
            catch (Exception ex)
            {
                MessageBox.Show("连接失败，请重新连接");
                return;
            }
            ShowMsg("连接服务器成功！");
            ShowMsg("请输入昵称，点击发送键确认。(回车键快捷发送消息)");

            //3. 发送消息   接收消息
            Thread th = new Thread(ReceiveData);
            th.IsBackground = true;
            th.Start(ClientSocekt);
        }


        public void ShowMsg(string str)
        {
            txtLog.AppendText(str);
            txtLog.AppendText("\r\n");
        }
        /*private void btnChose_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.InitialDirectory = @"D\";
            open.Title = "选择要发送的文件";
            open.Filter = "所有文件|*.*";
            open.ShowDialog();
            txtFilePath.Text = open.FileName;
        }

        private void btnSendFile_Click(object sender, EventArgs e)
        {
            try
            {
                using (FileStream fs = new FileStream(txtFilePath.Text.Trim(), FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[1024 * 1024 * 5];
                    int r = fs.Read(buffer, 0, buffer.Length);
                    List<byte> list = new List<byte>();
                    list.Add(3);
                    list.AddRange(buffer);
                    byte[] newBuffer = list.ToArray();
                    ClientSocket.Send(newBuffer, 0, r + 1, SocketFlags.None);
                }
            }
            catch { }
        }
    }*/

    public void ReceiveData(object o)
        {
            Socket proxSocket = o as Socket;
            while (true)
            {
                byte[] data = new byte[1024 * 1024];
                //客户端连接成功后，服务器应该接收客户端发来的消息

                //获取收到数据的字节数
                int len = 0;
                try
                {
                    len = proxSocket.Receive(data, 0, data.Length, SocketFlags.None);
                }
                catch (Exception ex)
                {
                    //异常退出
                    try
                    {
                        ShowMsg(string.Format("服务器端非正常退出！"));
                    }
                    catch (Exception ex1)
                    {

                    }
                    StopConnect();
                    return;
                }

                //服务端正常退出
                if (len <= 0)
                {
                    try
                    {
                        ShowMsg(string.Format("服务器端正常退出！"));
                    }
                    catch (Exception ex2)
                    {

                    }
                    //关闭链接
                    StopConnect();
                    return;
                }

                //接收到的数据中的第一个字节 1：字符串，2：闪屏，3：文件
                #region 接收到的是字符串
                if (data[0] == 1)
                {
                    string strMsg = ProcessRecieveString(data);
                    ShowMsg(strMsg);

                }

                #endregion

                #region 接收到的是闪屏
                else if (data[0] == 2)
                {
                    shock();
                }
                #endregion
                
            }
        }


        private void StopConnect()
        {
            try
            {
                if (ClientSocekt.Connected)
                {
                    ClientSocekt.Shutdown(SocketShutdown.Both);
                    //超过100s未关闭成功则强行关闭
                    ClientSocekt.Close(100);
                }
            }
            catch (Exception ex)
            {

            }

        }

        #region 处理接收到的字符串ProcessRecieveString(byte[] data)
        public string ProcessRecieveString(byte[] data)
        {
            //把实际的字符串拿到
            string str = Encoding.Default.GetString(data, 1, data.Length - 1);
            return str;
        }
        #endregion

        #region 闪屏方法shock()
        private void shock()
        {
            //把窗体最原始的坐标记住
            Point oldLocation = this.Location;
            Random r = new Random();
            for (int i = 0; i < 50; i++)
            {
                this.Location = new Point(r.Next(oldLocation.X - 5, oldLocation.X), r.Next(oldLocation.Y, oldLocation.Y));
                Thread.Sleep(50);
                this.Location = oldLocation;

            }
        }
        #endregion

        private void btnSendMsg_Click(object sender, EventArgs e)
        {
            if (txtMsg.Text.Equals("")) { ShowMsg("请输入内容");return; }
            if(MyName.n == 0&& ClientSocekt.Connected)
            {
                MyName.name = txtMsg.Text;
                MyName.n++;
                ShowMsg("你的昵称为："+MyName.name+" 开始发送消息吧！");
                txtMsg.Clear();
                txtMsg.Focus();
            }
            else if(ClientSocekt.Connected)
            {
                ShowMsg(MyName.name+"："+txtMsg.Text);

                //原始字符串转成字节数组
                byte[] data = Encoding.Default.GetBytes(txtMsg.Text);

                //对原始的数据数组加上协议的头部字节
                byte[] result = new byte[data.Length + 1];

                //设置当前的协议头部字节是1：代表字符串
                result[0] = 1;

                //把原始的数据放到最终的字节数组里去
                Buffer.BlockCopy(data, 0, result, 1, data.Length);

                ClientSocekt.Send(result, 0, result.Length, SocketFlags.None);
                txtMsg.Clear();
                txtMsg.Focus();
            }
            else
            {
                ShowMsg("发送失败，未连接服务器！");
            }
        }

        private void btnShock_Click(object sender, EventArgs e)
        {
            ClientSocekt.Send(new byte[] { 2 }, SocketFlags.None);
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void ClientForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            StopConnect();
            System.Environment.Exit(0);
        }

    }
}
