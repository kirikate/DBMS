namespace DbmsApp.Models;

public class ReadableGood
{
	public long Id { get; set; }
	public string Name { get; set; } = null!;
	public string? Ingredients { get; set; }
	public decimal Price { get; set; }
	public string? Size { get; set; }
}