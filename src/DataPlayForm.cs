using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace modbusPlcSimulator
{
    public partial class DataPlayForm : Form
    {

       // DataPlay _playThread = null;

        List<DataPlay> _threadList = new List<DataPlay>();
        bool _isPlaying = false;

        public DataPlayForm()
        {
            InitializeComponent();
            //startPlay(null,null);
        }

        private void startPlay(object sender, EventArgs e)
        {
            if (_isPlaying)
                return;

            for (int i = 0; i < NodeMgr._nodeList.Count;i++ )
            {
                DataPlay playThread = new DataPlay(this, i);
                playThread._playMode = getPlayMode();
                _threadList.Add(playThread);
                playThread.toStart();
            }
            _isPlaying = true;

        }

        private void stopPlay(object sender, EventArgs e)
        {
            if (_isPlaying == false)
                return;

            for (int i = 0; i < _threadList.Count; i++)
            {
                DataPlay playThread = _threadList[i];
                if (playThread != null)
                {
                    playThread.toStop();
                }
            }
            _threadList.Clear();
            _isPlaying = false;

            Thread.Sleep(200);//暂停一下让写线程结束
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < _threadList.Count; i++)
            {
                DataPlay playThread = _threadList[i];
                playThread._playMode = getPlayMode();
            }
        }
        private int getPlayMode()
        {
            return radioButton1.Checked ? 0 : 1;// 0按序，1随机
        }

        private void formClosing(object sender, FormClosingEventArgs e)
        {
            stopPlay(sender, e);
        }

        delegate void addLogCallBack(string text);
        private void AddLogText(string text)
        {
            if (this.richTextBox1.InvokeRequired)
            {
                addLogCallBack stcb = new addLogCallBack(AddLogText);
                this.Invoke(stcb, new object[] { text });
            }
            else
            {
                lock (richTextBox1)
                {
                    if (richTextBox1.Lines.Length > 300)
                    {
                        richTextBox1.Clear();
                    }

                    this.richTextBox1.AppendText(text);
                    //设置光标的位置到文本尾   
                    richTextBox1.Select(richTextBox1.TextLength, 0);
                    //滚动到控件光标处   
                    richTextBox1.ScrollToCaret();
                }
            }
        }
        public void log(string logStr)//对外调用的接口
        {
            AddLogText(logStr);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            stopPlay(sender, e);
        }


    }
}
