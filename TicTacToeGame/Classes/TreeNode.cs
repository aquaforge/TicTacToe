using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{
    public class TreeNode<TData> : IEnumerable<TreeNode<TData>>
    {
        public TData Data { get; set; }
        public TreeNode<TData>? Parent { get; set; }
        public List<TreeNode<TData>> Children { get; set; }

        public TreeNode(TData data)
        {
            this.Data = data;
            this.Children = new();
        }

        public Boolean IsRoot => Parent == null;

        public Boolean IsLeaf => Children.Count == 0;


        public int Level
        {
            get
            {
                if (IsRoot) return 0;
                return Parent.Level + 1;
            }
        }

        public override string ToString()
        {
            if (Data != null)
                return Data.ToString();
            else
                return "[null]";
        }

        public TreeNode<TData> AddChild(TData data)
        {
            TreeNode<TData> childNode = new(data) { Parent = this };
            Children.Add(childNode);
            return childNode;
        }

        #region IEnumerable
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<TreeNode<TData>> GetEnumerator()
        {
            yield return this;
            foreach (var child in this.Children)
                foreach (var a in child)
                    yield return a;
        }
        #endregion

    }
}
