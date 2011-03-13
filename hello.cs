// desc: "Hello, World!" program for C#

// command: gmcs hello.cs && mono hello.exe

using System;

// List のテスト ... のために。
using System.Collections.Generic;

//// オブジェクト初期化子のテスト ... のためのクラス。
//class Person {
//  public string name;
//  public int    age;
//}

class HelloWorld {
  // List のテスト
  static void Main() {
    List<int> a = new List<int>();
    a.Add(2); a.Add(3); a.Add(5);
    foreach(int p in a) { Console.Write("{0} ", p); }
    Console.WriteLine("");
    Console.WriteLine("a.Count={0}; a[1]={1}", a.Count, a[1]);
  }

  //static void Main() {
  //  // オブジェクト初期化子のテスト 匿名型のテスト
  //  var x = new { s = "simple", n = 5 };
  //  Person p = new Person { name="Urza", age=4000 };
  //  Console.WriteLine("anonymous ... s=\"{0}\", n={1}", x.s, x.n);
  //  Console.WriteLine("{0} is {1} years old.", p.name, p.age);
  //}

  //static void Main() {
  //  int a,b;
  //  a = 3*5*7;
  //  b = 5*7*11;
  //  Console.WriteLine("gcd({0},{1})=={2}", a, b, gcd(a,b));
  //}
  //static int gcd(int a, int b) {
  //  int r = a%b;
  //  if (r==0) {
  //    return b;
  //  } else {
  //    return gcd(b,r);
  //  }
  //}

  //static void Main() {
  //  double d;
  //  double[] d1 = { 1,0 }, d2 = { 3,4 };
  //  Console.WriteLine("こんちゃ！ Hello, World!");
  //  d = dist(d1, d2);
  //  Console.WriteLine("sqsum : " + d);
  //}
  //static double dist(double[] d1, double[] d2) {
  //  return Math.Sqrt( sq(d2[0]-d1[0]) + sq(d2[1]-d1[1]) );
  //}
  //static double sq(double d) { return d*d; }
}

