using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MVCIdentity_3.Models.Entities;

namespace MVCIdentity_3.Models.Configurations
{
    public class OrderConfiguration : BaseConfiguration<Order>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            base.Configure(builder);
            builder.HasMany(x => x.OrderDetails).WithOne(x => x.Order).HasForeignKey(x => x.OrderId).IsRequired();
        }
    }
}
