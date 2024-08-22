using Microsoft.AspNetCore.Mvc;

namespace DotnetTurbo.Web.Turbo;

public interface ITurboStreamResult
{
    Task<string> RenderToString(ActionContext context);
}
