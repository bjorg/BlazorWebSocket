using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorWebSocket {

    public class Program {

        //--- Class Methods ---
        public static async Task Main(string[] args) {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            await builder.Build().RunAsync();
        }
    }
}
