using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Tests.Schedule;

[TestClass]
[TestSubject(typeof(Scheduler<FakeRunner>))]
public class SchedulerTest
{
    private static void AssertRatio(int a, int b, int x, int y, double eta = 0.01) => 
        Assert.IsTrue(Math.Abs((double) a / b - (double) x / y) <= eta);

    private class FakeRunner : IRunner
    {
        public double RunnerSpeed { get; }
        public double RunnerDistance { get; set; }
        
        public FakeRunner(double runnerSpeed)
        {
            RunnerSpeed = runnerSpeed;
            RunnerDistance = 0;
        }
    }

    [TestMethod]
    public void Normal()
    {
        var a = new FakeRunner(100);
        var b = new FakeRunner(150);
        var c = new FakeRunner(200);

        var runners = new HashSet<FakeRunner> { a, b, c };
        var scheduler = new Scheduler<FakeRunner>();

        var countA = 0;
        var countB = 0;
        var countC = 0;

        for (var i = 1; i <= 1000; i += 1)
        {
            var chosen = scheduler.Schedule(runners);
            if (chosen == a) countA += 1;
            else if (chosen == b) countB += 1;
            else if (chosen == c) countC += 1;
        }
        
        AssertRatio(countA, countB, 100, 150);
        AssertRatio(countB, countC, 150, 200);
        AssertRatio(countA, countC, 100, 200);
    }
}
