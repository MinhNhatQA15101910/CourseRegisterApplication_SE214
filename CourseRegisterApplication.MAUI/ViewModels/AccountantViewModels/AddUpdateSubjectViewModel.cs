﻿using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views.AccountantViews;

namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels
{
    public partial class AddUpdateSubjectViewModel : ObservableObject
    {
        #region Services
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Constructor
        public AddUpdateSubjectViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        #endregion

        #region Properties
        public int SubjectId { get; set; } = -1;

        [ObservableProperty] private string commandName;

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddUpdateSubjectCommand))] private string subjectSpecificId;

        [ObservableProperty] private Color subjectSpecificIdColor;

        [ObservableProperty] private string subjectSpecificIdMessageText;

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddUpdateSubjectCommand))] private string subjectName;

        [ObservableProperty] private Color subjectNameColor;

        [ObservableProperty] private string subjectNameMessageText;

        [ObservableProperty] private string totalLesson;

        [ObservableProperty] private Color totalLessonColor;

        [ObservableProperty] private string totalLessonMessageText;

        [ObservableProperty] private SubjectType selectedSubjectType = null;

        [ObservableProperty]
        private ObservableCollection<SubjectType> subjectTypesList = null;

        #endregion

        #region Commands
        [RelayCommand]
        public async Task ClosePopup()
        {
            ClearState();

            Popup popup = _serviceProvider.GetService<AddUpdateSubjectPopup>();
            await popup.CloseAsync();
        }

        [RelayCommand]
        public async Task GetSubjectTypes()
        {
            var subjectTypesService = _serviceProvider.GetService<ISubjectTypeService>();
            
            SubjectTypesList = (await subjectTypesService.GetAllSubjectType()).ToObservableCollection();
        }

        [RelayCommand(CanExecute = nameof(CanAddUpdateSubjectExecuted))]
        public async Task AddUpdateSubject()
        {
            if (CommandName == "Add Subject")
            {
                await AddSubject();
            }
            else if (CommandName == "Update Subject")
            {
                await UpdateSubject();
            }
        }
        public bool CanAddUpdateSubjectExecuted()
        {
            bool isValidSubjectSpecificId = true;
            bool isValidSubjectName = true;

            // Validate Subject ID
            if (string.IsNullOrEmpty(SubjectSpecificId))
            {
                SubjectSpecificIdColor = Color.FromArgb("#BF1D28");
                SubjectSpecificIdMessageText = "Subject ID cannot be empty.";
                isValidSubjectSpecificId = false;
            }
            else
            {
                SubjectSpecificIdColor = Color.FromArgb("#007D3A");
                SubjectSpecificIdMessageText = "Valid Subject ID.";
            }

            // Validate Subject Name
            if (string.IsNullOrEmpty(SubjectName))
            {
                SubjectNameColor = Color.FromArgb("#BF1D28");
                SubjectNameMessageText = "Subject Name cannot be empty.";
                isValidSubjectName = false;
            }
            else
            {
                SubjectNameColor = Color.FromArgb("#007D3A");
                SubjectNameMessageText = "Valid Subject Name.";
            }

            return isValidSubjectSpecificId && isValidSubjectName;
        }

        #endregion

        #region Property Change
        async partial void OnCommandNameChanged(string value)
        {
           
            var subjectService = _serviceProvider.GetService<ISubjectService>();

            if (!string.IsNullOrEmpty(value))
            {
                await GetSubjectTypesCommand.ExecuteAsync(null);

                if (value.Equals("Update Subject"))
                {
                    var subject = await subjectService.GetSubjectById(SubjectId);
                    SubjectSpecificId = subject.SubjectSpecificId;
                    SubjectName = subject.Name;
                    TotalLesson = subject.TotalLessons.ToString();
                    SelectedSubjectType = SubjectTypesList.First(x => x.Id == subject.SubjectTypeId);
                }
            }
        }
        #endregion

        #region Helper
        void ClearState()
        {
            SubjectSpecificId = "";
            SubjectName = "";
            SelectedSubjectType = null;
            TotalLesson = "";
        }

        private async Task AddSubject()
        {
            var accept = await Application.Current.MainPage.DisplayAlert("Question", "Do you want to add this new subject?", "Yes", "No");
            if (accept)
            {
                var subjectService = _serviceProvider.GetService<ISubjectService>();
                var subjects = await subjectService.GetAllSubjects();

                //Check if there is any subject with the same subject specific id
                var sameSpecifcIdSubject = subjects.Where(x => x.SubjectSpecificId.ToLower() == SubjectSpecificId.ToLower());
                if(sameSpecifcIdSubject.Any())
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "There is already a subject with the same subject specific id.", "OK");
                    return;
                }

                //Check if there is any subject with the same subject name
                var sameNameSubject = subjects.Where(x => x.Name.ToLower() == SubjectName.ToLower());
                if (sameNameSubject.Any())
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "There is already a subject with the same subject name.", "OK");
                    return;
                }

                //Add new subject
                var subject = await subjectService.CreateSubject( new Subject()
                {
                    SubjectSpecificId = SubjectSpecificId.Trim(),
                    Name = SubjectName.Trim(),
                    SubjectTypeId = SelectedSubjectType.Id,
                    TotalLessons = int.Parse(TotalLesson)
                });
                if (subject != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Add subject successfully!", "OK");

                    SubjectManagementViewModel subjectManagementViewModel = _serviceProvider.GetService<SubjectManagementViewModel>();
                    subjectManagementViewModel.GetAllSubjectCommand.Execute(null);

                    ClearState();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Failed", "Add subject failed!", "OK");
                }
            }
        }

        private async Task UpdateSubject()
        {
            var accept = await Application.Current.MainPage.DisplayAlert("Question", "Do you want to Update this new subject?", "Yes", "No");
            if (accept)
            {
                var subjectService = _serviceProvider.GetService<ISubjectService>();
                var subjects = await subjectService.GetAllSubjects();

                //Check if there is any subject with the same subject specific id
                var sameSpecifcIdSubject = subjects.Where(x => x.SubjectSpecificId.ToLower() == SubjectSpecificId.ToLower());
                if (sameSpecifcIdSubject.Any())
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "There is already a subject with the same subject specific id.", "OK");
                    return;
                }

                //Check if there is any subject with the same subject name
                var sameNameSubject = subjects.Where(x => x.Name.ToLower() == SubjectName.ToLower());
                if (sameNameSubject.Any())
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "There is already a subject with the same subject name.", "OK");
                    return;
                }

                //Update new subject
                var success = await subjectService.UpdateSubject(SubjectId, new Subject()
                {
                    SubjectSpecificId = SubjectSpecificId.Trim(),
                    Name = SubjectName.Trim(),
                    SubjectTypeId = SelectedSubjectType.Id,
                    TotalLessons = int.Parse(TotalLesson)
                });
                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Update subject successfully!", "OK");

                    SubjectManagementViewModel subjectManagementViewModel = _serviceProvider.GetService<SubjectManagementViewModel>();
                    subjectManagementViewModel.GetAllSubjectCommand.Execute(null);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Failed", "Update subject failed!", "OK");
                }
            }
        }
        #endregion
    }
}
