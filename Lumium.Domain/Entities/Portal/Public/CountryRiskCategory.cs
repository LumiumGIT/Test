namespace Domain.Entities.Portal.Public;

public class CountryRiskCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Score { get; set; }

    public ICollection<Country> Countries { get; set; } = new List<Country>();
}