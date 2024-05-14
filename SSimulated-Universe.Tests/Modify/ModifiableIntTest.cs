using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSimulated_Universe.Modifiable;
using SSimulated_Universe.Modifiable.Number;

namespace SSimulated_Universe.Tests.Modify;

[TestClass]
[TestSubject(typeof(ModifiableInt))]
public class ModifiableIntTest
{
    private readonly ModifiableInt x = new(1000);
    private readonly ModifiableInt y = new(500);
    private readonly ModifiableInt z = new(100);
    
    private readonly ModifierIntImmediate plus10 = new(10);
    private readonly ModifierIntImmediate plus20 = new(20);
    private readonly ModifierIntProportion prop10 = new(0.1);
    private readonly ModifierIntProportion prop20 = new(0.2);
    
    [TestMethod]
    public void Normal()
    {
        plus10.Modify(x);
        plus20.Modify(x);
        prop10.Modify(x);
        prop20.Modify(x);
        Assert.AreEqual(1330, x.Eval);
        
        prop10.DismissAll();
        Assert.AreEqual(1230, x.Eval);
        
        plus20.DismissAll();
        Assert.AreEqual(1210, x.Eval);
    }

    [TestMethod]
    public void OnlyModifyOnce()
    {
        plus10.Modify(x);
        plus10.Modify(x);
        plus10.Modify(x);
        Assert.AreEqual(1010, x.Eval);
    }

    [TestMethod]
    public void ModifyAndDismissMultiple()
    {
        plus10.Modify(x);
        plus10.Modify(y);
        plus10.Modify(z);
        Assert.AreEqual(1010, x.Eval);
        Assert.AreEqual(510, y.Eval);
        Assert.AreEqual(110, z.Eval);
        
        plus10.Dismiss(y);
        Assert.AreEqual(1010, x.Eval);
        Assert.AreEqual(500, y.Eval);
        Assert.AreEqual(110, z.Eval);
        
        plus10.DismissAll();
        Assert.AreEqual(1000, x.Eval);
        Assert.AreEqual(500, y.Eval);
        Assert.AreEqual(100, z.Eval);
    }
}