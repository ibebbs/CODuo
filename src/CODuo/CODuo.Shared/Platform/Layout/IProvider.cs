using System;
using System.Reactive.Subjects;

namespace CODuo.Platform.Layout
{
    public interface IProvider
    {
        /// <summary>
        /// Returns an <see cref="IObservable{T}" of <see cref="ILayout"/>
        /// </summary>
        /// <remarks>
        /// This observable should be a <see cref="BehaviorSubject{T}"/> such that it always
        /// return the current layout on subscription
        /// </remarks>
        IObservable<ILayout> Changes { get; }
    }
}
