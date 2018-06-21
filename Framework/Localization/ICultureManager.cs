namespace Framework.Localization
{
    public interface ICultureManager
    {
        string GetCurrentCulture();

        bool IsValidCulture(string cultureName);
    }
}