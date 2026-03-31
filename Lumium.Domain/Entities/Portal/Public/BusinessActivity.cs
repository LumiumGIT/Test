namespace Domain.Entities.Portal.Public;

public class BusinessActivity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Valid { get; set; } = true;
    public DateOnly? ValidTo { get; set; }

    public int RiskCategoryId { get; set; }
    public BaRiskCategory RiskCategory { get; set; } = null!;
}