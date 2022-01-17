using System.Collections.Generic;

namespace DrawerTools.CodeGeneration
{
    public class EnumBuilder : IBuilder
    {
        public string NameSpace { get; set; }
        public string EnumName { get; set; }
        public List<string> Values { get; set; }
        
        
        public EnumBuilder(string className)
        {
            EnumName = className;
        }

        public EnumBuilder SetNamespace(string nameSpace)
        {
            NameSpace = nameSpace;
            return this;
        }

        public EnumBuilder AddValue(string value)
        {
            Values.Add(value);
            return this;
        }

        public EnumBuilder AddValue(IEnumerable<string> values)
        {
            Values.AddRange(values);
            return this;
        }

        public EnumBuilder AddValue(params string[] values)
        {
            Values.AddRange(values);
            return this;
        }

        public string Build()
        {
            var lines = new List<string>();
            lines.Add($"public enum {EnumName}");
            lines.Add("{");
            foreach (var value in Values)
            {
                lines.Add($"\t{value},");
            }

            lines.Add("}");

            if (!string.IsNullOrEmpty(NameSpace))
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    lines[i] = "\t" + lines[i];
                }

                lines.Insert(0, "{");
                lines.Insert(0, $"namespace {NameSpace}");
                lines.Add("}");
            }

            var result = string.Join("\n", lines);
            return result;
        }
    }
}