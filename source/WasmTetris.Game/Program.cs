using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WasmTetris.Game;
using WasmTetris.Game.Engine;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<Game>("#app");

builder.Services.AddSingleton<IRenderEngine, RenderEngine>();

await builder.Build().RunAsync();
