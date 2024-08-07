using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace modbusPlcSimulator
{
    static class Program
    {
        static public LogForm _logForm = null;
        static public RegisterInspector.NumericConvertType numericConvertType
            = RegisterInspector.NumericConvertType.LittleEndian;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {
            string turbine;
            if(args.Count<string>()>0) //用来传入参数
            {
                turbine = args[1];
            }
            if (!MAppConfig.InitFromFile())
            { 
            
            };
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            _logForm = new LogForm();
            _logForm.Show();//不show 会出问题
            _logForm.Visible = false;
            
            Application.Run(new Form1());
        }

        
    }
}
