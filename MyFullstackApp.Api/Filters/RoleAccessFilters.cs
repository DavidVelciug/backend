using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyApi.Filters;

public static class AppRoles
{
    public const string Guest = "guest";
    public const string User = "user";
    public const string Moderator = "moderator";
    public const string Admin = "admin";

    public static readonly string[] All = [Guest, User, Moderator, Admin];
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AdminModAttribute : ActionFilterAttribute, IOrderedFilter
{
    public new int Order => -200;

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var role = RoleResolver.GetRole(context);
        if (role is AppRoles.Admin or AppRoles.Moderator)
        {
            base.OnActionExecuting(context);
            return;
        }

        context.Result = ForbiddenResultFactory.BuildForbiddenResult(context);
    }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class RoleAccessAttribute(params string[] allowedRoles) : ActionFilterAttribute, IOrderedFilter
{
    private readonly HashSet<string> _allowedRoles = allowedRoles
        .Select(r => r.Trim().ToLowerInvariant())
        .Where(r => !string.IsNullOrWhiteSpace(r))
        .ToHashSet();

    public new int Order => -100;

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var role = RoleResolver.GetRole(context);
        if (_allowedRoles.Count == 0 || _allowedRoles.Contains(role))
        {
            base.OnActionExecuting(context);
            return;
        }

        context.Result = ForbiddenResultFactory.BuildForbiddenResult(context);
    }
}

internal static class ForbiddenResultFactory
{
    public static IActionResult BuildForbiddenResult(ActionExecutingContext context)
    {
        var request = context.HttpContext.Request;
        var asRolePresent = request.Query.ContainsKey("asRole");
        var acceptValues = request.Headers.Accept.ToArray();
        var wantsHtml = acceptValues.Any(v => !string.IsNullOrEmpty(v) && v.Contains("text/html", StringComparison.OrdinalIgnoreCase));
        if (asRolePresent && wantsHtml)
        {
            var origin = request.Headers.Origin.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(origin))
            {
                var referer = request.Headers.Referer.FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(referer) && Uri.TryCreate(referer, UriKind.Absolute, out var parsed))
                {
                    origin = parsed.GetLeftPart(UriPartial.Authority);
                }
            }

            var baseUrl = string.IsNullOrWhiteSpace(origin) ? "http://localhost:5173" : origin.TrimEnd('/');
            return new RedirectResult($"{baseUrl}/error/403");
        }

        return new ForbidResult();
    }
}

internal static class RoleResolver
{
    private const string HeaderName = "X-User-Role";
    private const string QueryName = "asRole";

    public static string GetRole(ActionExecutingContext context)
    {
        var request = context.HttpContext.Request;

        var rawRole = request.Headers[HeaderName].FirstOrDefault()
                      ?? request.Query[QueryName].FirstOrDefault()
                      ?? AppRoles.Guest;

        var role = rawRole.Trim().ToLowerInvariant();
        return AppRoles.All.Contains(role) ? role : AppRoles.Guest;
    }
}
