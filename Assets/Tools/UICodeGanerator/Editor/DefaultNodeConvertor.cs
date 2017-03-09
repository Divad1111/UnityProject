using UnityEngine;
using System;
using System.Collections.Generic;


namespace UICodeGenerator
{
    public class DefaultNodeConvertor : INodeConvertor
    {
        //Dictionary<int, Node> _nodeMap = new Dictionary<int, Node>();
        List<Node> _nodeList = new List<Node>();

        public List<Node> ToNodeList(Node rootNode)
        {
            //_nodeMap.Clear();
            _nodeList.Clear();
            TraverseNodeTree(rootNode);
            return _nodeList;
        }

        private void TraverseNodeTree(Node node)
        {   
            if (node == null)
            { 
                return;
            }

            if (!_nodeList.Contains(node))
            {
                _nodeList.Add(node);
            }

            foreach(var child in node.child)
            {
                TraverseNodeTree(child);
            }
        }

        public string ToFieldsName(Node node)
        {
            if (node == null || node.gameObject == null)
            {
                return string.Empty;
            }

            var fieldsName = GetValidFieldsName(node);
            if (string.IsNullOrEmpty(fieldsName))
            {
                return string.Empty;
            }

            fieldsName = fieldsName.Substring(0, 1).ToLower() + fieldsName.Substring(1, fieldsName.Length - 1);

            return "_" + fieldsName;
        }

        public string ToClickEventFunctionName(Node node)
        {
            if (node == null || node.gameObject == null)
                return string.Empty;

            var fieldsName = GetValidFieldsName(node);
            if (string.IsNullOrEmpty(fieldsName))
            {
                return string.Empty;
            }

            return "OnClick" + fieldsName;
        }

        public string ToChangeValueEventFunctionName(Node node)
        {
            if (node == null || node.gameObject == null)
                return string.Empty;

            var fieldsName = GetValidFieldsName(node);
            if (string.IsNullOrEmpty(fieldsName))
            {
                return string.Empty;
            }

            return string.Format("On{0}Change", fieldsName);
        }

        private string GetValidFieldsName(Node node)
        {
            var goName = node.gameObject.name;
            var fieldsName = GetStringInBrackets(goName);
            if (string.IsNullOrEmpty(fieldsName))
            {
                fieldsName = goName;
            }

            return fieldsName.Trim();
        }

        private string GetStringInBrackets(string name)
        {
            string ret = string.Empty;
            if (string.IsNullOrEmpty(name))
            {
                return ret;
            }
            var leftBracketIndex = name.IndexOf('(');
            var rightBracketIndex = name.IndexOf(')');
            if (leftBracketIndex == -1 || rightBracketIndex == -1 || leftBracketIndex >= rightBracketIndex)
            {
                return ret;
            }
            return name.Substring(leftBracketIndex + 1, rightBracketIndex - leftBracketIndex - 1);
        }

        
    }
}
