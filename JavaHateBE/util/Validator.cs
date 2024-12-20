using JavaHateBE.Exceptions;

namespace JavaHateBE.Util
{
    public class Validator<T> where T : class, new()
    {
        public void Validate<U>(
            T entity, 
            U value, 
            Func<Exception>? entityException = null
        )
            where U : IComparable
        {
            if (entity == null) 
            {
                throw entityException?.Invoke() 
                    ?? new IllegalArgumentException(nameof(entity), "Entity cannot be null.");
            }
            if (value.CompareTo(default(U)) <= 0) 
            {
                throw new IllegalArgumentException(nameof(value), "Value must be greater than its default value.");
            }
        }
    }
}