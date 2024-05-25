namespace SSimulated_Universe.Universe;

public interface IRunner
{
    double RunnerSpeed { get; }
    double RunnerDistance { get; set; }

    public double TimeLeft => (1 - RunnerDistance) / RunnerSpeed;
    public double TimeRun => RunnerDistance / RunnerSpeed;

    public void Advance(double distance) => RunnerDistance = Math.Min(1, RunnerDistance + distance);
    public void Delay(double distance) => Advance(-distance);
    public void GoBack() => RunnerDistance = 0;
}
