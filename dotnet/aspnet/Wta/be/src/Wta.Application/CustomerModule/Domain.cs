using Wta.Application.Platform;

namespace Wta.Application.CustomerModule;

[DependsOn<PlatformDbContext>, CustomerManagement, Display(Name = "客户", Order = 10)]
public class Custom : Entity
{
}

[DependsOn<PlatformDbContext>, CustomerManagement, Display(Name = "跟进记录", Order = 20)]
public class FollowUpRecord : Entity
{
}

[DependsOn<PlatformDbContext>, CustomerManagement, Display(Name = "客户分析", Order = 30)]
public class CustomerAnalysis : Entity
{
}
