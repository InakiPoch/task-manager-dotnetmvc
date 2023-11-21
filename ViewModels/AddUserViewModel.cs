using tl2_tp10_2023_InakiPoch.Models;

namespace tl2_tp10_2023_InakiPoch.ViewModels;

public class AddUserViewModel {
    public string Name { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }

    public AddUserViewModel() {}
}