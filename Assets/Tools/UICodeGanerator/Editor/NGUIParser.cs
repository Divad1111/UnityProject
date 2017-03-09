//#define NGUI

#if NGUI
using UnityEngine;
using System.Collections.Generic;

namespace UICodeGenerator
{
    class NGUIParser : Parser
    {
        public NGUIParser(GameObject root)
            :base(root)
        {

        }

        protected override bool IsPanel(GameObject go)
        {
            string goName = go.name.ToLower();
        
            if (goName.Contains("panel"))
            {
                return true;
            }
            return false;
        }

        protected override bool IsEmptyObject(GameObject go)
        {
            return false;
        }

        protected override bool IsTemplate(GameObject go)
        {
            string goName = go.name.ToLower();

            if (goName.Contains("template"))
            {   
                return true;
            }
            return false;
        }

        protected override bool IsLabel(GameObject go)
        {
            string goName = go.name.ToLower();

            if (goName.Contains("label") &&
                goName.Contains("(") &&
                goName.Contains(")") &&
                go.GetComponent<UILabel>() != null)
            {
                return true;
            }
            return false;
        }

        protected override bool IsTextBox(GameObject go)
        {
            if (go.GetComponent<UIInput>() != null)
            {
                return true;
            }
            return false;
        }

        protected override bool IsSprite(GameObject go)
        {
            string goName = go.name.ToLower();

            if (goName.Contains("sprite") &&
                goName.Contains("(") &&
                goName.Contains(")") &&
                go.GetComponent<UISprite>() != null)
            {
                return true;
            }
            return false;
        }

        protected override bool IsTexture(GameObject go)
        {
            string goName = go.name.ToLower();
            if (goName.Contains("texture") &&
               goName.Contains("(") &&
               goName.Contains(")") &&
               go.GetComponent<UITexture>() != null)
            {
                return true;
            }
            return false;
        }

        protected override bool IsButton(GameObject go)
        {
            string goName = go.name.ToLower();

            if (goName.Contains("button") &&
                goName.Contains("(") &&
                goName.Contains(")") )
            {
                return true;
            }
            return false;
        }

        protected override bool IsSlider(GameObject go)
        {
            string goName = go.name.ToLower();

            if (goName.Contains("slider") &&
                goName.Contains("(") &&
                goName.Contains(")") &&
                go.GetComponent<UISlider>() != null)
            {
                return true;
            }
            return false;
        }

        protected override bool IsCheckBox(GameObject go)
        {
            string goName = go.name.ToLower();

            if (goName.Contains("checkbox") &&
                goName.Contains("(") &&
                goName.Contains(")") &&
                go.GetComponent<UIToggle>() != null)
            {
                return true;
            }
            return false;
        }

        protected override bool IsGrid(GameObject go)
        {
            string goName = go.name.ToLower();

            if (go.GetComponent<UIGrid>() != null &&
                goName.Contains("(") &&
                goName.Contains(")"))
            {
                return true;
            }
            return false;
        }
        protected override bool IsTable(GameObject go)
        {
            string goName = go.name.ToLower();

            if (go.GetComponent<UITable>() != null &&
                goName.Contains("(") &&
                goName.Contains(")"))
            {
                return true;
            }
            return false;
        }

        protected override bool IsGroup(GameObject go)
        {
            string goName = go.name.ToLower();

            if (goName.Contains("group") &&
                goName.Contains("(") &&
                goName.Contains(")"))
            {
                return true;    
            }
            return false;
        }
    }
}
#endif