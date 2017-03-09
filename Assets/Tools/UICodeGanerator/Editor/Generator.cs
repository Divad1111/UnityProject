using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace UICodeGenerator
{
    public class Generator
    {	
        private Parser Parser { get; set; }
        private INodeConvertor NodeConvertor { get; set; }
        private IFileGenerator FileGenerator { get; set; }


	    public static Generator CreateGenerator(Parser parser, INodeConvertor nodeConvertor, IFileGenerator fileGenerator)
	    {
            if(parser == null)
            {
                return null;
            }
                
            if (fileGenerator == null)
            {
                return null;
            }

            if(nodeConvertor == null)
            {
                nodeConvertor = new DefaultNodeConvertor();
            }
		    return new Generator(parser, nodeConvertor, fileGenerator);
	    }


	    protected Generator(Parser parser, INodeConvertor nodeConvertor, IFileGenerator fileGenerator)
	    {
		    Parser = parser;
            NodeConvertor = nodeConvertor;
            FileGenerator = fileGenerator;
	    }

        public ErrorCode GenerateFile(string filePath, string author, System.Text.Encoding encoder, INodeConvertor nodeConvertor = null)
	    {
		    //parsing node from root
		    var rootNode = Parser.Parse();
		   
            //init node convertor
            if (nodeConvertor != null)
            {
                NodeConvertor = nodeConvertor;
            }

            var nodeList = NodeConvertor.ToNodeList(rootNode);
            if (nodeList == null || nodeList.Count <= 0)
            {
                return ErrorCode.NodeConvertFailed;
            }

            // check same name
            if(HaveSameFiledsName(nodeList))
            {
                return ErrorCode.SameFieldsName;
            }

            //creating file
            var sw = new StreamWriter(filePath, false, encoder);
            if (sw == null)
            {
                return ErrorCode.CreateFileFailed;
            }

            //starting generate file
            string strFileName = Path.GetFileNameWithoutExtension(filePath);
		    return  BeginGenerateFile (sw, nodeList, strFileName, author) ? ErrorCode.None : ErrorCode.GenerateFileFailed;
	    }

        private bool HaveSameFiledsName(List<Node> nodeList)
        {
            if (nodeList == null || nodeList.Count <= 0)
            {
                return false;
            }

            HashSet<string> nameSet = new HashSet<string>();
            foreach (var child in nodeList)
            {
                if (child.gameObject == null)
                    continue;

                var childName = child.gameObject.name;
                if (nameSet.Contains(childName))
                {   
                    return true;
                }
                nameSet.Add(childName);
            }

            return false;
        }

        public Dictionary<string, List<GameObject>> GetSameNameObjects( )
        {            
            var outSameNameObjects = new Dictionary<string, List<GameObject>>();
            //parsing node from root
            var rootNode = Parser.Parse();
            var nodeList = NodeConvertor.ToNodeList(rootNode);

            if (nodeList == null || nodeList.Count <= 0)
            {
                return null;
            }
           
            foreach(var child in nodeList)
            {
                if (child.gameObject == null)
                    continue;

                var childName = child.gameObject.name;
                if (outSameNameObjects.ContainsKey(childName))
                {
                    outSameNameObjects[childName].Add(child.gameObject);
                }
                else
                {
                    var gameObjects = new List<GameObject>();
                    gameObjects.Add(child.gameObject);
                    outSameNameObjects.Add(childName, gameObjects);
                }
            }
            
            return outSameNameObjects;
        }

        private bool BeginGenerateFile(StreamWriter sw, List<Node> nodeList, string fileName, string author)
        {
            if (FileGenerator == null)
            {
                return false;
            }

            // write file header
            FileGenerator.OnBeginWriteFileHeader(sw, fileName, author);
            FileGenerator.OnBeginWriteClass(sw, fileName);

            // write fields
            foreach(var node in nodeList)
            {
                if (node.gameObject != null)
                { 
                    FileGenerator.OnWriteFields(sw, node, NodeToFieldsName(node));
                }
            }

            // write init function
            FileGenerator.OnBeginWriteInitFunction(sw);
            foreach (var node in nodeList)
            {
                if (node.gameObject != null)
                {
                    FileGenerator.OnWriteInitFunction(sw, node, NodeToFieldsName(node), node.parent, NodeToFieldsName(node.parent));
                }
            }

            // write event bind
            FileGenerator.OnBeginWriteEventBind(sw);
            foreach (var node in nodeList)
            {
                if (node.gameObject == null)
                    continue;

                switch (node.type)
                {
                    case NodeType.Button:
                        FileGenerator.OnWriteEventBind(sw, node, NodeToFieldsName(node), NodeToClickEventFunctionName(node));
                        break;
                    case NodeType.Slider:
                    case NodeType.CheckBox:
                        FileGenerator.OnWriteEventBind(sw, node, NodeToFieldsName(node), NodeToChangeValueEventFunctionName(node));
                        break;
                    default:
                        break;
                }
            }   
            FileGenerator.OnEndWriteInitFunction(sw);
            
            // write other function
            FileGenerator.OnWriteOtherFunction(sw);

            // write event function
            foreach (var node in nodeList)
            {
                if (node.gameObject == null)
                    continue;

                switch (node.type)
                {
                    case NodeType.Button:
                        FileGenerator.OnWriteEventFunction(sw, node, NodeToClickEventFunctionName(node));
                        break;
                    case NodeType.Slider:
                    case NodeType.CheckBox:
                        FileGenerator.OnWriteEventFunction(sw, node, NodeToChangeValueEventFunctionName(node));
                        break;
                    default:
                        break;
                }
            }   
            
            // write end of class and file header
            FileGenerator.OnEndWriteClass(sw, fileName);
            FileGenerator.OnEndWriteFileHeader(sw, fileName, author);
            
            sw.Flush();
            sw.Close();
            sw.Dispose();
            return true;
        }

        protected string NodeToFieldsName(Node node)
        {       
            var nodeFields = NodeConvertor.ToFieldsName(node);
            if (string.IsNullOrEmpty(nodeFields))
            {
                nodeFields = "transform";
            }
            return nodeFields;
        }

        protected string NodeToClickEventFunctionName(Node node)
        {
            return NodeConvertor.ToClickEventFunctionName(node);
        }

        protected string NodeToChangeValueEventFunctionName(Node node)
        {
            return NodeConvertor.ToChangeValueEventFunctionName(node);
        }
    }

}
