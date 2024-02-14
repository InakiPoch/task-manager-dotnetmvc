using tl2_tp10_2023_InakiPoch.Models;

namespace tl2_tp10_2023_InakiPoch.Repositories;


//Provides methods to check session's settings. Made non-static to access it on a View 
public class RoleCheck {
    private IHttpContextAccessor accessor;

    public RoleCheck(IHttpContextAccessor accessor) {
        this.accessor = accessor;
    }

    public bool IsAdmin() => accessor.HttpContext.Session.GetString("Role") == Enum.GetName(Role.Admin);
    public bool NotLogged() => string.IsNullOrEmpty(accessor.HttpContext.Session.GetString("User")); 
    public int LoggedUserId() => Convert.ToInt32(accessor.HttpContext.Session.GetString("Id"));
    public string LoggedUserName() => accessor.HttpContext.Session.GetString("User"); 
}