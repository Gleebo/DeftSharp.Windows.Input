using System.Windows.Input;
using DeftSharp.Windows.Input.Keyboard;

namespace DeftSharp.Windows.Input.Tests.Keyboard;

public class ManipulatorTests
{
    private readonly WPFEmulator _emulator;

    public ManipulatorTests()
    {
        _emulator = new WPFEmulator();
    }

    [Fact]
    public void Manipulator_Test1()
    {
        var keyboardManipulator1 = new KeyboardManipulator();
        var keyboardManipulator2 = new KeyboardManipulator();

        _emulator.Run(() =>
        {
            keyboardManipulator1.Prevent(Key.A);
            keyboardManipulator2.Prevent(Key.A);
            
            Assert.Equal(keyboardManipulator1.LockedKeys.Count(), keyboardManipulator2.LockedKeys.Count());
            
            keyboardManipulator1.ReleaseAll();
        });
    }
    
    [Fact]
    public void Manipulator_Test2()
    {
        var keyboardManipulator1 = new KeyboardManipulator();
        var keyboardManipulator2 = new KeyboardManipulator();

        _emulator.Run(() =>
        {
            keyboardManipulator1.Prevent(Key.A);
            keyboardManipulator2.Prevent(Key.A);
            
            Assert.Equal(keyboardManipulator1.LockedKeys.Count(), keyboardManipulator2.LockedKeys.Count());
            
            keyboardManipulator1.ReleaseAll();
            
            Assert.Empty(keyboardManipulator2.LockedKeys);
        });
    }
    
    [Fact]
    public void Manipulator_Test3()
    {
        var keyboardManipulator1 = new KeyboardManipulator();
        var keyboardManipulator2 = new KeyboardManipulator();
        var keyboardManipulator3 = new KeyboardManipulator();

        _emulator.Run(() =>
        {
            keyboardManipulator1.Prevent(Key.Q);
            keyboardManipulator2.Prevent(Key.W);
            keyboardManipulator3.Prevent(Key.R);
            
            Assert.Equal(keyboardManipulator1.LockedKeys.Count(), keyboardManipulator2.LockedKeys.Count());
            Assert.Equal(keyboardManipulator2.LockedKeys.Count(), keyboardManipulator3.LockedKeys.Count());
            
            keyboardManipulator1.ReleaseAll();
            
            Assert.Empty(keyboardManipulator2.LockedKeys);
        });
    }
    
    [Fact]
    public void Manipulator_Test4()
    {
        var keyboardManipulator1 = new KeyboardManipulator();
        var keyboardManipulator2 = new KeyboardManipulator();
        var keyboardManipulator3 = new KeyboardManipulator();

        _emulator.Run(() =>
        {
            keyboardManipulator1.Prevent(Key.Q);
            keyboardManipulator2.Prevent(Key.W);

            keyboardManipulator3.ReleaseAll();
            
            Assert.Empty(keyboardManipulator1.LockedKeys);
            Assert.Empty(keyboardManipulator2.LockedKeys);
        });
    }
    
    [Fact]
    public void Manipulator_Test5()
    {
        _emulator.Run(() =>
        {
            using (var keyboardManipulator1 = new KeyboardManipulator())
            {
                using (var keyboardManipulator2 = new KeyboardManipulator())
                {
                    using (var keyboardManipulator3 = new KeyboardManipulator())
                    {
                        keyboardManipulator1.Prevent(Key.Q);
                        keyboardManipulator2.Prevent(Key.W);
                    }
                    
                    Assert.Empty(keyboardManipulator2.LockedKeys);
                }
            }
        });
    }
    
    [Fact]
    public void Manipulator_Test6()
    {
        var counterOfPreventEvents = 0;
        var counterOfReleasedEvents = 0;
        var keyboardManipulator1 = new KeyboardManipulator();
        _emulator.Run(() =>
        {
            var keyboardManipulator2 = new KeyboardManipulator();
            keyboardManipulator2.KeyPrevented += key =>
            {
                counterOfPreventEvents++;
            };
            
            keyboardManipulator1.Prevent(Key.A);
            keyboardManipulator1.Prevent(Key.A);
            
            var keyboardManipulator3 = new KeyboardManipulator();
            keyboardManipulator3.Prevent(Key.E);

            keyboardManipulator3.KeyReleased += key =>
            {
                counterOfReleasedEvents++;
            };
            
            Assert.Equal(2, counterOfPreventEvents);
            
            keyboardManipulator3.ReleaseAll();
            
            Assert.Equal(counterOfReleasedEvents, counterOfPreventEvents);
        });
    }
}