using AcklenAvenue.Email;

namespace Ironhide.TemplateEngines.Razor
{
    public class RazorViewEngine : IViewEngine
    {
        #region IViewEngine Members

        public string Render(object model, string formattedString)
        {
            dynamic dynamicModel = model.ToDynamic();
            return RazorEngine.Razor.Parse(formattedString, dynamicModel);
        }

        #endregion
    }
}