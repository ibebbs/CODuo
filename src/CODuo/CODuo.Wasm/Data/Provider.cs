using System.Net.Http;

namespace CODuo.Data
{
    public partial class Provider
    {
        partial void OverrideHttpClient(ref HttpClient client)
        {
            client = new HttpClient(new Uno.UI.Wasm.WasmHttpHandler());
        }
    }
}
