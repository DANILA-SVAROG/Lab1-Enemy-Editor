using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ClickerMechanics;

public class CObject
{
    private Point position;
    private Size size;
    private double lifetime;
    private double pointsValue;
    private Ellipse sprite;

    public CObject(Point position, double size, double lifetime)
    {
        this.position = position;
        this.size = new Size(size, size);
        this.lifetime = lifetime;
        sprite = new Ellipse();
        sprite.Fill = Brushes.BlueViolet;
        sprite.StrokeThickness = 2;
        sprite.Stroke = Brushes.Black;
        sprite.HorizontalAlignment = HorizontalAlignment.Center;
        sprite.VerticalAlignment = VerticalAlignment.Center;
        sprite.Width = this.size.Width;
        sprite.Height = this.size.Height;
        sprite.RenderTransform = new TranslateTransform(position.X, position.Y);
        pointsValue = ((1 / this.size.Width) / lifetime) * 1000;
    }

    public Ellipse getSprite()
    {
        return sprite;
    }

    public double getPointsValue()
    {
        return pointsValue;
    }

    // Проверяет попадание в круг, а не только в его прямоугольник.
    public bool isMouseOnObject(Point mousePosition)
    {
        double radius = size.Width / 2;
        double x = mousePosition.X - position.X - radius;
        double y = mousePosition.Y - position.Y - radius;
        return x * x + y * y <= radius * radius;
    }

    public bool UpdateLifetime(double delta)
    {
        lifetime = Math.Max(0, lifetime - delta);
        return lifetime <= 0;
    }

    // Оставляет круг внутри сцены при изменении размера окна.
    public void fitToScene(Size sceneSize)
    {
        position.X = Math.Clamp(position.X, 0, Math.Max(0, sceneSize.Width - size.Width));
        position.Y = Math.Clamp(position.Y, 0, Math.Max(0, sceneSize.Height - size.Height));
        sprite.RenderTransform = new TranslateTransform(position.X, position.Y);
    }
}
