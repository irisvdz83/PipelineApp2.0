using Microsoft.AspNetCore.Components;
using Pipeline.UI.ViewModels;
using PipelineApp2._0.Helpers;
using System;

namespace PipelineApp2._0.Pages
{
    public partial class Index : ComponentBase
    {
        private string? _elapsedTimeCurrentTimeBlock;
        public string? ElapsedTimeCurrentTimeBlock
        {
            get => _elapsedTimeCurrentTimeBlock;
            set
            {
                _elapsedTimeCurrentTimeBlock = value;
                StateHasChanged();
            }
        }
        private TimeSpan ElapsedTimeSpanCurrentTimeBlock { get; set; }
        private string? _elapsedTotalTime;
        public string? ElapsedTotalTime
        {
            get => _elapsedTotalTime;
            set
            {
                _elapsedTotalTime = value;
                StateHasChanged();
            }
        }
        private TimeSpan ElapsedTotalTimeSpan { get; set; }

        private DateEntryViewModel CurrentDateEntryViewModel { get; set; } = null!;
        private bool IsRunning { get; set; }
        private Timer? PipelineTimer { get; set; }
        private long Seconds { get; set; }
        private long TotalTimeSeconds { get; set; }
        protected override void OnInitialized()
        {
            var today = DateEntryController.GetToday();
            CurrentDateEntryViewModel = DateEntryViewModel.MapToDateEntry(today);
            GetTotalElapsedTime();
            base.OnInitialized();
        }

        private void GetTotalElapsedTime()
        {
            ElapsedTotalTimeSpan = DateEntryController.GetTotalWorkedTimeToday();
            _elapsedTotalTime = TimeSpanHelper.GetFormattedTimeSpan(ElapsedTotalTimeSpan);
        }
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
                ElapsedTimeSpanCurrentTimeBlock = new TimeSpan(0, 0, 0);
                CurrentDateEntryViewModel.StartTime = DateTime.Now;
                DateEntryController.AddNewStartTime(CurrentDateEntryViewModel.Id, CurrentDateEntryViewModel.StartTime);
                TotalTimeSeconds = DateEntryController.GetTotalWorkedTimeToday().Seconds;
                
                PipelineTimer = new Timer(_ =>
                {
                    Seconds += 1;
                    TotalTimeSeconds += 1;
                    ElapsedTimeSpanCurrentTimeBlock = TimeSpan.FromSeconds(Seconds);
                    ElapsedTotalTimeSpan = TimeSpan.FromSeconds(TotalTimeSeconds);
                    var hours = ElapsedTimeSpanCurrentTimeBlock.Hours < 10 ? $"0{ElapsedTimeSpanCurrentTimeBlock.Hours}" : $"{ElapsedTimeSpanCurrentTimeBlock.Hours}";
                    var minutes = ElapsedTimeSpanCurrentTimeBlock.Minutes < 10 ? $"0{ElapsedTimeSpanCurrentTimeBlock.Minutes}" : $"{ElapsedTimeSpanCurrentTimeBlock.Minutes}";
                    var seconds = ElapsedTimeSpanCurrentTimeBlock.Seconds < 10 ? $"0{ElapsedTimeSpanCurrentTimeBlock.Seconds}" : $"{ElapsedTimeSpanCurrentTimeBlock.Seconds}";
                    _elapsedTimeCurrentTimeBlock = $"{hours}:{minutes}:{seconds}";
                    var totalHours = ElapsedTotalTimeSpan.Hours < 10 ? $"0{ElapsedTotalTimeSpan.Hours}" : $"{ElapsedTotalTimeSpan.Hours}";
                    var totalMinutes = ElapsedTotalTimeSpan.Minutes < 10 ? $"0{ElapsedTotalTimeSpan.Minutes}" : $"{ElapsedTotalTimeSpan.Minutes}";
                    var totalSeconds = ElapsedTotalTimeSpan.Seconds < 10 ? $"0{ElapsedTotalTimeSpan.Seconds}" : $"{ElapsedTotalTimeSpan.Seconds}";
                    _elapsedTotalTime = $"{totalHours}:{totalMinutes}:{totalSeconds}";
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
            DateEntryController.UpdateEndTime(CurrentDateEntryViewModel.Id, CurrentDateEntryViewModel.EndTime.Value);
            ElapsedTotalTimeSpan = DateEntryController.GetTotalWorkedTimeToday();
        }
    }
}
