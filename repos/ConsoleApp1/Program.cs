using ConsoleApplication3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Program 
    {
        static void Main(string[] args)
        {
            //Solution solution = new Solution();
            //solution.solution(new int[] { 1, 2, 3, 4, 5, 6 });

            //A objA = new A();
            //objA.method();


            //A objB = new B();
            //objB.method();


            //B objC = new C();
            //objC.method();
            //InvoiceRepository invoiceRepository = new InvoiceRepository(null) ;

            //invoiceRepository.GetItemsReport(DateTime.Now.AddDays(-1), DateTime.Now);

            //Class1 class1 = new Class1();

            //class1.DictTest();

            //Task.Run(() => MainAsync(args)).Wait();

            //int[] input1 = { 1, 2, 3, 4 };
            //int[] input2 = { 5, 6, 8, 7 };
            //int[] input3 = { 89, 10, 11, 12 };
            //List<int> list1 = input1.AsEnumerable().ToList();
            //List<int> list2 = input2.AsEnumerable().ToList();

            //List<int> first = new List<int>() { 1, 2, 3, 4, 5 };
            //List<int> second = new List<int>() { 6, 7, 8, 9 };

            //List<int> result = first.Intersect(second).ToList();

            //list1.Join(list2);

            //var outpqut = input1.ToList().Join(input2.ToList());

            ////.Join(input3.ToList()));

            //int[] output = new int[input1.Length + input2.Length + input3.Length];
            //for (int i = 0; i < output.Length; i++)
            //{
            //    if (i >= (input1.Length + input2.Length))
            //        output[i] = input3[i - (input1.Length + input2.Length)];
            //    else if (i >= input2.Length)
            //        output[i] = input2[i - input1.Length];
            //    else
            //        output[i] = input1[i];
            //}


            //int[] intArray = new int[] { 2, 9, 4, 3, 5, 1, 7 };
            //int temp = 0;

            //for (int i = 0; i < intArray.Length; i++)
            //{
            //    for (int j = i + 1; j < intArray.Length; j++)
            //    {
            //        if (intArray[i] > intArray[j])
            //        {
            //            temp = intArray[i];
            //            intArray[i] = intArray[j];
            //            intArray[j] = temp;
            //        }
            //    }
            //}

            //foreach (var item in intArray)
            //{
            //    Console.WriteLine(item);
            //}


            //var devicenames = new List<string>() { "mixer", "toaster", "mixer", "tv" };

            //var devicenamearr = devicenames.ToArray();


            ////var grpdict = (from d in devicenames
            ////               group d by d
            ////              into d
            ////               select d).ToDictionary(gdc => gdc.Key, gdc => gdc.Count());

            //var uni = devicenames.GroupBy(i => i);


            //foreach (var item in uni)
            //{
            //    int count = 0;

            //    for (int j = 0; j < devicenamearr.Count(); j++)
            //    {
            //        if (item.Key == devicenamearr[j])
            //        {
            //            count++;
            //        }

            //        if (count > 1 && (devicenamearr[j] == item.Key))
            //        {
            //            devicenamearr[j] = devicenamearr[j] + (count - 1).ToString();
            //        }
            //    }

            //}

            //foreach (var item in uni)
            //{
            //    int c = item.Count();
            //    int count = 0;
            //    for (int j = 0; j < devicenamearr.Length; j++)
            //    {
            //        if (item.Key == devicenamearr[j])
            //        {

            //        }

            //        if (count > 1)
            //        {
            //            devicenamearr[j] = devicenamearr[j] + (count - 1).ToString();
            //        }

            //    }
            //}

            // return devicenamearr.ToList();

            Car c = new Car();
            c.Model = "dfds";
            //c.GetAssets();

            volatile int x;
        }


        public struct Car
        {

            // Declaring different data types 
            private string Brand;
            public string Model;
            public string Color;
        }
        //static async void MainAsync(string[] args)
        //{
        //    AsyncTest asyncTest = new AsyncTest();
        //    Console.WriteLine(DateTime.Now.Millisecond);
        //    await asyncTest.TestAsync1();
        //    Console.WriteLine(DateTime.Now.Millisecond);
        //    await asyncTest.TestAsync2();
        //    Console.WriteLine(DateTime.Now.Millisecond);

        //    Console.WriteLine(DateTime.Now.Millisecond);
        //    asyncTest.TestAsync1();
        //    asyncTest.TestAsync2();
        //    Console.WriteLine(DateTime.Now.Millisecond);

        //    Console.WriteLine(DateTime.Now.Millisecond);
        //    var result1 = asyncTest.TestAsync1().Result;
        //    var result2 = asyncTest.TestAsync2().Result;
        //    Console.WriteLine(DateTime.Now.Millisecond);

        //}

    }

    public interface IStringMap<TValue> where TValue : class
    {
        /// <summary>Returns number of elements in a map</summary>
        int Count { get; }

        /// <summary>
        /// If <c>GetValue</c> method is called but a given key is not in a map then <c>DefaultValue</c> is returned.
        /// </summary>
        TValue DefaultValue { get; set; }

        /// <summary>
        /// Adds a given key and value to a map.
        /// If the given key already exists in the map, then the value associated with this key should be overridden.
        /// </summary>
        /// <returns>true if the value for the key was overridden, otherwise false</returns>
        /// <exception cref="System.ArgumentNullException">If the key is null</exception>
        /// <exception cref="System.ArgumentException">If the key is an empty string</exception>
        /// <exception cref="System.ArgumentNullException">If the value is null</exception>
        bool AddElement(string key, TValue value);

        /// <summary>
        /// Removes a given key and associated value from a map.
        /// </summary>
        /// <returns>true if the key was in the map and was removed, otherwise false</returns>
        /// <exception cref="System.ArgumentNullException">If the key is null</exception>
        /// <exception cref="System.ArgumentException">If the key is an empty string</exception>
        bool RemoveElement(string key);

        /// <summary>
        /// Returns the value associated with a given key.
        /// </summary>
        /// <returns>The value associated with a given key or <c>DefaultValue</c> if the key does not exist in a map</returns>
        /// <exception cref="System.ArgumentNullException">If the key is null</exception>
        /// <exception cref="System.ArgumentException">If the key is an empty string</exception>
        TValue GetValue(string key);
    }

    public class StringMap<TValue> : IStringMap<TValue>
        where TValue : class
    {

        private Dictionary<string, Object> dict = new Dictionary<string, Object>();
        /// <summary> Returns number of elements in a map</summary>
        public int Count => dict.Count;

        /// <summary>
        /// If <c>GetValue</c> method is called but a given key is not in a map then <c>DefaultValue</c> is returned.
        /// </summary>
        // Do not change this property
        public TValue DefaultValue { get; set; }

        /// <summary>
        /// Adds a given key and value to a map.
        /// If the given key already exists in a map, then the value associated with this key should be overriden.
        /// </summary>
        /// <returns>true if the value for the key was overriden otherwise false</returns>
        /// <exception cref="System.ArgumentNullException">If the key is null</exception>
        /// <exception cref="System.ArgumentException">If the key is an empty string</exception>
        /// <exception cref="System.ArgumentNullException">If the value is null</exception>
        public bool AddElement(string key, TValue value)
        {
            dict.Add(key, value);
            return true;
        }

        /// <summary>
        /// Removes a given key and associated value from a map.
        /// </summary>
        /// <returns>true if the key was in the map and was removed otherwise false</returns>
        /// <exception cref="System.ArgumentNullException">If the key is null</exception>
        /// <exception cref="System.ArgumentException">If the key is an empty string</exception>
        public bool RemoveElement(string key)
        {
            if (dict.ContainsKey(key))
                dict.Remove(key);
            return true;
        }

        /// <summary>
        /// Returns the value associated with a given key.
        /// </summary>
        /// <returns>The value associated with a given key or <c>DefaultValue</c> if the key does not exist in a map</returns>
        /// <exception cref="System.ArgumentNullException">If a key is null</exception>
        /// <exception cref="System.ArgumentException">If a key is an empty string</exception>
        public TValue GetValue(string key)
        {
            if (dict.ContainsKey(key))
                return (TValue)dict[key];

            return null;
        }
    }

    class A
    {
        public virtual void method()
        {
            Console.Write("A");
        }
    }
    class B : A
    {
        public override void method()
        {
            Console.Write("B");
        }
    }
    class C : B
    {
        public new void method()
        {
            Console.Write("c");
        }
    }


    public class Parent
    {
        public virtual void GetAssets()
        {
            Console.WriteLine("calling parent");
        }

    }

    
    public class child : Parent
    {
        public override void GetAssets()
        {
            //Parent P = new child();
            //P.GetAssets();
            Console.WriteLine("calling child");
        }

    }

    public sealed class Singleton
    {
        private static Singleton instance = null;
        private static readonly object padlock = new object();

        Singleton()
        {
        }

        public static Singleton Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Singleton();
                    }
                    return instance;
                }
            }
        }
    }
}
