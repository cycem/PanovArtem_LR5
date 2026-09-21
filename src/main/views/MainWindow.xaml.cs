using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using calc.src.main.services;

namespace calc.src.main.views
{
    public partial class MainWindow : Window
    {
        private bool _isDone = false;
        private readonly CalculatorService _calcService;
        private readonly HistoryService _historyService;

        public MainWindow()
        {
            InitializeComponent();
            _calcService = new CalculatorService();
            _historyService = new HistoryService();
        }



        private void BtnDigit_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string val = btn.Content.ToString();

            if (_isDone || TxtDisplay.Text == "0")
            {
                TxtDisplay.Text = val == "," ? "0," : val;
                _isDone = false;
            }
            else
            {
                string text = TxtDisplay.Text;
                if (text[text.Length - 1] != ',' && val != ",")
                {
                    TxtDisplay.Text += val;
                }
            }
        }

        private void BtnOperation_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string op = btn.Content.ToString();
            string text = TxtDisplay.Text;

            if (text.Length > 0)
            {
                char last = text[text.Length - 1];
                if (last == '+' || last == '-' || last == '×' || last == '÷' || last == '^')
                {
                    TxtDisplay.Text = text.Remove(text.Length - 1) + op;
                }
                else
                {
                    TxtDisplay.Text += op;
                }
                _isDone = false;
            }
        }

        private void BtnEqual_Click(object sender, RoutedEventArgs e)
        {
            string expression = TxtDisplay.Text.Replace(',', '.');
            char last = expression[expression.Length - 1];
            if (last == '+' || last == '-' || last == '×' || last == '÷' || last == '^')
            {
                expression = expression.Remove(expression.Length - 1);
            }
            try
            {
                double result = _calcService.Calculate(expression);
                string sRes = result.ToString("G10").Replace('.', ',');

                _historyService.SaveRecord(expression, sRes);

                TxtDisplay.Text = sRes;
                _isDone = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                TxtDisplay.Text = "0";
                _isDone = true;
            }
        }

        private void BtnPercent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TxtDisplay.Text = _calcService.CalculatePercent(TxtDisplay.Text);
            }
            catch { }
        }

        private void BtnEng_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] parts = TxtDisplay.Text.Split(new char[] { '+', '-', '×', '÷', '^' });
                string lastNumberStr = parts[parts.Length - 1];
                double val = double.Parse(lastNumberStr.Replace(',', '.'));

                string op = ((Button)sender).Content.ToString();
                double res = _calcService.CalculateEngineering(op, val);

                string sRes = res.ToString("G10").Replace('.', ',');
                int index = TxtDisplay.Text.LastIndexOf(lastNumberStr);
                TxtDisplay.Text = TxtDisplay.Text.Remove(index) + sRes;
                _isDone = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                TxtDisplay.Text = "0";
                _isDone = true;
            }
        }

        private void BtnModeNormal_Click(object sender, RoutedEventArgs e)
        {
            Width = 450;
            ColEng.Width = new GridLength(0);
            UpdateTheme(Brushes.DeepSkyBlue, Brushes.White);
        }

        private void BtnModeEng_Click(object sender, RoutedEventArgs e)
        {
            Width = 650;
            ColEng.Width = new GridLength(200);
            UpdateTheme(Brushes.LimeGreen, new SolidColorBrush(Color.FromRgb(30, 30, 30)));
        }

        private void UpdateTheme(Brush accent, Brush bg)
        {
            Application.Current.Resources["AccentColor"] = accent;
            Application.Current.Resources["WindowBg"] = bg;
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            TxtDisplay.Text = "0";
            _isDone = true;
        }

        private void BtnBackspace_Click(object sender, RoutedEventArgs e)
        {
            TxtDisplay.Text = TxtDisplay.Text.Length > 1
                ? TxtDisplay.Text.Remove(TxtDisplay.Text.Length - 1)
                : "0";
        }

        private void BtnHistory_Click(object sender, RoutedEventArgs e)
        {
            string history = _historyService.GetHistory();
            MessageBox.Show(string.IsNullOrEmpty(history) ? "История пуста" : history, "История");
        }

        private void BtnClearHistory_Click(object sender, RoutedEventArgs e)
        {
            _historyService.ClearHistory();
        }
    }
}