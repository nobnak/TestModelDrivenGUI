using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ModelDrivenGUISystem {

    public class ModelDrivenGUI<T> : System.IDisposable {
        public enum DataDivisionEnum {
            Primitive,
            ValueType,
            Class
        }
        public enum DataSectionEnum {
            Primitive_Bool,
            Primitive_Int,
            Primitive_Float,
            Primitive_Other,
            ValueType_Struct,
            ValueType_Enum,
            ValueType_Vector,
            Class_UserDefined,
            Class_IList
        }

        protected T target;

        public ModelDrivenGUI(T target) {
            this.target = target;
        }

        #region IDisposable
        public virtual void Dispose() {}
        #endregion
        
        protected virtual string Dump<S>(S v, int depth) {
            return null;
        }

        public static IEnumerable<FieldInfo> IterateFIelds<S>(S s) {
            return s.GetType().GetFields(BindingFlags.Instance
                | BindingFlags.Public
                | BindingFlags.NonPublic
                | BindingFlags.FlattenHierarchy);
        }
        public static void Division(System.Type t, out DataDivisionEnum div, DataSectionEnum sec) {
            div = t.IsPrimitive ? DataDivisionEnum.Primitive
                : (t.IsValueType ? DataDivisionEnum.ValueType : DataDivisionEnum.Class);

            switch (div) {
                case DataDivisionEnum.Class:
                    if (typeof(IList).IsAssignableFrom(t))
                        sec = DataSectionEnum.Class_IList;
                    else
                        sec = DataSectionEnum.Class_UserDefined;
                    break;

                case DataDivisionEnum.ValueType:
                    sec = t.IsEnum ? DataSectionEnum.ValueType_Enum : DataSectionEnum.ValueType_Struct;
                    break;

                default:
                    if (t == typeof(int))
                        sec = DataSectionEnum.Primitive_Int;
                    else if (t == typeof(float))
                        sec = DataSectionEnum.Primitive_Float;
                    else if (t == typeof(bool))
                        sec = DataSectionEnum.Primitive_Bool;
                    else
                        sec = DataSectionEnum.Primitive_Other;

                    break;
            }
        }
    }
}
