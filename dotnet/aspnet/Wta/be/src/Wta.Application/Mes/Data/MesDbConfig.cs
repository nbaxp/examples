using Wta.Application.Mes.Domain;
using Wta.Application.System.Data;

namespace Wta.Application.Mes.Data;

public class MesDbConfig : BaseDbConfig<DefaultDbContext>,
    IEntityTypeConfiguration<Material>,
    IEntityTypeConfiguration<Bom>,
    IEntityTypeConfiguration<Technology>

{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
    }

    public void Configure(EntityTypeBuilder<Bom> builder)
    {
    }

    public void Configure(EntityTypeBuilder<Technology> builder)
    {
    }
}
