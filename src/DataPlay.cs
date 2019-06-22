using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;

namespace modbusPlcSimulator
{
    class DataPlay
    {
        DataTable _dt;

       public  int _playMode{get; set;}//播放模式 0按序播放，1随机播放
        bool _bStop = false;
        DataPlayForm _form;
        int _deviceIndex = 0;
        string _dataFileName;

        public DataPlay(DataPlayForm form,int index)
        {
            _form = form;//窗口指针
            _deviceIndex = index;
            _playMode = 0;
        }
        public bool  readData(string filePath)
        {
            return CSVReader.readCSV(filePath, out _dt);
        }

        public void toStart()
        {
            string configDirStr = MAppConfig.getValueByName("defaultCfgDir");
            _dataFileName = configDirStr+"/" + NodeMgr._nodeList[_deviceIndex]._typeStr + "_data.csv";
           
            if (!readData(_dataFileName))
            { 
                FormLog(_dataFileName+"读取失败");
                return;
            }

            Thread thread = new Thread(run) { Name = "DataPlay"+NodeMgr._nodeList[_deviceIndex]._name, IsBackground = true };
            thread.Start();
        }
        public void toStop()
        {
            _bStop = true;//
            FormLog("device:" + NodeMgr._nodeList[_deviceIndex]._name + " 停止更新数据\n");

        }
        void FormLog(string logStr)
        {
            _form.log(logStr);
        }
        static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
        void run()
        {
            int i = 0;
            Random rand = new Random(GetRandomSeed());
            while(!_bStop)
            {
                try
                {
                    int index = 0;
                    if(_playMode == 0)//按序模式
                    {
                        index = i % _dt.Rows.Count;
                        i++;
                    }
                    else //if (_playMode == 1)
                    {
                        index = rand.Next() % _dt.Rows.Count;//随机模式
                    }
                    FormLog(NodeMgr._nodeList[_deviceIndex]._name.PadLeft(10) + " 更新数据第" + index.ToString().PadLeft(4) +"行\n");

                    DataRow Row = _dt.Rows[index];
                    for(int col=1; col <_dt.Columns.Count; col++)//第0列作为时间暂不处理
                    {
                        string ioName = _dt.Columns[col].ToString();
                        string valueStr = Row[col].ToString();
                        NodeMgr._nodeList[_deviceIndex].setValueByName(ioName, valueStr);
                        
                    }
                }  catch (Exception )
                {
                    
                }finally
                {
                    Thread.Sleep(1000);	
                }
            }//while
        }




    }
}
