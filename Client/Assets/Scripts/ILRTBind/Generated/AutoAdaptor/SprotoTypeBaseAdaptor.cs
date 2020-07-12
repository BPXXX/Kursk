using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using UnityEngine;

namespace Sproto
{   
    public class SprotoTypeBaseAdaptor : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(SprotoTypeBase);
            }
        }

        public override Type AdaptorType
        {
            get
            {
                return typeof(Adaptor);
            }
        }

        public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            return new Adaptor(appdomain, instance);
        }

		internal class Adaptor : SprotoTypeBase, CrossBindingAdaptorType
        {
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            public Adaptor()
            {

            }

            public Adaptor(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
            {
                this.appdomain = appdomain;
                this.instance = instance;
            }

            public ILTypeInstance ILInstance { get { return instance; } }

            
            IMethod mencode;
            public override Int32 encode(SprotoStream stream)
            {
                if(mencode == null)
                {
                    mencode = instance.Type.GetMethod("encode", 1);
                }

                if (mencode != null)
                {
					return (Int32)appdomain.Invoke(mencode, instance ,stream);
                }
                
                return 0;
            }



            IMethod mdecode;
            protected override void decode()
            {
                if(mdecode == null)
                {
                    mdecode = instance.Type.GetMethod("decode", 0);
                }
                if (mdecode != null)
                    appdomain.Invoke(mdecode, instance );
            }

            IMethod mTag;
            public override Int32 Tag()
            {
                if(mTag == null)
                {
                    mTag = instance.Type.GetMethod("Tag", 0);
                }

                if (mTag != null)
                {
					return (Int32)appdomain.Invoke(mTag, instance );
                }
                
                return 0;
            }



            
            
            public override string ToString()
            {
                IMethod m = appdomain.ObjectType.GetMethod("ToString", 0);
                m = instance.Type.GetVirtualMethod(m);
                if (m == null || m is ILMethod)
                {
                    return instance.ToString();
                }
                else
                    return instance.Type.FullName;
            }
        }
    }

	
}