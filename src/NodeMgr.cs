using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;



namespace modbusPlcSimulator
{
    class NodeMgr
    {
        public static List<Node> _nodeList = new List<Node>();
        private static bool _initFlag = false;//初始化标识




        static public List<Node> getNodeList()
        { 
            return _nodeList; 
        }
        static public bool init(string cfgFile)
        {
            if(_initFlag)
            {
                stopAll();
                _nodeList.Clear();
            }
            //读csv文件，初始化每个node
            DataTable dt;
            bool ret = CSVReader.readCSV(cfgFile, out dt);
            if (!ret)
                return false;
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++) //写入各行数据
                {

                    int id = int.Parse(dt.Rows[i][0].ToString());
                    int port = int.Parse(dt.Rows[i][1].ToString());
                    string typeStr = dt.Rows[i][2].ToString();
                    
                    Node node = new Node(id, port,typeStr);
                    _nodeList.Add(node);
                }
                _initFlag = true;//已初始化
              return true; 
            }
            catch(Exception)
            {
                return false;
            }

        }

         static public bool openCfgFile(string cfgFile)//打开一个配置文件
        {
             if(String.IsNullOrEmpty(cfgFile))
                 return false;

            
             
                 init(cfgFile);
       

            return true;
        }

        static public void stopAll()
        {
            foreach (Node node in _nodeList)
            {
                node.stop();
            }
        }

        static public void startAll()
        {
            foreach (Node node in _nodeList)
            {
                node.start();
            }
        }

        static public void startNode(int id)
        {
            foreach (Node node in _nodeList)
            {
                if(node._id == id)
                {
                    node.start();
                }
            }
        }

        static public void stopNode(int id)
        {
            foreach (Node node in _nodeList)
            {
                if (node._id == id)
                {
                    node.stop();
                }
            }
        }


 
    }
}
