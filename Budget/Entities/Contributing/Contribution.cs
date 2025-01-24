namespace Budget.Entities.Contributing;

public class Contribution
{
    public int Id { get; set; }
    public double Amount { get; set; }
    public int ContributorId { get; set; }
    public int EventId { get; set; }
    public ICollection<Contributor> Contributors { get; set; }
}