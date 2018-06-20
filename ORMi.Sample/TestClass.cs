using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMi.Sample
{
    public class TestClass
    {
        public void DoSomething()
        {
            var mth = new StackTrace().GetFrame(1).GetMethod();
            var cls = mth.ReflectedType.Name;
            var cls2 = mth.Name;

            Type t = mth.ReflectedType;

            DoSomethingElse();
        }

        public void DoSomethingElse()
        {
            var mth = new StackTrace().GetFrame(1).GetMethod();
            var cls = mth.ReflectedType.Name;

            Type t = mth.ReflectedType;
        }
    }
}
