namespace galagatests;
using NUnit.Framework;
using System.IO;
using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Math;
using DIKUArcade.Events;
using DIKUArcade.Input;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Physics;
using System.Collections.Generic;
using Galaga.MovementStrategy;
using Galaga.Squadron;
using Galaga;

public class TestHealth {
    private Health testSubject;
    [SetUp]
    public void Setup()
    {
        DIKUArcade.GUI.Window.CreateOpenGLContext();
        testSubject = new Health(new Vec2F(0.03f, -0.3f), new Vec2F(0.4f, 0.4f));
    }

    [Test]
    public void TestAliveness()
    {
        Assert.AreEqual(testSubject.isDead(), false);
        testSubject.LoseHealth();
        Assert.AreEqual(testSubject.isDead(), false);
    }

    [Test]
    public void TestDeath() {
        testSubject.LoseHealth();
        testSubject.LoseHealth();
        testSubject.LoseHealth();
        Assert.AreEqual(testSubject.isDead(), true);
    }
}