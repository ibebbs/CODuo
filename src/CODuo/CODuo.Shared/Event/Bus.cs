namespace CODuo.Event
{
    public interface IBus : MVx.Observable.IBus { }

    public class Bus : MVx.Observable.Bus, IBus { }
}
