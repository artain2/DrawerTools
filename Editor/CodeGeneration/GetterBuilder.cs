namespace DrawerTools.CodeGeneration
{
    public class GetterBuilder
    {
        public FieldBuilder TargetField { get; set; }
        public string CustomName { get; set; }

        public GetterBuilder(FieldBuilder targetField)
        {
            TargetField = targetField;
            CustomName = targetField.FieldName.TrimStart('_');
            CustomName = char.ToUpper(CustomName[0]) + CustomName.Substring(1);
        }

        public GetterBuilder SetCustomName(string customName)
        {
            CustomName = customName;
            return this;
        }

        public string Build(int tabs = 0)
        {
            var result = "";
            for (var i = 0; i < tabs; i++)
            {
                result += "\t";
            }

            var strType = TargetField.FieldType.Name;
            result += $"public {strType} {CustomName} => {TargetField.FieldName};";
            return result;
        }
    }

}