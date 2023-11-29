using tl2_tp10_2023_InakiPoch.Models;

namespace tl2_tp10_2023_InakiPoch.ViewModels;

public class GetUsersViewModel {
    public List<User> Users { get; set; }

    public GetUsersViewModel() {}

    public GetUsersViewModel(List<User> users) {
        Users = users;
    }
}