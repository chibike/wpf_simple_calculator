using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Simple_Calculator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        string solution = "";
        string answer = "";
        string numericBuffer = "";

        int numberOfBracketsOpen = 0;
        Boolean numberHasDot = false;
        private ArithmeticModule stringEvaluator = new ArithmeticModule();

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void BtnSeven_Click(object sender, RoutedEventArgs e)
        {
            ButtonPressed("7");
        }

        private void ButtonPressed(string buttonContent)
        {
            buttonContent = buttonContent.Trim();

            if (stringEvaluator.isNumber(buttonContent))
            {
                numericBuffer += buttonContent;
            }
            else
            {
                if (buttonContent.Equals("CE"))
                {
                    answer = "";
                    solution = "";
                    numericBuffer = "";
                    numberHasDot = false;
                    numberOfBracketsOpen = 0;
                }
                else if (buttonContent.Equals("."))
                {
                    if ( numericBuffer.LastOrDefault<char>() != '.' && !numberHasDot)
                    {
                        if (numericBuffer.Length <= 0 || (numericBuffer.Length > 0 && !stringEvaluator.isNumber(numericBuffer.LastOrDefault<char>())))
                        {
                            numericBuffer += "0";
                        }

                        if (stringEvaluator.isNumber(numericBuffer.LastOrDefault<char>()))
                        {
                            numericBuffer += buttonContent;
                            numberHasDot = true;
                        }
                    }
                }
                else if (buttonContent.Equals("%"))
                {
                    if (stringEvaluator.isNumber(numericBuffer.LastOrDefault<char>()) || numericBuffer.LastOrDefault<char>() == ')')
                    {
                        numericBuffer += buttonContent;
                    }
                }
                else if (buttonContent.Equals("+"))
                {
                    if (numericBuffer.Length > 0)
                    {
                        if (numberOfBracketsOpen <= 0)
                        {
                            solution += numericBuffer;
                            solution += " + ";
                            numericBuffer = "";
                            numberHasDot = false;
                        }
                        else if (stringEvaluator.isNumber(numericBuffer.LastOrDefault<char>()) || numericBuffer.LastOrDefault<char>() == '%')
                        {
                            numericBuffer += " + ";
                            numberHasDot = false;
                        }
                    }
                }
                else if (buttonContent.Equals("—"))
                {
                    if (numericBuffer.Length > 0)
                    {
                        if (numberOfBracketsOpen <= 0)
                        {
                            solution += numericBuffer;
                            solution += " — ";
                            numericBuffer = "";
                            numberHasDot = false;
                        }
                        else if (stringEvaluator.isNumber(numericBuffer.LastOrDefault<char>()) || numericBuffer.LastOrDefault<char>() == '%')
                        {
                            numericBuffer += " — ";
                            numberHasDot = false;
                        }
                    }
                }
                else if (buttonContent.Equals("/"))
                {
                    if (numericBuffer.Length > 0)
                    {
                        if (numberOfBracketsOpen <= 0)
                        {
                            solution += numericBuffer;
                            solution += " / ";
                            numericBuffer = "";
                        }
                        else if (stringEvaluator.isNumber(numericBuffer.LastOrDefault<char>()) || numericBuffer.LastOrDefault<char>() == '%')
                        {
                            numericBuffer += " / ";
                        }

                    }
                }
                else if (buttonContent.Equals("*"))
                {
                    if (numericBuffer.Length > 0)
                    {
                        if (numberOfBracketsOpen <= 0)
                        {
                            solution += numericBuffer;
                            solution += " * ";
                            numericBuffer = "";
                            numberHasDot = false;
                        }
                        else if (stringEvaluator.isNumber(numericBuffer.LastOrDefault<char>()) || numericBuffer.LastOrDefault<char>() == '%')
                        {
                            numericBuffer += " * ";
                            numberHasDot = false;
                        }
                    }
                }
                else if (buttonContent.Equals("("))
                {
                    numberOfBracketsOpen += 1;
                    numericBuffer += buttonContent;
                    numberHasDot = false;
                }
                else if (buttonContent.Equals(")") && numberOfBracketsOpen > 0)
                {
                    numberOfBracketsOpen = Math.Max(0, numberOfBracketsOpen - 1);
                    numericBuffer += buttonContent;
                }
            }

            
            if (buttonContent.Equals("="))
            {
                try
                {
                    // evalute string and display solution
                    solution += numericBuffer;
                    numericBuffer = stringEvaluator.eval(solution);
                    textBoxNumericView.Text = renderString(numericBuffer);
                    textBoxSolutionView.Text = renderString(solution);
                    textBoxNumericView.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0x73, 0, 0, 0xC8));
                    textBoxNumericView.Text = renderString(numericBuffer);
                    textBoxSolutionView.Text = renderString(solution);

                    // wipe solution ready for next task
                    solution = "";
                }
                catch (Exception e)
                {
                    textBoxNumericView.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0x73, 0xC8, 0, 0));
                    textBoxNumericView.Text = renderString(numericBuffer);
                    textBoxSolutionView.Text = renderString(solution);
                }
            }
            else
            {
                textBoxNumericView.Text = renderString(numericBuffer);
                textBoxSolutionView.Text = renderString(solution);
            }
        }

        private string renderString(string sentence)
        {
            return sentence.Replace('—', '-');
        }

        private void carryOver()
        {

        }

        private void TextBoxNumericView_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ButtonPressed("CE");
        }

        private void BtnEight_Click(object sender, RoutedEventArgs e)
        {
            ButtonPressed("8");
        }

        private void BtnNine_Click(object sender, RoutedEventArgs e)
        {
            ButtonPressed("9");
        }

        private void BtnFour_Click(object sender, RoutedEventArgs e)
        {
            ButtonPressed("4");
        }

        private void BtnFive_Click(object sender, RoutedEventArgs e)
        {
            ButtonPressed("5");
        }

        private void BtnSix_Click(object sender, RoutedEventArgs e)
        {
            ButtonPressed("6");
        }

        private void BtnOne_Click(object sender, RoutedEventArgs e)
        {
            ButtonPressed("1");
        }

        private void BtnTwo_Click(object sender, RoutedEventArgs e)
        {
            ButtonPressed("2");
        }

        private void BtnThree_Click(object sender, RoutedEventArgs e)
        {
            ButtonPressed("3");
        }

        private void BtnZero_Click(object sender, RoutedEventArgs e)
        {
            ButtonPressed("0");
        }

        private void BtnDot_Click(object sender, RoutedEventArgs e)
        {
            ButtonPressed(".");
        }

        private void BtnPercentage_Click(object sender, RoutedEventArgs e)
        {
            ButtonPressed("%");
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ButtonPressed("+");
        }

        private void BtnSubtract_Click(object sender, RoutedEventArgs e)
        {
            ButtonPressed("—");
        }

        private void BtnMultiply_Click(object sender, RoutedEventArgs e)
        {
            ButtonPressed("*");
        }

        private void BtnDivide_Click(object sender, RoutedEventArgs e)
        {
            ButtonPressed("/");
        }

        private void BtnOpenBracket_Click(object sender, RoutedEventArgs e)
        {
            ButtonPressed("(");
        }

        private void BtnCloseBracket_Click(object sender, RoutedEventArgs e)
        {
            ButtonPressed(")");
        }

        private void BtnEqual_Click(object sender, RoutedEventArgs e)
        {
            ButtonPressed("=");
        }
    }
}
