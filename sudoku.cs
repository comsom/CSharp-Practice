// desc: backtrack の練習のため数独を解かせる ; 理詰めゼロで ; 再帰無しで。

// date: 11;2011/3/13 (sun)     ... 今日中に出来るか？
// command: gmcs sudoku.cs && mono sudoku.exe

// vvvvvv 動作例 vvvvvv
// --- Problem ---
// begin
//   ___   _9_   ___   
//   _2_   ___   _7_   
//   __1   7_8   3__   
// 
//   __5   ___   8__   
//   4__   _6_   __9   
//   __3   ___   4__   
// 
//   __2   8_5   1__   
//   _1_   ___   _3_   
//   ___   _2_   ___   
// end
// 
// --- Solution ---
// begin
//   736   192   548   
//   824   356   971   
//   951   748   326   
// 
//   295   417   863   
//   487   563   219   
//   163   289   457   
// 
//   672   835   194   
//   518   974   632   
//   349   621   785   
// end

using System;
using System.Collections.Generic;

// 外側に配列(0-8)を置きたいけど普通に書くと error なのでクラスで包む。
class Range9 {
  static int [] private_ary = { 0,1,2,3,4,5,6,7,8 };
  public static int [] ary { get { return private_ary; } }
  public static int ith(int i) { return private_ary[i]; }
  public static int length { get { return private_ary.Length; } }
}
//class TestRange9 {
//  static void Main() {
//    int i;
//    for(i=0; i<Range9.length; i++)
//      Console.WriteLine( Range9.ith(i) );
//  }
//}    

//------------------------------------------------------------------------------
// Ary2d, ListFun
//------------------------------------------------------------------------------
class Ary2d { // abbrev: Array of 2-dimentions
  public static string to_sbs(int[,] ary2d) { // abbrev: to sudoku board string
    string res = "begin\n";
    foreach(int i in Range9.ary) {
      res += "  "; // indentation
      foreach(int j in Range9.ary) {
	res += (ary2d[i,j]==0 ? "_":ary2d[i,j].ToString());
	if ( (j+1)%3==0 ) { res += "   "; }
      }
      res += "\n";
      if ( (i+1)%3==0 && i<Range9.length-1 ) { res += "\n"; }
    }
    res += "end";
    return res;
  }
}
class ListFun <T> {
  public string to_s(List<T> list) {
    string res = "[";
    int i, n=list.Count;
    for (i=0; i<n-1; i++) { res += list[i].ToString() + ", "; }
    res += list[n-1].ToString() + "]";
    return res;
  }
}

//------------------------------------------------------------------------------
// Main
//------------------------------------------------------------------------------
class SudokuSolve {
  static void Main() {
    int[,] board = {
      // // 例題 from 「日経ソフトウェア アルゴリズムまるごと学習ブック」
      // { 0,0,8, 0,0,0, 0,0,0 },
      // { 1,0,0, 6,0,0, 3,0,0 },
      // { 0,0,0, 5,1,0, 9,0,0 },
      // 			     
      // { 0,0,1, 0,0,0, 4,0,0 },
      // { 0,6,0, 0,8,7, 0,0,0 },
      // { 0,0,0, 0,3,0, 5,0,2 },
      // 			     
      // { 0,0,9, 0,0,4, 0,0,0 },
      // { 0,7,0, 0,0,9, 0,4,0 },
      // { 5,3,0, 0,0,0, 0,0,0 },

      // // 解けない問題の例
      // { 1,0,8, 0,0,0, 0,0,0 },
      // { 1,0,0, 6,0,0, 3,0,0 },
      // { 0,0,0, 5,1,0, 9,0,0 },
      // 			     
      // { 0,0,1, 0,0,0, 4,0,0 },
      // { 0,6,0, 0,8,7, 0,0,0 },
      // { 0,0,0, 0,3,0, 5,0,2 },
      // 			     
      // { 0,0,9, 0,0,4, 0,0,0 },
      // { 0,7,0, 0,0,9, 0,4,0 },
      // { 5,3,0, 0,0,0, 0,0,0 },

      // // 例題：まっさら ... SudokuSolver は解を1つだけ見つける。
      // { 0,0,0, 0,0,0, 0,0,0 },
      // { 0,0,0, 0,0,0, 0,0,0 },
      // { 0,0,0, 0,0,0, 0,0,0 },
      // 			     
      // { 0,0,0, 0,0,0, 0,0,0 },
      // { 0,0,0, 0,0,0, 0,0,0 },
      // { 0,0,0, 0,0,0, 0,0,0 },
      // 			     
      // { 0,0,0, 0,0,0, 0,0,0 },
      // { 0,0,0, 0,0,0, 0,0,0 },
      // { 0,0,0, 0,0,0, 0,0,0 },

      // 例題 from 「レベル判定 IQナンプレ300 vol.2」の問題300(最後の問題)
      { 0,0,0, 0,9,0, 0,0,0 },
      { 0,2,0, 0,0,0, 0,7,0 },
      { 0,0,1, 7,0,8, 3,0,0 },
			     
      { 0,0,5, 0,0,0, 8,0,0 },
      { 4,0,0, 0,6,0, 0,0,9 },
      { 0,0,3, 0,0,0, 4,0,0 },
			     
      { 0,0,2, 8,0,5, 1,0,0 },
      { 0,1,0, 0,0,0, 0,3,0 },
      { 0,0,0, 0,2,0, 0,0,0 },
    };
    SudokuSolver ss = new SudokuSolver(board);

    // 盤面の表示
    Console.WriteLine( "--- Problem ---" );
    Console.WriteLine( Ary2d.to_sbs(board) );

    // 解く。
    while (true) {
      if ( ss.isend() ) {
	// 解を表示して終わり。
	Console.WriteLine( "\n--- Solution ---" );
	Console.WriteLine( Ary2d.to_sbs(board) );
	break;
      } else if ( ss.cannot_sovle_flag==true ) {
	Console.WriteLine( "\nCould not solve." );
	break;
      } else {
    	ss.putnum();
      }
    }

    // end of Main
  }
}

//------------------------------------------------------------------------------
// SudokuSolver
//------------------------------------------------------------------------------
class SudokuSolver {
  // - board と、埋めねばならない位置のリスト fillpos と、
  //   次に埋める位置を指す pos_i を保持する。
  int [,] board;
  List<Pos> fillpos = new List<Pos>(); // 埋めねばならない位置のリスト
  int pos_i;
  bool private_cannot_sovle_flag = false; // 解けないと分かったときに立てる。
  public bool cannot_sovle_flag { get { return private_cannot_sovle_flag; } }

  // ---------------------- Constructor ----------------------
  public SudokuSolver(int [,] b) {
    // ListFun<Pos> lf = new ListFun<Pos>(); // debugwrite

    board = b;
    init_fillpos();
    pos_i = 0; // index of fillpos.

    // Console.WriteLine( "debugwrite : fillpos : " + lf.to_s(fillpos) );
  }
  void init_fillpos() {
    foreach(int i in Range9.ary) {
      foreach(int j in Range9.ary) {
	if (board[i,j]==0) { fillpos.Add(new Pos(i,j)); }
      }
    }
  }

  // ---------------------- isend ----------------------
  public bool isend() {
    // board が埋まっていれば true, さもなくば false.
    foreach(int i in Range9.ary) {
      foreach(int j in Range9.ary) {
	if (board[i,j]==0) {
	  //Console.Write("(debugwrite)is_not_end ");
	  return false;
	}
      }
    }
    return true;
  }

  // ---------------------- putnum ----------------------
  public void putnum() {
    // - fillpos[pos_i] にある数 num を board からとる。
    // - num+1 以上 9 以下の、同行・同列・同ブロックにあるどの数とも異なる数のうち、
    //   最小のものを見つける。
    //   - 見つかった場合、その数をその位置に置く。 pos_i++;
    //   - 見つからなかった場合、 fillpos[pos_i] の位置に0を置く。その後 pos_i--;

    Pos pos = fillpos[pos_i];
    int num = board[pos.r, pos.c];
    int? guess = next_guess(num, pos.r, pos.c);
    if (guess.HasValue) {
      board[pos.r, pos.c] = guess.Value;
      pos_i++;
    } else {
      board[pos.r, pos.c] = 0;
      pos_i--;
    }
    if ( pos_i<0 ) { private_cannot_sovle_flag = true; } // この問題は解けない！
  }
  int? next_guess(int num, int r, int c) {
    // num+1 以上 9 以下の、同行・同列・同ブロックにあるどの数とも異なる数のうち、
    // 最小のものを見つける。
    // 見つかったらそれを返し、見つからなかったら null を返す。
    int k;
    for(k=num+1; k<=Range9.length; k++) {
      if (!( exists_in_rect(k, r,      0,       1,9) ||
	     exists_in_rect(k, 0,      c,       9,1) ||
	     exists_in_rect(k, (r/3)*3,(c/3)*3, 3,3) )) { return k; }
    }
    return null;
  }
  bool exists_in_rect(int k, int top, int left, int nrow, int ncol) {
    // top, left, nrow, ncol で指定される四角形の領域内に
    //  k があれば true, さもなくば false を返す。
    int i,j;
    for(i=top; i<top+nrow; i++) {
      for(j=left; j<left+ncol; j++) {
	if (board[i,j]==k) { return true; }
      }
    }
    return false;
  }
}
class Pos {
  int private_row;
  int private_col;
  public int r { get { return private_row; } }
  public int c { get { return private_col; } }
  public Pos(int row, int col) {
    private_row = row;
    private_col = col;
  }
  public override string ToString() {
    return String.Format("r{0}c{1}", private_row, private_col);
  }
}


