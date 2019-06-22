using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace modbusPlcSimulator
{
    class CSVReader
    {
        static public bool readCSV(string filePath, out DataTable dt)//从csv读取数据返回table
        {
           dt = new DataTable(); 

            try
            {
                System.Text.Encoding encoding = Encoding.Default;//GetType(filePath); //
                // DataTable dt = new DataTable();

                System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open,
                    System.IO.FileAccess.Read);

                System.IO.StreamReader sr = new System.IO.StreamReader(fs, encoding);

                //记录每次读取的一行记录
                string strLine = "";
                //记录每行记录中的各字段内容
                string[] aryLine = null;
                string[] tableHead = null;
                //标示列数
                int columnCount = 0;
                //标示是否是读取的第一行
                bool IsFirst = true;
                //逐行读取CSV中的数据
                while ((strLine = sr.ReadLine()) != null)
                {
                    if (IsFirst == true)
                    {
                        tableHead = strLine.Split(',');
                        IsFirst = false;
                        columnCount = tableHead.Length;
                        //创建列
                        for (int i = 0; i < columnCount; i++)
                        {
                            DataColumn dc = new DataColumn(tableHead[i]);
                            dt.Columns.Add(dc);
                        }
                    }
                    else
                    {
                        aryLine = strLine.Split(',');
                        DataRow dr = dt.NewRow();
                        for (int j = 0; j < columnCount; j++)
                        {
                            dr[j] = aryLine[j];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (aryLine != null && aryLine.Length > 0)
                {
                    dt.DefaultView.Sort = tableHead[0] + " " + "asc";
                }

                sr.Close();
                fs.Close();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show( filePath+" 解析失败!\n"+ e.Message, "CSV读取出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
