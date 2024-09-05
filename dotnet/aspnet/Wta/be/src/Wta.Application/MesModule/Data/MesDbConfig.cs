using Wta.Application.MesModule.Domain;
using Wta.Application.SystemModule.Data;

namespace Wta.Application.MesModule.Data;

public class MesDbConfig : BaseDbConfig<SystemDbContext>,
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
