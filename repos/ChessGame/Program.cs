//using System;
//using System.Collections.Generic;
//using System.Dynamic;
//using System.Runtime.InteropServices;
//using System.Text.RegularExpressions;

//namespace ChessGame
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            //ChessGame chessGame = new ChessGame();

//            //dynamic result = chessGame.ValidateMove(Enum.Parse<Move>(args[0]), Convert.ToBoolean(args[1]));

//            //Console.WriteLine("{0},{1}",result.CanMove,result.Message);

//            //Console.ReadLine();
//            //Employee obj = new Employee();
//            //obj.Id = 10;
//            //int i = 10;
//            //ChangeValue(i,obj);
//            //Console.WriteLine(obj.Id);
//            //Console.ReadLine();

//            //Solution.solution(955);
//            //arrasol();

//            string config = @"UserName:admin;
//                            Password:""super%^&*333password;
//                            DNSName:SomeName;

//                            TimeToLive:4;
//                            ClusterSize:2;
//                            PortNumber:2222;

//                            IsEnabled:true;
//                            EnsureTransaction:false;
//                            PersistentStorage:false;";
//            Parse(config);
//        }

//        public class ConfigModel
//        {
//            public string UserName { get; set; }
//            public string Password { get; set; }
//            public string DNSName { get; set; }
//            public int TimeToLive { get; set; }
//            public int ClusterSize { get; set; }
//            public int PortNumber { get; set; }
//            public bool IsEnabled { get; set; }
//            public bool EnsureTransaction { get; set; }
//            public bool PersistentStorage { get; set; }
//        }
//        public static dynamic Parse(string configuration)
//        {

//            if (string.IsNullOrEmpty(configuration))
//            {
//                throw new EmptyKeyException();

//            }

//            ConfigModel model = new ConfigModel();


//            string[] strarr = configuration.Split("\n");

//            foreach (string item in strarr)
//            {
//                if (!string.IsNullOrEmpty(item.Trim()))
//                {
//                    string propname = item.Trim().Split(":")[0].Trim();

//                    if (string.IsNullOrEmpty(propname))
//                    {
//                        throw new EmptyKeyException();
//                    }

//                    bool isexist = IsPropertyExist(model, propname);

//                    if (!isexist)
//                    {
//                        throw new UnknownKeyException();
//                    }

//                    if (!Regex.Match(propname, "^[^0-9][a-zA-Z0-9]+$").Success)
//                    {
//                        throw new InvalidKeyException();
//                    }
//                }
//            }

//            return null;
//        }

//        public static bool IsPropertyExist(dynamic settings, string name)
//        {
//            return settings.GetType().GetProperty(name) != null;
//        }

//        public static int GetParis()
//        {
//            int[] array = { 1, 1, 1, 1, 2 };
//            int[] temparra = new int[array.Length];
//            int pairs = 0;

//            for (int i = 0; i < array.Length; i++)
//            {
//                int count = 0;
//                bool hasEle = false;
//                for (int k = 0; k < temparra.Length; k++)
//                {
//                    if (temparra[k] == array[i])
//                    {
//                        hasEle = true;
//                        break;
//                    }
//                }
//                if (!hasEle)
//                {
//                    for (int j = 0; j < array.Length; j++)
//                    {

//                        if (array[i] == array[j])
//                        {
//                            count = count + 1;

//                        }
//                    }

//                    temparra[i] = array[i];
//                    pairs += Convert.ToInt32(Math.Floor((decimal)count / 2));
//                }

//            }

//            return pairs;
//        }

//        public class Employee
//        {
//            public int Id { get; set; }
//        }

//        public static string GetNextName(int id)
//        {
//            string returnText = "Next-" + id.ToString();
//            id += 1;
//            return returnText;
//        }
//        public static string GetNextName(ref int id)
//        {
//            string returnText = "Next-" + id.ToString();
//            id += 1;
//            return returnText;
//        }

//        public static void ChangeValue(int i, Employee obj)
//        {
//            //string add = Get(obj);
//            Employee obj1 = new Employee();
//            obj1 = obj;
//            //string add1 = Get(obj);

//            obj1.Id = 30;
//            //i = 20;
//            //obj.Id = 20;
//            //return i;
//        }

//        public static string Get(object a)
//        {
//            GCHandle handle = GCHandle.Alloc(a, GCHandleType.Pinned);
//            try
//            {
//                IntPtr pointer = GCHandle.ToIntPtr(handle);
//                return "0x" + pointer.ToString("X");
//            }
//            finally
//            {
//                handle.Free();
//            }
//        }

//        public static void arrasol(int[] A = null)
//        {
//            A = new int[5] { 1, 4, 3, -1, 2 };
//            // write your code in C# 6.0 with .NET 4.5 (Mono)

//            int ele = 0;
//            int i = 0;
//            int count = 0;
//            while (ele != -1)
//            {
//                ele = A[i];
//                i = ele;
//                count++;
//            }

//            Console.WriteLine(count);
//            Console.ReadKey();
//        }

//    }

//    static class Solution
//    {
//        public static int solution(int n)
//        {
//            int[] d = new int[31];
//            int l = 0;
//            int p;
//            while (n > 0)
//            {
//                d[l] = n % 2;
//                n /= 2;
//                l++;
//            }
//            for (p = 1; p < 1 + l / 2; ++p)
//            {
//                int i;
//                bool ok = true;
//                for (i = 0; i < l - p; ++i)
//                {
//                    if (d[i] != d[i + p])
//                    {
//                        ok = false;
//                        break;
//                    }
//                }
//                if (ok)
//                {
//                    return p;
//                }
//            }
//            return -1;
//        }
//    }

//}
