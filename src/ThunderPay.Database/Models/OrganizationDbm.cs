using System.ComponentModel.DataAnnotations.Schema;

namespace ThunderPay.Database.Models;

[Table("organizations")]
public class OrganizationDbm
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<MerchantDbm>? Merchants { get; set; }
}
