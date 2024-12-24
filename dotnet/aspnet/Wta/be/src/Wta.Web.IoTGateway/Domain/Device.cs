using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wta.Web.IoTGateway.Infrastructure.Drivers;

namespace Wta.Web.IoTGateway.Domain;

public class Device : BaseEntity, IEntityTypeConfiguration<Device>
{
    public string Name { get; set; } = default!;
    public string Number { get; set; } = default!;

    public string Driver { get; set; } = default!;
    public bool Paused { get; set; }

    public int Interval { get; set; } = 1;

    public bool UploadChangesOnly { get; set; }

    public bool Enabled { get; set; } = true;
    public List<Data> Datas { get; set; } = [];

    public void Configure(EntityTypeBuilder<Device> builder)
    {
    }
}

public class Data : BaseEntity, IEntityTypeConfiguration<Data>
{
    public string Key { get; set; } = default!;
    public string Address { get; set; } = default!;
    public int ByteLength { get; set; } = 1;
    public DataFormat DataFormat { get; set; }
    public string? Value { get; set; }
    public Guid DeviceId { get; set; }
    public Device? Device { get; set; }

    public void Configure(EntityTypeBuilder<Data> builder)
    {
        builder.HasOne(o => o.Device)
            .WithMany(o => o.Datas)
            .HasForeignKey(o => o.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
