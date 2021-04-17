﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
	using System;

	/* C# Program to find distance between n1 and n2 
	using one traversal */
	public class GFG
	{
		public class Node
		{
			public int value;
			public Node left;
			public Node right;

			public Node(int value)
			{
				this.value = value;
			}
		}

		public static Node LCA(Node root, int n1, int n2)
		{
			if (root == null)
			{
				return root;
			}
			if (root.value == n1 || root.value == n2)
			{
				return root;
			}

			Node left = LCA(root.left, n1, n2);
			Node right = LCA(root.right, n1, n2);

			if (left != null && right != null)
			{
				return root;
			}
			if (left == null && right == null)
			{
				return null;
			}

			if (left != null)
			{
				return LCA(root.left, n1, n2);
			}
			else
			{
				return LCA(root.right, n1, n2);
			}
		}

		// Returns level of key k if it is present in 
		// tree, otherwise returns -1 
		public static int findLevel(Node root, int a, int level)
		{
			if (root == null)
			{
				return -1;
			}
			if (root.value == a)
			{
				return level;
			}
			int left = findLevel(root.left, a, level + 1);
			if (left == -1)
			{
				return findLevel(root.right, a, level + 1);
			}
			return left;
		}

		public static int findDistance(Node root, int a, int b)
		{
			Node lca = LCA(root, a, b);

			int d1 = findLevel(lca, a, 0);
			int d2 = findLevel(lca, b, 0);

			return d1 + d2;
		}

		// Driver program to test above functions 
		//public static void Main(string[] args)
		//{

		//	// Let us create binary tree given in 
		//	// the above example 
		//	Node root = new Node(1);
		//	root.left = new Node(2);
		//	root.right = new Node(3);
		//	root.left.left = new Node(4);
		//	root.left.right = new Node(5);
		//	root.right.left = new Node(6);
		//	root.right.right = new Node(7);
		//	root.right.left.right = new Node(8);
		//	//Console.WriteLine("Dist(4, 5) = " + findDistance(root, 4, 5));
		//	Console.WriteLine("Dist(5, 6) = " + findDistance(root, 5, 6));

		//	//Console.WriteLine("Dist(4, 6) = " + findDistance(root, 4, 6));

		//	//Console.WriteLine("Dist(3, 4) = " + findDistance(root, 3, 4));

		//	//Console.WriteLine("Dist(2, 4) = " + findDistance(root, 2, 4));

		//	//Console.WriteLine("Dist(8, 5) = " + findDistance(root, 8, 5));

		//}
	}

	// This code is contributed by Shrikant13

}
