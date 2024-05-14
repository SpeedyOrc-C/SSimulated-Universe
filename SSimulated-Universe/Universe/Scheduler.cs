namespace SSimulated_Universe.Universe;

public class Scheduler<T> where T : IRunner
{
    public double OverallTimeRun { get; private set; }
    
    /// <summary>
    /// Let the runners run and find out who reaches the destination first.
    /// That winner is then sent back to the starting line.
    /// </summary>
    /// <returns>Runner that reaches the destination first.</returns>
    /// <exception cref="Exception">When there are no runners.</exception>
    public T Schedule(IEnumerable<T> enumerableRunners)
    {
        var runners = enumerableRunners.ToList();
        
        var winner = runners.MinBy(t => t.TimeLeft);
        
        if (winner is null)
            throw new Exception("Cannot determine the next entity since I saw nothing.");

        var timeRun = winner.TimeLeft;
        OverallTimeRun += timeRun;
        
        foreach (var runner in runners)
            if (ReferenceEquals(winner, runner))
                runner.GoBack();
            else
                runner.Advance(runner.RunnerSpeed * timeRun);

        return winner;
    }
}
