using System;
using UnityEngine;

namespace Skyunion
{
    // 这个是用在跟着摄像机走的 暂时用不上但是不删掉
    public class AudioListener : MonoBehaviour
    {
        public AnimationCurve m_curve;

        private float predis;

        private Vector3 viewcenter3d;

        private void Awake()
        {
        }

        private void Start()
        {
        }

        private void Update()
        {
            //float cameraDist = Common.GetCameraDist();
            //if (Common.GetCameraDist() > 0.01f && CSWorldCamera.camera != null && Mathf.Abs(this.predis - cameraDist) > 3f)
            //{
            //    Vector2 viewCenter = WorldCameraImpl.GetInstance().GetViewCenter();
            //    if (viewCenter.x != WorldCameraImpl.INVALID_FLOAT_VALUE || viewCenter.y != WorldCameraImpl.INVALID_FLOAT_VALUE)
            //    {
            //        Vector3 position = CSWorldCamera.camera.transform.position;
            //        this.viewcenter3d.x = viewCenter.x;
            //        this.viewcenter3d.y = 0f;
            //        this.viewcenter3d.z = viewCenter.y;
            //        float d = this.m_curve.Evaluate(cameraDist);
            //        base.transform.position = (position - this.viewcenter3d).normalized * d + this.viewcenter3d;
            //    }
            //    this.predis = cameraDist;
            //}
        }

        private void LateUpdate()
        {
        }

        private void OnPostRender()
        {
        }

        private void OnDestroy()
        {
        }
    }
}