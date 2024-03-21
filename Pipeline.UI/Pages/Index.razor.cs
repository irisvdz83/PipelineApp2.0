﻿using Microsoft.AspNetCore.Components;
using Pipeline.UI.ViewModels;

namespace Pipeline.UI.Pages
{
    public partial class Index : ComponentBase
    {
        private string? _elapsedTime ;
        private DateEntryViewModel CurrentDateEntryViewModel { get; set; } = null!;

        protected override void OnInitialized()
        {
            var today = DateController.GetToday();
            CurrentDateEntryViewModel = DateEntryViewModel.MapToDateEntry(today);
            
            base.OnInitialized();
        }

        public string? ElapsedTime
        {
            get => _elapsedTime;
            set
            {
                _elapsedTime = value;
                StateHasChanged();
            }
        }

        private bool IsRunning { get; set; }
        private TimeSpan? TimeSpan { get; set; } 
        private Timer? PipelineTimer { get; set; }
        private long Seconds { get; set; }
        
        private void ToggleTimer()
        {
            if (IsRunning && PipelineTimer is not null)
            {
                IsRunning = false;
                PipelineTimer.Dispose();
                PipelineTimer = null;
            }
            else
            {
                IsRunning = true;
                TimeSpan ??= new TimeSpan(0, 0, 0);
                CurrentDateEntryViewModel.StartTime = DateTime.Now;
                DateController.AddNewStartTime(CurrentDateEntryViewModel.StartTime);
                PipelineTimer = new Timer(_ =>
                {
                    Seconds += 1;
                    TimeSpan = System.TimeSpan.FromSeconds(Seconds);
                    var hours = TimeSpan.Value.Hours < 10 ? $"0{TimeSpan.Value.Hours}" : $"{TimeSpan.Value.Hours}";
                    var minutes = TimeSpan.Value.Minutes < 10 ? $"0{TimeSpan.Value.Minutes}" : $"{TimeSpan.Value.Minutes}";
                    var seconds = TimeSpan.Value.Seconds < 10 ? $"0{TimeSpan.Value.Seconds}" : $"{TimeSpan.Value.Seconds}";
                    _elapsedTime = $"{hours}:{minutes}:{seconds}";

                    InvokeAsync(StateHasChanged);
                }, null, 0, 1000);
            }
        }

        private void StopTimer()
        {
            if (!IsRunning || PipelineTimer is null) return;
            IsRunning = false;
            PipelineTimer.Dispose();
            PipelineTimer = null;
            CurrentDateEntryViewModel.EndTime = DateTime.Now;
            DateController.UpdateEndTime(CurrentDateEntryViewModel.Id);
        }
    }
}
