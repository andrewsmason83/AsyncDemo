namespace AsyncDemo.Extensions
{
    public static class UIntExtensions
    {

        public static bool IsPrime(this uint number)
        {
            if (number == 1 || (number > 2 && number.IsEven()))
            {
                return false;
            }

            for (int i = 3; i < number; i++)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsEven(this uint number)
        {
            return number % 2 == 0;
        }
    }
}
