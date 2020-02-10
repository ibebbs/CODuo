using System;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace CODuo.Data
{
    public interface IProvider
    {
        IObservable<Common.Container> Container { get; }

        IDisposable Activate();
    }

    public class Provider : IProvider
    {
        private const string Uri = "https://coduo.blob.core.windows.net/ukenergy/CurrentV1.json";

        private readonly IConnectableObservable<Common.Container> _observable;

        public Provider(Platform.ISchedulers schedulers)
        {
            var httpClient = new HttpClient();

            _observable = Observable
                .Interval(TimeSpan.FromMinutes(15), schedulers.Default)
                .StartWith(0)
                .SelectMany(_ => FetchContainer(httpClient))
                .Replay(1);
        }

        public IDisposable Activate()
        {
            return _observable.Connect();
        }

        private IObservable<Common.Container> FetchContainer(HttpClient httpClient)
        {
            return Observable.Create<Common.Container>(
                async observer =>
                {
                    try
                    {
                        var result = await httpClient.GetAsync(Uri).ConfigureAwait(false);

                        result.EnsureSuccessStatusCode();

                        var stream = await result.Content.ReadAsStreamAsync().ConfigureAwait(false);
                        var container = await Common.Serializer.Deserialize(stream).ConfigureAwait(false);

                        observer.OnNext(container);
                        observer.OnCompleted();
                    }
                    catch (Exception e)
                    {
                        observer.OnError(e);
                    }
                });
        }

        public IObservable<Common.Container> Container => _observable;
    }
}
