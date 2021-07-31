using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace RanterTools.UI
{
    [Serializable]
    public class ComboboxOption<T>
    {
        static List<ComboboxOption<T>> all = new List<ComboboxOption<T>>();
        
        int id;
        public int ID { get { return id; } }
        public string Name;
        public T Meta;

        public ComboboxOption()
        {
            this.Name = "";
            this.Meta = default(T);
            all.RemoveAll((o) => o == null);

            all.Add(this);
            id = all.Count;
#if UNITY_EDITOR

#endif

        }
        public ComboboxOption(string Name, T Meta = default(T))
        {
            this.Name = Name;
            this.Meta = Meta;

            all.RemoveAll((o) => o == null);
            all.Add(this);
            id = all.Max((o) => o.ID) + 1;
        }
        
        public static implicit operator string(ComboboxOption<T> option)
        {
            return option.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(ComboboxOption<T>))
            {
                if ((obj as ComboboxOption<T>).ID == ID) return true;
                else return false;
            }
            else return false;
        }
        public override int GetHashCode()
        {
            return ID;
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Name)) return Name;
            else return Meta.ToString();
        }

        public void Remove()
        {
            all.Remove(this);
        }
    }

    [Serializable]
    public class ComboBoxOptionString : ComboboxOption<string>
    {
        public ComboBoxOptionString() : base() { }
        public ComboBoxOptionString(string Name, string Data) : base(Name, Data) { }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}