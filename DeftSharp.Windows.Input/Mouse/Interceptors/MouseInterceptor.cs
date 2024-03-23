﻿using System;
using DeftSharp.Windows.Input.InteropServices.Mouse;
using DeftSharp.Windows.Input.Pipeline;
using DeftSharp.Windows.Input.Shared.Interceptors;

namespace DeftSharp.Windows.Input.Mouse.Interceptors;

/// <summary>
/// Parent class for all mouse interceptors
/// </summary>
public abstract class MouseInterceptor : IInterceptor
{
    protected readonly string Name;
    protected readonly IMouseInterceptor Mouse;
    
    /// <summary>
    /// The property indicates whether the hook is captured or not.
    /// </summary>
    public bool IsHandled { get; private set; }

    internal MouseInterceptor(IMouseInterceptor mouseInterceptor)
    {
        Mouse = mouseInterceptor;
        Name = GetType().Name;
    }

    public virtual void Dispose() => Unhook();

    protected abstract InterceptorResponse OnMouseInput(MouseInputArgs args);
    
    /// <summary>
    /// The method is called if any interceptor has called the Unhook method. If we are still working with mouse handling,
    /// we return false. If we don't need the system hook anymore, return true. This is intended for optimization. 
    /// </summary>
    /// <returns>If we need hook - false, otherwise true</returns>
    protected abstract bool OnPipelineUnhookRequested();

    public void Hook()
    {
        if (IsHandled)
            return;
        
        IsHandled = true;
        Mouse.MouseInput += OnMouseInput;
        Mouse.UnhookRequested += OnPipelineUnhookRequested;
        Mouse.Hook();
    }

    public void Unhook()
    {
        IsHandled = false;
        Mouse.Unhook();
        Mouse.MouseInput -= OnMouseInput;
        Mouse.UnhookRequested -= OnPipelineUnhookRequested;
    }

    public event Func<bool>? UnhookRequested;
}