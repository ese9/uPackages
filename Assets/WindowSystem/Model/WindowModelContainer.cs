using System.Collections.Generic;
using UnityEngine;

namespace IdleCivilization.Client.UI
{
    [CreateAssetMenu(fileName = "WindowModelContainer", menuName = "Game/Window/Model Container")]
    public class WindowModelContainer : ScriptableObject
    {
        [SerializeField] private bool loadFromPath;
        [SerializeField] private string resourceModelPath;
        [SerializeField] private WindowModelBase[] windowModels;

        public IEnumerable<WindowModelBase> Models => windowModels;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!loadFromPath)
                return;

            windowModels = Resources.LoadAll<WindowModelBase>(resourceModelPath);
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}