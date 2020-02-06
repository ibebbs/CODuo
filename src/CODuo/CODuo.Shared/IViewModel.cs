using System;

namespace CODuo
{
    public interface IViewModel
    {
        IDisposable Activate();
    }

    public interface IViewAware
    {
        void AttachView(object view);

        void DetachViews();
    }
}
