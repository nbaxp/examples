# ˵��

## ����Ǩ��

<https://learn.microsoft.com/zh-cn/training/modules/persist-data-ef-core/3-migrations>

���Σ�

```bash
# ��װ����
dotnet tool install --global dotnet-ef
# ���¹���
dotnet tool update --global dotnet-ef
# ��ʼ��Ǩ��
dotnet ef migrations add InitialCreate --context [Context]
# Ӧ��Ǩ��
dotnet ef database update --context [Context]
```

������

```bash
# ����Ǩ��
dotnet ef migrations add [Name] --context [Context]
# Ӧ��Ǩ��
dotnet ef database update --context [Context]
```