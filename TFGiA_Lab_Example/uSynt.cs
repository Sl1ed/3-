using System;
using System.Windows.Forms;

namespace TFGiA_Lab_Example
{
    class uSynt
    {
        public uLex Lex = new uLex();

        public void S()
        {
            if (Lex.tokenOfLxm == Token.lxmIdentifier)
            {
                Lex.NextToken();
                A();
                if (Lex.tokenOfLxm == Token.lxmNumber)
                {
                    Lex.NextToken();
                    B();
                    if (Lex.tokenOfLxm == Token.lxmEmpty)
                    {
                        throw new Exception("Конец слова, текст верный");
                    }
                }
            }

        }

        public void A()
        {
            if (Lex.tokenOfLxm == Token.lxmNumber)
            {
                Lex.NextToken();
                if (Lex.tokenOfLxm == Token.lxmEmpty)
                {
                    throw new Exception("Ожидалось число");
                }
            }
            else if (Lex.tokenOfLxm == Token.lxmls)
            {
                Lex.NextToken();
                A();
                if (Lex.tokenOfLxm == Token.lxmrs)
                {
                    Lex.NextToken();
                }
                else throw new Exception("Ожидалась ]");
            }


        }
        public void B()
        {
            if (Lex.tokenOfLxm == Token.lxmIdentifier)
            {
                Lex.NextToken();
                if (Lex.tokenOfLxm == Token.lxmEmpty)
                {
                    throw new Exception("Конец слова, текст верный");
                }
                else if(Lex.tokenOfLxm == Token.lxmIdentifier)
                {
 
                    B();
                }
                else throw new Exception("Ожидался идентификатор");
            }
        }
    }
}
