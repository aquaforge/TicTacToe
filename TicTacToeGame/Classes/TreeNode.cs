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


        public int Level
        {
            get
            {
                if (IsRoot) return 0;
                return (Parent?.Level ?? 0) + 1;
            }
        }

        public override string ToString() => Data?.ToString() ?? "";
        

        public TreeNode<TData> AddChild(TData data)
        {
            TreeNode<TData> childNode = new(data, this);
            Children.Add(childNode);
            return childNode;
        }
    }
}
