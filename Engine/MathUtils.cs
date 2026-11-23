using System;
using Microsoft.Xna.Framework;

namespace Engine;

/// <summary>
/// Contains useful math functions.
/// </summary>
public static class MathUtils
{
   /// <summary>
   /// Using an epsilon is very common when comparing floating-point numbers (dealing with error margins):
   /// https://www.reddit.com/r/explainlikeimfive/comments/4ufw5t/eli5_within_epsilon_of_in_programming/
   /// </summary>
   public const float Epsilon = 0.00001f;

   /// <summary>
   /// Returns the component-wise absolute value of a vector.
   /// </summary>
   /// <param name="v">The vector to take the component-wise absolute value of.</param>
   /// <returns>The component-wise absolute value of <paramref name="v"/>.</returns>
   public static Vector2 Abs(Vector2 v)
   {
      return new Vector2(MathF.Abs(v.X), MathF.Abs(v.Y));
   }

   /// <summary>
   /// Recall that % is the remainder operator and technically not modulo.
   /// </summary>
   /// <param name="l">The dividend.</param>
   /// <param name="r">The divisor.</param>
   /// <returns>The remainder of l divided by r (l mod r).</returns>
   public static float Modulo(float l, float r)
   {
      // Implementation is the same as:
      // https://stackoverflow.com/questions/1082917/mod-of-negative-number-is-melting-my-brain
      return ((l % r) + r) % r;
   }

   public static bool IsApproxEqual(float a, float b)
   {
      // Comparing floating-point numbers is not necessarily straightforward.
      // Credit: https://github.com/godotengine/godot/blob/master/core/math/math_funcs.h

      // First, check for exact equality to handle "infinity" values.
      if (a == b) return true;

      // Then, check for approximate equality.
      var toleranceMargin = Epsilon * MathF.Sqrt(a * b);
      return MathF.Abs(a - b) <= toleranceMargin;
   }

   public static bool IsApproxEqual(Vector2 a, Vector2 b)
   {
      return IsApproxEqual(a.X, b.X) && IsApproxEqual(a.Y, b.Y);
   }
}
