using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Windows.UI.Xaml;

namespace CODuo
{
    public static class Extensions
    {
        public static IObservable<TArgs> FromDependencyPropertyChanged<TArgs>(this DependencyObject dependencyObject, DependencyProperty dependencyProperty, Func<DependencyProperty, TArgs> adapter)
        {
            return Observable
                .Create<TArgs>(
                    observer =>
                    {
                        var token = dependencyObject.RegisterPropertyChangedCallback(dependencyProperty, (s, dp) => observer.OnNext(adapter(dp)));

                        return Disposable.Create(() => dependencyObject.UnregisterPropertyChangedCallback(dependencyProperty, token));
                    }
                );
        }

        public static IEnumerable<TSeed> Scan<TSource, TSeed, TResult>(this IEnumerable<TSource> source, TSeed seed, Func<TSeed, TSource, TSeed> projection)
        {
            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    seed = projection(seed, enumerator.Current);

                    yield return seed;
                }
            }
        }
    }
}
