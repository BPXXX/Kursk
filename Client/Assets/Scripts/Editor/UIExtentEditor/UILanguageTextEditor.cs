using System;
using UnityEngine;
using UnityEngine.UI;

using GameFramework;
using Skyunion;

namespace UnityEditor.UI
{
    // TODO REVIEW
    // Have material live under text
    // move stencil mask into effects *make an efects top level element like there is
    // paragraph and character

    /// <summary>
    /// Editor class used to edit UI Labels.
    /// </summary>

    [CustomEditor(typeof(LanguageText), true)]
    [CanEditMultipleObjects]
    public class UILanguageTextEditor : TextEditor
    {
        protected LanguageText Target;
        private static IDataService dataService;

        protected override void OnEnable()
        {
            base.OnEnable();

        }
        public override void OnInspectorGUI()
        {
            Target = target as LanguageText;
            Target.languageId = EditorGUILayout.IntField(new GUIContent("语言包ID"), Target.languageId);
            if(EditorGUILayout.DropdownButton(new GUIContent("刷新"), FocusType.Passive))
            {
                Target.UpdateLanguage();
            }
            base.OnInspectorGUI();
        }
    }
}