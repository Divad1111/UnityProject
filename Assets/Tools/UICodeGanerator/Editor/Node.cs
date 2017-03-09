using UnityEngine;
using System.Collections.Generic;

namespace UICodeGenerator
{

    public enum NodeType 
    {
        Root,
        EmptyObject,
        Panel,
        Template,
        Group,
        Widget,
        Label,
        TextBox,
        Sprite,
        Texture,
        Button,
        Slider,
        CheckBox,
        Table,
        Grid,
    }

    public class Node
    {   
        public LinkedList<Node> child = new LinkedList<Node>();
        public Node parent { get; set; }
        public NodeType type { get; set; }
        public GameObject gameObject { get; set; }    

	    public Node(){	}
        public Node(NodeType nType) { type = nType; }
	    public Node(NodeType nType, GameObject goObj)
	    {
		    type = nType;
		    gameObject = goObj;
	    }

        public Node AddChild(Node childNode)
        {
            if (childNode == null)        
                return null;

            child.AddLast(childNode);
            return childNode;
        }

        public bool RemoveChild(Node childNode)
        {
            if (childNode == null)
                return false;

            return child.Remove(childNode);
        }

        public bool HasChildNode()
        {
            if (child.Count > 0)
                return true;
            return false;
        }

        public int Depth()
        {
            int depth = 1;
            _Depth(this, ref depth, 1);
            return depth;
        }

        private void _Depth(Node root, ref int depth, int nDym)
        {
            if (root == null)
                return;
            foreach (var item in root.child)
            {
                ++nDym;
                if (nDym > depth)
                    depth = nDym;
                _Depth( item, ref depth, nDym);
                --nDym;
            }
        }
    }
}
