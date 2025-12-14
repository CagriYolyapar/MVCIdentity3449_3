using Microsoft.AspNetCore.Identity;
using MVCIdentity_3.Models.Enums;
using MVCIdentity_3.Models.Interfaces;

namespace MVCIdentity_3.Models.Entities
{
    public class AppUserRole : IdentityUserRole<int>, IEntity
    {

        public AppUserRole()
        {
            CreatedDate = DateTime.Now;
            Status = DataStatus.Inserted;
            
            
        }
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DataStatus Status { get; set; }

        //Relational Properties
        public virtual AppUser User { get; set; } //Bu isim standartlarına dikkat etmezseniz Identity ek gereksiz key sütunları olusturacaktır...
        public virtual AppRole Role { get; set; }

    }
}
