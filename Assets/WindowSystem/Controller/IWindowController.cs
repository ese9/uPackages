using System;
using Cysharp.Threading.Tasks;
using Zenject;

namespace IdleCivilization.Client.UI
{
    public interface IWindowController : IInitializable, IDisposable
    {
        UniTask ShowWindowAsync(bool animated);
        UniTask HideWindowAsync(bool animated);
    }
}