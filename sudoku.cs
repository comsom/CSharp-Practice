// desc: backtrack の練習のため数独を解かせる ; 理詰めありで ; 再帰無しで。

// date: 12;2011/3/20 (sun)
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

//---------------------------------------------------------------------------------------------
// Ary2d, ListFun
//---------------------------------------------------------------------------------------------
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

//---------------------------------------------------------------------------------------------
// Main
//---------------------------------------------------------------------------------------------
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

      // 例題：まっさら ... SudokuSolver は解を1つだけ見つける。
      { 0,0,0, 0,0,0, 0,0,0 },
      { 0,0,0, 0,0,0, 0,0,0 },
      { 0,0,0, 0,0,0, 0,0,0 },
      			     
      { 0,0,0, 0,0,0, 0,0,0 },
      { 0,0,0, 0,0,0, 0,0,0 },
      { 0,0,0, 0,0,0, 0,0,0 },
      			     
      { 0,0,0, 0,0,0, 0,0,0 },
      { 0,0,0, 0,0,0, 0,0,0 },
      { 0,0,0, 0,0,0, 0,0,0 },

      // // 例題 from 「レベル判定 IQナンプレ300 vol.2」の問題300(最後の問題)
      // { 0,0,0, 0,9,0, 0,0,0 },
      // { 0,2,0, 0,0,0, 0,7,0 },
      // { 0,0,1, 7,0,8, 3,0,0 },
      // 			     
      // { 0,0,5, 0,0,0, 8,0,0 },
      // { 4,0,0, 0,6,0, 0,0,9 },
      // { 0,0,3, 0,0,0, 4,0,0 },
      // 			     
      // { 0,0,2, 8,0,5, 1,0,0 },
      // { 0,1,0, 0,0,0, 0,3,0 },
      // { 0,0,0, 0,2,0, 0,0,0 },
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
	Console.WriteLine( Ary2d.to_sbs(ss.show_board()) );
	break;
      } else if ( ss.cannot_sovle_flag==true ) {
	Console.WriteLine( "\nCould not solve." );
	break;
      } else {
    	ss.putnum();
	//Console.WriteLine( "\ndebugwrite after putnum : " + Ary2d.to_sbs(ss.show_board()) );
      }
    }

    // end of Main
  }
}

//---------------------------------------------------------------------------------------------
// SudokuSolver
//---------------------------------------------------------------------------------------------
class SudokuSolver {
  // - 盤面 board と、 flagseq の盤 fsboard と、
  //   それらを保持するスタック board_stack, fsboard_stack を持つ。
  // - board[i,j]>0 なら, fsboard[i,j] の値は don't care (見ない・考えない)とする。
  int     [,] board   = new int     [ Range9.length, Range9.length ];
  FlagSeq [,] fsboard = new FlagSeq [ Range9.length, Range9.length ];
  List<int[,]>     board_stack   = new List<int[,]>();
  List<FlagSeq[,]> fsboard_stack = new List<FlagSeq[,]>();
  bool private_cannot_sovle_flag = false; // 解けないと分かったときに立てる。
  public bool cannot_sovle_flag { get { return private_cannot_sovle_flag; } }
  // 表示用のメソッド
  public int [,] show_board() { return board; }

  // ---------------------- Constructor ----------------------
  public SudokuSolver(int [,] b) {
    // board の初期化
    board = b;
    // fsboard の初期化
    foreach(int i in Range9.ary) {
      foreach(int j in Range9.ary) {
	if (board[i,j]==0) {
	  fsboard[i,j] = new FlagSeq(); // すべての flag が true である配列を生成して返す。
	  initial_check_num(i,j);
	}
      }
    }
    // 最初の理詰め
    logical_put(); // 確定したマスに数字を入れて、その影響計算を計算して……
  }
  void initial_check_num(int i, int j) {
    // 位置 r,c に置けうる数の選択肢を、回りに置かれた数を見て絞る。
    initial_check_num_rect(i,j, i,      0,       1,9);
    initial_check_num_rect(i,j, 0,      j,       9,1);
    initial_check_num_rect(i,j, (i/3)*3,(j/3)*3, 3,3);
  }
  void initial_check_num_rect(int r, int c, int top, int left, int nrow, int ncol) {
    int i,j;
    for(i=top; i<top+nrow; i++) {
      for(j=left; j<left+ncol; j++) {
	if (board[i,j]!=0) { fsboard[r,c][ board[i,j]-1 ] = false; }
      }
    }
  }
  // -------------------- logical_put --------------------
  void logical_put() {
    // 理詰めを行う。
    // - flag が高々1本立ってるマスを見つける。
    //   - 確定マスが取れた場合、そこに board 値を入れて、その影響を計算し、再び logical_put();
    //   - 全滅マスが取れた場合は fail.
    //   - 取れなかった場合はそのまま終わる。
    Pos pos = new Pos(-1,-1);
    // 確定してるマスを探す ... 候補が全滅してる
    foreach(int i in Range9.ary) {
      foreach(int j in Range9.ary) {
	if ( board[i,j]==0 ) {
	  if ( fsboard[i,j].decided() ) {
	    // 確定マスが取れた場合
	    pos = new Pos(i,j);
	    break;
	  } else if ( fsboard[i,j].all_false() ) {
	    // 全滅マスが取れた場合
	    //Console.WriteLine("\ndebugwrite : failed.");
	    stack_pop();
	    off_min();
	    break;
	  }
	  // いずれとも異なる(2つ以上の選択肢がある)ならば、次のマスへ。
	}
      }
    }
    // 確定マスが取れた場合、そこに確定した数字を置いて、その影響を計算する。
    if (pos.r>=0 && pos.c>0) {
      put_min(pos);
      // Console.WriteLine( "\ndebugwrite after put_min : " + Ary2d.to_sbs(board) );
      logical_put();
    }
    // さもなくば、何もしない。
  }
  // -------------------- stack_pop, off_min --------------------
  void stack_pop() {
    int i=board_stack.Count-1, j=fsboard_stack.Count-1;
    if (i>=0 && j>=0) {
      // pop する。
      board = board_stack[i];
      board_stack.RemoveAt(i);
      fsboard = fsboard_stack[j];
      fsboard_stack.RemoveAt(j);
    } else {
      // 解けないと分かった。
      private_cannot_sovle_flag = true;
    }
  }
  void off_min() {
    Pos pos = choose_pos();
    FlagSeq fs = fsboard[pos.r, pos.c];
    foreach(int i in Range9.ary) {
      if (fs[i]==true){
	fs[i] = false;
	break;
      }
    }
  }
  // -------------------- put_min --------------------
  void put_min(Pos pos) {
    int r=pos.r, c=pos.c;
    FlagSeq fs = fsboard[r,c];
    int i;
    // 置きにいく。
    for(i=0; i<Range9.length; i++) { if (fs[i]==true) { break; } }
    board[r,c] = i+1;    // flag を倒したりはしない(don't care)。
    // 影響計算をする。
    influence_rect(r,c, r,      0,       1,9);
    influence_rect(r,c, 0,      c,       9,1);
    influence_rect(r,c, (r/3)*3,(c/3)*3, 3,3);
  }
  void influence_rect(int r, int c, int top, int left, int nrow, int ncol) {
    // 位置 r,c の数が他のマスの flagseq に与える影響を計算する。
    int i,j;
    for(i=top; i<top+nrow; i++) {
      for(j=left; j<left+ncol; j++) {
	if (board[i,j]==0) { fsboard[i,j][ board[r,c]-1 ] = false; }
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
    // 仮の数を決めて置く(choose)。理詰めを行う。
    // もし置けなければ fail. 
    Pos pos = choose_pos(); // board に値が設定されていない位置の中から一つとる。
    if (pos.r>=0 && pos.c>=0) {
      if (! fsboard[pos.r, pos.c].all_false()) {
	// 置ける数がある場合
	stack_push();  // board と fsboard をそれぞれの stack に copy を push.
	put_min(pos);  // 選択肢のうち、最小の数を置く。
	logical_put(); // 理詰め。
      } else {
	// 置ける数がない場合 : fail.
	stack_pop(); // board と fsboard に stack から pop した値を入れる。 pop できなければ解無し
	off_min();   // 選択肢内の最小のものの flag を倒す。
      }
    }
  }
  // -------------------- choose_pos --------------------
  Pos choose_pos() {
    // board に値が設定されていない位置の中から一つとる。
    foreach(int i in Range9.ary) {
      foreach(int j in Range9.ary) {
	if (board[i,j]==0) { return new Pos(i,j); }
      }
    }
    return new Pos(-1,-1);
  }
  // -------------------- stack_push --------------------
  void stack_push() {
    // board と fsboard をそれぞれの stack に copy を push.
    board_stack.Add( board_dup() );
    fsboard_stack.Add( fsboard_dup() );
  }
  int [,] board_dup() {
    int [,] b = new int [ Range9.length, Range9.length ];
    foreach(int i in Range9.ary) {
      foreach(int j in Range9.ary) {
	b[i,j] = board[i,j];
      }
    }
    return b;
  }
  FlagSeq [,] fsboard_dup() {
    FlagSeq [,] fsb = new FlagSeq [ Range9.length, Range9.length ];
    foreach(int i in Range9.ary) {
      foreach(int j in Range9.ary) {
	if (board[i,j]==0) { fsb[i,j] = new FlagSeq( fsboard[i,j] ); }
      }
    }
    return fsb;
  }
}
//---------------------------------------------------------------------------------------------
// Pos, FlagSeq
//---------------------------------------------------------------------------------------------
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
class FlagSeq {
  bool [] private_fs = new bool [9];
  public FlagSeq()           { foreach(int i in Range9.ary) { private_fs[i] = true;  } }
  public FlagSeq(FlagSeq fs) { foreach(int i in Range9.ary) { private_fs[i] = fs[i]; } }//copy
  public bool this[int i] { // indexer
    get { return private_fs[i];  }
    set { private_fs[i] = value; }
  }
  public bool decided() {
    // flag がちょうど一本だけ立っていれば true, さもなくば false.
    int ntrue = 0;
    foreach(int i in Range9.ary) {
      if (private_fs[i]==true && ++ntrue >= 2) { return false; }
    }
    return (ntrue==0 ? false : true);
  }
  public bool all_false() {
    // flag がすべて false なら true, さもなくば false.
    foreach(int i in Range9.ary) { if (private_fs[i]==true) { return false; } }
    return true;
  }
}

