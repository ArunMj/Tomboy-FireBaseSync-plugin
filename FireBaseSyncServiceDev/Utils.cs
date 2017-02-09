using System;
using System.Collections;
using System.Collections.Generic;

namespace Tomboy.FirebaseAddin
{
    public static class  Utils
    {
        public static string str(Object x){
            String r = "Nothing";
           
            if (x is IList)
            {
                r = "[";
                foreach (Object i in (IList)x)
                {
                    r += i.ToString() + ", ";
                }
                r = r.Trim().TrimEnd(',') + "]";
            }
            else if( x is IDictionary){
                r = "{";
                foreach(KeyValuePair<Object, Object> item in (IDictionary)x){
                    r += (item.Key.ToString() + "=>" + item.Value.ToString() + ", ");
                }
                r = r.Trim().TrimEnd(',') + "}";
            }
            return r;
        }

        public static string str(Object x,Func<Object, Object> lambda){
            String r = "Nothing";
            if (x is IList)
            {
                r = "[";
                foreach (Object i in (IList)x)
                {
                    r += lambda(i).ToString() + ", ";
                }
                r = r.Trim().TrimEnd(',') + "]";
            }
            else if( x is IDictionary){
                r = "{";
                foreach(KeyValuePair<Object, Object> item in (IDictionary)x){
                    r += (item.Key.ToString() + "=>" + item.Value.ToString() + ", ");
                }
                r = r.Trim().TrimEnd(',') + "}";
            }
            return r;
        }

    }
}

