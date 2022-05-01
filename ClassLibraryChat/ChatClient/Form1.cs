using ChatClient.ServiceChat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class Form1 : Form, IServiceChatCallback
    {
        bool isConnected = false;

        ServiceChatClient client;
        int ID;

        public Form1()
        {
            InitializeComponent();
        }

        //btn connect
        void ConnectUser()
        {
            if (!isConnected)
            {
                client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this));
                ID = client.Connect(tbUserName.Text);
                tbUserName.Enabled = false;
                bConnect.Text = "Выйти";
                isConnected = true;
            }
        }

        void DisconnectUser()
        {
            if (isConnected)
            {
                client.Disconnect(ID);
                client = null;
                tbUserName.Enabled = true;
                bConnect.Text = "Войти";
                isConnected = false;
            }

        }

        private void bConnect_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                DisconnectUser();
            }
            else
            {
                ConnectUser();
            }
        }

        public void MsgCallback(string msg)
        {
            lbChat.Items.Add(msg);

            //Прокрутка вниз
            int visibleItems = lbChat.ClientSize.Height / lbChat.ItemHeight;
            lbChat.TopIndex = Math.Max(lbChat.Items.Count - visibleItems + 1, 0);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisconnectUser();
        }

        private void tbMsg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (client != null)
                {
                    client.SendMsg(tbMsg.Text, ID);
                    e.SuppressKeyPress = true;
                    tbMsg.Text = "";
                }
            }
        }
    }
}
