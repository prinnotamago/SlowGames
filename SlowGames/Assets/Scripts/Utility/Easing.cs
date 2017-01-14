/// <author>
/// 新井大一
/// </author>

static public class Easing
{
    static public double InQuad(double t, double totaltime, double max, double min)
    {
        max -= min;
        t /= totaltime;
        return max * t * t + min;
    }

    static public double OutQuad(double t, double totaltime, double max, double min)
    {
        max -= min;
        t /= totaltime;
        return -max * t * (t - 2) + min;
    }

    static public double InOutQuad(double t, double totaltime, double max, double min)
    {
        max -= min;
        t /= totaltime;
        if (t / 2 < 1)
            return max / 2 * t * t + min;
        --t;
        return -max * (t * (t - 2) - 1) + min;
    }

    static public double InCubic(double t, double totaltime, double max, double min)
    {
        max -= min;
        t /= totaltime;
        return max * t * t * t + min;
    }

    static public double OutCubic(double t, double totaltime, double max, double min)
    {
        max -= min;
        t = t / totaltime - 1;
        return max * (t * t * t + 1) + min;
    }

    static public double InOutCubic(double t, double totaltime, double max, double min)
    {
        max -= min;
        t /= totaltime;
        if (t / 2 < 1)
            return max / 2 * t * t * t + min;
        t -= 2;
        return max / 2 * (t * t * t + 2) + min;
    }

    static public double InQuart(double t, double totaltime, double max, double min)
    {
        max -= min;
        t /= totaltime;
        return max * t * t * t * t + min;
    }

    static public double OutQuart(double t, double totaltime, double max, double min)
    {
        max -= min;
        t = t / totaltime - 1;
        return -max * (t * t * t * t - 1) + min;
    }

    static public double InOutQuart(double t, double totaltime, double max, double min)
    {
        max -= min;
        t /= totaltime;
        if (t / 2 < 1)
            return max / 2 * t * t * t * t + min;
        t -= 2;
        return -max / 2 * (t * t * t * t - 2) + min;
    }

    static public double InQuint(double t, double totaltime, double max, double min)
    {
        max -= min;
        t /= totaltime;
        return max * t * t * t * t * t + min;
    }

    static public double OutQuint(double t, double totaltime, double max, double min)
    {
        max -= min;
        t = t / totaltime - 1;
        return max * (t * t * t * t * t + 1) + min;
    }

    static public double InOutQuint(double t, double totaltime, double max, double min)
    {
        max -= min;
        t /= totaltime;
        if (t / 2 < 1)
            return max / 2 * t * t * t * t * t + min;
        t -= 2;
        return max / 2 * (t * t * t * t * t + 2) + min;
    }

    static public double InSine(double t, double totaltime, double max, double min)
    {
        max -= min;
        return -max * System.Math.Cos(t * (System.Math.PI / 2.0) / totaltime) + max + min;
    }

    static public double OutSine(double t, double totaltime, double max, double min)
    {
        max -= min;
        return max * System.Math.Sin(t * (System.Math.PI / 2.0) / totaltime) + min;
    }

    static public double InOutSine(double t, double totaltime, double max, double min)
    {
        max -= min;
        return -max / 2 * (System.Math.Cos(t * System.Math.PI / totaltime) - 1) + min;
    }

    static public double InExp(double t, double totaltime, double max, double min)
    {
        max -= min;
        return t == 0.0 ? min : max * System.Math.Pow(2, 10 * (t / totaltime - 1)) + min;
    }

    static public double OutExp(double t, double totaltime, double max, double min)
    {
        max -= min;
        return t == totaltime ? max + min : max * (-System.Math.Pow(2, -10 * t / totaltime) + 1) + min;
    }

    static public double InOutExp(double t, double totaltime, double max, double min)
    {
        if (t == 0.0)
            return min;
        if (t == totaltime)
            return max;
        max -= min;
        t /= totaltime;
        
        if (t / 2 < 1)
            return max / 2 * System.Math.Pow(2, 10 * (t - 1)) + min;
        --t;
        return max / 2 * (-System.Math.Pow(2, -10 * t) + 2) + min;

    }

    static public double InCirc(double t, double totaltime, double max, double min)
    {
        max -= min;
        t /= totaltime;
        return -max * (System.Math.Sqrt(1 - t * t) - 1) + min;
    }

    static public double OutCirc(double t, double totaltime, double max, double min)
    {
        max -= min;
        t = t / totaltime - 1;
        return max * System.Math.Sqrt(1 - t * t) + min;
    }

    static public double InOutCirc(double t, double totaltime, double max, double min)
    {
        max -= min;
        t /= totaltime;
        if (t / 2 < 1)
            return -max / 2 * (System.Math.Sqrt(1 - t * t) - 1) + min;
        t -= 2;
        return max / 2 * (System.Math.Sqrt(1 - t * t) + 1) + min;
    }

    static public double InBack(double t, double totaltime, double max, double min, double s)
    {
        max -= min;
        t /= totaltime;
        return max * t * t * ((s + 1) * t - s) + min;
    }

    static public double OutBack(double t, double totaltime, double max, double min, double s)
    {
        max -= min;
        t = t / totaltime - 1;
        return max * (t * t * ((s + 1) * t + s) + 1) + min;
    }

    static public double InOutBack(double t, double totaltime, double max, double min, double s)
    {
        max -= min;
        s *= 1.525;
        if (t / 2 < 1)
        {
            return max * (t * t * ((s + 1) * t - s)) + min;
        }
        t -= 2;
        return max / 2 * (t * t * ((s + 1) * t + s) + 2) + min;
    }

    static public double OutBounce(double t, double totaltime, double max, double min)
    {
        max -= min;
        t /= totaltime;

        if (t < 1 / 2.75)
            return max * (7.5625 * t * t) + min;
        else if (t < 2 / 2.75)
        {
            t -= 1.5 / 2.75;
            return max * (7.5625 * t * t + 0.75) + min;
        }
        else if (t < 2.5 / 2.75)
        {
            t -= 2.25 / 2.75;
            return max * (7.5625 * t * t + 0.9375) + min;
        }
        else
        {
            t -= 2.625 / 2.75;
            return max * (7.5625 * t * t + 0.984375) + min;
        }
    }

    static public double InBounce(double t, double totaltime, double max, double min)
    {
        return max - OutBounce(totaltime - t, totaltime, max - min, 0) + min;
    }

    static public double InOutBounce(double t, double totaltime, double max, double min)
    {
        if (t < totaltime / 2)
            return InBounce(t * 2, totaltime, max - min, max) * 0.5 + min;
        else
            return OutBounce(t * 2 - totaltime, totaltime, max - min, 0) * 0.5 + min + (max - min) * 0.5;
    }

    static public double Linear(double t, double totaltime, double max, double min)
    {
        return (max - min) * t / totaltime + min;
    }
}
