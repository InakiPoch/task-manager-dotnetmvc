using tl2_tp10_2023_InakiPoch.Models;

namespace tl2_tp10_2023_InakiPoch.Repositories;

public class RoleCheck {
    private IHttpContextAccessor accesor;

    public RoleCheck(IHttpContextAccessor accesor) {
        this.accesor = accesor;
    }

    public bool IsAdmin() => accesor.HttpContext.Session.GetString("Role") == Enum.GetName(Role.Admin);
    public bool NotLogged() => string.IsNullOrEmpty(accesor.HttpContext.Session.GetString("User")); 
}