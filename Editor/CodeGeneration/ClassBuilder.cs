using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DrawerTools.CodeGeneration
{
    public class ClassBuilder : IBuilder
    {
        public string NameSpace { get; set; }
        public string ClassName { get; set; }
        public List<string> Usages { get; set; } = new List<string>();
        public List<AttributeBuilder> Attributes { get; set; } = new List<AttributeBuilder>();
        public List<Type> DerrivedFrom { get; set; } = new List<Type>();
        public List<FieldBuilder> Fields { get; set; } = new List<FieldBuilder>();
        public List<GetterBuilder> Getters { get; set; } = new List<GetterBuilder>();

        public string Build()
        {
            var lines = new List<string>();
            foreach (var usage in Usages)
            {
                lines.Add($"using {usage};");
            }

            lines.Add("\n");

            string tabulations = "";
            bool hasNamespace = !string.IsNullOrEmpty(NameSpace);

            if (hasNamespace)
            {
                lines.Add($"namespace {NameSpace}");
                lines.Add("{");
                tabulations += "\t";
            }

            var classDefStr = $"{tabulations}public class {ClassName}";
            if (DerrivedFrom != null && DerrivedFrom.Count > 0)
            {
                classDefStr = classDefStr + " : " + string.Join(", ", DerrivedFrom.Select(x => x.Name));
            }

            lines.Add(classDefStr);
            lines.Add(tabulations + "{");

            tabulations += "\t";

            lines.AddRange(Fields.Select(field => tabulations + field.Build()));
            lines.Add("\n");
            lines.AddRange(Getters.Select(getter => tabulations + getter.Build()));
            DecreaseTbulations();
            lines.Add(tabulations + "}");
            if (hasNamespace)
            {
                DecreaseTbulations();
                lines.Add(tabulations + "}");
            }

            var result = string.Join("\n", lines);
            return result;

            void DecreaseTbulations() => tabulations = tabulations.Length > 1 ? tabulations.Substring(1) : "";
        }

        #region General

        public ClassBuilder(string className)
        {
            ClassName = className;
        }

        public ClassBuilder SetNamespace(string nameSpace)
        {
            NameSpace = nameSpace;
            return this;
        }

        #endregion

        #region Usages

        public ClassBuilder AddUsage(string usage)
        {
            Usages.Add(usage);
            return this;
        }

        public ClassBuilder AddUsage(IEnumerable<string> usage)
        {
            Usages.AddRange(usage);
            return this;
        }

        public ClassBuilder AddUsage(params string[] usage)
        {
            Usages.AddRange(usage);
            return this;
        }

        public ClassBuilder AddDefaultUsages(params string[] usage)
            => AddUsage("System", "System.Collections", "System.Collections.Generic", "UnityEngine");

        #endregion

        #region Attributes

        public ClassBuilder AddAttribute(AttributeBuilder atr)
        {
            Attributes.Add(atr);
            return this;
        }

        public ClassBuilder AddAttribute(IEnumerable<AttributeBuilder> atr)
        {
            Attributes.AddRange(atr);
            return this;
        }

        public ClassBuilder AddAttribute(params AttributeBuilder[] atr)
        {
            Attributes.AddRange(atr);
            return this;
        }

        #endregion

        #region Derrived

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
        

        #endregion

        #region Fields

        public ClassBuilder AddField(FieldBuilder fieldBuilder)
        {
            Fields.Add(fieldBuilder);
            return this;
        }

        public ClassBuilder AddField(string fieldName, Type fieldType,
            MemberProtection memberProtection = MemberProtection.Private,
            IEnumerable<AttributeBuilder> attributes = null, bool addGetter = false)
        {
            var fieldBuilder = new FieldBuilder(fieldType, fieldName).SetProtection(memberProtection)
                .AddAttribute(attributes);
            Fields.Add(fieldBuilder);
            if (addGetter)
            {
                var field = Fields.Last();
                Getters.Add(new GetterBuilder(field));
            }
            return this;
        }

        public ClassBuilder AddField(FieldBuilder fb, bool addGetter = false) =>
            AddField(fb.FieldName, fb.FieldType, fb.Protection, fb.Attributes, addGetter);


        public ClassBuilder AddPublicField(string fieldName, Type fieldType,
            IEnumerable<AttributeBuilder> attributes = null)
            => AddField(fieldName, fieldType, MemberProtection.Public, attributes);

        public ClassBuilder AddPrivateField(string fieldName, Type fieldType,
            IEnumerable<AttributeBuilder> attributes = null, bool addGetter = false)
        {
            AddField(fieldName, fieldType, MemberProtection.Private, attributes, addGetter);
            return this;
        }

        public ClassBuilder AddSerializedField(string fieldName, Type fieldType, bool addGetter = false)
            => AddPrivateField(fieldName, fieldType, new[] {AttributeBuilder.SerializeField}, addGetter);
        
        #endregion
        
        #region Shortcuts
        
        public static ClassBuilder CreateEmptyScriptableObject(string soName, string nameSpace)
        {
            var cb = new ClassBuilder(soName);
            cb.AddDefaultUsages();
            cb.SetNamespace(nameSpace);
            cb.AddDerring(typeof(ScriptableObject));
            
            return cb;
        }
        
        public static ClassBuilder CreateEmptySerializableClass(string className, string nameSpace)
        {
            var cb = new ClassBuilder(className);
            cb.SetNamespace(nameSpace);
            cb.AddAttribute(AttributeBuilder.Serializable);
            
            return cb;
        }
        
        #endregion
    }
}