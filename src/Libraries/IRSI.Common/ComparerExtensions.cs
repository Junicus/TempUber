namespace IRSI.Common;

public static class ComparerExtensions
{
    public static List<Variance> DetailedCompare<T>(this T leftVal, T rightVal) where T : notnull
    {
        List<Variance> variances = [];
        var fi = leftVal.GetType().GetFields();
        foreach (var f in fi)
        {
            var v = new Variance(f.Name, f.GetValue(leftVal), f.GetValue(rightVal));
            if (!Equals(v.LeftValue, v.RightValue)) variances.Add(v);
        }

        return variances;
    }
}