using System.Collections.Generic;
using NUnit.Framework;

public class CarModuleControllerTests
{
    [Test]
    public void Initialize_CallsInitializeForEveryModule()
    {
        var moduleA = new FakeCarModule();
        var moduleB = new FakeCarModule();
        var controller = new CarModuleController<FakeCarModule>(new List<FakeCarModule> { moduleA, moduleB });

        controller.Initialize();

        Assert.That(moduleA.InitializeCalls, Is.EqualTo(1));
        Assert.That(moduleB.InitializeCalls, Is.EqualTo(1));
        Assert.That(moduleA.TickCalls, Is.EqualTo(0));
        Assert.That(moduleB.TickCalls, Is.EqualTo(0));
        Assert.That(moduleA.DisposeCalls, Is.EqualTo(0));
        Assert.That(moduleB.DisposeCalls, Is.EqualTo(0));
    }

    [Test]
    public void Tick_CallsTickForEveryModule()
    {
        var moduleA = new FakeCarModule();
        var moduleB = new FakeCarModule();
        var controller = new CarModuleController<FakeCarModule>(new List<FakeCarModule> { moduleA, moduleB });

        controller.Tick();

        Assert.That(moduleA.TickCalls, Is.EqualTo(1));
        Assert.That(moduleB.TickCalls, Is.EqualTo(1));
        Assert.That(moduleA.InitializeCalls, Is.EqualTo(0));
        Assert.That(moduleB.InitializeCalls, Is.EqualTo(0));
        Assert.That(moduleA.DisposeCalls, Is.EqualTo(0));
        Assert.That(moduleB.DisposeCalls, Is.EqualTo(0));
    }

    [Test]
    public void Dispose_CallsDisposeForEveryModule()
    {
        var moduleA = new FakeCarModule();
        var moduleB = new FakeCarModule();
        var controller = new CarModuleController<FakeCarModule>(new List<FakeCarModule> { moduleA, moduleB });

        controller.Dispose();

        Assert.That(moduleA.DisposeCalls, Is.EqualTo(1));
        Assert.That(moduleB.DisposeCalls, Is.EqualTo(1));
        Assert.That(moduleA.InitializeCalls, Is.EqualTo(0));
        Assert.That(moduleB.InitializeCalls, Is.EqualTo(0));
        Assert.That(moduleA.TickCalls, Is.EqualTo(0));
        Assert.That(moduleB.TickCalls, Is.EqualTo(0));
    }

    private sealed class FakeCarModule : ICarModule
    {
        public int InitializeCalls { get; private set; }
        public int TickCalls { get; private set; }
        public int DisposeCalls { get; private set; }

        public void Initialize()
        {
            InitializeCalls++;
        }

        public void Tick()
        {
            TickCalls++;
        }

        public void Dispose()
        {
            DisposeCalls++;
        }
    }
}
