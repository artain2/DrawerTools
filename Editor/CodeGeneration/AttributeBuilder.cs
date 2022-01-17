using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DrawerTools.CodeGeneration
{
    public class AttributeBuilder
    {
        public Type AttributeType { get; set; }
        public List<string> Parametres { get; set; } = new List<string>();

        public AttributeBuilder(Type attributeType)
        {
            AttributeType = attributeType;
        }

        public AttributeBuilder AddParametre(string param)
        {
            Parametres.Add(param);
            return this;
        }

        public AttributeBuilder AddParametre(IEnumerable<string> param)
        {
            Parametres.AddRange(param);
            return this;
        }

        public AttributeBuilder AddParametre(params string[] param)
        {
            Parametres.AddRange(param);
            return this;
        }

        public string Build()
        {
            var result = AttributeType.Name;
            if (Parametres.Count > 0)
            {
                var paramsStr = string.Join(", ", Parametres);
                result = $"{result}({paramsStr})";
            }

            return result;
        }

        public static string MergeAttributes(IEnumerable<AttributeBuilder> builders)
        {
            var joinStr = string.Join(", ", builders.Select(x => x.Build()));
            var result = $"[{joinStr}]";
            return result;
        }

        public static AttributeBuilder SerializeField => new AttributeBuilder(typeof(SerializeField));
       
        public static AttributeBuilder Serializable => new AttributeBuilder(typeof(SerializableAttribute));

        public static AttributeBuilder CreateAssetMenu(string file, string path) =>
            new AttributeBuilder(typeof(CreateAssetMenuAttribute)).AddParametre(file, path);
    }
}