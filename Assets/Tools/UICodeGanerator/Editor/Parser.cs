using UnityEngine;
using System.Collections.Generic;

namespace UICodeGenerator
{
    public abstract class Parser
    {
        GameObject Root { get; set; }


        public Parser(GameObject root)
        {
            Root = root;
        }

        public Node Parse()
        {
            return Parse(Root);
        }

        public Node Parse(GameObject go)
        {
            Node rootNode = new Node(NodeType.Root);
            if (go != null)
            {
                int nChildCnt = go.transform.childCount;
                for (int i = 0; i < nChildCnt; ++i)
                {
                    var childGo = go.transform.GetChild(i).gameObject;

                    InnerParse(childGo, rootNode);
                }
            }
      
            return rootNode;
        }

        private void InnerParse(GameObject go, Node rootNode)
        {
            if (go == null)
                return; 

            Node childNode = ParseOne(go, rootNode);
            if (IsTemplate(go) || IsPanel(go))
                return;

            int nChildCnt = go.transform.childCount;
            for (int i = 0; i < nChildCnt; ++i)
            {
                var childGo = go.transform.GetChild(i).gameObject;

                InnerParse(childGo, childNode);
            }
        }

        private Node ParseOne(GameObject go, Node rootNode)
        {
            if (rootNode == null)
                return null;

            if (IsPanel(go))
            {
                Node node = new Node();
                node.parent = rootNode;
                node.gameObject = go;
                node.type = NodeType.Panel;
                return rootNode.AddChild(node);
            }
            else if (IsEmptyObject(go))
            {
                Node node = new Node();
                node.parent = rootNode;
                node.gameObject = go;
                node.type = NodeType.EmptyObject;
                return rootNode.AddChild(node);
            }
            else if (IsTemplate(go))
            {
                Node node = new Node();
                node.parent = rootNode;
                node.gameObject = go;
                node.type = NodeType.Template;
                return rootNode.AddChild(node);
            }
            else if (IsGroup(go))
            {
                Node node = new Node();
                node.parent = rootNode;
                node.gameObject = go;
                node.type = NodeType.Group;
                return rootNode.AddChild(node);
            }
            else if (IsLabel(go))
            {
                Node node = new Node();
                node.parent = rootNode;
                node.gameObject = go;
                node.type = NodeType.Label;
                return rootNode.AddChild(node);
            }
            else if (IsTextBox(go))
            {
                Node node = new Node();
                node.parent = rootNode;
                node.gameObject = go;
                node.type = NodeType.TextBox;
                return rootNode.AddChild(node);
            }
            else if (IsSprite(go))
            {
                Node node = new Node();
                node.parent = rootNode;
                node.gameObject = go;
                node.type = NodeType.Sprite;
                return rootNode.AddChild(node);
            }
            else if (IsTexture(go))
            {
                Node node = new Node();
                node.parent = rootNode;
                node.gameObject = go;
                node.type = NodeType.Texture;
                return rootNode.AddChild(node);
            }
            else if (IsButton(go))
            {
                Node node = new Node();
                node.parent = rootNode;
                node.gameObject = go;
                node.type = NodeType.Button;
                return rootNode.AddChild(node);
            }
            else if (IsSlider(go))
            {
                Node node = new Node();
                node.parent = rootNode;
                node.gameObject = go;
                node.type = NodeType.Slider;
                return rootNode.AddChild(node);
            }
            else if (IsCheckBox(go))
            {
                Node node = new Node();
                node.parent = rootNode;
                node.gameObject = go;
                node.type = NodeType.CheckBox;
                return rootNode.AddChild(node);
            }
            else if (IsGrid(go))
            {
                Node node = new Node();
                node.parent = rootNode;
                node.gameObject = go;
                node.type = NodeType.Grid;
                return rootNode.AddChild(node);
            }
            else if (IsTable(go))
            {
                Node node = new Node();
                node.parent = rootNode;
                node.gameObject = go;
                node.type = NodeType.Table;
                return rootNode.AddChild(node);
            }
            else
            {
                return rootNode;
            }
        }


        //interface
        protected abstract bool IsPanel(GameObject go);
        protected abstract bool IsEmptyObject(GameObject go);
        protected abstract bool IsTemplate(GameObject go);
        //public abstract bool IsWidget(GameObject go);
        protected abstract bool IsLabel(GameObject go);
        protected abstract bool IsTextBox(GameObject go);
        protected abstract bool IsSprite(GameObject go);
        protected abstract bool IsTexture(GameObject go);
        protected abstract bool IsButton(GameObject go);
        protected abstract bool IsSlider(GameObject go);
        protected abstract bool IsCheckBox(GameObject go);
        protected abstract bool IsGrid(GameObject go);
        protected abstract bool IsTable(GameObject go);
        protected abstract bool IsGroup(GameObject go);
    }

}