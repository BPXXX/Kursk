using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace Skyunion
{
    public class ObjctPoolAutoDespawn : ObjectPoolItem
    {
        public float m_despawn_delay = 2f;

        private void OnSpawn()
        {
            base.StartCoroutine(this.AutoDespawn());
        }

        [DebuggerHidden]
        private IEnumerator AutoDespawn()
        {
            yield return new WaitForSeconds(m_despawn_delay);
            try
            {
                Disable();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
        }

        public override void Disable()
        {
            base.Disable();
        }
    }
}
