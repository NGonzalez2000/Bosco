using System;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using Bosco.XAML.Converters;

namespace Bosco.XAML.Controls.TextBoxs
{
    class CUITTextBox : TextBox
    {
        private string bindingPath;
        public string BindingPath
        {
            private get => bindingPath;
            set
            {
                if (value != null)
                {
                    Binding binding = new(value)
                    {
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                        ValidatesOnDataErrors = true,
                        ValidatesOnExceptions = true,
                        Converter = new CUITConverter()
                    };
                    SetBinding(TextProperty, binding);
                };
            }
        }
        public CUITTextBox()
        {
            Text = "00-00000000-0";
            bindingPath = string.Empty;
            TextChanged += CUITTextBox_TextChanged;
            SelectionChanged += CUITTextBox_SelectionChanged;
            PreviewTextInput += CUITTextBox_PreviewTextInput;
        }
        private void CUITTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            SelectionChanged -= CUITTextBox_SelectionChanged;
            CaretIndex = int.MaxValue;
            SelectionChanged += CUITTextBox_SelectionChanged;
        }
        private void CUITTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text[0]))
            {
                e.Handled = true;
            }
        }
        private void CUITTextBox_TextChanged(object sender, EventArgs e)
        {
            string text = Text.Replace("-", "").TrimStart('0');
            if (text.Length == 12) text = text.Remove(11);
            text = StrReverse(text);
            while (text.Length < 11) text += "0";
            text = StrReverse(text);
            text = $"{text[..2]}-{text[2..10]}-{text[^1..]}";
            TextChanged -= CUITTextBox_TextChanged;
            Text = text;
            TextChanged += CUITTextBox_TextChanged;

        }

        private static string StrReverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
