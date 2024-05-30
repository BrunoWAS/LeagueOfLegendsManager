public class Campeao
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Title { get; set; }
    public List<Classe> Classes { get; set; } = new List<Classe>();
}
