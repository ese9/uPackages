using System.Collections.Generic;
using UnityEngine;

namespace NineGames.Storage
{
    public abstract class DataPatch : ScriptableObject
    {
        public abstract DataVersion Version { get; }
        public abstract void Patch(Dictionary<string, string> containers);
    }
}