namespace Domain.Entities.Portal.Public;

public class Country
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string IsoCode { get; set; } = string.Empty;

    public int RiskCategoryId { get; set; }
    public CountryRiskCategory RiskCategory { get; set; } = null!;
}