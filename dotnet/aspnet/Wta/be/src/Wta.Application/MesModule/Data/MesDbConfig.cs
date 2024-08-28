using Wta.Application.MesModule.Domain;
using Wta.Application.BaseModule.Data;

namespace Wta.Application.MesModule.Data;

public class MesDbConfig : BaseDbConfig<BaseDbContext>,
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
