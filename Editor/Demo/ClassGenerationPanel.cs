using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DrawerTools.CodeGeneration;
using UnityEngine;

namespace DrawerTools.Demo
{
    public class ClassGenerationPanel : DTPanel
    {
        private DTExpandToggle expand = new DTExpandToggle();
        private DTString _classNameDrawer;
        private DTString _namespaceDrawer;
        private DTList<FieldDrawer> _fieldDrawers;
        private DTString _preview = new DTString("");

        public ClassGenerationPanel(IDTPanel parent) : base(parent)
        {
            _classNameDrawer = new DTString("ClassName", "MyClass");
            _classNameDrawer.AddChangeListener(UpdatePreview);
            _namespaceDrawer = new DTString("Namespace", "");
            _namespaceDrawer.AddChangeListener(UpdatePreview);

            _fieldDrawers = new DTList<FieldDrawer>(this).SetName("Fields");
            _fieldDrawers.SetList(new List<FieldDrawer>(), CreateFieldDrawer);
            _fieldDrawers.OnListChange += UpdatePreview;

            _preview = new DTString("");
            _preview.IsTextArea = true;
            _preview.Disabled = true;
            _preview.SetHeight(200);

            UpdatePreview();
        }

        protected override void AtDraw()
        {
            using (DTScope.Box)
            {
                using (DTScope.Horizontal)
                {
                    expand.Draw();
                    DT.Label("Class Generation", font_style: FontStyle.Bold);
                }

                if (expand.Pressed)
                {
                    _classNameDrawer.Draw();
                    _namespaceDrawer.Draw();
                    _fieldDrawers.Draw();
                    _preview.Draw();
                }
            }
        }

        private FieldDrawer CreateFieldDrawer()
        {
            FieldDrawer fd = new FieldDrawer();
            fd.OnValueChange += sender => UpdatePreview();
            return fd;
        }

        private void UpdatePreview()
        {
            var cb = CreateCB();
            _preview.Value = cb.Build();
        }

        private ClassBuilder CreateCB()
        {
            ClassBuilder cb = new ClassBuilder(_classNameDrawer.Value);
            cb.SetNamespace(_namespaceDrawer.Value);
            foreach (var field in _fieldDrawers.ItemsList)
            {
                cb.AddField(field.GetBuilder(), addGetter: field.HasGetter);
            }

            return cb;
        }
    }

    public class FieldDrawer : DTDrawable
    {
        public event Action<FieldDrawer> OnValueChange;
        public bool HasGetter => addGetterField.Value;

        private DTEnum<MemberProtection> protDrawer;
        private DTString fieldType;
        private DTString fieldName;
        private DTBool addGetterField;

        private Type _typeToReturn = typeof(string);

        public FieldDrawer()
        {
            protDrawer = new DTEnum<MemberProtection>("");
            protDrawer.SetWidth(80);
            protDrawer.AddChangeListener(AtValueChange);

            fieldType = new DTString("", "string");
            fieldType.SetWidth(80);
            fieldType.AddStringChangeCallback(AtTypeFieldChange);

            fieldName = new DTString("", "_value");
            fieldName.AddChangeListener(AtValueChange);

            addGetterField = new DTBool("", false);
            addGetterField.SetWidth(20);
            addGetterField.AddChangeListener(AtValueChange);
        }

        protected override void AtDraw()
        {
            using (DTScope.Horizontal)
            {
                protDrawer.Draw();
                fieldType.Draw();
                fieldName.Draw();
                addGetterField.Draw();
            }
        }

        private void AtTypeFieldChange(string typeStr)
        {
            var newType = DTReflections.GetUnityType(typeStr);
            
            if (newType != null)
            {
                _typeToReturn = newType;
            }

            AtValueChange();
        }

        private void AtValueChange()
        {
            OnValueChange?.Invoke(this);
        }

        public FieldBuilder GetBuilder()
        {
            var result = new FieldBuilder(_typeToReturn, fieldName.Value).SetProtection(protDrawer.Value);
            return result;
        }
    }
}