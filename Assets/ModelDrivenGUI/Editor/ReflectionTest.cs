using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using System.Linq;

namespace ModelDrivenGUISystem {

    public class ReflectionTest {

        [Test]
        public void ReflectionTestSimplePasses() {
            var buf = new StringBuilder();

            foreach (var f in IterateFields(typeof(Data))) {
                var t = f.FieldType;
                buf.AppendFormat("{0}\t{1}", f.FieldType.Name, f.Name);
                buf.AppendFormat("\tprimitive={0},valueType={1},class={2},array={3}",
                    t.IsPrimitive, t.IsValueType, t.IsClass, t.IsArray);
                buf.AppendFormat("\tAttributes:{0}",
                    string.Join(", ",
                        f.GetCustomAttributes(true).Select(o => o.ToString()).ToArray())
                    );
                buf.AppendLine();
            }

            Debug.Log(buf.ToString());
        }

        public static IEnumerable<FieldInfo> IterateFields(System.Type t) {
            return t.GetFields(
                BindingFlags.Instance
                | BindingFlags.Public
                | BindingFlags.NonPublic
                | BindingFlags.FlattenHierarchy);
        }

        public struct SomeStruct {
            public int intData;
        }

        [System.Serializable]
        public class Data {
            public List<float> floatList;
            public float[] floatArray;
            public SomeStruct someStruct;

            [SerializeField]
            protected int serializeFieldInt;
        }
    }
}
