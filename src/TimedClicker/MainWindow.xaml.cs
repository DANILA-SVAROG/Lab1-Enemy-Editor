using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ClickerMechanics;

namespace TimedClicker;

public partial class MainWindow : Window
{
    private readonly DispatcherTimer timer;
    private readonly Stopwatch clock = new Stopwatch();
    private CController? controller;

    public MainWindow()
    {
        InitializeComponent();
        timer = new DispatcherTimer();
        timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
        timer.Tick += Timer_Tick;
        Closed += (sender, e) => timer.Stop();
    }

    private void StartButton_Click(object sender, RoutedEventArgs e)
    {
        controller = new CController(0.8, 30, new Size(SceneCanvas.ActualWidth, SceneCanvas.ActualHeight), BonusesCheckBox.IsChecked == true);
        controller.start();
        clock.Restart();
        timer.Start();
        StartButton.IsEnabled = false;
        BonusesCheckBox.IsEnabled = false;
        StatusTextBlock.Text = "Нажимайте на круги, чтобы набрать очки.";
        DrawScene();
    }

    private void StopButton_Click(object sender, RoutedEventArgs e)
    {
        controller?.stop();
        timer.Stop();
        StartButton.IsEnabled = true;
        BonusesCheckBox.IsEnabled = true;
        StatusTextBlock.Text = "Раунд остановлен.";
        DrawScene();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        UpdateRound();
        DrawScene();
    }

    // Проверяет прошедшее время и при клике, чтобы не собирать круги после конца.
    private void UpdateRound()
    {
        if (controller is null) return;
        controller.update(clock.Elapsed.TotalSeconds);
        clock.Restart();
        if (!controller.isRunning())
        {
            timer.Stop();
            StartButton.IsEnabled = true;
            BonusesCheckBox.IsEnabled = true;
            StatusTextBlock.Text = $"Время вышло! Набрано очков: {controller.getPoints():F1}.";
        }
    }

    private void SceneCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        UpdateRound();
        controller?.mouseClick(e.GetPosition(SceneCanvas));
        DrawScene();
    }

    private void SceneCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        controller?.setSceneSize(e.NewSize);
    }

    // Окно только рисует объекты, которыми управляет контроллер.
    private void DrawScene()
    {
        SceneCanvas.Children.Clear();
        if (controller is null) return;
        foreach (CCollectable obj in controller.getObjects())
        {
            SceneCanvas.Children.Add(obj.getSprite());
        }

        TimeTextBlock.Text = $"Время: {controller.getTimeLeft():F1} с";
        PointsTextBlock.Text = $"Очки: {controller.getPoints():F1}";
        CooldownTextBlock.Text = $"Перезарядка: {controller.getPlayer().getTimeBeforeClick():F2} с; осталось: {controller.getPlayer().getRemainingCooldown():F2} с";
        SettingsTextBlock.Text = $"Появление: {controller.getSpawnRate():F2} с; время жизни: ×{controller.getLifetimeModifier():F2}";
    }
}
