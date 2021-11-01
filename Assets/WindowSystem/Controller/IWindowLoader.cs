using Cysharp.Threading.Tasks;

namespace IdleCivilization.Client.UI
{
    public interface IWindowLoader
    {
        UniTask<bool> LoadWindowAsync();
    }
}