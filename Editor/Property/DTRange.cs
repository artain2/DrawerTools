using System;
using UnityEditor;
using UnityEngine;

namespace DrawerTools
{
    public class DTRange : DTDrawable
    {
        public event Action<float, float> OnValueChange;

        private float _min;
        private float _max;
        public float MinTreshold { get; private set; }
        public float MaxTreshold { get; private set; }
        public float MinValue => _min;
        public float MaxValue => _max;

        public DTRange(float maxTh) : this(0f, maxTh)
        {
        }

        public DTRange(float minTh, float maxTh) : this(minTh, maxTh, minTh, maxTh)
        {
        }

        public DTRange(float minTh, float maxTh, float minValue, float maxValue) : base()
        {
            SetTreshold(minTh, maxTh);
            SetValues(minValue, maxValue);
        }

        public DTRange AddCallback(Action<float, float> callback)
        {
            OnValueChange += callback;
            return this;
        }

        public DTRange SetMinTreshold(float value, bool invokeCallback = false)
        {
            MinTreshold = value;
            if (MinValue < MinTreshold)
                _min = MinTreshold;

            if (invokeCallback)
                OnValueChange?.Invoke(MinValue, MaxValue);
            return this;
        }

        public DTRange SetMaxTreshold(float value, bool invokeCallback = false)
        {
            MaxTreshold = value;
            if (MaxTreshold > MinTreshold)
                _max = MaxTreshold;

            if (invokeCallback)
                OnValueChange?.Invoke(MinValue, MaxValue);
            return this;
        }

        public DTRange SetTreshold(float min, float max, bool invokeCallback = false)
        {
            SetMinTreshold(min, false);
            SetMaxTreshold(max, false);
            if (invokeCallback)
                OnValueChange?.Invoke(MinValue, MaxValue);
            return this;
        }

        public DTRange SetMin(float value, bool invokeCallback = false)
        {
            _min = Mathf.Max(value, MinTreshold);

            if (invokeCallback)
                OnValueChange?.Invoke(MinValue, MaxValue);
            return this;
        }

        public DTRange SetMax(float value, bool invokeCallback = false)
        {
            _max = Mathf.Min(value, MaxTreshold);

            if (invokeCallback)
                OnValueChange?.Invoke(MinValue, MaxValue);
            return this;
        }

        public DTRange SetValues(float min, float max, bool invokeCallback = false)
        {
            SetMin(min, false);
            SetMax(max, false);
            if (invokeCallback)
                OnValueChange?.Invoke(MinValue, MaxValue);
            return this;
        }

        protected override void AtDraw()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.MinMaxSlider(ref _min, ref _max, 
                MinTreshold, MaxTreshold, Sizer.Options);
            if (EditorGUI.EndChangeCheck())
                OnValueChange?.Invoke(_min, _max);
        }
    }
}