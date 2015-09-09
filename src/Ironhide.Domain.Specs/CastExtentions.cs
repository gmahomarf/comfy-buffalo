namespace Ironhide.Domain.Specs
{
    public static class CastExtentions
    {
        public static T As<T>(this object obj)
        {
            return (T) obj;
        }
    }
}