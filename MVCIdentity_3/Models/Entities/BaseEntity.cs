using MVCIdentity_3.Models.Enums;
using MVCIdentity_3.Models.Interfaces;

namespace MVCIdentity_3.Models.Entities
{
    public abstract class BaseEntity : IEntity
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DataStatus Status { get; set; }
    }
}
