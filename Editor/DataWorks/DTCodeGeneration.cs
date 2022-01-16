using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Compilation;

namespace DrawerTools
{
    public static class DTCodeGeneration
    {
        public static void CreateEnum(string path, string nameSpace, string enumName, string[] values)
        {
            List<string> lines = new List<string>();
            lines.Add($"public enum {enumName}");
            lines.Add("{");
            foreach (var value in values)
            {
                lines.Add($"\t{value},");
            }
            lines.Add("}");

            if (!string.IsNullOrEmpty(nameSpace))
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    lines[i] = "\t" + lines[i];
                }
                lines.Insert(0, "{");
                lines.Insert(0, $"namespace {nameSpace}");
                lines.Add("}");
            }

            string result = string.Join("\n", lines);

            using (var sw = (File.Exists(path)) ? new StreamWriter(path, false) : File.CreateText(path))
            {
                sw.Write(result);
            }

            CompilationPipeline.RequestScriptCompilation();
        }

        public static void CreateClass(string path, string nameSpace, string className, string[] derrivedFrom)
        {

        }

        public class ClassBuilder
        {
            public string NameSpace { get; set; }
            public string ClassName { get; set; }
            public List<Type> DerrivedFrom { get; set; } = new List<Type>();
            public List<FieldBuilder> Fields { get; set; } = new List<FieldBuilder>();

            public ClassBuilder(string className)
            {
                ClassName = className;
            }

            public ClassBuilder SetNamespace(string nameSpace)
            {
                NameSpace = nameSpace;
                return this;
            }

            public ClassBuilder AddDerrived(Type derrivedFrom)
            {
                DerrivedFrom.Add(derrivedFrom);
                return this;
            }

            public ClassBuilder AddDerring(IEnumerable<Type> derrivedFrom)
            {
                DerrivedFrom.AddRange(derrivedFrom);
                return this;
            }

            public ClassBuilder AddDerring(params Type[] derrivedFrom)
            {
                DerrivedFrom.AddRange(derrivedFrom);
                return this;
            }

            public ClassBuilder AddField(FieldBuilder fieldBuilder)
            {
                Fields.Add(fieldBuilder);
                return this;
            }
            public ClassBuilder AddField(string fieldName, Type fieldType, MemberProtection memberProtection = MemberProtection.Private, IEnumerable<string> attributes = null)
            {
                var fieldBuilder = new FieldBuilder(fieldType, fieldName).SetProtection(memberProtection).AddAttribute(attributes);
                Fields.Add(fieldBuilder);
                return this;
            }


            public ClassBuilder AddPublicField(FieldBuilder fieldBuilder)
            {
                return this;
            }
        }

        public class FieldBuilder
        {
            public MemberProtection Protection { get; set; } = MemberProtection.Private;
            public Type FieldType { get; set; }
            public string FieldName { get; set; }
            public List<string> Attributes { get; set; } = new List<string>();

            public FieldBuilder(Type fieldType, string fieldName)
            {
                FieldType = fieldType;
                FieldName = fieldName;
            }

            public FieldBuilder AddAttribute(string attribute)
            {
                Attributes.Add(attribute);
                return this;
            }

            public FieldBuilder AddAttribute(IEnumerable<string> attribute)
            {
                if (attribute == null)
                {
                    return this;
                }
                Attributes.AddRange(attribute);
                return this;
            }

            public FieldBuilder AddAttribute(params string[] attribute)
            {
                Attributes.AddRange(attribute);
                return this;
            }

            public FieldBuilder SetProtection(MemberProtection prot)
            {
                Protection = prot;
                return this;
            }


        }

        public enum MemberProtection
        {
            Private,
            Protected,
            Public,
        }

    }
}