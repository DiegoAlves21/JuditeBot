using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public static class Utils
    {
        public static object GetProperty(object target, string name)
        {
            var site =
                CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(0, name, target.GetType(),
                    new[] { CSharpArgumentInfo.Create(0, null) }));
            return site.Target(site, target);
        }
    }
}
