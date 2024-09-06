using Wta.Application.SystemModule.Data;

namespace Wta.Application.MesModule;

[DependsOn<SystemDbContext>, Mes, Display(Name = "生产计划", Order = 10)]
public class ProductionPlan : Entity
{
}

[DependsOn<SystemDbContext>, Mes, Display(Name = "生产工单", Order = 20)]
public class ProductionOrder : Entity
{
}

[DependsOn<SystemDbContext>, Mes, Display(Name = "生产领料", Order = 30)]
public class ProductionMaterialPick : Entity
{
}

[DependsOn<SystemDbContext>, Mes, Display(Name = "生产退料", Order = 40)]
public class ProductionMaterialReturn : Entity
{
}

[DependsOn<SystemDbContext>, Mes, Display(Name = "生产报工", Order = 50)]
public class ProductionReport : Entity
{
}

[DependsOn<SystemDbContext>, Mes, Display(Name = "生产入库", Order = 60)]
public class ProductionStorage : Entity
{
}

[DependsOn<SystemDbContext>, Mes, Display(Name = "生产计划看板", Order = 70)]
public class ProductionPlanBoard : Entity
{
}

[DependsOn<SystemDbContext>, Mes, Display(Name = "生产执行跟踪", Order = 80)]
public class ProductionExecutionTracking : Entity
{
}

[DependsOn<SystemDbContext>, Mes, Display(Name = "生产用料统计", Order = 90)]
public class ProductionMaterialStatistics : Entity
{
}

[DependsOn<SystemDbContext>, Mes, Display(Name = "生产数据统计", Order = 100)]
public class ProductionDataStatistics : Entity
{
}

[DependsOn<SystemDbContext>, Mes, Display(Name = "生产任务池", Order = 110)]
public class ProductionTaskPool : Entity
{
}

//[DependsOn<SystemDbContext>, Technology, Display(Name = "辅助表-子表单行数", Order = 100)]
//public class Test : Entity
//{
//}
