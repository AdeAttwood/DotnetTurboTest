using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace DotnetTurbo.Web.Turbo;

public class TurboStreamResults : IActionResult, ITurboStreamResult
{
    private const string TurboStreamContentType = "text/vnd.turbo-stream.html";

    public ITurboStreamResult[] Results { get; }

    public TurboStreamResults(params ITurboStreamResult[] results)
    {
        Results = results;
    }

    public async Task<string> RenderToString(ActionContext context)
    {
        var content = new StringBuilder();
        foreach (var result in Results)
        {
            content.Append(await result.RenderToString(context));
        }

        return content.ToString();
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        var services = context.HttpContext.RequestServices;
        var executor = services.GetRequiredService<IActionResultExecutor<ContentResult>>();

        await executor.ExecuteAsync(
            context,
            new ContentResult
            {
                Content = await RenderToString(context),
                ContentType = TurboStreamContentType,
                StatusCode = 200,
            }
        );
    }
}
