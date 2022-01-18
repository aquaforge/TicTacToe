using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{
    public class Tree<TData>
    {
        TreeNode<TData> Root;

        public Tree(TreeNode<TData> root)
        {
            Root = root;
        }

        public override string ToString() => ToStringNodeWithChildren(Root);
        static string ToStringData(TreeNode<TData> node) => new string('>', node.Level) + node?.Data?.ToString() ?? "";

        static string ToStringNodeWithChildren(TreeNode<TData> node)
        {
            string result = (node.IsRoot ? "[root]" : ToStringData(node)) + Environment.NewLine;

            foreach (var child in node.Children)
                result += ToStringNodeWithChildren(child);
            if (node.Children.Count != 0) result += Environment.NewLine;

            return result;
        }


    }
}
