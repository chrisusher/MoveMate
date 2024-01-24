namespace ChrisUsher.MoveMate.Shared
{
    public static class CurrencyLogic
    {
        public static int CountDecimalPlaces(double number)
        {
            var currencyString = number.ToString();

            if (!currencyString.Contains('.'))
            {
                return 0;
            }

            var afterDecimalPlace = currencyString.Substring(currencyString.IndexOf('.') + 1);

            return afterDecimalPlace.Length;
        }
    }
}