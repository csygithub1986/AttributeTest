using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributeTest.Ex
{
    public static class DictionaryEx
    {
        public static void AddOrUpdate<K, V>(this Dictionary<K, V> dic, K key, V value)
        {
            if (key == null || value == null) return;
            if (dic.ContainsKey(key))
            {
                dic.Remove(key);
            }
            dic.Add(key, value);
        }

        public static void AddOrUpdateList<K, V_List, V>(this Dictionary<K, V_List> dic, K key, V value)
        {
            if (key == null || value == null) return;
            Type t = typeof(V_List);
            //如果V_List不是IList集合类型
            if (!typeof(IList).IsAssignableFrom(t)) return;
            if (t.GenericTypeArguments.Length != 1) return;
            //value不是V_List的泛型类型
            if (t.GenericTypeArguments[0] != value.GetType()) return;
            if (dic.ContainsKey(key))
            {
                ((IList)dic[key]).Add(value);
            }
            else
            {
                var list = (IList)t.Assembly.CreateInstance(t.FullName);
                list.Add(value);
                dic.Add(key, (V_List)list);
            }
        }

        public static void AddOrUpdateListRange<K, V_List>(this Dictionary<K, V_List> dic, K key, IList value)
        {
            if (key == null || value == null) return;
            Type t = typeof(V_List);
            Type tv = value.GetType();
            //如果V_List不是IList集合类型
            if (!typeof(IList).IsAssignableFrom(t)) return;
            if (t.GenericTypeArguments.Length != 1 || tv.GenericTypeArguments.Length != 1) return;
            //两个泛型集合的泛型不一致
            if (t.GenericTypeArguments[0] != tv.GenericTypeArguments[0]) return;

            if (dic.ContainsKey(key))
            {
                foreach (var item in value)
                {
                    ((IList)dic[key]).Add(item);
                }
            }
            else
            {
                IList list = (IList)t.Assembly.CreateInstance(t.FullName);
                foreach (var item in value)
                {
                    list.Add(item);
                }
                dic.Add(key, (V_List)list);
            }
        }

    }
}
