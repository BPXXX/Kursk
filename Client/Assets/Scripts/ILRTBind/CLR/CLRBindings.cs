using System;
using System.Collections.Generic;
using System.Reflection;
using ILRuntime.Runtime.Generated;

namespace ILRTBind
{
    public class CLRBindings
    {
        /// <summary>
        /// Initialize the CLR binding, please invoke this AFTER CLR Redirection registration
        /// </summary>
        public static void Initialize(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            IDataServiceBinding.Register(app);
            ITableBinding.Register(app);
            SQLiteConnectionBinding.Register(app);
        }

        /// <summary>
        /// Release the CLR binding, please invoke this BEFORE ILRuntime Appdomain destroy
        /// </summary>
        public static void Shutdown(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
        }
    }
}
