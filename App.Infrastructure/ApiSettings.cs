using MudBlazor;

namespace App.Infrastructure;

public class ApiSettings
{
    public string BaseApiUrl { get; set; }
    public TokenEndpoints TokenEndpoints { get; set; }
    public UserEndpoints UserEndpoints { get; set; }
    public TenantEndpoints TenantEndpoints { get; set; }
    public RoleEndpoints RoleEndpoints { get; set; }
}

public class TokenEndpoints
{
    public string Login { get; set; }
    public string RefreshToken { get; set; }
}

public class UserEndpoints
{
    public string Update { get; set; }
    public string ChangePassword { get; set; }
    public string All { get; set; }
    public string ById { get; set; }
    public string Register { get; set; }
    public string RolesById { get; set; }
    public string UpdateRoles { get; set; }
    public string UpdateStatus { get; set; }

    public string UserByIdEndpoint(string userId)
    {
        return $"{ById}{userId}";
    }

    public string RolesByIdEndpoint(string userId)
    {
        return $"{RolesById}{userId}";
    }

    public string UpdateRolesEndpoint(string userId)
    {
        return $"{UpdateRoles}{userId}";
    }
}

public class RoleEndpoints
{
    public string Create { get; set; }
    public string Update { get; set; }
    public string UpdatePermissions { get; set; }
    public string Delete { get; set; }
    public string All { get; set; }
    public string ById { get; set; }
    public string ByIdPartial { get; set; }

    public string GetByIdEndpointUrl(string roleId)
    {
        return $"{ById}{roleId}";
    }

    public string GetByIdPartialEndpointUrl(string roleId)
    {
        return $"{ByIdPartial}{roleId}";
    }

    public string GetDeleteEndpointUrl(string roleId)
    {
        return $"{Delete}{roleId}";
    }
}

public class TenantEndpoints
{
    public string Create { get; set; }
    public string Upgrade { get; set; }
    public string ById { get; set; }
    public string All { get; set; }
    public string Activate { get; set; }
    public string DeActivate { get; set; }

    public string GetByIdEndpointUrl(string tenantId)
    {
        return $"{ById}{tenantId}";
    }

    public string GetActivateEndpointUrl(string tenantId)
    {
        return $"{Activate}{tenantId}/{nameof(Activate)}";
    }

    public string GetDeActivateEndpointUrl(string tenantId)
    {
        return $"{DeActivate}{tenantId}/{nameof(DeActivate)}";
    }
}
