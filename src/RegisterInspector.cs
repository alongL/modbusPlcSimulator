using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Modbus.Data;



namespace modbusPlcSimulator
{
    public partial class RegisterInspector : Form
    {

        public string _selectIdStr { get; set; }
        private Node _node;

        public RegisterInspector(string str)
        {
            _selectIdStr = str;
            InitializeComponent();
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            
            initUI();
            timer1.Start();
        }


        private void initUI()
        {
            this.listView1.Clear();
            comboBox1.Items.Clear();
            for(int i = 0;i<NodeMgr._nodeList.Count;i++)
            {
                Node node = NodeMgr._nodeList[i];
                string name = node._name;
                comboBox1.Items.Add(name);
                
                if (node._id.ToString() == _selectIdStr)
                {
                    _node = node;
                    comboBox1.SelectedIndex = i;
                }
            }
            comboBox_Register.SelectedIndex = 0;
            refreshUI();
        }//initUI

        private void refreshUI()
        {
            //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度 
            ModbusDataCollection<ushort> data = getRegisters();
            
            int startAdress = 0;
            int length = 1000;
            if( !int.TryParse(textBox_adress.Text, out startAdress))
            {
                startAdress = 0;
            }
            if (!int.TryParse(textBox_length.Text, out length))
            {
                length = 1000;
            }
           listView1.BeginUpdate();

           listView1.Clear();
           for (int index = 0; index < length ; index++)
            {
                int address = index + startAdress;
                string line = "<" + address.ToString().PadLeft(4, '0') + ">: ";
                ushort value = data[address];
                line += value.ToString();

               ListViewItem item = new ListViewItem();
               item.Text=line;
               listView1.Items.Add(item);
            }
            this.listView1.EndUpdate();  //结束数据处理，UI界面一次性绘制。 
        }

        private ModbusDataCollection<ushort> getRegisters()
        {
            ModbusDataCollection<ushort> data = null;
            switch(comboBox_Register.SelectedIndex)
            {
                case 0: data = _node.getDataStore().HoldingRegisters; break;//03功能
                case 1: data = _node.getDataStore().InputRegisters; break;//04功能
                default: data = _node.getDataStore().HoldingRegisters; break;
            }
           return data;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            refreshUI();
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)//设备变更
        {
            int index = comboBox1.SelectedIndex;
            _node = NodeMgr._nodeList[index];
            refreshUI();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;
            MessageBox.Show(listView1.FocusedItem.Text);
        }


        private void comboBox_Register_SelectionChangeCommitted(object sender, EventArgs e)
        {
            refreshUI();
        }



    }
}
