using System;
using System.Linq;

/// <summary>
/// Contains functions to convert the values of a quadratic expression (a/b/c) into the factored form.
/// </summary>
public static class Quadratic
{
	/// <summary>
	/// Calculates the common factors of two numbers.
	/// </summary>
	/// <param name="a">The first number.</param>
	/// <param name="b">The second number.</param>
	/// <returns>The common factors of two numbers.</returns>
	private static int[] cf(int a, int b)
	{
		int[] getFactors(int n)
		{
			int[] factors = new int[0];
			for (int i = 0; i <= n; i++)
				if (n % i == 0) factors.Append(i);

			return factors;
		}

		int[] aFactors = getFactors(Math.Abs(a)), bFactors = getFactors(Math.Abs(b));
		int[] cFactors = new int[0];

		for (int i = 0; i < aFactors.Length; i++)
			for (int j = 0; j < bFactors.Length; j++)
				if (aFactors[i] == bFactors[j]) cFactors.Append(aFactors[i]);

		return cFactors;
	}

	/// <summary>
	/// Calculates the highest common factors of two numbers.
	/// </summary>
	/// <param name="a">The first number.</param>
	/// <param name="b">The second number.</param>
	/// <returns>The highest common factors of two numbers.</returns>
	private static int hcf(int a, int b)
	{
		int[] cFactors = cf(a, b);

		int max = int.MinValue;
		for (int i = 0; i < cFactors.Length; i++)
			if (cFactors[i] > max) max = cFactors[i];

		return max;
	}

	/// <summary>
	/// Calculates the left/right sides (two solutions) of the diamond problem to solve the quadratic area model.
	/// </summary>
	/// <param name="a">The a value.</param>
	/// <param name="b">The b value.</param>
	/// <param name="c">The c value.</param>
	/// <returns>The left/right sides (two solutions) of the diamond problem to solve the quadratic area model.</returns>
	/// <exception cref="ArithmeticException"></exception>
	private static (int left, int right) diamond(int a, int b, int c)
	{
		double left = (-b - Math.Sqrt(Math.Pow(b, 2) - 4 * a * c)) / -2;
		double right = (-b + Math.Sqrt(Math.Pow(b, 2) - 4 * a * c)) / -2;

		if (!left.ToString().EndsWith(".0") || right.ToString().EndsWith(".0") ||
			!a.ToString().EndsWith(".0") || !b.ToString().EndsWith(".0") || !c.ToString().EndsWith(".0"))
			throw new ArithmeticException("Cannot solve diamond problem if solutions are decimals.");

		return ((int)left, (int)right);
	}

	/// <summary>
	/// Returns the string variant of a number, in its simplified form.
	/// </summary>
	/// <param name="value">The number to simplify into a string.</param>
	/// <returns>The string variant of a number, in its simplified form.</returns>
	private static string simplifyNumber(int value) => value == 1 ? "" : (value == -1 ? "-" : value.ToString());

	/// <summary>
	/// Calculates the factored form of a quadratic expression using its a value.
	/// </summary>
	/// <param name="a">The a value.</param>
	/// <returns>The factored form of a quadratic expression using its a value.</returns>
	public static (string factor, string latex) factor(int a)
	{
		string f = $"{simplifyNumber(a)}x^2";

		return (f, f);
	}

	/// <summary>
	/// Calculates the factored form of a quadratic expression using its a/b values.
	/// </summary>
	/// <param name="a">The a value.</param>
	/// <param name="b">The b value.</param>
	/// <returns>The factored form of a quadratic expression using its a/b values.</returns>
	public static (string factor, string latex) factor_ab(int a, int b)
	{
		bool negative = a != Math.Abs(a);
		if (negative)
		{
			a = -a;
			b = -b;
		}

		int gcf = hcf(a, b);
		a /= gcf;
		b /= gcf;

		string f = $"{(negative ? '-' : '\x0')}{(gcf == 1 ? '\x0' : gcf)}x(${(a == 1 ? '\x0' : a)} ${(b == Math.Abs(b) ? '+' : '-')} ${Math.Abs(b)})";

		return (f, f.Replace("(", "\\left(").Replace(")", "\\right)"));
	}

	/// <summary>
	/// Calculates the factored form of a quadratic expression using its a/c values.
	/// </summary>
	/// <param name="a">The a value.</param>
	/// <param name="c">The c value.</param>
	/// <returns>The factored form of a quadratic expression using its a/c values.</returns>
	public static (string factor, string latex) factor_ac(int a, int c)
	{
		bool negative = a != Math.Abs(a);
		if (a != Math.Abs(a) && c == Math.Abs(c))
		{
			a = -a;
			c = -c;
		}

		if (Math.Sqrt(a) != (int)Math.Sqrt(a) || Math.Sqrt(c) != (int)Math.Sqrt(c))
			throw new ArithmeticException("Unable to factor quadratic equation because the difference of squares contains decimals.");
		if (c > 0) throw new ArithmeticException("Unable to factor quadratic equation because an addition of squares was provided, not a difference of squares.",
			new ArgumentOutOfRangeException("c", c, "Unable to factor quadratic equation because the c value provided is higher than 0."));

		string f1 = $"{(negative ? '-' : '\x0')}({a}x + ${c})";
		string f2 = $"({a}x - {c})";

		return (f1 + f2, f1.Replace("(", "\\left(").Replace(")", "\\right)") + f2.Replace("(", "\\left(").Replace(")", "\\right)"));
	}

	/// <summary>
	/// Calculates the factored form of a quadratic expression using its a/b/c values.
	/// </summary>
	/// <param name="a">The a value.</param>
	/// <param name="b">The b value.</param>
	/// <param name="c">The c value.</param>
	/// <returns>The factored form of a quadratic expression using its a/b/c values.</returns>
	public static (string factor, string latex) factor_abc(int a, int b, int c)
	{
		double vertex = -b / (2.0 * a);
		double yvertex = (a * Math.Pow(vertex, 2)) + (b * vertex) + c;

		bool negative = a != Math.Abs(a);

		if ((!negative && yvertex > 0.0) || (negative && yvertex < 0.0))
			throw new ArithmeticException("Unable to factor quadratic equation because it does not intersect the x-axis.");

		int gcf = hcf(a, hcf(b, c));
		a /= gcf;
		b /= gcf;
		c /= gcf;

		if (negative)
		{
			a = -a;
			b = -b;
			c = -c;
		}

		if (a == 1)
		{
			(int left, int right) diamond = Quadratic.diamond(a, b, c);
			int left = diamond.left;
			int right = diamond.right;

			string f1 = $"{(negative ? '-' : '\x0')}(x {(left == Math.Abs(left) ? '+' : '-')} ${Math.Abs(left)})";
			string f2 = $"(x {(right == Math.Abs(right) ? '+' : '-')} {Math.Abs(right)})";

			return (f1 + f2, f1.Replace("(","\\left(").Replace(")","\\right)") + f2.Replace("(","\\left(").Replace(")","\\right)"));
		}
		else
		{
			(int left, int right) diamond = Quadratic.diamond(a, b, c);
			int topLeft = a;
			int topRight = diamond.right;
			int bottomLeft = diamond.left;
			int bottomRight = c;

			int top_left, top_right;
			int left_top, left_bottom;

			bool nTopLeft = topLeft != Math.Abs(topLeft);
			bool nTopRight = topRight != Math.Abs(topRight);
			bool nBottomLeft = bottomLeft != Math.Abs(bottomLeft);
			bool nBottomRight = bottomRight != Math.Abs(bottomRight);

			top_left = hcf(topLeft, bottomLeft);
			top_right = hcf(topRight, bottomRight);

			if (nTopLeft && nBottomLeft) top_left = -top_left;
			if (nTopRight && nBottomRight) top_right = -top_right;

			left_top = hcf(topLeft / top_left, topRight / top_right);
			left_bottom = hcf(bottomLeft / top_left, bottomRight / top_right);

			if (nTopLeft && nTopRight) left_top = -left_top;
			if (nBottomLeft && nBottomRight) left_bottom = -left_bottom;

			string top_left_str = simplifyNumber(top_left);
			string left_top_str = simplifyNumber(left_top);

			string f1 = $"{(negative ? '-' : '\x0')}{(gcf == 1 ? '\x0' : gcf)}({top_left_str}x {(top_right == Math.Abs(top_right) ? '+' : '-')} {Math.Abs(top_right)})";
			string f2 = $"({left_top_str}x {(left_bottom == Math.Abs(left_bottom) ? '+' : '-')} {Math.Abs(left_bottom)})";

			return (f1 + f2, f1.Replace("(","\\left(").Replace(")","\\right)") + f2.Replace("(","\\left(").Replace(")","\\right)"));
		}
	}
}
