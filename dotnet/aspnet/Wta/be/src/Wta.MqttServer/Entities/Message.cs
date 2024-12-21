using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Wta.MqttServer.Entities;

public class Message : IEntityTypeConfiguration<Message>
{
    public Guid Id { get; set; }
    public string Topic { get; set; } = default!;
    public string Value { get; set; } = default!;

    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(e => e.Id);
    }
}
