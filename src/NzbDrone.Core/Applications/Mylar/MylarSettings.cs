using System.Collections.Generic;
using FluentValidation;
using NzbDrone.Core.Annotations;
using NzbDrone.Core.Indexers;
using NzbDrone.Core.Validation;

namespace NzbDrone.Core.Applications.Mylar
{
    public class MylarSettingsValidator : AbstractValidator<MylarSettings>
    {
        public MylarSettingsValidator()
        {
            RuleFor(c => c.BaseUrl).IsValidUrl();
            RuleFor(c => c.ProwlarrUrl).IsValidUrl();
            RuleFor(c => c.ApiKey).NotEmpty();
            RuleFor(c => c.SyncCategories).NotEmpty();
        }
    }

    public class MylarSettings : IApplicationSettings
    {
        private static readonly MylarSettingsValidator Validator = new MylarSettingsValidator();

        public MylarSettings()
        {
            ProwlarrUrl = "http://localhost:9696";
            BaseUrl = "http://localhost:8090";
            SyncCategories = new[] { NewznabStandardCategory.BooksComics.Id };
        }

        [FieldDefinition(0, Label = "Prowlarr Server", HelpText = "Prowlarr server URL as Mylar sees it, including http(s)://, port, and urlbase if needed")]
        public string ProwlarrUrl { get; set; }

        [FieldDefinition(1, Label = "Mylar Server", HelpText = "Mylar server URL, including http(s):// and port if needed")]
        public string BaseUrl { get; set; }

        [FieldDefinition(2, Label = "ApiKey", Privacy = PrivacyLevel.ApiKey, HelpText = "The ApiKey generated by Mylar in Settings/Web Interface")]
        public string ApiKey { get; set; }

        [FieldDefinition(3, Label = "Sync Categories", Type = FieldType.Select, SelectOptions = typeof(NewznabCategoryFieldConverter), Advanced = true, HelpText = "Only Indexers that support these categories will be synced")]
        public IEnumerable<int> SyncCategories { get; set; }

        public NzbDroneValidationResult Validate()
        {
            return new NzbDroneValidationResult(Validator.Validate(this));
        }
    }
}
