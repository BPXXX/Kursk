using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skyunion;

namespace Client
{
    public class AutoPlayAndDestroyTip : MonoBehaviour
    {
        private Animation ani;
        public AnimationClip m_showAni;
        public AnimationClip m_closeAni;

        private void Start()
        {
            this.ani = base.gameObject.GetComponent<Animation>();
            if (this.ani == null)
            {
                this.ani = base.gameObject.AddComponent<Animation>();
            }

            if(m_showAni!=null)
            {
                this.ani.AddClip(this.m_showAni, "Show");
                this.ani.Play("Show");
            }
            if (this.m_closeAni != null)
            {
                this.ani.AddClip(this.m_closeAni, "Close");
            }

        }

        public void PlayEndAni()
        {
            if(this.ani != null&&this.m_closeAni != null)
            {
                ani.Play("Close");
            }
            isPlayingClose = true;
        }

        private bool isPlayingClose = false;
        public void LateUpdate()
        {
            if(isPlayingClose)
            {
                if(this.ani == null||!this.ani.isPlaying)
                {
                    GameObject.DestroyImmediate(this.gameObject);
                }
            }
        }
    }
}

