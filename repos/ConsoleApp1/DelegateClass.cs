using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace ConsoleApplication3
{
    public delegate int MyDelegate(int x, int y);
    public class DelegateClass
    {
        public static int Add(int x, int y)
        {
            return x + y;
        }
        public static int GetValue(int x)
        {
            return (10 + x);
        }
        public static void ShowValue(int x)
        {
            Console.WriteLine("Value" + x);
        }
        public static int GetResult()
        {
            int x = 30;
            int y = 20;
            return x * y;
        }
        public static void ShowEmploye(int age, String name)
        {
            Console.WriteLine("My Name is" + name);
            Console.WriteLine("My age is " + age);
        }
        public static void ShowMessage(String msg)
        {
            Console.WriteLine(msg);
        }
        public static bool IsNumeric(object Expression)
        {
            double retNum;
            bool isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }
    }
    public class MainClass
    {
        public static void Main()
        {
            //Assign method to delegates  
            MyDelegate delAdd = DelegateClass.Add;
            //Call add method using custome delegates  
            int sum = delAdd(1, 2);
            Console.WriteLine(sum);
            //You can't call other method which is having only one input parameter  
            //MyDelegate del = DelegateClass.GetValue(1); //Compilation error since signature of delegates not matching  

            Car c = new Car();
            c.Color = "sfdsf";
            
        }
    }

    public struct Car
    {

        // Declaring different data types 
        private string Brand;
        public string Model;
        public string Color;
    }
}

