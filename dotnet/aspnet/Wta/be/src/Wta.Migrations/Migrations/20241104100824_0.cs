using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wta.Migrations.Migrations;

/// <inheritdoc />
public partial class _0 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "AccountsPayableChecking",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AccountsPayableChecking", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AccountsPayableStatistics",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AccountsPayableStatistics", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AccountsReceivableStatistics",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AccountsReceivableStatistics", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AssetCategory",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Number = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AssetCategory", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AssetVersion",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Type = table.Column<string>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Number = table.Column<string>(type: "TEXT", nullable: false),
                Attributes = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AssetVersion", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Custom",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Custom", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "CustomerAnalysis",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CustomerAnalysis", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Dict",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Number = table.Column<string>(type: "TEXT", nullable: false),
                Order = table.Column<float>(type: "REAL", nullable: false),
                Path = table.Column<string>(type: "TEXT", nullable: false),
                ParentId = table.Column<Guid>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Dict", x => x.Id);
                table.ForeignKey(
                    name: "FK_Dict_Dict_ParentId",
                    column: x => x.ParentId,
                    principalTable: "Dict",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateTable(
            name: "EqpCategory",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Type = table.Column<string>(type: "TEXT", nullable: false),
                Color = table.Column<string>(type: "TEXT", nullable: false),
                Disabled = table.Column<bool>(type: "INTEGER", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Number = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_EqpCategory", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "EqpType",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Number = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_EqpType", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "FinancialRevenueExpenseKanban",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FinancialRevenueExpenseKanban", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "FollowUpRecord",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FollowUpRecord", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "InputInvoice",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_InputInvoice", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "InventoryAllocation",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_InventoryAllocation", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "InventoryCheck",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_InventoryCheck", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "InventoryInOutStatistics",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_InventoryInOutStatistics", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "InventoryStatisticsKanban",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_InventoryStatisticsKanban", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "KanbanHomepage",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_KanbanHomepage", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "LoginProvider",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Number = table.Column<string>(type: "TEXT", nullable: false),
                Icon = table.Column<string>(type: "TEXT", nullable: false),
                ClientId = table.Column<string>(type: "TEXT", nullable: false),
                ClientSecret = table.Column<string>(type: "TEXT", nullable: false),
                UserIdName = table.Column<string>(type: "TEXT", nullable: true),
                CallbackPath = table.Column<string>(type: "TEXT", nullable: false),
                AuthorizationEndpoint = table.Column<string>(type: "TEXT", nullable: false),
                TokenEndpoint = table.Column<string>(type: "TEXT", nullable: false),
                UserInformationEndpoint = table.Column<string>(type: "TEXT", nullable: false),
                UserInformationRequestMethod = table.Column<string>(type: "TEXT", nullable: false),
                UserInformationTokenPosition = table.Column<string>(type: "TEXT", nullable: false),
                Disabled = table.Column<bool>(type: "INTEGER", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_LoginProvider", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "OeeConfiguration",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                IsDefault = table.Column<bool>(type: "INTEGER", nullable: false),
                Numerator = table.Column<string>(type: "TEXT", nullable: false),
                Denominator = table.Column<string>(type: "TEXT", nullable: false),
                AvailabilitMode = table.Column<string>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_OeeConfiguration", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "OtherInboundOrder",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_OtherInboundOrder", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "OtherOutboundOrder",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_OtherOutboundOrder", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PaymentOrder",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PaymentOrder", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PaymentReceipt",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PaymentReceipt", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PeriodStartEnd",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PeriodStartEnd", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Permission",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                AuthType = table.Column<string>(type: "TEXT", nullable: false),
                Roles = table.Column<string>(type: "TEXT", nullable: true),
                ButtonType = table.Column<string>(type: "TEXT", nullable: true),
                ClassList = table.Column<string>(type: "TEXT", nullable: true),
                Command = table.Column<string>(type: "TEXT", nullable: true),
                Component = table.Column<string>(type: "TEXT", nullable: true),
                Disabled = table.Column<bool>(type: "INTEGER", nullable: false),
                Hidden = table.Column<bool>(type: "INTEGER", nullable: false),
                Icon = table.Column<string>(type: "TEXT", nullable: true),
                Method = table.Column<string>(type: "TEXT", nullable: true),
                NoCache = table.Column<bool>(type: "INTEGER", nullable: false),
                Redirect = table.Column<string>(type: "TEXT", nullable: true),
                RoutePath = table.Column<string>(type: "TEXT", nullable: false),
                Type = table.Column<string>(type: "TEXT", nullable: false),
                Url = table.Column<string>(type: "TEXT", nullable: true),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Number = table.Column<string>(type: "TEXT", nullable: false),
                Order = table.Column<float>(type: "REAL", nullable: false),
                Path = table.Column<string>(type: "TEXT", nullable: false),
                ParentId = table.Column<Guid>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Permission", x => x.Id);
                table.ForeignKey(
                    name: "FK_Permission_Permission_ParentId",
                    column: x => x.ParentId,
                    principalTable: "Permission",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateTable(
            name: "ProductBom",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProductBom", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ProductInfo",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProductInfo", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ProductionDataStatistics",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProductionDataStatistics", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ProductionExecutionTracking",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProductionExecutionTracking", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ProductionMaterialPick",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProductionMaterialPick", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ProductionMaterialReturn",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProductionMaterialReturn", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ProductionMaterialStatistics",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProductionMaterialStatistics", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ProductionOrder",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProductionOrder", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ProductionPlan",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProductionPlan", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ProductionPlanBoard",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProductionPlanBoard", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ProductionReport",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProductionReport", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ProductionStatisticsKanban",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProductionStatisticsKanban", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ProductionStorage",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProductionStorage", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ProductionTaskPool",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProductionTaskPool", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ProductList",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProductList", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PrudoctionProcess",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PrudoctionProcess", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PruductCategory",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PruductCategory", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PruductionTeam",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PruductionTeam", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PurchaseDemandStatistics",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PurchaseDemandStatistics", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PurchaseExecutionTrackingBoard",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PurchaseExecutionTrackingBoard", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PurchaseOrder",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PurchaseOrder", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PurchaseOrderKanban",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PurchaseOrderKanban", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PurchaseOrderStatistics",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PurchaseOrderStatistics", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PurchaseRequest",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PurchaseRequest", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PurchaseReturn",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PurchaseReturn", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PurchaseReturnPayable",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PurchaseReturnPayable", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PurchaseStorage",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PurchaseStorage", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PurchaseStoragePayable",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PurchaseStoragePayable", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "QuotationOrder",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_QuotationOrder", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "QuotationStatistics",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_QuotationStatistics", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ReceivableAccountChecking",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ReceivableAccountChecking", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Role",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Number = table.Column<string>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Role", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "SaleDelivery",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SaleDelivery", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "SaleOrder",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SaleOrder", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "SaleOrderStatistics",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SaleOrderStatistics", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "SaleReturn",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SaleReturn", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "SalesExecutionTrackingBoard",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SalesExecutionTrackingBoard", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "SalesInvoice",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SalesInvoice", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "SalesOrderKanban",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SalesOrderKanban", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "SalesOutboundReceivable",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SalesOutboundReceivable", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "SalesReturnReceivable",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SalesReturnReceivable", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "SchemeDesign",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SchemeDesign", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "SupplierInfo",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SupplierInfo", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "SupplierInfoQuery",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SupplierInfoQuery", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "SupplierPriceList",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SupplierPriceList", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Tenant",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Number = table.Column<string>(type: "TEXT", nullable: false),
                Logo = table.Column<string>(type: "TEXT", nullable: true),
                Copyright = table.Column<string>(type: "TEXT", nullable: true),
                Disabled = table.Column<bool>(type: "INTEGER", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Tenant", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "UnitCategory",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Number = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UnitCategory", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "WarehouseInfo",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_WarehouseInfo", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "WarehouseLocationInfo",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_WarehouseLocationInfo", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "WarehouseTransactionKanban",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_WarehouseTransactionKanban", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "WorkGroup",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                ManagerId = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Number = table.Column<string>(type: "TEXT", nullable: false),
                Order = table.Column<float>(type: "REAL", nullable: false),
                Path = table.Column<string>(type: "TEXT", nullable: false),
                ParentId = table.Column<Guid>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_WorkGroup", x => x.Id);
                table.ForeignKey(
                    name: "FK_WorkGroup_WorkGroup_ParentId",
                    column: x => x.ParentId,
                    principalTable: "WorkGroup",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateTable(
            name: "WorkstationCategory",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Number = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_WorkstationCategory", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "WorkstationDeviceLog",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                WorkstationNumber = table.Column<string>(type: "TEXT", nullable: false),
                DeviceNumber = table.Column<string>(type: "TEXT", nullable: false),
                DeviceStatusNumber = table.Column<string>(type: "TEXT", nullable: false),
                Start = table.Column<DateTime>(type: "TEXT", nullable: false),
                End = table.Column<DateTime>(type: "TEXT", nullable: false),
                Duration = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_WorkstationDeviceLog", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Asset",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CategoryId = table.Column<Guid>(type: "TEXT", nullable: true),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Number = table.Column<string>(type: "TEXT", nullable: false),
                Order = table.Column<float>(type: "REAL", nullable: false),
                Path = table.Column<string>(type: "TEXT", nullable: false),
                ParentId = table.Column<Guid>(type: "TEXT", nullable: true),
                Values = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Asset", x => x.Id);
                table.ForeignKey(
                    name: "FK_Asset_AssetCategory_CategoryId",
                    column: x => x.CategoryId,
                    principalTable: "AssetCategory",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
                table.ForeignKey(
                    name: "FK_Asset_Asset_ParentId",
                    column: x => x.ParentId,
                    principalTable: "Asset",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateTable(
            name: "RolePermission",
            columns: table => new
            {
                RoleId = table.Column<Guid>(type: "TEXT", nullable: false),
                PermissionId = table.Column<Guid>(type: "TEXT", nullable: false),
                IsReadOnly = table.Column<bool>(type: "INTEGER", nullable: false),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RolePermission", x => new { x.RoleId, x.PermissionId });
                table.ForeignKey(
                    name: "FK_RolePermission_Permission_PermissionId",
                    column: x => x.PermissionId,
                    principalTable: "Permission",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_RolePermission_Role_RoleId",
                    column: x => x.RoleId,
                    principalTable: "Role",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Uom",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CategoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                Ratio = table.Column<float>(type: "REAL", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Number = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Uom", x => x.Id);
                table.ForeignKey(
                    name: "FK_Uom_UnitCategory_CategoryId",
                    column: x => x.CategoryId,
                    principalTable: "UnitCategory",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Workstation",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CategoryId = table.Column<Guid>(type: "TEXT", nullable: true),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Number = table.Column<string>(type: "TEXT", nullable: false),
                Order = table.Column<float>(type: "REAL", nullable: false),
                Path = table.Column<string>(type: "TEXT", nullable: false),
                ParentId = table.Column<Guid>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Workstation", x => x.Id);
                table.ForeignKey(
                    name: "FK_Workstation_WorkstationCategory_CategoryId",
                    column: x => x.CategoryId,
                    principalTable: "WorkstationCategory",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
                table.ForeignKey(
                    name: "FK_Workstation_Workstation_ParentId",
                    column: x => x.ParentId,
                    principalTable: "Workstation",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateTable(
            name: "WorkstationDevice",
            columns: table => new
            {
                WorkstationId = table.Column<Guid>(type: "TEXT", nullable: false),
                AssetId = table.Column<Guid>(type: "TEXT", nullable: false),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_WorkstationDevice", x => new { x.WorkstationId, x.AssetId });
                table.ForeignKey(
                    name: "FK_WorkstationDevice_Asset_AssetId",
                    column: x => x.AssetId,
                    principalTable: "Asset",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_WorkstationDevice_Workstation_WorkstationId",
                    column: x => x.WorkstationId,
                    principalTable: "Workstation",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "WorkstationTimeGroup",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                WorkstationId = table.Column<Guid>(type: "TEXT", nullable: false),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Start = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                End = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_WorkstationTimeGroup", x => x.Id);
                table.ForeignKey(
                    name: "FK_WorkstationTimeGroup_Workstation_WorkstationId",
                    column: x => x.WorkstationId,
                    principalTable: "Workstation",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "TimeRange",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                GroupId = table.Column<Guid>(type: "TEXT", nullable: false),
                Start = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                End = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TimeRange", x => x.Id);
                table.ForeignKey(
                    name: "FK_TimeRange_WorkstationTimeGroup_GroupId",
                    column: x => x.GroupId,
                    principalTable: "WorkstationTimeGroup",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Department",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                ManagerId = table.Column<Guid>(type: "TEXT", nullable: true),
                PostId = table.Column<Guid>(type: "TEXT", nullable: true),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Number = table.Column<string>(type: "TEXT", nullable: false),
                Order = table.Column<float>(type: "REAL", nullable: false),
                Path = table.Column<string>(type: "TEXT", nullable: false),
                ParentId = table.Column<Guid>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Department", x => x.Id);
                table.ForeignKey(
                    name: "FK_Department_Department_ParentId",
                    column: x => x.ParentId,
                    principalTable: "Department",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateTable(
            name: "Post",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                DepartmentId = table.Column<Guid>(type: "TEXT", nullable: true),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Number = table.Column<string>(type: "TEXT", nullable: false),
                Order = table.Column<float>(type: "REAL", nullable: false),
                Path = table.Column<string>(type: "TEXT", nullable: false),
                ParentId = table.Column<Guid>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Post", x => x.Id);
                table.ForeignKey(
                    name: "FK_Post_Department_DepartmentId",
                    column: x => x.DepartmentId,
                    principalTable: "Department",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
                table.ForeignKey(
                    name: "FK_Post_Post_ParentId",
                    column: x => x.ParentId,
                    principalTable: "Post",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateTable(
            name: "User",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                UserName = table.Column<string>(type: "TEXT", nullable: false),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Sex = table.Column<string>(type: "TEXT", nullable: false),
                Birthday = table.Column<DateTime>(type: "TEXT", nullable: true),
                Avatar = table.Column<string>(type: "TEXT", nullable: true),
                NormalizedUserName = table.Column<string>(type: "TEXT", nullable: false),
                Email = table.Column<string>(type: "TEXT", nullable: true),
                NormalizedEmail = table.Column<string>(type: "TEXT", nullable: true),
                EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                SecurityStamp = table.Column<string>(type: "TEXT", nullable: false),
                PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false),
                LockoutEnd = table.Column<DateTime>(type: "TEXT", nullable: true),
                LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                IsReadOnly = table.Column<bool>(type: "INTEGER", nullable: false),
                DepartmentId = table.Column<Guid>(type: "TEXT", nullable: true),
                PostId = table.Column<Guid>(type: "TEXT", nullable: true),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_User", x => x.Id);
                table.ForeignKey(
                    name: "FK_User_Department_DepartmentId",
                    column: x => x.DepartmentId,
                    principalTable: "Department",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
                table.ForeignKey(
                    name: "FK_User_Post_PostId",
                    column: x => x.PostId,
                    principalTable: "Post",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateTable(
            name: "ExternalApp",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Description = table.Column<string>(type: "TEXT", nullable: false),
                Icon = table.Column<string>(type: "TEXT", nullable: false),
                Home = table.Column<string>(type: "TEXT", nullable: false),
                Callback = table.Column<string>(type: "TEXT", nullable: false),
                Logout = table.Column<string>(type: "TEXT", nullable: true),
                ClientId = table.Column<string>(type: "TEXT", nullable: false),
                ClientSecret = table.Column<string>(type: "TEXT", nullable: false),
                Disabled = table.Column<bool>(type: "INTEGER", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ExternalApp", x => x.Id);
                table.ForeignKey(
                    name: "FK_ExternalApp_User_UserId",
                    column: x => x.UserId,
                    principalTable: "User",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "UserLogin",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                DisplayName = table.Column<string>(type: "TEXT", nullable: false),
                ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                Remark = table.Column<string>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserLogin", x => x.Id);
                table.ForeignKey(
                    name: "FK_UserLogin_User_UserId",
                    column: x => x.UserId,
                    principalTable: "User",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "UserRole",
            columns: table => new
            {
                UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                RoleId = table.Column<Guid>(type: "TEXT", nullable: false),
                IsReadOnly = table.Column<bool>(type: "INTEGER", nullable: false),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                table.ForeignKey(
                    name: "FK_UserRole_Role_RoleId",
                    column: x => x.RoleId,
                    principalTable: "Role",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_UserRole_User_UserId",
                    column: x => x.UserId,
                    principalTable: "User",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "WorkGroupUser",
            columns: table => new
            {
                WorkGroupId = table.Column<Guid>(type: "TEXT", nullable: false),
                UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                TenantNumber = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_WorkGroupUser", x => new { x.WorkGroupId, x.UserId });
                table.ForeignKey(
                    name: "FK_WorkGroupUser_User_UserId",
                    column: x => x.UserId,
                    principalTable: "User",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_WorkGroupUser_WorkGroup_WorkGroupId",
                    column: x => x.WorkGroupId,
                    principalTable: "WorkGroup",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Asset_CategoryId",
            table: "Asset",
            column: "CategoryId");

        migrationBuilder.CreateIndex(
            name: "IX_Asset_ParentId",
            table: "Asset",
            column: "ParentId");

        migrationBuilder.CreateIndex(
            name: "IX_Asset_TenantNumber_Number",
            table: "Asset",
            columns: new[] { "TenantNumber", "Number" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_AssetCategory_TenantNumber_Number",
            table: "AssetCategory",
            columns: new[] { "TenantNumber", "Number" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_AssetVersion_TenantNumber_Number",
            table: "AssetVersion",
            columns: new[] { "TenantNumber", "Number" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Department_ManagerId",
            table: "Department",
            column: "ManagerId");

        migrationBuilder.CreateIndex(
            name: "IX_Department_ParentId",
            table: "Department",
            column: "ParentId");

        migrationBuilder.CreateIndex(
            name: "IX_Department_PostId",
            table: "Department",
            column: "PostId");

        migrationBuilder.CreateIndex(
            name: "IX_Department_TenantNumber_Number",
            table: "Department",
            columns: new[] { "TenantNumber", "Number" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Dict_ParentId",
            table: "Dict",
            column: "ParentId");

        migrationBuilder.CreateIndex(
            name: "IX_Dict_TenantNumber_Number",
            table: "Dict",
            columns: new[] { "TenantNumber", "Number" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_EqpCategory_TenantNumber_Number",
            table: "EqpCategory",
            columns: new[] { "TenantNumber", "Number" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_EqpType_TenantNumber_Number",
            table: "EqpType",
            columns: new[] { "TenantNumber", "Number" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_ExternalApp_TenantNumber_Name",
            table: "ExternalApp",
            columns: new[] { "TenantNumber", "Name" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_ExternalApp_UserId",
            table: "ExternalApp",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_Permission_ParentId",
            table: "Permission",
            column: "ParentId");

        migrationBuilder.CreateIndex(
            name: "IX_Permission_TenantNumber_Number",
            table: "Permission",
            columns: new[] { "TenantNumber", "Number" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Post_DepartmentId",
            table: "Post",
            column: "DepartmentId");

        migrationBuilder.CreateIndex(
            name: "IX_Post_ParentId",
            table: "Post",
            column: "ParentId");

        migrationBuilder.CreateIndex(
            name: "IX_Post_TenantNumber_Number",
            table: "Post",
            columns: new[] { "TenantNumber", "Number" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Role_TenantNumber_Number",
            table: "Role",
            columns: new[] { "TenantNumber", "Number" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_RolePermission_PermissionId",
            table: "RolePermission",
            column: "PermissionId");

        migrationBuilder.CreateIndex(
            name: "IX_Tenant_Number",
            table: "Tenant",
            column: "Number",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_TimeRange_GroupId",
            table: "TimeRange",
            column: "GroupId");

        migrationBuilder.CreateIndex(
            name: "IX_UnitCategory_TenantNumber_Number",
            table: "UnitCategory",
            columns: new[] { "TenantNumber", "Number" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Uom_CategoryId",
            table: "Uom",
            column: "CategoryId");

        migrationBuilder.CreateIndex(
            name: "IX_Uom_TenantNumber_Number",
            table: "Uom",
            columns: new[] { "TenantNumber", "Number" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_User_DepartmentId",
            table: "User",
            column: "DepartmentId");

        migrationBuilder.CreateIndex(
            name: "IX_User_PostId",
            table: "User",
            column: "PostId");

        migrationBuilder.CreateIndex(
            name: "IX_User_TenantNumber_NormalizedUserName",
            table: "User",
            columns: new[] { "TenantNumber", "NormalizedUserName" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_UserLogin_UserId",
            table: "UserLogin",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_UserRole_RoleId",
            table: "UserRole",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "IX_WorkGroup_ParentId",
            table: "WorkGroup",
            column: "ParentId");

        migrationBuilder.CreateIndex(
            name: "IX_WorkGroup_TenantNumber_Number",
            table: "WorkGroup",
            columns: new[] { "TenantNumber", "Number" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_WorkGroupUser_UserId",
            table: "WorkGroupUser",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_Workstation_CategoryId",
            table: "Workstation",
            column: "CategoryId");

        migrationBuilder.CreateIndex(
            name: "IX_Workstation_ParentId",
            table: "Workstation",
            column: "ParentId");

        migrationBuilder.CreateIndex(
            name: "IX_Workstation_TenantNumber_Number",
            table: "Workstation",
            columns: new[] { "TenantNumber", "Number" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_WorkstationCategory_TenantNumber_Number",
            table: "WorkstationCategory",
            columns: new[] { "TenantNumber", "Number" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_WorkstationDevice_AssetId",
            table: "WorkstationDevice",
            column: "AssetId");

        migrationBuilder.CreateIndex(
            name: "IX_WorkstationTimeGroup_WorkstationId",
            table: "WorkstationTimeGroup",
            column: "WorkstationId");

        migrationBuilder.AddForeignKey(
            name: "FK_Department_Post_PostId",
            table: "Department",
            column: "PostId",
            principalTable: "Post",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Department_User_ManagerId",
            table: "Department",
            column: "ManagerId",
            principalTable: "User",
            principalColumn: "Id",
            onDelete: ReferentialAction.SetNull);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Department_Post_PostId",
            table: "Department");

        migrationBuilder.DropForeignKey(
            name: "FK_User_Post_PostId",
            table: "User");

        migrationBuilder.DropForeignKey(
            name: "FK_Department_User_ManagerId",
            table: "Department");

        migrationBuilder.DropTable(
            name: "AccountsPayableChecking");

        migrationBuilder.DropTable(
            name: "AccountsPayableStatistics");

        migrationBuilder.DropTable(
            name: "AccountsReceivableStatistics");

        migrationBuilder.DropTable(
            name: "AssetVersion");

        migrationBuilder.DropTable(
            name: "Custom");

        migrationBuilder.DropTable(
            name: "CustomerAnalysis");

        migrationBuilder.DropTable(
            name: "Dict");

        migrationBuilder.DropTable(
            name: "EqpCategory");

        migrationBuilder.DropTable(
            name: "EqpType");

        migrationBuilder.DropTable(
            name: "ExternalApp");

        migrationBuilder.DropTable(
            name: "FinancialRevenueExpenseKanban");

        migrationBuilder.DropTable(
            name: "FollowUpRecord");

        migrationBuilder.DropTable(
            name: "InputInvoice");

        migrationBuilder.DropTable(
            name: "InventoryAllocation");

        migrationBuilder.DropTable(
            name: "InventoryCheck");

        migrationBuilder.DropTable(
            name: "InventoryInOutStatistics");

        migrationBuilder.DropTable(
            name: "InventoryStatisticsKanban");

        migrationBuilder.DropTable(
            name: "KanbanHomepage");

        migrationBuilder.DropTable(
            name: "LoginProvider");

        migrationBuilder.DropTable(
            name: "OeeConfiguration");

        migrationBuilder.DropTable(
            name: "OtherInboundOrder");

        migrationBuilder.DropTable(
            name: "OtherOutboundOrder");

        migrationBuilder.DropTable(
            name: "PaymentOrder");

        migrationBuilder.DropTable(
            name: "PaymentReceipt");

        migrationBuilder.DropTable(
            name: "PeriodStartEnd");

        migrationBuilder.DropTable(
            name: "ProductBom");

        migrationBuilder.DropTable(
            name: "ProductInfo");

        migrationBuilder.DropTable(
            name: "ProductionDataStatistics");

        migrationBuilder.DropTable(
            name: "ProductionExecutionTracking");

        migrationBuilder.DropTable(
            name: "ProductionMaterialPick");

        migrationBuilder.DropTable(
            name: "ProductionMaterialReturn");

        migrationBuilder.DropTable(
            name: "ProductionMaterialStatistics");

        migrationBuilder.DropTable(
            name: "ProductionOrder");

        migrationBuilder.DropTable(
            name: "ProductionPlan");

        migrationBuilder.DropTable(
            name: "ProductionPlanBoard");

        migrationBuilder.DropTable(
            name: "ProductionReport");

        migrationBuilder.DropTable(
            name: "ProductionStatisticsKanban");

        migrationBuilder.DropTable(
            name: "ProductionStorage");

        migrationBuilder.DropTable(
            name: "ProductionTaskPool");

        migrationBuilder.DropTable(
            name: "ProductList");

        migrationBuilder.DropTable(
            name: "PrudoctionProcess");

        migrationBuilder.DropTable(
            name: "PruductCategory");

        migrationBuilder.DropTable(
            name: "PruductionTeam");

        migrationBuilder.DropTable(
            name: "PurchaseDemandStatistics");

        migrationBuilder.DropTable(
            name: "PurchaseExecutionTrackingBoard");

        migrationBuilder.DropTable(
            name: "PurchaseOrder");

        migrationBuilder.DropTable(
            name: "PurchaseOrderKanban");

        migrationBuilder.DropTable(
            name: "PurchaseOrderStatistics");

        migrationBuilder.DropTable(
            name: "PurchaseRequest");

        migrationBuilder.DropTable(
            name: "PurchaseReturn");

        migrationBuilder.DropTable(
            name: "PurchaseReturnPayable");

        migrationBuilder.DropTable(
            name: "PurchaseStorage");

        migrationBuilder.DropTable(
            name: "PurchaseStoragePayable");

        migrationBuilder.DropTable(
            name: "QuotationOrder");

        migrationBuilder.DropTable(
            name: "QuotationStatistics");

        migrationBuilder.DropTable(
            name: "ReceivableAccountChecking");

        migrationBuilder.DropTable(
            name: "RolePermission");

        migrationBuilder.DropTable(
            name: "SaleDelivery");

        migrationBuilder.DropTable(
            name: "SaleOrder");

        migrationBuilder.DropTable(
            name: "SaleOrderStatistics");

        migrationBuilder.DropTable(
            name: "SaleReturn");

        migrationBuilder.DropTable(
            name: "SalesExecutionTrackingBoard");

        migrationBuilder.DropTable(
            name: "SalesInvoice");

        migrationBuilder.DropTable(
            name: "SalesOrderKanban");

        migrationBuilder.DropTable(
            name: "SalesOutboundReceivable");

        migrationBuilder.DropTable(
            name: "SalesReturnReceivable");

        migrationBuilder.DropTable(
            name: "SchemeDesign");

        migrationBuilder.DropTable(
            name: "SupplierInfo");

        migrationBuilder.DropTable(
            name: "SupplierInfoQuery");

        migrationBuilder.DropTable(
            name: "SupplierPriceList");

        migrationBuilder.DropTable(
            name: "Tenant");

        migrationBuilder.DropTable(
            name: "TimeRange");

        migrationBuilder.DropTable(
            name: "Uom");

        migrationBuilder.DropTable(
            name: "UserLogin");

        migrationBuilder.DropTable(
            name: "UserRole");

        migrationBuilder.DropTable(
            name: "WarehouseInfo");

        migrationBuilder.DropTable(
            name: "WarehouseLocationInfo");

        migrationBuilder.DropTable(
            name: "WarehouseTransactionKanban");

        migrationBuilder.DropTable(
            name: "WorkGroupUser");

        migrationBuilder.DropTable(
            name: "WorkstationDevice");

        migrationBuilder.DropTable(
            name: "WorkstationDeviceLog");

        migrationBuilder.DropTable(
            name: "Permission");

        migrationBuilder.DropTable(
            name: "WorkstationTimeGroup");

        migrationBuilder.DropTable(
            name: "UnitCategory");

        migrationBuilder.DropTable(
            name: "Role");

        migrationBuilder.DropTable(
            name: "WorkGroup");

        migrationBuilder.DropTable(
            name: "Asset");

        migrationBuilder.DropTable(
            name: "Workstation");

        migrationBuilder.DropTable(
            name: "AssetCategory");

        migrationBuilder.DropTable(
            name: "WorkstationCategory");

        migrationBuilder.DropTable(
            name: "Post");

        migrationBuilder.DropTable(
            name: "User");

        migrationBuilder.DropTable(
            name: "Department");
    }
}
