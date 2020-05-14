# Blazor WebAssembly with WebSocket

This repository contains a simple application that that connects to the `wss://echo.websocket.org` to show how WebSocket connectivity works in Blazor WebAssembly.

```cshtml
@page "/"
@using System.Net.WebSockets
@using System.Text
@using System.Threading
@implements IDisposable

<h1>Echo test</h1>
<h3>State: @webSocket.State</h3>

@if(webSocket.State == WebSocketState.Open) {
    <form @onsubmit="SendMessageAsync">
        Message: <input @bind="@message" />
        <button type="submit">Send</button>
    </form>
    <pre>@log</pre>
}

@code {

    // Sample adapted from https://gist.github.com/SteveSandersonMS/5aaff6b010b0785075b0a08cc1e40e01

    //--- Fields ---
    CancellationTokenSource disposalTokenSource = new CancellationTokenSource();
    ClientWebSocket webSocket = new ClientWebSocket();
    string message = "Hello, websocket!";
    string log = "";

    //--- Methods ---
    protected override async Task OnInitializedAsync() {
        await webSocket.ConnectAsync(new Uri("wss://echo.websocket.org"), disposalTokenSource.Token);
        _ = ReceiveLoop();
    }

    private Task SendMessageAsync() {
        log += $"Sending: {message}\n";
        var dataToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
        return webSocket.SendAsync(dataToSend, WebSocketMessageType.Text, true, disposalTokenSource.Token);
    }

    private async Task ReceiveLoop() {
        var buffer = new ArraySegment<byte>(new byte[1024]);
        while(!disposalTokenSource.IsCancellationRequested) {

            // Note that the received block might only be part of a larger message. If this applies in your scenario,
            // check the received.EndOfMessage and consider buffering the blocks until that property is true.
            // Or use a higher-level library such as SignalR.
            var received = await webSocket.ReceiveAsync(buffer, disposalTokenSource.Token);
            var receivedAsText = Encoding.UTF8.GetString(buffer.Array, 0, received.Count);
            log += $"Received: {receivedAsText}\n";
            StateHasChanged();
        }
    }

    public void Dispose() {
        disposalTokenSource.Cancel();
        _ = webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Bye", CancellationToken.None);
    }
}
```

## License

> Licensed under the Apache License, Version 2.0 (the "License");
> you may not use this file except in compliance with the License.
> You may obtain a copy of the License at
>
> http://www.apache.org/licenses/LICENSE-2.0
>
> Unless required by applicable law or agreed to in writing, software
> distributed under the License is distributed on an "AS IS" BASIS,
> WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
> See the License for the specific language governing permissions and
> limitations under the License.
