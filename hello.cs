// desc: "Hello, World!" program for C#

// command: gmcs hello.cs && mono hello.exe

class HelloWorld {
  //static void Main() {
  //  double d;
  //  double[] d1 = { 1,0 }, d2 = { 3,4 };
  //  System.Console.WriteLine("こんちゃ！ Hello, World!");
  //  d = dist(d1, d2);
  //  System.Console.WriteLine("sqsum : " + d);
  //}
  //static double dist(double[] d1, double[] d2) {
  //  return System.Math.Sqrt( sq(d2[0]-d1[0]) + sq(d2[1]-d1[1]) );
  //}
  //static double sq(double d) { return d*d; }
  static void Main() {
    int a,b;
    a = 3*5*7;
    b = 5*7*11;
    System.Console.WriteLine("gcd({0},{1})=={2}", a, b, gcd(a,b));
  }
  static int gcd(int a, int b) {
    int r = a%b;
    if (r==0) {
      return b;
    } else {
      return gcd(b,r);
    }
  }
}

