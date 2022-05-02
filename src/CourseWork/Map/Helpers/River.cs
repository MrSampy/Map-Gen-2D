namespace CourseWork.Map.Helpers;

public class River
{
    public List<Tile> _River = new List<Tile>();

    public void JoinRivers(List<Tile> river)
    {
        foreach (var tile in river)
            _River.Add(tile);
    }
}