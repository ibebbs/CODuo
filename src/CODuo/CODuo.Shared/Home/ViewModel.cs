using System;
using System.Reactive.Disposables;

namespace CODuo.Home
{
    public interface IViewModel : CODuo.IViewModel, CODuo.IViewAware
    {

    }

    public class ViewModel : IViewModel
    {
        public IDisposable Activate()
        {
            return Disposable.Empty;
        }

        public void AttachView(object view)
        {
        }

        public void DetachViews()
        {
        }
    }
}
