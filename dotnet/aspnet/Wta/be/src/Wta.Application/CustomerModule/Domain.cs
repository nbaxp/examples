using Wta.Application.SystemModule.Data;

namespace Wta.Application.CustomerModule;

[DependsOn<SystemDbContext>, CustomerManagement, Display(Name = "客户", Order = 10)]
public class Custom : Entity
{
}

[DependsOn<SystemDbContext>, CustomerManagement, Display(Name = "跟进记录", Order = 20)]
public class FollowUpRecord : Entity
{
}

[DependsOn<SystemDbContext>, CustomerManagement, Display(Name = "客户分析", Order = 30)]
public class CustomerAnalysis : Entity
{
}
