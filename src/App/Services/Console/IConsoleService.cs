using App.Services.Phone;
using App.Validators;
using Spectre.Console;

namespace App.Services.Console;

public interface IConsoleService
{
    void RenderTitle(string text);
    void RenderVersion(string version);
    void RenderProblem(string text);
    void RenderText(string text, Color color);
    void RenderSettingsFile(string filepath);
    void RenderUserSecretsFile(string filepath);
    void RenderException(Exception exception);
    Task RenderStatusAsync(Func<Task> action);
    Task<T> RenderStatusAsync<T>(Func<Task<T>> func);
    bool GetYesOrNoAnswer(string text, bool defaultAnswer);
    void RenderValidationErrors(ValidationErrors validationErrors);
    void RenderPhoneNumber(PhoneParameters parameters, PhoneNumber phoneNumber);
    void RenderPhoneNumbers(PhoneParameters parameters, IEnumerable<PhoneNumber> phoneNumbers);
}