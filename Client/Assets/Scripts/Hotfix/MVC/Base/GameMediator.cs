using System.Collections.Generic;
using System;
using PureMVC.Patterns.Mediator;
using UnityEngine;

namespace Game
{
    public class GameMediator : Mediator
    {

        public bool IsOpenUpdate = false;

        #region Constructors

        /// <summary>
        /// Constructs a new mediator with the default name and no view component
        /// </summary>
        public GameMediator()
                : this(NAME, null)
        {
        }

        /// <summary>
        /// Constructs a new mediator with the specified name and no view component
        /// </summary>
        /// <param name="mediatorName">The name of the mediator</param>
        public GameMediator(string mediatorName)
                : this(mediatorName, null)
        {
        }

        /// <summary>
        /// Constructs a new mediator with the specified name and view component
        /// </summary>
        /// <param name="mediatorName">The name of the mediator</param>
        /// <param name="viewComponent">The view component to be mediated</param>
        public GameMediator(string mediatorName, object viewComponent)
            : base(mediatorName,viewComponent)
        {
            MediatorName = (mediatorName != null) ? mediatorName : NAME;
            ViewComponent = viewComponent;
        }

        public override void OnRegister()
        {
            Debug.Log("OnRegister GameMediator:" + MediatorName);
            InitData();
            BindUIData();
            BindUIEvent();
        }

        public override void OnRemove()
        {
            Debug.Log("Remove GameMediator:" + MediatorName);
        }

        public virtual void Update()
        {
            
        }

        public virtual void FixedUpdate()
        {

        }

        public virtual void LateUpdate()
        {

        }


        protected virtual void InitData()
        {
            throw new NotImplementedException();
        }


        protected virtual void BindUIEvent()
        {
            throw new NotImplementedException();
        }

        protected virtual void BindUIData()
        {
            throw new NotImplementedException();
        }

        public virtual void OpenAniEnd(){}
        public virtual void WinFocus(){}

        public virtual void WinClose(){}

        public virtual void PrewarmComplete(){}
        #endregion
    }
}