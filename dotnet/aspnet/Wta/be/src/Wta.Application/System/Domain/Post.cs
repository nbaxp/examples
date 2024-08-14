namespace Wta.Application.System.Domain;

[OrganizationManagement]
[Display(Name = "岗位", Order = 2)]
public class Post : BaseTreeEntity<Post>
{
    [Hidden]
    public List<User> Users { get; set; } = [];
}
