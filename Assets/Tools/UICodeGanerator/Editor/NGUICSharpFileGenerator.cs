using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;


namespace UICodeGenerator
{
    class NGUICSharpFileGenerator : IFileGenerator
    {
        const int indent = 4;
        public void OnBeginWriteFileHeader(StreamWriter sw, string fileName, string author)
        {
            sw.WriteLine("// created: " + DateTime.Now.ToString());
            sw.WriteLine("// author: " + author);
            UICGTools.AppendNewLine(sw, 1);       
  
            sw.WriteLine("using UnityEngine;");
            sw.WriteLine("using System;");
            sw.WriteLine("using System.Collections;");
            sw.WriteLine("using System.Collections.Generic;");
            UICGTools.AppendNewLine(sw, 2);
        }

        public void OnEndWriteFileHeader(StreamWriter sw, string fileName, string author)
        {
            sw.Flush();
        }

        public void OnBeginWriteClass(StreamWriter sw, string fileName)
        {
            string classDeclaration = string.Format("public class {0} : MonoBehaviour", fileName);
            sw.Write(classDeclaration);
            UICGTools.AppendNewLine(sw, 1);
            sw.Write("{");
            UICGTools.AppendNewLine(sw, 1);
        }

        public void OnEndWriteClass(StreamWriter sw, string fileName)
        {
            UICGTools.AppendNewLine(sw, 2);
            sw.Write("}");
            UICGTools.AppendNewLine(sw, 1);
        }

        public void OnWriteFields(StreamWriter sw, Node node, string fieldsName)
        {
            string result = string.Empty;
            switch(node.type)
            {     
                case NodeType.Root:
                    result = "Transform";
                    break;
                case NodeType.EmptyObject:
                    result = "Transform";
                    break;
                case NodeType.Panel:
                    result = "Transform";
                    break;
                case NodeType.Template:
                    result = "GameObject";
                    break;
                case NodeType.Group:
                    result = "Transform";
                    break;
                case NodeType.Widget:
                    result = "UIWidget";
                    break;
                case NodeType.Label:
                    result = "UILabel";
                    break;
                case NodeType.TextBox:
                    result = "UIInput";
                    break;
                case NodeType.Sprite:
                    result = "UISprite";
                    break;
                case NodeType.Texture:
                    result = "UITexture";
                    break;
                case NodeType.Button:
                    result = "Transform";
                    break;
                case NodeType.Slider:
                    result = "UISlider";
                    break;
                case NodeType.CheckBox:
                    result = "UIToggle";
                    break;
                case NodeType.Table:
                    result = "UITable";
                    break;
                case NodeType.Grid:
                    result = "UIGrid";
                    break;
                default:
                    Debug.LogError("Type of node is unknown.");
                    break;
            }
            UICGTools.AppendSpace(sw, indent);
            sw.Write(string.Format("{0} {1};", result, fieldsName));
            UICGTools.AppendNewLine(sw, 1);
        }

        public void OnBeginWriteInitFunction(StreamWriter sw)
        {
            UICGTools.AppendNewLine(sw, 1, indent);
            sw.Write("void Awake()");
            UICGTools.AppendNewLine(sw, 1);
            UICGTools.AppendSpace(sw, indent);
            sw.WriteLine("{");
        }

        public void OnWriteInitFunction(StreamWriter sw, Node node, string fieldsName, Node parentNode, string parentFieldsName)
        {
            string parentName = parentFieldsName;
            if (parentNode != null &&
                (parentNode.type == NodeType.Widget||
                parentNode.type == NodeType.Label ||
                parentNode.type == NodeType.TextBox ||
                parentNode.type == NodeType.Sprite ||
                parentNode.type == NodeType.Texture ||
                parentNode.type == NodeType.Button ||
                parentNode.type == NodeType.Slider ||
                parentNode.type == NodeType.CheckBox||
                parentNode.type == NodeType.Table||
                parentNode.type == NodeType.Grid))
            {
                parentName = parentFieldsName + ".transform";
            }           

            var functionNameOfFindChild = GetFunctionNameOfFindChid();
            if (string.IsNullOrEmpty(functionNameOfFindChild))
            {
                functionNameOfFindChild = "UICGTools.FindChild";
            }

            var componentName = string.Empty;
            switch (node.type)
            {
                case NodeType.Root:
                    break;
                case NodeType.EmptyObject:                  
                    break;
                case NodeType.Panel:
                    break;
                case NodeType.Template:                   
                    break;
                case NodeType.Group:
                    break;
                case NodeType.Widget:
                    componentName = "UIWidget";
                    break;
                case NodeType.Label:
                    componentName = "UILabel";
                    break;
                case NodeType.TextBox:
                    componentName = "UIInput";
                    break;
                case NodeType.Sprite:
                    componentName = "UISprite";
                    break;
                case NodeType.Texture:
                    componentName = "UITexture";
                    break;
                case NodeType.Button:
                    break;
                case NodeType.Slider:
                    componentName = "UISlider";
                    break;
                case NodeType.CheckBox:
                    componentName = "UIToggle";
                    break;
                case NodeType.Table:
                    componentName = "UITable";
                    break;
                case NodeType.Grid:
                    componentName = "UIGrid";
                    break;
                default:
                    Debug.LogError("Type of node is unknown.");
                    break;
            }

            string result = string.Empty;
            if (string.IsNullOrEmpty(componentName))
            {
                if (node.type == NodeType.Template)
                {
                    result = string.Format ("{0} = {1}({2}, \"{3}\").gameObject;",
                    fieldsName, functionNameOfFindChild, parentName, node.gameObject.name);

                    UICGTools.AppendSpace(sw, 2 * indent);
                    sw.Write(result);
                    UICGTools.AppendNewLine(sw, 1);

                    result = string.Format ("{0}.SetActive(false);", fieldsName);

                } else
                {   
                    result = string.Format ("{0} = {1}({2}, \"{3}\");",
                    fieldsName, functionNameOfFindChild, parentName, node.gameObject.name);
                }
            }
            else
            {
                result = string.Format("{0} = {1}({2}, \"{3}\").GetComponent<{4}>();",
                fieldsName, functionNameOfFindChild, parentName, node.gameObject.name, componentName);
            }
            

            UICGTools.AppendSpace(sw, 2 * indent);
            sw.Write(result);
            UICGTools.AppendNewLine(sw, 1);
        }

        public void OnBeginWriteEventBind(StreamWriter sw)
        {
            UICGTools.AppendNewLine(sw, 1);
        }

        public void OnWriteEventBind(StreamWriter sw, Node node, string fieldsName, string functionName)
        {
            string writeContent = string.Empty;
            switch (node.type)
            {
                case NodeType.Button:
                    UICGTools.AppendNewLine(sw, 1);
                    writeContent = string.Format("UIEventListener.Get({0}.gameObject).onClick = {1};", fieldsName, functionName);
                    break;
                default:
                    return;
            }
            
            UICGTools.AppendSpace(sw, 2 * indent);
            sw.Write(writeContent);
        }

        public void OnEndWriteInitFunction(StreamWriter sw)
        {
            UICGTools.AppendNewLine(sw, 1);
            UICGTools.AppendSpace(sw, indent);
            sw.Write("}");
            UICGTools.AppendNewLine(sw, 2);
        }

        public void OnWriteOtherFunction(StreamWriter sw)
        {
            UICGTools.AppendSpace(sw, indent);
            sw.Write("void OnEnable()");
            UICGTools.AppendNewLine(sw, 1, indent);
            sw.Write("{");
            UICGTools.AppendNewLine(sw, 1, 2 * indent);
            sw.Write("RegisterMsgCallBack();");
            UICGTools.AppendNewLine(sw, 1, indent);
            sw.Write("}");

            UICGTools.AppendNewLine(sw, 2, indent);
            sw.Write("void OnDisable()");
            UICGTools.AppendNewLine(sw, 1, indent);
            sw.Write("{");
            UICGTools.AppendNewLine(sw, 1, 2 * indent);
            sw.Write("UnRegisterMsgCallBack();");
            UICGTools.AppendNewLine(sw, 1, indent);
            sw.Write("}");

            UICGTools.AppendNewLine(sw, 2, indent);
            sw.Write("void RegisterMsgCallBack()");
            UICGTools.AppendNewLine(sw, 1, indent);
            sw.Write("{");
            UICGTools.AppendNewLine(sw, 2, indent);
            sw.Write("}");

            UICGTools.AppendNewLine(sw, 2, indent);
            sw.Write("void UnRegisterMsgCallBack()");
            UICGTools.AppendNewLine(sw, 1, indent);
            sw.Write("{");
            UICGTools.AppendNewLine(sw, 2, indent);
            sw.Write("}");
            sw.Flush();
        }

        public void OnWriteEventFunction(StreamWriter sw, Node node, string functionName)
        {
            string eventFunction = string.Empty;

            switch (node.type)
            {
                case NodeType.Button:
                    eventFunction = string.Format("void {0}(GameObject go)", functionName);
                    break;
                default:
                    break;
            }


            UICGTools.AppendNewLine(sw, 2, indent);
            sw.Write(eventFunction);
            UICGTools.AppendNewLine(sw, 1, indent);
            sw.Write("{");
            UICGTools.AppendNewLine(sw, 2, indent);
            sw.Write("}");
        }

        public string GetFunctionNameOfFindChid()
        {
            return "UITools.FindChildRecursive";
        }
    }
}
