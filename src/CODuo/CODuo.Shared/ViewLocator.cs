using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace CODuo
{
    public interface IViewLocator
    {
        Page For(Type type);
    }

    public class ViewLocator : IViewLocator
    {
        private static readonly IReadOnlyDictionary<Type, Func<Page>> Pages = new Dictionary<Type, Func<Page>>
        {
            { typeof(MainPage), () => new MainPage() }
        };

        public Page For(Type type)
        {
            if (Pages.TryGetValue(type, out var factory))
            {
                return factory();
            }
            else
            {
                throw new ArgumentException($"Unknown page type: '{type.Name}'", nameof(type));
            }
        }
    }
}
