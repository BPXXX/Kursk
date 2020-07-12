using Skyunion;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class MakeChildrenGray : MonoBehaviour
    {
        private Material gray_material;
        private bool m_isLoadGrayMaterial;

        private Image[] allChildren;
        private SpriteRenderer spriteRenderer;
        private Material defaultMaterial;

        private bool IsNormal = false;

        private void Awake()
        {
            LoadGrayMaterial(null);
            this.allChildren = base.GetComponentsInChildren<Image>(true);
            this.spriteRenderer = GetComponentInChildren<SpriteRenderer>(true);
            if (this.spriteRenderer != null)
            {
                defaultMaterial = spriteRenderer.material;
            }
        }

        private void LoadGrayMaterial(Action action)
        {
            CoreUtils.assetService.LoadAssetAsync<Material>("UI_Gray", (IAsset asset) =>
            {
                this.gray_material = asset.asset() as Material;
                if (this.gray_material == null)
                {
                    Debug.LogWarning("UI_Gray Load Fail");
                }
                m_isLoadGrayMaterial = true;
                action?.Invoke();
            });
        }

        public void Gray()
        {
            IsNormal = false;
            if (this.allChildren == null)
            {
                return;
            }
            if (m_isLoadGrayMaterial == false)
            {
                LoadGrayMaterial(() =>
                {
                    if (IsNormal == false)
                    {
                        Gray();
                    }
                });
                return;
            }
            if (gray_material != null)
            {
                Image[] array = this.allChildren;
                for (int i = 0; i < array.Length; i++)
                {
                    Image image = array[i];
                    image.material = this.gray_material;
                }
            }
        }

        public void Normal()
        {
            IsNormal = true;
            if (this.allChildren == null)
            {
                return;
            }
            Image[] array = this.allChildren;
            for (int i = 0; i < array.Length; i++)
            {
                Image image = array[i];
                image.material = image.defaultMaterial;
            }
        }
        public void GraySpriteRender()
        {
            IsNormal = false;
            if (this.spriteRenderer == null)
            {
                return;
            }
            if (m_isLoadGrayMaterial == false)
            {
                LoadGrayMaterial(() =>
                {
                    if (IsNormal == false)
                    {
                        GraySpriteRender();
                    }
                });
                return;
            }
            if (this.gray_material != null)
            {
                spriteRenderer.material = this.gray_material;
            }
        }
    

    public void NormalSpriteRender()
        {
            IsNormal = true;
            if (this.spriteRenderer == null)
            {
                return;
            }
            if (defaultMaterial != null)
            {
                spriteRenderer.material = defaultMaterial;
            }
        }
    }
}