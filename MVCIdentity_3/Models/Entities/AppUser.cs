using Microsoft.AspNetCore.Identity;
using MVCIdentity_3.Models.Enums;
using MVCIdentity_3.Models.Interfaces;

namespace MVCIdentity_3.Models.Entities
{
    public class AppUser : IdentityUser<int>, IEntity //BU generic şekilde bir argüman verdigimiz primary key yani Id bizim istedigimiz tip olarak miras verilir..Yani su anda Id property'imiz int tipindedir...Dikkat edin eger böyle vermezseniz o property default olarak string olur...
    {
        public AppUser()
        {
            CreatedDate = DateTime.Now;
            Status = DataStatus.Inserted;
        }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DataStatus Status { get; set; }

        //Relational Properties (Burada Identity'nin kendi Relation sistemine uymak en saglıklısıdır)
        public virtual AppUserProfile AppUserProfile { get; set; }
        public virtual ICollection<AppUserRole> UserRoles { get; set; }
        public virtual ICollection<Order> Orders { get; set; }


    }
}
