using tl2_tp10_2023_InakiPoch.Models;

namespace tl2_tp10_2023_InakiPoch.ViewModels;

public class UpdateUserViewModel {
    public int Id { get; set; }
    public string Username { get; set; }
    public Role Role { get; set; }

    public UpdateUserViewModel(User user) {
        Id = user.Id;
        Username = user.Username;
        Role = user.Role;
    }
}