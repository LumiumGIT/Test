namespace Domain.Entities.Portal.Public;

public class BaRiskCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Score { get; set; }

    public ICollection<BusinessActivity> BusinessActivities { get; set; } = new List<BusinessActivity>();
}