//using System.CodeDom.Compiler;
//using System.Collections.Generic;
//using System.Collections;
//using System.ComponentModel;
//using System.Diagnostics.CodeAnalysis;
//using System.Globalization;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Runtime.Serialization;
//using System.Text.RegularExpressions;
//using System.Text;
//using System;

//namespace ConsoleApp1
//{

//    class Result
//    {

//        /*
//         * Complete the 'isSimilar' function below.
//         *
//         * The function is expected to return a BOOLEAN.
//         * The function accepts following parameters:
//         *  1. STRING_ARRAY sentence_1
//         *  2. STRING_ARRAY sentence_2
//         *  3. 2D_STRING_ARRAY similarity_matrix
//         */

//        //        List<string> sentence_1 = new List<string>() { "amazing", "acting", "abilities" };
//        //        List<string> sentence_2 = new List<string>() { "fine", "theatrics", "talent" };


//        //        List<List<string>> similarity_matrix = new List<List<string>>();

//        //        similarity_matrix.Add(new List<string>() {"amazing","fine" });
//        //            similarity_matrix.Add(new List<string>() {"fine","good" });
//        //similarity_matrix.Add(new List<string>() { "acting", "theatrics" });
//        //similarity_matrix.Add(new List<string>() { "abilities", "talent" });

//        public static bool isSimilar(List<string> sentence_1, List<string> sentence_2, List<List<string>> similarity_matrix)
//        {
//            bool issimilar = true;

//            List<string> nl = new List<string>();

//            nl = similarity_matrix.Select(i => string.Join(",", i)).ToList();


//            for (int i = 0; i < similarity_matrix.Count(); i++)
//            {

//                string finsimword = string.Empty;
//                for (int j = 0; j < similarity_matrix.Count() - 1; j++)
//                {
//                    //string s = similarity_matrix[0][0] + similarity_matrix[0][1];

//                    if (similarity_matrix[i][1] == similarity_matrix[j + 1][i])
//                    {
//                        nl.Add(similarity_matrix[i][0] + similarity_matrix[i + 1][1]);
//                    }
//                    else
//                    {
//                        nl.Add(similarity_matrix[i][0] + similarity_matrix[i][1]);
//                    }
//                }



//                if (sentence_1.Count() < i && sentence_2.Count() < i)
//                {

//                    if (nl.Any(j => j.Contains(sentence_1[i] + "," + sentence_2[i])))
//                    {
//                        issimilar = true;
//                    }
//                    else if (nl.Any(j => j.Contains(sentence_1[i]) || j.Contains(sentence_2[i])))
//                    {
//                        issimilar = true;
//                    }
//                    else
//                    {
//                        issimilar = false;
//                        break;
//                    }
//                }
//            }

//            if


//            return issimilar;

//        }

//    }

//    class Solution1
//    {
//        public static void Main(string[] args)
//        {

//            List<string> sentence_1 = new List<string>() { "amazing", "acting", "abilities" };
//            List<string> sentence_2 = new List<string>() { "good", "theatrics", "talent" };


//            List<List<string>> similarity_matrix = new List<List<string>>();

//            similarity_matrix.Add(new List<string>() { "amazing", "fine" });
//            similarity_matrix.Add(new List<string>() { "fine", "good" });
//            similarity_matrix.Add(new List<string>() { "acting", "theatrics" });
//            similarity_matrix.Add(new List<string>() { "abilities", "talent" });

//            //for (int i = 0; i < similarity_matrixRows; i++)
//            //{
//            //    similarity_matrix.Add(Console.ReadLine().TrimEnd().Split(' ').ToList());
//            //}




//            bool result = Result.isSimilar(sentence_1, sentence_2, similarity_matrix);

//            //textWriter.WriteLine((result ? 1 : 0));

//            //textWriter.Flush();
//            //textWriter.Close();
//        }
//    }
//}
