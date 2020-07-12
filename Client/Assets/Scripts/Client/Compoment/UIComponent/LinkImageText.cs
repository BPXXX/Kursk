using Skyunion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client
{
    public class LinkImageText : LanguageText, IPointerClickHandler, IEventSystemHandler
    {
        [Serializable]
        public class HrefClickEvent : UnityEvent<string>
        {
        }

        public class HrefDrawEvent : UnityEvent<List<Rect>, string>
        {
        }

        private class HrefInfo
        {
            public int startIndex;

            public int endIndex;

            public string name;

            public readonly List<Rect> boxes = new List<Rect>();
        }

        private string m_OutputText;

        private bool redrawUnderLine;

        protected readonly List<Image> m_ImagesPool = new List<Image>();

        private readonly List<int> m_ImagesVertexIndex = new List<int>();

        private readonly List<LinkImageText.HrefInfo> m_HrefInfos = new List<LinkImageText.HrefInfo>();

        protected static readonly StringBuilder s_TextBuilder = new StringBuilder();

        private static Dictionary<string, string> EmojiDatas = new Dictionary<string, string>();

        [SerializeField]
        private LinkImageText.HrefClickEvent m_OnHrefClick = new LinkImageText.HrefClickEvent();

        private LinkImageText.HrefDrawEvent m_OnHrefDraw = new LinkImageText.HrefDrawEvent();

        private static readonly Regex s_ImageRegex = new Regex("<quad name=(.+?) size=(\\d*\\.?\\d+%?) width=(\\d*\\.?\\d+%?) />", RegexOptions.Singleline);

        private static readonly Regex s_HrefRegex = new Regex("<href=([^>\\n\\s]+)>(.*?)(</href>)", RegexOptions.Singleline);

        private static readonly Regex s_ColorRegex = new Regex("<href=([^>\\n\\s]+)><color=(.*?)>", RegexOptions.Singleline);

        private string emojiTextTemplate = "<quad name={0} size={1} width=1 />";

        public static Func<string, Sprite> funLoadSprite;

        public LinkImageText.HrefClickEvent onHrefClick
        {
            get
            {
                return this.m_OnHrefClick;
            }
            set
            {
                this.m_OnHrefClick = value;
            }
        }

        public LinkImageText.HrefDrawEvent onHrefDraw
        {
            get
            {
                return this.m_OnHrefDraw;
            }
            set
            {
                this.m_OnHrefDraw = value;
            }
        }

        public override float preferredWidth
        {
            get
            {
                TextGenerationSettings generationSettings = base.GetGenerationSettings(Vector2.zero);
                return base.cachedTextGeneratorForLayout.GetPreferredWidth(this.m_OutputText, generationSettings) / base.pixelsPerUnit;
            }
        }

        public override float preferredHeight
        {
            get
            {
                TextGenerationSettings generationSettings = base.GetGenerationSettings(new Vector2(base.rectTransform.rect.size.x, 0f));
                return base.cachedTextGeneratorForLayout.GetPreferredHeight(this.m_OutputText, generationSettings) / base.pixelsPerUnit;
            }
        }

        public override void SetVerticesDirty()
        {
            base.SetVerticesDirty();
            this.UpdateQuadImage();
        }

        protected void UpdateQuadImage()
        {
            this.text = this.ConvertToEmojiText(this.text);
            this.m_OutputText = this.GetOutputText(this.text);
            this.m_ImagesVertexIndex.Clear();
            IEnumerator enumerator = LinkImageText.s_ImageRegex.Matches(this.m_OutputText).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Match match = (Match)enumerator.Current;
                    int index = match.Index;
                    int item = index * 4 + 3;
                    this.m_ImagesVertexIndex.Add(item);
                    this.m_ImagesPool.RemoveAll((Image image) => image == null);
                    if (this.m_ImagesPool.Count == 0)
                    {
                        base.GetComponentsInChildren<Image>(this.m_ImagesPool);
                    }
                    if (this.m_ImagesVertexIndex.Count > this.m_ImagesPool.Count)
                    {
                        GameObject gameObject = DefaultControls.CreateImage(default(DefaultControls.Resources));
                        gameObject.layer = base.gameObject.layer;
                        RectTransform rectTransform = gameObject.transform as RectTransform;
                        if (rectTransform)
                        {
                            rectTransform.SetParent(base.rectTransform);
                            rectTransform.localPosition = Vector3.zero;
                            rectTransform.localRotation = Quaternion.identity;
                            rectTransform.localScale = Vector3.one;
                        }
                        this.m_ImagesPool.Add(gameObject.GetComponent<Image>());
                    }
                    string value = match.Groups[1].Value;
                    float num = float.Parse(match.Groups[2].Value);
                    Image image2 = this.m_ImagesPool[this.m_ImagesVertexIndex.Count - 1];
                    if (image2.sprite == null || image2.sprite.name != value)
                    {
                        image2.sprite = ((LinkImageText.funLoadSprite == null) ? Resources.Load<Sprite>(value) : LinkImageText.funLoadSprite(value));
                    }
                    image2.rectTransform.sizeDelta = new Vector2(num, num);
                    image2.enabled = true;
                }
            }
            finally
            {
                IDisposable disposable;
                if ((disposable = (enumerator as IDisposable)) != null)
                {
                    disposable.Dispose();
                }
            }
            for (int i = this.m_ImagesVertexIndex.Count; i < this.m_ImagesPool.Count; i++)
            {
                if (this.m_ImagesPool[i])
                {
                    this.m_ImagesPool[i].enabled = false;
                }
            }
        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            string text = m_Text;
            m_Text = m_OutputText;
            base.OnPopulateMesh(toFill);
            m_Text = text;
            UIVertex vertex = default(UIVertex);
            for (int i = 0; i < this.m_ImagesVertexIndex.Count; i++)
            {
                int num = this.m_ImagesVertexIndex[i];
                RectTransform rectTransform = this.m_ImagesPool[i].rectTransform;
                Vector2 sizeDelta = rectTransform.sizeDelta;
                if (num < toFill.currentVertCount)
                {
                    toFill.PopulateUIVertex(ref vertex, num);
                    rectTransform.anchoredPosition = new Vector2(vertex.position.x + sizeDelta.x / 2f, vertex.position.y + sizeDelta.y / 2f);
                    toFill.PopulateUIVertex(ref vertex, num - 3);
                    Vector3 position = vertex.position;
                    int j = num;
                    int num2 = num - 3;
                    while (j > num2)
                    {
                        toFill.PopulateUIVertex(ref vertex, num);
                        vertex.position = position;
                        toFill.SetUIVertex(vertex, j);
                        j--;
                    }
                }
            }
            if (this.m_ImagesVertexIndex.Count != 0)
            {
                this.m_ImagesVertexIndex.Clear();
            }

            foreach (LinkImageText.HrefInfo hrefInfo in this.m_HrefInfos)
            {
                hrefInfo.boxes.Clear();
                if (hrefInfo.startIndex < toFill.currentVertCount)
                {
                    toFill.PopulateUIVertex(ref vertex, hrefInfo.startIndex);
                    Vector3 position2 = vertex.position;
                    Bounds bounds = new Bounds(position2, Vector3.zero);
                    int j = hrefInfo.startIndex;
                    for (int endIndex = hrefInfo.endIndex; j < endIndex && j < toFill.currentVertCount; j++)
                    {
                        toFill.PopulateUIVertex(ref vertex, j);
                        position2 = vertex.position;
                        float x = position2.x;
                        Vector3 min = bounds.min;
                        if (x < min.x)
                        {
                            hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
                            bounds = new Bounds(position2, Vector3.zero);
                        }
                        else
                        {
                            bounds.Encapsulate(position2);
                        }
                    }
                    hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
                }
            }

            this.redrawUnderLine = true;
        }

        private void Update()
        {
            try
            {
                if (this.redrawUnderLine)
                {
                    this.redrawUnderLine = false;
                    List<Rect> list = new List<Rect>();
                    string arg = string.Empty;
                    IEnumerator enumerator = LinkImageText.s_ColorRegex.Matches(this.text).GetEnumerator();
                    try
                    {
                        if (enumerator.MoveNext())
                        {

                            Match match = (Match)enumerator.Current;
                            arg = match.Groups[2].Value;
                        }
                    }
                    finally
                    {
                        IDisposable disposable;
                        if ((disposable = (enumerator as IDisposable)) != null)
                        {
                            disposable.Dispose();
                        }
                    }
                    foreach (LinkImageText.HrefInfo current in this.m_HrefInfos)
                    {
                        foreach (Rect current2 in current.boxes)
                        {
                            list.Add(current2);
                        }
                    }
                    if (list.Count < 1)
                    {
                        return;
                    }

                    //添加下划线
                    AddUnderLine(list, arg);

                    //this.m_OnHrefDraw.Invoke(list, arg);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        //添加下划线
        private void AddUnderLine(List<Rect> list, string arg)
        {
            int idx = 0;
            Transform trans = transform;
            foreach (Rect rect in list)
            {
                idx = idx + 1;
                string lineName = "line" + idx.ToString();
                Transform lineObj = trans.Find(lineName);
                if (lineObj != null)
                {
                    lineObj.gameObject.SetActive(true);

                    RectTransform rectTransform = lineObj.gameObject.GetComponent<RectTransform>();
                    rectTransform.sizeDelta = new Vector2(rect.width, 2);
                    rectTransform.localPosition = new Vector3(rect.x + rect.width / 2, rect.y - 1, 0);
                    lineObj.gameObject.GetComponent<Image>().color = UICommon.HexRGBToColor(arg.Substring(1));
                }
                else
                {
                    CoreUtils.assetService.Instantiate("UI_Item_Underline", (GameObject tObj) =>
                    {
                        if (gameObject == null)
                        {
                            return;
                        }
                        tObj.name = lineName;
                        tObj.transform.SetParent(gameObject.transform);
                        tObj.transform.localScale = Vector3.one;
                        RectTransform tRect = tObj.GetComponent<RectTransform>();
                        if (tRect != null)
                        {
                            if (tRect.anchorMin == Vector2.zero && tRect.anchorMax == Vector2.one)
                            {
                                tRect.anchoredPosition3D = Vector3.zero;
                                tRect.sizeDelta = Vector2.zero;
                            }
                            else
                            {
                                tRect.anchoredPosition3D = Vector3.zero;
                            }
                        }
                        lineObj = tObj.transform;

                        RectTransform rectTransform = lineObj.gameObject.GetComponent<RectTransform>();
                        rectTransform.sizeDelta = new Vector2(rect.width, 2);
                        rectTransform.localPosition = new Vector3(rect.x + rect.width / 2, rect.y - 1, 0);
                        lineObj.gameObject.GetComponent<Image>().color = UICommon.HexRGBToColor(arg.Substring(1));
                    });
                }
                bool isFor = true;
                while (isFor)
                {
                    idx = idx + 1;
                    Transform tlineObj = trans.Find("line" + idx.ToString());
                    if (tlineObj != null)
                    {
                        tlineObj.gameObject.SetActive(false);
                    }
                    else
                    {
                        isFor = false;
                    }
                }
            }
        }

        protected virtual string GetOutputText(string outputText)
        {
            LinkImageText.s_TextBuilder.Length = 0;
            this.m_HrefInfos.Clear();
            int num = 0;
            IEnumerator enumerator = LinkImageText.s_HrefRegex.Matches(outputText).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Match match = (Match)enumerator.Current;
                    LinkImageText.s_TextBuilder.Append(outputText.Substring(num, match.Index - num));
                    Group group = match.Groups[1];
                    LinkImageText.HrefInfo item = new LinkImageText.HrefInfo
                    {
                        startIndex = LinkImageText.s_TextBuilder.Length * 4,
                        endIndex = (LinkImageText.s_TextBuilder.Length + match.Groups[2].Length - 1) * 4 + 3,
                        name = group.Value
                    };
                    this.m_HrefInfos.Add(item);
                    LinkImageText.s_TextBuilder.Append(match.Groups[2].Value);
                    num = match.Index + match.Length;
                }
            }
            finally
            {
                IDisposable disposable;
                if ((disposable = (enumerator as IDisposable)) != null)
                {
                    disposable.Dispose();
                }
            }
            LinkImageText.s_TextBuilder.Append(outputText.Substring(num, outputText.Length - num));
            return LinkImageText.s_TextBuilder.ToString();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Vector2 point;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(base.rectTransform, eventData.position, eventData.pressEventCamera, out point);
            foreach (LinkImageText.HrefInfo current in this.m_HrefInfos)
            {
                List<Rect> boxes = current.boxes;
                for (int i = 0; i < boxes.Count; i++)
                {
                    if (boxes[i].Contains(point))
                    {
                        this.m_OnHrefClick.Invoke(current.name);
                        return;
                    }
                }
            }
        }

        public string ConvertToEmojiText(string inputString)
        {
            StringBuilder stringBuilder = new StringBuilder(string.Empty);
            int i = 0;
            while (i < inputString.Length)
            {
                string key = inputString.Substring(i, 1);
                string key2 = string.Empty;
                string key3 = string.Empty;
                if (i < inputString.Length - 1)
                {
                    key2 = inputString.Substring(i, 2);
                }
                if (i < inputString.Length - 3)
                {
                    key3 = inputString.Substring(i, 4);
                }
                if (LinkImageText.EmojiDatas.ContainsKey(key3))
                {
                    string text = (string)this.emojiTextTemplate.Clone();
                    text = string.Format(text, LinkImageText.EmojiDatas[key3], base.fontSize);
                    stringBuilder.Append(text);
                    i += 4;
                }
                else if (LinkImageText.EmojiDatas.ContainsKey(key2))
                {
                    string text2 = (string)this.emojiTextTemplate.Clone();
                    text2 = string.Format(text2, LinkImageText.EmojiDatas[key2], base.fontSize);
                    stringBuilder.Append(text2);
                    i += 2;
                }
                else if (LinkImageText.EmojiDatas.ContainsKey(key))
                {
                    string text3 = (string)this.emojiTextTemplate.Clone();
                    text3 = string.Format(text3, LinkImageText.EmojiDatas[key], base.fontSize);
                    stringBuilder.Append(text3);
                    i++;
                }
                else
                {
                    stringBuilder.Append(inputString[i]);
                    i++;
                }
            }
            return stringBuilder.ToString();
        }

        private static string ConvertStringToUnicode(string inputString)
        {
            string[] array = inputString.Split(new char[]
            {
            '_'
            });
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = char.ConvertFromUtf32(Convert.ToInt32(array[i], 16));
            }
            return string.Join(string.Empty, array);
        }

        public static void AddEmojiData(string emoji_code, string img_name = null)
        {
            if (img_name == null)
            {
                img_name = emoji_code;
            }
            string key = LinkImageText.ConvertStringToUnicode(emoji_code);
            LinkImageText.EmojiDatas.Add(key, img_name);
        }
    }
}