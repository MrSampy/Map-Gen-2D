using Course_work.Map.Helpers.Storages;
namespace Course_work.Map.Helpers;

public class TilesHeat
{
    public Enums.HeatType THeat { get; set; }
        public Color _Color;
        public TilesHeat(Enums.HeatType heat, Color color)
        {
            THeat = heat;
            _Color = color;
        }
}