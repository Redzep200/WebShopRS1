namespace WebShopSportskeOpreme.Interfaces
{
    public interface ISoftDeletableEntity
    {
        bool IsDeleted { get; set; }
        DateTime? DeletionDate { get; set; }
    }
}
