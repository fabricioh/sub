using System;

public static class Util {
  public static void Color(dynamic msg, ConsoleColor color) {
    Console.ForegroundColor = color;
    Console.WriteLine(msg.ToString());
    Console.ResetColor();
  }
}
