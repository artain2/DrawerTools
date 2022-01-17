using System;
using System.Collections.Generic;

namespace DrawerTools.CodeGeneration
{
    public class FieldBuilder
    {
        public MemberProtection Protection { get; set; } = MemberProtection.Private;
        public Type FieldType { get; set; }
        public string FieldName { get; set; }
        public List<AttributeBuilder> Attributes { get; set; } = new List<AttributeBuilder>();

        public FieldBuilder(Type fieldType, string fieldName)
        {
            FieldType = fieldType;
            FieldName = fieldName;
        }

        public FieldBuilder AddAttribute(AttributeBuilder attribute)
        {
            Attributes.Add(attribute);
            return this;
        }

        public FieldBuilder AddAttribute(IEnumerable<AttributeBuilder> attribute)
        {
            if (attribute == null)
            {
                return this;
            }

            Attributes.AddRange(attribute);
            return this;
        }

        public FieldBuilder AddAttribute(params AttributeBuilder[] attribute)
        {
            Attributes.AddRange(attribute);
            return this;
        }

        public FieldBuilder SetProtection(MemberProtection prot)
        {
            Protection = prot;
            return this;
        }

        public string Build(int tabs = 0)
        {
            string result = "";
            for (int i = 0; i < tabs; i++)
            {
                result += "\t";
            }

            if (Attributes.Count > 0)
            {
                result += AttributeBuilder.MergeAttributes(Attributes) + " ";
            }

            var strProt = Protection.ToString().ToLower();
            var strType = FieldType.Name;
            var strName = FieldName;
            result += $"{strProt} {strType} {strName};";
            return result;
        }
    }
}