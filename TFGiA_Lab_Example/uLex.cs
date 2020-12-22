using System;
using System.Windows.Forms;

namespace TFGiA_Lab_Example
{
    public enum CharType { Letter, Digit, EndRow, EndText, Space, ReservedSymbol }; // тип символа
    public enum Token { lxmIdentifier, lxmNumber, lxmUnknown, lxmEmpty, lxmLeftParenth, lxmRightParenth, lxmIs, lxmDot, lxmComma, lxmText, lxmtz, lxmdt, lxmr, lxmrs, lxmls, lxmStar, lxmPlus };

    public class uLex
    {
        public String[] text;
        public String item;
        public int numberOfStr;
        public int numberOfCol;
        public char currentSymbol;
        public bool isFirst = true;
        public CharType typeOfSymbol;
        public Token tokenOfLxm;

        public uLex()
        {
            numberOfStr = 0;
            numberOfCol = -1;
        }

        public void GetSymbol() //метод класса лексический анализатор
        {
            numberOfCol++; // продвигаем номер колонки
            if (numberOfCol > text[numberOfStr].Length - 1)
            {
                numberOfStr++;
                if (numberOfStr <= text.Length - 1)
                {
                    numberOfCol = -1;
                    currentSymbol = '\0';
                    typeOfSymbol = CharType.EndRow;
                }
                else
                {
                    currentSymbol = '\0';
                    typeOfSymbol = CharType.EndText;
                }
            }
            else
            {
                currentSymbol = text[numberOfStr][numberOfCol]; //классификация прочитанной литеры
                if (currentSymbol == ' ') typeOfSymbol = CharType.Space;
                else if (currentSymbol >= 'a' && currentSymbol <= 'd') typeOfSymbol = CharType.Letter;
                else if (
                    currentSymbol == '0' ||
                    currentSymbol == '1'
                ) typeOfSymbol = CharType.Digit;
                else if (
                    currentSymbol == ';' ||
                    currentSymbol == ',' ||
                    currentSymbol == '[' ||
                    currentSymbol == ']' ||
                    currentSymbol == '=' ||
                    currentSymbol == ':' ||
                    currentSymbol == '/' ||
                    currentSymbol == '*' ||
                    currentSymbol == '+' ||
                    currentSymbol == '(' ||
                    currentSymbol == ')'
                ) typeOfSymbol = CharType.ReservedSymbol;
                else throw new Exception("Cимвол вне алфавита");
            }
        }
        private void TakeSymbol()
        {
            item += currentSymbol;
            GetSymbol();
        }
        public void NextToken()
        {
            item = "";

            if (isFirst)
            {
                GetSymbol();
                isFirst = false;
            }

            while (typeOfSymbol == CharType.Space || typeOfSymbol == CharType.EndRow)
            {
                GetSymbol();
            }

            if (currentSymbol == '/')
            {
                GetSymbol();
                if (currentSymbol == '/')
                    while (typeOfSymbol != CharType.EndRow)
                    {
                        GetSymbol();
                    }
                GetSymbol();
            }

            switch (typeOfSymbol)
            {
                case CharType.Letter:
                    {
                    // В3
                    //          a       b     c      d
                    //  AFin | AFin | CFin | AFin | AFin |
                    //  CFin | AFin |      | AFin | AFin |
                    AFin:
                        {
                            if (currentSymbol == 'a' || currentSymbol == 'c' || currentSymbol == 'd')
                            {
                                TakeSymbol();
                                goto AFin;
                            }
                            else if (currentSymbol == 'b')
                            {
                                TakeSymbol();
                                goto CFin;
                            }
                            else if (typeOfSymbol != CharType.Letter) { tokenOfLxm = Token.lxmIdentifier; return; }
                        }

                        CFin:
                        {
                            if (currentSymbol == 'a' || currentSymbol == 'c' || currentSymbol == 'd')
                            {
                                TakeSymbol();
                                goto AFin;
                            }
                            else throw new Exception("Есть bb");
                        }

                    }
                case CharType.Digit:
                    {
                    //           0     1  
                    //    A   |  BC |     |
                    //    BC  |  D  |  E  |
                    //    D   |  A  |     |
                    //    E   | FFin|     |
                    //   FFin |  G  |     |
                    //    G   |     |  H  |
                    //    H   |     | FFin|

                    A:
                        if (currentSymbol == '0')
                        {
                            TakeSymbol();
                            goto BC;
                        }
                       
                        else throw new Exception("Ожидался 0");

                        BC:
                        if (currentSymbol == '0')
                        {
                            TakeSymbol();
                            goto D;
                        }
                        else if (currentSymbol == '1')
                        {
                            TakeSymbol();
                            goto E;
                        }
                        else throw new Exception("Ожидался 0 или 1");



                        D:
                        if (currentSymbol == '0')
                        {
                            TakeSymbol();
                            goto A;
                        }
                        else throw new Exception("Ожидалась 0");
                        E:
                        if (currentSymbol == '0')
                        {
                            TakeSymbol();
                            goto FFin;
                        }
                        else throw new Exception("Ожидалась 0");


                        FFin:
                        if (currentSymbol == '0')
                        {
                            TakeSymbol();
                            goto G;
                        }
                        else if (typeOfSymbol != CharType.Digit) { tokenOfLxm = Token.lxmNumber; return; }
                        else throw new Exception("Ожидалась 0");

                        G:
                        if (currentSymbol == '1')
                        {
                            TakeSymbol();
                            goto H;
                        }
                        else throw new Exception("Ожидалась 1");


                        H:
                        if (currentSymbol == '1')
                        {
                            TakeSymbol();
                            goto FFin;
                        }
                        else throw new Exception("Ожидался 1");

                    }
                case CharType.ReservedSymbol:
                    {
                        if (currentSymbol == '/')
                        {
                            GetSymbol();
                            if (currentSymbol == '/')
                            {
                                while (typeOfSymbol != CharType.EndRow)
                                    GetSymbol();
                            }
                            GetSymbol();
                        }
                        if (currentSymbol == '(')
                        {
                            tokenOfLxm = Token.lxmLeftParenth;
                            GetSymbol();
                            return;
                        }
                        if (currentSymbol == ')')
                        {
                            tokenOfLxm = Token.lxmRightParenth;
                            GetSymbol();
                            return;
                        }
                        if (currentSymbol == '[')
                        {
                            tokenOfLxm = Token.lxmls;
                            GetSymbol();
                            return;
                        }
                        if (currentSymbol == ']')
                        {
                            tokenOfLxm = Token.lxmrs;
                            GetSymbol();
                            return;
                        }
                        if (currentSymbol == ',')
                        {
                            tokenOfLxm = Token.lxmComma;
                            GetSymbol();
                            return;
                        }
                        if (currentSymbol == ':')
                        {
                            tokenOfLxm = Token.lxmdt;
                            GetSymbol();
                            return;
                        }
                        if (currentSymbol == '=')
                        {
                            tokenOfLxm = Token.lxmr;
                            GetSymbol();
                            return;
                        }
                        if (currentSymbol == '*')
                        {
                            tokenOfLxm = Token.lxmStar;
                            GetSymbol();
                            return;
                        }
                        if (currentSymbol == '+')
                        {
                            tokenOfLxm = Token.lxmPlus;
                            GetSymbol();
                            return;
                        }
                        break;
                    }
                case CharType.EndText:
                    {
                        tokenOfLxm = Token.lxmEmpty;
                        break;
                    }
            }
        }
    }
}
