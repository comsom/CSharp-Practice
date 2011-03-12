// desc: クラスライブラリ class library ... 配列に対する関数を定義する。

// date: 11;2011/3/12 (sat)
// afs :abbrev: array functions
// command: gmcs -target:library afs.cs ... 通ったら afs.dll が作られた。
// ^^^ Generic class は mcs では support されてない ^^^

// note : Array ary を引数にすると ary[i] ができず error となる。
// note : Generic のため静的クラス(static class : instance を生成しない)にはしない。

using System; // Object もこの中に入ってるっぽい。

namespace ArrayFunctions {
  public class AFs<T> {
    public string to_s(T [] ary) { // Array ary) { // Object [] ary) {
      string res = "{";
      int i;
      for(i=0; i<ary.Length; i++) {
        res += ((Object)ary[i]).ToString();
	if (i==ary.Length-1) break;
	res += ", ";
      }
      res += "}";
      return res;
    }
    public void shuffle(T [] ary) { // Array ary) { // Object [] ary) {
      // Fisher-Yates or Knuth shuffle
      int n = ary.Length;
      Random r = new System.Random();
      while (n > 1) {
	n--;
	int k = r.Next(n);
	T tmp = ary[n];
	ary[n] = ary[k];
	ary[k] = tmp;
      }
    }
  }
}

