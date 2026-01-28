public class PieChartDrawable : IDrawable
{
    public IList<double> Values { get; set; } = new List<double>();

    private readonly Color[] _colors =
    {
        Colors.Red, Colors.Orange, Colors.Yellow,
        Colors.Green, Colors.Blue, Colors.Purple
    };

    public void Draw(ICanvas canvas, RectF rect)
    {
        if (Values.Count == 0)
            return;

        float radius = Math.Min(rect.Width, rect.Height) / 2 - 10;
        float cx = rect.Center.X;
        float cy = rect.Center.Y;

        double total = Values.Sum();
        float start = 0;
        int i = 0;

        foreach (var value in Values)
        {
            float sweep = (float)(value / total * 360);
            canvas.FillColor = _colors[i++ % _colors.Length];
            canvas.FillArc(cx - radius, cy - radius,
                           radius * 2, radius * 2,
                           start, sweep, true);
            start += sweep;
        }
    }
}
