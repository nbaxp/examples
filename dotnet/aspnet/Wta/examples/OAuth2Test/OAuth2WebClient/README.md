# 说明

## 数据迁移

<https://learn.microsoft.com/zh-cn/training/modules/persist-data-ef-core/3-migrations>

初次：

```bash
# 安装工具
dotnet tool install --global dotnet-ef
# 更新工具
dotnet tool update --global dotnet-ef
# 初始化迁移
dotnet ef migrations add InitialCreate --context [Context]
# 应用迁移
dotnet ef database update --context [Context]
```

开发：

```bash
# 生成迁移
dotnet ef migrations add [Name] --context [Context]
# 应用迁移
dotnet ef database update --context [Context]
```