using Wta.Application.Default.Data;
using Wta.Application.Mes.Domain;

namespace Wta.Application.Mes.Data;

public class MesDbConfig : BaseDbConfig<DefaultDbContext>,
    IEntityTypeConfiguration<Material>,
    IEntityTypeConfiguration<Product>,
    IEntityTypeConfiguration<Bom>,
    IEntityTypeConfiguration<Technology>

{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
    }

    public void Configure(EntityTypeBuilder<Product> builder)
    {
    }

    public void Configure(EntityTypeBuilder<Bom> builder)
    {
    }

    public void Configure(EntityTypeBuilder<Technology> builder)
    {
    }
}
