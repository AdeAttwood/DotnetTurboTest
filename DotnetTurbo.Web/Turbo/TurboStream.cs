using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DotnetTurbo.Web.Turbo;

public class TurboStream
{
    public static TurboStreamContentResult Update(string target, string content)
    {
        return ContentResult("update", target, content);
    }

    public static TurboStreamPartialViewResult Update(string target, PartialViewResult partialView)
    {
        return PartialResult("update", target, partialView);
    }

    public static TurboStreamContentResult Replace(string target, string content)
    {
        return ContentResult("replace", target, content);
    }

    public static TurboStreamPartialViewResult Replace(string target, PartialViewResult partialView)
    {
        return PartialResult("update", target, partialView);
    }

    public static TurboStreamContentResult Append(string target, string content)
    {
        return ContentResult("append", target, content);
    }

    public static TurboStreamPartialViewResult Append(string target, PartialViewResult partialView)
    {
        return PartialResult("append", target, partialView);
    }

    public static TurboStreamPartialViewResult PartialResult(
        string action,
        string target,
        PartialViewResult partialView
    )
    {
        var viewData = new ViewDataDictionary(
            new EmptyModelMetadataProvider(),
            partialView.ViewData.ModelState
        );

        viewData.Model = partialView.Model;

        return new TurboStreamPartialViewResult
        {
            TurboAction = action,
            TurboTarget = target,
            Partial = new PartialViewResult
            {
                ViewName = partialView.ViewName,
                TempData = partialView.TempData,
                ViewData = viewData,
            }
        };
    }

    private static TurboStreamContentResult ContentResult(
        string action,
        string target,
        string content
    )
    {
        return new TurboStreamContentResult
        {
            TurboAction = action,
            TurboTarget = target,
            Content = content,
        };
    }
}
