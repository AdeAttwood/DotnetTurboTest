using Microsoft.AspNetCore.Mvc;

namespace DotnetTurbo.Web.Turbo;

public class TurboStreamContentResult : ContentResult, ITurboStreamResult
{
    public string? TurboAction { get; set; }
    public string? TurboTarget { get; set; }

    public new string? Content
    {
        set => base.Content = value;
        get
        {
            return $@"<turbo-stream action=""{TurboAction}"" target=""{TurboTarget}""><template>{base.Content}</template></turbo-stream>";
        }
    }

    public Task<string> RenderToString(ActionContext context)
    {
        return Task.FromResult(this.Content ?? string.Empty);
    }
}
