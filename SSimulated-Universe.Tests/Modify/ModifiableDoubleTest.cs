using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSimulated_Universe.Utility.Modifiable.Number;

namespace SSimulated_Universe.Tests.Modify;

[TestClass]
[TestSubject(typeof(ModifiableDouble))]
public class ModifiableDoubleTest
{
    private readonly ModifiableDouble x = new(1000);
    private readonly ModifiableDouble y = new(500);
    private readonly ModifiableDouble z = new(100);
    
    private readonly ModifierDoubleImmediate plus10 = new(10.0);
    private readonly ModifierDoubleImmediate plus20 = new(20.0);
    private readonly ModifierDoubleProportion prop10 = new(0.1);
    private readonly ModifierDoubleProportion prop20 = new(0.2);
    
    [TestMethod]
    public void Normal()
    {
        plus10.Modify(x);
        plus20.Modify(x);
        prop10.Modify(x);
        prop20.Modify(x);
        Assert.AreEqual(1330.0, x.Eval);
        
        prop10.DismissAll();
        Assert.AreEqual(1230.0, x.Eval);
        
        plus20.DismissAll();
        Assert.AreEqual(1210.0, x.Eval);
    }

    [TestMethod]
    public void OnlyModifyOnce()
    {
        plus10.Modify(x);
        plus10.Modify(x);
        plus10.Modify(x);
        Assert.AreEqual(1010.0, x.Eval);
    }

    [TestMethod]
    public void ModifyAndDismissMultiple()
    {
        plus10.Modify(x);
        plus10.Modify(y);
        plus10.Modify(z);
        Assert.AreEqual(1010.0, x.Eval);
        Assert.AreEqual(510.0, y.Eval);
        Assert.AreEqual(110.0, z.Eval);
        
        plus10.Dismiss(y);
        Assert.AreEqual(1010.0, x.Eval);
        Assert.AreEqual(500.0, y.Eval);
        Assert.AreEqual(110.0, z.Eval);
        
        plus10.DismissAll();
        Assert.AreEqual(1000.0, x.Eval);
        Assert.AreEqual(500.0, y.Eval);
        Assert.AreEqual(100.0, z.Eval);
    }
}