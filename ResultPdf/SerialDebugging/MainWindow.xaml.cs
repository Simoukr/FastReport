using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SerialDebugging
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private SerialPort sp = null;   //声明串口类
        bool isOpen = false;    //打开串口标志
        bool isSetProperty = false; //属性设置标志
        private string  itemsOn = null;
        private string itemsOff = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private bool CheckPortSetting()     //串口是否设置
        {
            if (cbxCOMPort.Text.Trim() == "") return false;
            if (cbxBaudRate.Text.Trim() == "") return false;

            return true;
        }
        /// <summary>
        /// 开启串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            if (isOpen == false)
            {
                itemsOn = null;
                itemsOff = null;
                if (!CheckPortSetting())        //检测串口设置
                {
                    MessageBox.Show("串口未设置！", "错误提示");
                    return;
                }
                if (!isSetProperty)             //串口未设置则设置串口
                {
                    SetPortProPerty();
                    isSetProperty = true;
                }
                try
                {
                    sp.Open();
                    isOpen = true;
                    OpenBtn.Content = "关闭串口";
                    //串口打开后则相关串口设置按钮便不可再用
                    cbxCOMPort.IsEnabled = false;
                    cbxBaudRate.IsEnabled = false;
                }
                catch (Exception)
                {
                    //打开串口失败后，相应标志位取消
                    isSetProperty = false;
                    isOpen = false;
                    MessageBox.Show("串口无效或已被占用！", "错误提示");
                }
            }
            else
            {
                try       //关闭串口       
                {
                    sp.Close();
                    isOpen = false;
                    OpenBtn.Content = "打开串口";
                    //关闭串口后，串口设置选项可以继续使用
                    cbxCOMPort.IsEnabled = true;
                    cbxBaudRate.IsEnabled = true;
                }
                catch (Exception)
                {
                    MessageBox.Show("关闭串口时发生错误！", "错误提示");
                }
            }

           
        }
        /// <summary>
        /// 串口设置
        /// </summary>
        private void SetPortProPerty()
        {
            sp = new SerialPort();
            sp.PortName = cbxCOMPort.Text.Trim();       //串口名
            sp.DataBits = 8;//数据位
            sp.BaudRate = Convert.ToInt32(cbxBaudRate.Text.Trim());//波特率
            sp.StopBits = StopBits.One;//停止位
            sp.Parity = Parity.None;//校验位
            sp.ReadTimeout = -1;         //设置超时读取时间
            sp.RtsEnable = true;
        }
        /// <summary>
        /// 加载串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void detectionBtn_Click(object sender, RoutedEventArgs e)
        {
            bool comExistence = false;  //是否有可用的串口
            cbxCOMPort.Items.Clear();   //清除当前串口号中的所有串口名称
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    SerialPort sp = new SerialPort("COM" + (i + 1).ToString());
                    sp.Open();
                    sp.Close();
                    cbxCOMPort.Items.Add("COM" + (i + 1).ToString());
                    comExistence = true;
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            if (comExistence)
            {
                cbxCOMPort.SelectedIndex = 0;//使ListBox显示第一个添加的索引
            }
            else
            {
                MessageBox.Show("没有找到可用串口！", "错误提示");
            }
        }
        
        /// <summary>
        /// 键盘屏蔽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            bool shiftKey = (Keyboard.Modifiers & ModifierKeys.Shift) != 0;
            if (shiftKey == true)
            {
                if (e.Key != Key.OemPipe)
                {
                    e.Handled = true;
                }
            }
            else if (e.Key != Key.Delete && e.Key != Key.Back &&
                     e.Key != Key.D0 && e.Key != Key.D1 &&
                     e.Key != Key.D2 && e.Key != Key.D3 &&
                     e.Key != Key.D4 && e.Key != Key.D5 &&
                     e.Key != Key.D6 && e.Key != Key.D7 &&
                     e.Key != Key.D8 && e.Key != Key.D9 &&
                     e.Key != Key.NumPad0 && e.Key != Key.NumPad1 &&
                     e.Key != Key.NumPad2 && e.Key != Key.NumPad3 &&
                     e.Key != Key.NumPad4 && e.Key != Key.NumPad5 &&
                     e.Key != Key.NumPad6 && e.Key != Key.NumPad7 &&
                     e.Key != Key.NumPad8 && e.Key != Key.NumPad9)
            {
                e.Handled = true;
            }
        }


        /// <summary>
        /// 高位输入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox tb = sender as TextBox;
            wrapOn.Init(int.Parse(e.Text));
            wrapOff.Init(int.Parse(e.Text));
        }
        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            if (isOpen)
            {
                try
                {
                   
                    foreach (var item in wrapOn.wrap.Children)
                    {
                        TextBox txt = item as TextBox;
                        if (string.IsNullOrEmpty(txt.Text))
                        {
                            MessageBox.Show("请输入要发送的数据", "错误提示");
                            return;
                        }
                        itemsOn += txt.Text + " ";
                    }
                    foreach (var item in wrapOff.wrap.Children)
                    {
                        TextBox txt = item as TextBox;
                        if (string.IsNullOrEmpty(txt.Text))
                        {
                            MessageBox.Show("请输入要发送的数据", "错误提示");
                            return;
                        }
                        itemsOff += txt.Text + " ";
                    }
                    if (!string.IsNullOrEmpty(itemsOn))
                    {
                        string export = ExportText(itemsOn,true);
                        sp.Write(HexStringToByteArray(export), 0, HexStringToByteArray(export).Length);
                    }
                    if (!string.IsNullOrEmpty(itemsOff))
                    {
                        string export = ExportText(itemsOff);
                        sp.Write(HexStringToByteArray(export), 0, HexStringToByteArray(export).Length);
                    }
                  
                }
                catch (Exception ex)
                {
                    MessageBox.Show("发送数据时发生错误！", "错误提示");
                    return;
                }
            }
            else
            {
                MessageBox.Show("串口未打开", "错误提示");
                return;
            }
            if (!CheckSendData())
            {
                MessageBox.Show("请输入要发送的数据!", "错误提示");
                return;
            }
        }


        /// <summary>
        /// 检查是否输入值
        /// </summary>
        /// <returns></returns>
        private bool CheckSendData()
        {
            if (string.IsNullOrEmpty(itemsOn)) return false;
            return true;
        }
        /// <summary>
        /// 数组十六进制转换
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }
        /// <summary>
        /// 输出 控制码
        /// </summary>
        /// <param name="text"></param>
        /// <param name="IsOn"></param>
        /// <returns></returns>
        private string ExportText(string text,bool IsOn=false)
        {
            string offOrOn = "";
            if (!IsOn == true)
            {
                offOrOn = "0" + HigtTxt.Text;
            }
            else
            {
                offOrOn = "1" + HigtTxt.Text;
            }
            string[] Num = text.Split(' ');
            if (Num.Length < 16)
            {
                for (int i = 0; i < 16 - Num.Length; i++)
                {
                    text += " 00";
                }
            }
            string output = string.Format(@"12 34 {0} {1} AA", offOrOn, text);
            return output;
        }
    }
}
