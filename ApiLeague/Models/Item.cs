public class Item
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int Custo { get; set; }
    public List<Tag> Tags { get; set; } = new List<Tag>();
    public List<ItemStats> Stats { get; set; } = new List<ItemStats>();
}
