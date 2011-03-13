// desc: backtrack の練習のため数独を解かせる ; 理詰めゼロで ; 再帰無しで。

// date: 11;2011/3/13 (sun)     ... 今日中に出来るか？
// command: gmcs sudoku.cs && mono sudoku.exe

// やること：
// 1. 最上行の最左列(tl)から埋めていく。
// 2. tl に置ける値の内、最小のものを入れる。 goto 1.
// 3. もし置ける値がなければ一つ前に戻る。 goto 1.
// '1.'の時点で埋められるマスがなければゲーム終了。

// やりかた：
// 数字を置くときは board に書き込み、置いた位置を stack に push する。
// 「1つ前に戻る」ときには stack から pop して得た位置にある数字を board から取り除く。

using System;

// 外側に配列(0-8)を置きたいけど普通に書くと error なのでクラスで包む。
class Range9 {
  int [] private_ary = { 0,1,2,3,4,5,6,7,8 };
  public int [] ary { get { return private_ary; } }
}

//------------------------------------------------------------------------------
// Ary2d
//------------------------------------------------------------------------------
class Ary2d {
  static Range9 r = new Range9();
  public static string to_s(int[,] ary2d) {
    string res = "{ ";
    foreach(int i in r.ary) {
      res += "{";
      foreach(int j in r.ary) {
	res += ary2d[i,j] + (j==r.ary.Length-1 ? "":",");
      }
      res += "}" + (i==r.ary.Length-1 ? "":", ");
    }
    res += " }";
    return res;
  }
  public static string to_sbs(int[,] ary2d) { // abbrev: to sudoku board string
    string res = "begin\n";
    foreach(int i in r.ary) {
      res += "  "; // indentation
      foreach(int j in r.ary) {
	res += (ary2d[i,j]==0 ? "_":ary2d[i,j].ToString());
	if ( (j+1)%3==0 ) { res += "  "; }
      }
      res += "\n";
      if ( (i+1)%3==0 && i<r.ary.Length-1 ) { res += "\n"; }
    }
    res += "end";
    return res;
  }
}

//------------------------------------------------------------------------------
// Main
//------------------------------------------------------------------------------
class SudokuSolve {
  static void Main() {
    int[,] board = {
      { 0,0,8, 0,0,0, 0,0,0 },
      { 1,0,0, 6,0,0, 3,0,0 },
      { 0,0,0, 5,1,0, 9,0,0 },
			     
      { 0,0,1, 0,0,0, 4,0,0 },
      { 0,6,0, 0,8,7, 0,0,0 },
      { 0,0,0, 0,3,0, 5,0,2 },
			     
      { 0,0,9, 0,0,4, 0,0,0 },
      { 0,7,0, 0,0,9, 0,4,0 },
      { 5,3,0, 0,0,0, 0,0,0 },
    };
    SudokuSolver ss = new SudokuSolver(board);

    Console.WriteLine( Ary2d.to_sbs(board) );

    while (true) {
      if (!ss.isend()) {
	ss.putnum();
      } else {
	break;
      }
    }
  }
}

//------------------------------------------------------------------------------
// SudokuSolver
//------------------------------------------------------------------------------
class SudokuSolver {
  // - board と、埋めねばならない位置のリスト fillpos と, list placeable_flags と,
  //   guess (num)の列と、「次に埋める位置 pos」を保持する。
  // - placeable_flags : 初期盤面にて数字が置かれていない各マスに対し9本の flag を置く。
  //   そのマスに置けるかもしれない数字に対しては true, さもなくば false とする。
  // - 盤面が固定されてるので、各マスに対して始めに placeable_flags を計算しておく。
  static Range9 r = new Range9();
  int [,] board;
  List<Pos>    fillpos         = new List<Pos>(); // 埋めねばならない位置のリスト
  List<bool[]> placeable_flags = new List<bool[]>();
  List<int>    guess_seq       = new List<int>();
  int pos_i;

  // ---------------------- Constructor ----------------------
  public SudokuSolver(int [,] b) {
    board = b;
    init_fillpos();
    init_placeable_flags();
    pos_i = 0; // fillpos, placeable_flags の index.
  }
  void init_fillpos() {
    foreach(i in r.ary) {
      foreach(j in r.ary) {
	if (board[i,j]!=0) { fillpos.Add(new Pos(i,j)); }
      }
    }
  }
  void init_placeable_flags() {
    foreach(int [] pos in fillpos) {
      bool [] flags = { true,true,true,true,true,true,true,true,true };
      int i=pos.r, j=pos.c;
      check_rect_area(flags, i,      j,       1,9);
      check_rect_area(flags, i,      j,       9,1);
      check_rect_area(flags, (i/3)*3,(j/3)*3, 3,3);
      placeable_flags.Add( flags );
    }
  }
  void check_rect_area(bool[] flags, int top, int left, int nrow, int ncol) {
    // top, left, nrow, ncol で指定される四角形内を見る。
    for(i=top; i<top+nrow; i++) {
      for(j=left; j<left+ncol; j++) {
	if (board[i,j]!=0) { flags[ board[i,j]+1 ] = false; }
      }
    }
  }

  // ---------------------- isend ----------------------
  public bool isend() {
    // board が埋まっていれば true, さもなくば false.
    foreach(int i in r.ary) {
      foreach(int j in r.ary) {
	if (board[i,j]==0) { return false; }
      }
    }
    return true;
  }

  // ---------------------- putnum ----------------------
  public void putnum() {
    // fillpos[pos_i] の位置に数字を置く。
    // pos を見る。
    // - 可能な guess があればそれを push する。
    //   pos_i++;
    //   関連するマスの placeable_flags に修正を加える。
    // - なければ直前の guess が失敗だったことになるので pop してその guess を取る。
    //   guess.pos を見て board を元に戻し、
    //   guess.num を見てその flag を false にする。
    //   pos_i--;
    // - pop ができなかった場合、問題は解けないことになる。
    bool [] flags = placeable_flags[pos_i];
    Pos pos       = fillpos[pos_i];
    if( is_anytrue(flags) ) {
      int num = flags.IndexOf(true);
      guess_seq.Add( num );           // guess の追加。
      board[pos.r, pos.c] = num;      // board への書き込み。
      update_related_flags(pos, num); // flags の更新。
      pos_i++;                        // pos_i のインクリメント。
    } else { // fillpos[pos_i] の位置に置ける数字がなかった場合。
      if ( guess_seq.Count > 0 ) {
	// ****** 盤面は戻せても flags は元に戻せない ******
	// 倒される前の20マスの flags の値を保持しておかねば。
	pos_i--;
      } else {
	
      }
    }
  }
  void update_related_flags(Pos pos, int num) {
    // pos と同行・同列・同ブロックにある各マスにある flags の
    // num に対する値を false にする。
    int i = pos.r, j = pos.c;
    update_rect_area(num, i,      j,       1,9);
    update_rect_area(num, i,      j,       9,1);
    update_rect_area(num, (i/3)*3,(j/3)*3, 3,3);
  }
  void update_rect_area(int num, int top, int left, int nrow, int ncol) {
    // top, left, nrow, ncol で指定される四角形内の各マスの flags に対し、
    // num に対する値を false にする。
    int npos = fillpos.Count;
    for(i=0; i<npos; i++) {
      int r = fillpos[i].r, c = fillpos[i].c;
      if ( top<=r && r<top+nrow && left<=c && c<left+ncol ) {
	placeable_flags[i][ num-1 ] = false;
      }
    }
  }
}
class Pos {
  int private_row;
  int private_col;
  public int r { get { return private_row; } }
  public int c { get { return private_col; } }
}
//class Guess {
//  Pos private_pos;
//  int private_num;
//  public int pos { get { return private_pos; } }
//  public int num { get { return private_num; } }
//}

// topleft pos に対し bool [] placeable_flags = new bool [9]; を割り当てる。
// 一番小さい placeable_num を打つ ...
// - その pos と上記の placeable_flags と置いた数 placeable_num を stack にのせる。
// もし placeable_num がなければ(placeable_flags がすべて false のとき)、
// - stack から pop して pos にある数字を消す。
//   placeable_flags 内の placeable_num に対する flag を false にする。
// - stack が空なら could not solve.




// 埋めねばならぬ位置 pos の全体は List で管理する。
