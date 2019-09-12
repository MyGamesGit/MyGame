using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;



public enum AccessModifier
{
    PUBLIC,
    PRIVATE,
    PROTECTED,
    INTERNAL
}

namespace CustomTool
{
    public class ScriptBuildHelper
    {

        private StringBuilder _Context;
        private string _LineBrake = "\r\n";
        private int _CurrentIndex = 0;
        public int _IndentCount { get; set; }

        public ScriptBuildHelper()
        {
            _Context = new StringBuilder();
        }

        /// <summary>
        /// 写的方法
        /// </summary>
        /// <param name="context">内容</param>
        /// <param name="isIndent">是否需要缩进</param>
        public void Write(string context, bool isIndent = false)
        {
            if (isIndent)
            {
                context = SetIndent() + context;
            }
            if (_CurrentIndex == _Context.Length)
            {
                _Context.Append(context);
            }
            else
            {
                _Context.Insert(_CurrentIndex, context);
            }
            _CurrentIndex += context.Length;
        }
        /// <summary>
        /// 写入一行
        /// </summary>
        /// <param name="context"></param>
        public void WriteLine(string context, bool isIndent = false)
        {
            Write(context + _LineBrake, isIndent);
        }
        /// <summary>
        /// 设置缩进
        /// </summary>
        public string SetIndent()
        {
            string indent = "";
            for (int i = 0; i < _IndentCount; i++)
            {
                indent += "   ";
            }
            return indent;
        }

        /// <summary>
        /// 添加大括号
        /// </summary>
        public void WriteCurlyBrackets()
        {
            var start = SetIndent() + "{" + _LineBrake;
            var end = SetIndent() + "}" + _LineBrake;
            Write(start + end, true);
            _CurrentIndex -= end.Length;
        }
        /// <summary>
        /// 写入namespace
        /// </summary>
        /// <param name="nameSpaceName"></param>
        public void WriteNameSpace(string nameSpaceName)
        {
            Write("namespace " + nameSpaceName, false);
            WriteCurlyBrackets();
        }
        /// <summary>
        /// 写入类
        /// </summary>
        /// <param name="className"></param>
        public void WriteClass(string className)
        {
            _IndentCount = 1;
            Write(SetIndent() + "public class " + className + ": MonoBehaviour" + _LineBrake, true);
            WriteCurlyBrackets();
        }

        /// <summary>
        /// 写方法
        /// </summary>
        /// <param name="funName"></param>
        /// <param name="accessModifier"></param>
        /// <param name="returnType"></param>
        /// <param name="paramList"></param>
        public void WriteFunction(string funName, AccessModifier accessModifier = AccessModifier.PUBLIC, string returnType = "", params string[] paramList)
        {
            string classStr = "";
            _IndentCount = 2;
            classStr += SetIndent() + accessModifier.ToString().ToLower() + " ";

            if (returnType != "")
            {
                classStr += returnType + " ";
            }
            else
            {
                classStr += "void ";
            }
            classStr += funName + "(";
            if (paramList.Length != 0)
            {
                foreach (string item in paramList)
                {
                    if (item != paramList[paramList.Length - 1])
                    {
                        classStr += "string " + item + ",";
                    }
                    else
                    {
                        classStr += "string " + item;
                    }
                }
            }
            classStr += ")" + _LineBrake;
            Write(classStr, true);
            WriteCurlyBrackets();
        }


        public string GetCurrContext()
        {
            return _Context.ToString();
        }

        public void WriteUsing()
        {
            string usingStr = "using System.IO;" + _LineBrake;
            usingStr += "using UnityEngine;" + _LineBrake;
            usingStr += "using System.Collections;" + _LineBrake;
            usingStr += "using System.Collections.Generic; " + _LineBrake;
            Write(usingStr);
        }
    }
}
