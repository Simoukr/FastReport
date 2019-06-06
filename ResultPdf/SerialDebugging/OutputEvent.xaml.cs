using System;
using System.Collections.Generic;
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
    /// OutputEvent.xaml 的交互逻辑
    /// </summary>
    public partial class OutputEvent : UserControl
    {
        public OutputEvent()
        {
            InitializeComponent();
        }
        public void Init(int num)
        {
            wrap.Children.Clear();
           
            for (int i = 0; i < num; i++)
            {
                TextBox txt = new TextBox();
                txt.Width = 30;
                txt.Height = 30;
                txt.Margin = new Thickness(5, 10, 5, 5);
                txt.Name = "txt_" + i.ToString();
                txt.MaxLength = 2;
                txt.TextChanged += Txt_TextChanged;
                txt.PreviewKeyDown += TextBox_PreviewKeyDown;
                wrap.Children.Add(txt);
            }
        }

        private void MoveNextTextBox(string tbName)
        {
            foreach (var item in wrap.Children)
            {
                TextBox txt = item as TextBox;
                if (SplitGet(tbName, true) == SplitGet(txt.Name, false))
                {
                    txt.Focus();
                }
            }
        }
        private int SplitGet(string a, bool KK = false)
        {
            string[] sArray = a.Split('_');
            if (!KK)
            {
                return int.Parse(sArray[1]);
            }
            else
            {
                return int.Parse(sArray[1]) + 1;
            }

        }
        private void Txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Text.Length == 0)
                return;
            if (tb.Text.Length == 2)
            {
                MoveNextTextBox(tb.Name);
            }
        }
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
                     e.Key != Key.A && e.Key != Key.B &&
                     e.Key != Key.C && e.Key != Key.D &&
                     e.Key != Key.E && e.Key != Key.F &&
                     e.Key != Key.NumPad0 && e.Key != Key.NumPad1 &&
                     e.Key != Key.NumPad2 && e.Key != Key.NumPad3 &&
                     e.Key != Key.NumPad4 && e.Key != Key.NumPad5 &&
                     e.Key != Key.NumPad6 && e.Key != Key.NumPad7 &&
                     e.Key != Key.NumPad8 && e.Key != Key.NumPad9)
            {
                e.Handled = true;
            }
        }
    }
}
