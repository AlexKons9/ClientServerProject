using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    public static class Calculator
    {
        public static decimal Add(decimal num1, decimal num2)
        {
            return num1 + num2;
        }

        public static decimal Subtract(decimal num1, decimal num2)
        {
            return num1 - num2;
        }

        public static decimal Multiply(decimal num1, decimal num2)
        {
            return num1 * num2;
        }

        public static decimal Divide(decimal num1, decimal num2)
        {
            try
            {
                return num1 / num2;
            }
            catch (DivideByZeroException ex)
            {

                throw new DivideByZeroException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
