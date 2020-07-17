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

    public partial class Provider : IProvider
    {
        //private const string Uri = "http://127.0.0.1:10000/devstoreaccount1/ukenergy/CurrentV1.json";
        private const string Uri = "https://coduo.blob.core.windows.net/ukenergy/CurrentV1.json";

        private readonly Event.IBus _eventBus;
        private readonly IConnectableObservable<Common.Container> _observable;

        public Provider(Platform.ISchedulers schedulers, Event.IBus eventBus)
        {
            _eventBus = eventBus;

            var httpClient = CreateHttpClient();

            var timedSource = Observable
                .Interval(TimeSpan.FromMinutes(15), schedulers.Default);

            var requestSource = eventBus
                .GetEvent<Event.Data.Requested>()
                .Select(_ => schedulers.Default.Now.Ticks);

            _observable = Observable
                .Merge(timedSource, requestSource)
                .StartWith(0)
                .SelectMany(_ => FetchContainer(httpClient))
                .Replay(1);
        }

        partial void OverrideHttpClient(ref HttpClient client);

        private HttpClient CreateHttpClient()
        {
            HttpClient client = null;

            OverrideHttpClient(ref client);

            return client == null
                ? new HttpClient()
                : client;
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
                        _eventBus.Publish(new Event.Data.Fetching());

                        var result = await httpClient.GetAsync(Uri).ConfigureAwait(false);

                        result.EnsureSuccessStatusCode();

                        var stream = await result.Content.ReadAsStreamAsync().ConfigureAwait(false);
                        var container = await Common.Serializer.Deserialize(stream).ConfigureAwait(false);

                        _eventBus.Publish(new Event.Data.Available(container));

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
