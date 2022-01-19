using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{
    public class TreeNode<TData>
    {
        private const int LEVEL_TO_TEXT = 5;

        public TData Data { get; set; }
        public TreeNode<TData>? Parent { get; set; }
        public List<TreeNode<TData>> Children { get; set; } = new();

        public TreeNode(TData data, TreeNode<TData>? parent)
        {
            Data = data;
            Parent = parent;
        }

        public Boolean IsRoot => Parent == null;
        public Boolean IsLeaf => Children.Count == 0;
        public int Level => (Parent?.Level ?? -1) + 1;


        public TreeNode<TData> AddChild(TData data)
        {
            TreeNode<TData> childNode = new(data, this);
            Children.Add(childNode);
            return childNode;
        }

        public override string ToString() => (IsRoot ? "[root]" : new string('>', Level) + Data?.ToString() ?? "");
        public string ToText() => ToStringNodeWithChildren(this);

        static string ToStringNodeWithChildren(TreeNode<TData> node)
        {
            if (node.Level > LEVEL_TO_TEXT) return "";

            string result = node.ToString() + Environment.NewLine;

            foreach (var child in node.Children)
                result += ToStringNodeWithChildren(child);

            return result;
        }

    }
}
