using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace modbusPlcSimulator
{
    public partial class Form1 : Form
    {
        private DataTable _dt=new DataTable();
        public Form1()
        {
            InitializeComponent();
            readConfigFile();

            initUI();
            timer1.Start();
        }
        private void initUI()
        {
            dataGridView1.Rows.Clear();
            List<Node> nodeList = NodeMgr.getNodeList();
            foreach (Node node in nodeList)
            {
                int index = this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[index].Cells[0].Value = node._id;
                this.dataGridView1.Rows[index].Cells[1].Value = node._name;
                this.dataGridView1.Rows[index].Cells[2].Value = node._port;

                this.dataGridView1.Rows[index].Cells[4].Value = node._status;
            }
        }
        private void updateUI()
        {
            try
            {
                List<Node> nodeList = NodeMgr.getNodeList();
                for(int index=0;index<nodeList.Count;index++)
                {
                    this.dataGridView1.Rows[index].Cells[0].Value = nodeList[index]._id;
                    this.dataGridView1.Rows[index].Cells[1].Value = nodeList[index]._name;
                    this.dataGridView1.Rows[index].Cells[2].Value = nodeList[index]._port;
                    this.dataGridView1.Rows[index].Cells[3].Value = nodeList[index]._typeStr;
                    this.dataGridView1.Rows[index].Cells[4].Value = nodeList[index]._status;

                    if(nodeList[index]._isRunning == true)
                    {
                         this.dataGridView1.Rows[index].Cells[4].Style.BackColor = Color.DeepSkyBlue;
                    }
                    else { this.dataGridView1.Rows[index].Cells[4].Style.BackColor = Color.White; }
                }
                }catch
                {            }

        }

        private void readConfigFile()
        {
            if(MAppConfig.InitFromFile())
            {
                string defaultCfgFile = MAppConfig.getValueByName("defaultCfgFile");
                string configDirStr = MAppConfig.getValueByName("defaultCfgDir");
                string fileName = configDirStr + "/" + defaultCfgFile;


                if (!String.IsNullOrEmpty(fileName))
                {
                    if (!NodeMgr.init(fileName))//
                    {
                        MessageBox.Show(fileName+"风场模型解析失败!检查配置文件","出错了",MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
           
        }
        private void button_Start(object sender, EventArgs e)
        {
            NodeMgr.startAll();
        }
        private void button_Stop(object sender, EventArgs e)
        {
            NodeMgr.stopAll();
        }


 

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("是否要退出程序？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if(result == DialogResult.Cancel)
            {
                e.Cancel = true;
                return;
            }

            NodeMgr.stopAll();
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
          
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = System.Environment.CurrentDirectory.ToString();
            ofd.Filter = "CSV文件(*.csv)|*.csv;|所有文件|*.*";
            ofd.ValidateNames = true;
            ofd.CheckPathExists = true;
            ofd.CheckFileExists = true;
            string strFileName="";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                strFileName = ofd.FileName;
            }
              timer1.Stop();
            if(NodeMgr.openCfgFile(strFileName))
            {
                initUI();
            }
            timer1.Start();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            updateUI();
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program._logForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)//show Register Inspector Window
        {
            if (dataGridView1.SelectedRows.Count <= 0)
                return;
            string selectIdStr = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            RegisterInspector regWindow = new RegisterInspector(selectIdStr);
            regWindow.Show();
        }

        private void ToolStripMenuItem_Click_about(object sender, EventArgs e)
        {
          
        }

        private void button1_Click(object sender, EventArgs e)  //stop one 
        {
            try { 
                if (dataGridView1.SelectedRows.Count <= 0) 
                    return;

                foreach (DataGridViewRow Row in dataGridView1.SelectedRows)
                {
                    string selectIdStr = Row.Cells[0].Value.ToString();//查找第0列
                    int id = int.Parse(selectIdStr);
                    NodeMgr.stopNode(id);
                }
           }catch(Exception ex)
            {

            }
        }

        private void dataPlay_Click(object sender, EventArgs e)
        {
            button_play_Click(sender, e);
        }

        private void button_play_Click(object sender, EventArgs e)
        {
            DataPlayForm playWindow = new DataPlayForm();
            playWindow.Show();
        }



    }
}
