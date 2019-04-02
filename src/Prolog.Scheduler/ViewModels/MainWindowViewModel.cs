using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Prolog.Scheduler.Models;
using ReactiveUI;

namespace Prolog.Scheduler.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            _schedule = new Schedule();
            Scheduler = new Scheduler(Configuration);
            OnFileExitCommand = ReactiveCommand.Create(OnFileExit);
            OnRestartCommand = ReactiveCommand.Create(OnRestart);
            OnNextCommand = ReactiveCommand.Create(OnNext);
        }

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
                string elapsedTime = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds:00}";
                StatusMsg = $"Calculation took {elapsedTime} - Ready";
            });
        }

        string _statusMsg;
        public string StatusMsg
        {
            get => _statusMsg;
            private set => this.RaiseAndSetIfChanged(ref _statusMsg, value);
        }

        public Scheduler Scheduler { get; }

        Schedule _schedule;
        public Schedule Schedule
        {
            get => _schedule;
            private set => this.RaiseAndSetIfChanged(ref _schedule, value);
        }

        static IConfiguration _configuration;
        static IConfiguration Configuration => _configuration ?? (_configuration = CreateConfiguration());

        static IReadOnlyDictionary<string, string> DefaultConfigurationStrings { get; } =
            new Dictionary<string, string>()
            {
                [$"Paths:DataFolder"] = "data",
            };

        static IConfiguration CreateConfiguration()
        {
            return new ConfigurationBuilder()
                .AddIniFile("app.ini", true, true)
                .AddInMemoryCollection(DefaultConfigurationStrings)
                .Build();
            // This allows us to set a system environment variable to Development
            // when running a compiled Release build on a local workstation, so we don't
            // have to alter our real production appsettings file for compiled-local-test.
            //.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            // .AddEnvironmentVariables()
            //.AddAzureKeyVault()
        }
    }
}
