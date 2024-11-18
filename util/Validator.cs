namespace JavaHateBE.Util
{
    public class Validator<T> where T : class, new()
    {
        public bool Validate<U>(T entity, U value)
            where U : IComparable
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return value.CompareTo(default(U)) > 0;
        }
    }
}
