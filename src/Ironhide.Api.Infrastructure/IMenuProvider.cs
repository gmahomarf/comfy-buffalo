namespace Ironhide.Api.Infrastructure
{
    public interface IMenuProvider
    {
        string[] getFeatures(string claim);
        string[] getFeatures(string[] claims);
        string[] getAllFeatures();
    }


}