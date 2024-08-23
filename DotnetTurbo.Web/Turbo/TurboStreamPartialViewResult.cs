using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DotnetTurbo.Web.Turbo;

public class TurboStreamPartialViewResult : IActionResult, ITurboStreamResult
{
    public string? TurboAction { get; set; }
    public string? TurboTarget { get; set; }
    public PartialViewResult Partial { get; set; } = new PartialViewResult();

    public async Task<string> RenderToString(ActionContext context)
    {
        ArgumentNullException.ThrowIfNull(TurboAction);
        ArgumentNullException.ThrowIfNull(TurboTarget);
        ArgumentNullException.ThrowIfNull(context);

        var services = context.HttpContext.RequestServices;
        var executor = services.GetRequiredService<IActionResultExecutor<PartialViewResult>>();

        if (executor == null || executor.GetType().Name != "PartialViewResultExecutor")
        {
            throw new InvalidOperationException(
                "Invalid executor. It must be a PartialViewResultExecutor"
            );
        }

        var partialViewExecutor = (PartialViewResultExecutor)executor;
        var viewEngineResult = partialViewExecutor.FindView(context, Partial);
        viewEngineResult.EnsureSuccessful(originalLocations: null);

        var writer = new StringWriter();

        var view = viewEngineResult.View;
        ArgumentNullException.ThrowIfNull(view);

        var viewContext = new ViewContext(
            context,
            view,
            Partial.ViewData,
            Partial.TempData,
            TextWriter.Null,
            new HtmlHelperOptions()
        )
        {
            Writer = writer
        };

        writer.Write(
            $@"<turbo-stream action=""{TurboAction}"" target=""{TurboTarget}""><template>"
        );

        await view.RenderAsync(viewContext);

        writer.Write($@"</template></turbo-stream>");

        return writer.ToString();
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        await new TurboStreamResults(this).ExecuteResultAsync(context);
    }
}