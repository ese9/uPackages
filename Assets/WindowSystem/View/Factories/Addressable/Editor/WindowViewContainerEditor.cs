using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace IdleCivilization.Client.UI
{
    [CustomEditor(typeof(AddressableWindowViewContainer))]
    public class WindowViewContainerEditor : UnityEditor.Editor, IPreprocessBuildWithReport
    {
        private SerializedProperty prefabList;
        private ReorderableList reorderableList;
        private readonly Dictionary<int, bool> foldouts = new Dictionary<int, bool>();

        private AddressableWindowViewContainer Target => (AddressableWindowViewContainer)target;

        int IOrderedCallback.callbackOrder => -1;

        void IPreprocessBuildWithReport.OnPreprocessBuild(BuildReport report) => UpdateViewTypeStrings();

        public override void OnInspectorGUI()
        {
            if (serializedObject.UpdateIfRequiredOrScript())
            {
                UpdateViewTypeStrings();
                reorderableList.DoLayoutList();
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void OnEnable()
        {
            prefabList = serializedObject.FindProperty("windowAssets");
            reorderableList = new ReorderableList(serializedObject, prefabList, true, true, true, true);
            reorderableList.drawHeaderCallback += rect => EditorGUI.LabelField(rect, "Windows");
            reorderableList.drawElementCallback += OnDrawElement;
            reorderableList.elementHeightCallback += OnGetElementHeight;
        }

        private float OnGetElementHeight(int index)
        {
            if (!foldouts.ContainsKey(index))
                foldouts[index] = false;

            return EditorGUIUtility.singleLineHeight * GetFoldoutLevel(index);
        }

        private int GetFoldoutLevel(int index) => foldouts[index] ? 5 : 1;

        private void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            var item = prefabList.GetArrayElementAtIndex(index);
            var viewProp = item.FindPropertyRelative("view");
            var viewTypeProp = item.FindPropertyRelative("viewType");
            var cacheProp = item.FindPropertyRelative("cacheInMemory");
            var assetRefProp = item.FindPropertyRelative("assetReference");

            var singleLineHeight = EditorGUIUtility.singleLineHeight;
            var pHeight = EditorGUIUtility.singleLineHeight;

            var labelRect = rect;
            labelRect.x += 15;
            labelRect.y += (EditorGUIUtility.singleLineHeight -
                            EditorGUIUtility.singleLineHeight * GetFoldoutLevel(index)) * 0.5f;
            var values = viewTypeProp.stringValue.Split('.');
            var label = values.Length == 0 ? "NONE" : values.Last();
            foldouts[index] = EditorGUI.Foldout(labelRect, foldouts[index], label);

            if (!foldouts[index])
                return;

            EditorGUI.PropertyField(new Rect(rect.x, rect.y + pHeight * 1, rect.width, singleLineHeight), viewProp);
            EditorGUI.PropertyField(new Rect(rect.x, rect.y + pHeight * 2, rect.width, singleLineHeight), cacheProp);
            EditorGUI.PropertyField(new Rect(rect.x, rect.y + pHeight * 3, rect.width, singleLineHeight), viewTypeProp);
            EditorGUI.PropertyField(new Rect(rect.x, rect.y + pHeight * 4, rect.width, singleLineHeight), assetRefProp, GUIContent.none);
        }

        private void UpdateViewTypeStrings()
        {
            var windowAssets = Target.WindowAssets;

            if (windowAssets == null)
                return;

            for (var i = windowAssets.Count - 1; i >= 0; i--)
            {
                var windowViewAsset = windowAssets[i];
                var view = windowViewAsset.view;
                windowViewAsset.viewType = view ? view.GetType().ToString() : string.Empty;
            }
        }

        private static AssetReference GetAssetReference(Object obj)
        {
            var entry = GetAssetEntry(obj);

            if (entry != null)
                return new AssetReference(entry.guid);

            return CreateAssetReference(obj);
        }

        private static AssetReference CreateAssetReference(Object obj)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var assetPathToGuid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(obj));
            var assetReference = settings.CreateAssetReference(assetPathToGuid);
            return assetReference;
        }

        private static AddressableAssetEntry GetAssetEntry(Object obj)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            return settings.FindAssetEntry(AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(obj)));
        }
    }
}