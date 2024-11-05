/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Diagnostics;
using System.Reactive;
using System.Threading.Tasks;
using Lingua;
using Prolog.Scheduler.Models;
using ReactiveUI;

namespace Prolog.Scheduler.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        _schedule = new Schedule();
        Scheduler = new Models.Scheduler();

        MenuViewModel = new MenuViewModel();
        
        OnFileExitCommand = ReactiveCommand.Create(OnFileExit);
        OnRestartCommand = ReactiveCommand.Create(OnRestart);
        OnNextCommand = ReactiveCommand.Create(OnNext);

        // This takes up to 40s before result is shown when active
        LinguaTrace.TraceSource.Listeners.Clear();
        
        // Now we can directly get the results
        OnNext();
    }

    internal MenuViewModel MenuViewModel { get; }
    
    public ReactiveCommand<Unit, Unit> OnFileExitCommand { get; }
    void OnFileExit()
    {
        Environment.Exit(0);
    }

    public ReactiveCommand<Unit, Unit> OnRestartCommand { get; }
    void OnRestart()
    {
        StatusMsg = "Restarting, please wait...";
        Scheduler.Restart();
        OnNext();
    }

    public ReactiveCommand<Unit, Unit> OnNextCommand { get; }
    async void OnNext()
    {
        StatusMsg = "Calculating, please wait...";
        await Task.Run(() =>
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var schedule = Scheduler.Execute();
            if (schedule == null)
            {
                StatusMsg = "No more schedules exist.";
                return;
            }

            Schedule = schedule;
            stopWatch.Stop();

            var ts = stopWatch.Elapsed;
            var elapsedTime = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds:00}";
            StatusMsg = $"Calculation took {elapsedTime} - Ready";
        });
    }

    string _statusMsg;
    public string StatusMsg
    {
        get => _statusMsg;
        private set => this.RaiseAndSetIfChanged(ref _statusMsg, value);
    }

    public Models.Scheduler Scheduler { get; }

    Schedule _schedule;
    public Schedule Schedule
    {
        get => _schedule;
        private set => this.RaiseAndSetIfChanged(ref _schedule, value);
    }

}