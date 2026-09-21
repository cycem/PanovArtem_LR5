using System;

namespace calc.src.main.services
{
    public class CalculatorService
    {
        public double Calculate(string expr)
        {
            string[] numbers = expr.Split(new char[] { '+', '-', '×', '÷', '^' });
            string operators = "";
            foreach (char c in expr)
            {
                if (c == '+' || c == '-' || c == '×' || c == '÷' || c == '^')
                    operators += c;
            }

            if (numbers.Length == 0) return 0;

            double total = double.Parse(numbers[0].Replace(',', '.'));

            for (int i = 0; i < operators.Length; i++)
            {
                double nextNum = double.Parse(numbers[i + 1].Replace(',', '.'));
                char op = operators[i];

                if (op == '+') total += nextNum;
                else if (op == '-') total -= nextNum;
                else if (op == '×') total *= nextNum;
                else if (op == '÷')
                {
                    if (nextNum != 0) total /= nextNum;
                    else throw new Exception("Делить на ноль нельзя!");
                }
                else if (op == '^') total = Math.Pow(total, nextNum);
            }
            return total;
        }

        public double CalculateEngineering(string op, double val)
        {
            switch (op)
            {
                case "x²": return val * val;
                case "√": return Math.Sqrt(val);
                case "sin": return Math.Sin(val * Math.PI / 180);
                case "cos": return Math.Cos(val * Math.PI / 180);
                case "tg": return Math.Tan(val * Math.PI / 180);
                case "обр.": return val != 0 ? 1 / val : 0;
                default: return 0;
            }
        }

        public string CalculatePercent(string text)
        {
            int lastOpIndex = text.LastIndexOfAny(new char[] { '+', '-', '×', '÷' });
            if (lastOpIndex == -1)
            {
                double val = double.Parse(text.Replace(',', '.'));
                return (val / 100).ToString().Replace('.', ',');
            }
            else
            {
                string partA = text.Substring(0, lastOpIndex);
                string partB = text.Substring(lastOpIndex + 1);
                char op = text[lastOpIndex];

                if (!string.IsNullOrEmpty(partB))
                {
                    double numA = Calculate(partA);
                    double numB = double.Parse(partB.Replace(',', '.'));
                    double percentVal = (numA * numB) / 100;
                    return partA + op + percentVal.ToString().Replace('.', ',');
                }
            }
            return text;
        }
    }
}