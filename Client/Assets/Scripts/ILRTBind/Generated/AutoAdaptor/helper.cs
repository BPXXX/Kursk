
using System;

namespace Skyunion
{
    class ILRuntimeHelper
    {
        public static void Init(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            if (app == null)
            {
                // should log error
                return;
            }

			// adaptor register 
                        
			app.RegisterCrossBindingAdaptor(new Sproto.ProtocolBaseAdaptor());            
			app.RegisterCrossBindingAdaptor(new Sproto.SprotoTypeBaseAdaptor());            
			app.RegisterCrossBindingAdaptor(new Skyunion.BehaviourBinderAdaptor());            
			app.RegisterCrossBindingAdaptor(new Skyunion.GameViewAdaptor());            
			app.RegisterCrossBindingAdaptor(new Skyunion.MonoLikeEntityAdaptor());            
			app.RegisterCrossBindingAdaptor(new Skyunion.ViewBinderAdaptor());   

			// delegate register 
						
			app.DelegateManager.RegisterFunctionDelegate<PureMVC.Interfaces.IFacade>();
			
			app.DelegateManager.RegisterMethodDelegate<PureMVC.Interfaces.IFacade>();
			
			app.DelegateManager.RegisterMethodDelegate<Skyunion.NetEvent>();
			
			app.DelegateManager.RegisterMethodDelegate<System.IO.MemoryStream>();
			
			app.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject>();
			
			app.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject>();
			
			app.DelegateManager.RegisterMethodDelegate<Client.ListView.ListItem>();
			
			app.DelegateManager.RegisterMethodDelegate<System.String>();
			
			app.DelegateManager.RegisterMethodDelegate<Client.ScrollView.ScrollItem>();
			
			app.DelegateManager.RegisterMethodDelegate<System.Single,System.Single,System.Single>();
			
			app.DelegateManager.RegisterFunctionDelegate<PureMVC.Interfaces.IController>();
			
			app.DelegateManager.RegisterFunctionDelegate<PureMVC.Interfaces.IModel>();
			
			app.DelegateManager.RegisterFunctionDelegate<PureMVC.Interfaces.IView>();


			// delegate convertor
            
        }
    }
}