// desc: トランプのカードを表すクラスを作ってみる。

// command: gmcs -r:afs.dll card.cs && mono card.exe

using System;
using ArrayFunctions;

enum Suit { SPADE, HEART, CLUB, DIAMOND };

// vvv error : A namespace can only contain types and namespace declarations
// Suit [] suits;
// int  [] nums;

class PlayingCard {
  Suit   suit;
  int    num;
  string suit_str;
  string num_str;
  public PlayingCard(Suit s, int n) {
    suit = s;
    num  = n;
    switch (suit) {
    case Suit.SPADE   : suit_str = "S"; break;
    case Suit.HEART   : suit_str = "H"; break;
    case Suit.CLUB    : suit_str = "C"; break;
    case Suit.DIAMOND : suit_str = "D"; break;
    }
    switch (num) {
    case 1  : num_str = "A";            break; // abbrev: Ace
    case 11 : num_str = "J";            break; // abbrev: Jack
    case 12 : num_str = "Q";            break; // abbrev: Queen
    case 13 : num_str = "K";            break; // abbrev: King
    default : num_str = num.ToString(); break;
    }
  }
  public override string ToString() { return suit_str + num_str; }
}

class GenDeck {
  static void Main() {
    Array suits          = Enum.GetValues(typeof(Suit));
    int [] nums          = { 1,2,3,4,5,6,7,8,9,10,11,12,13 };
    PlayingCard [] deck  = new PlayingCard [ suits.Length * nums.Length ];
    int i                = 0; // index for deck
    AFs<PlayingCard> afs = new AFs<PlayingCard>();
    // deck の中身を詰める。
    foreach(Suit s in suits) {
      foreach(int n in nums) {
        deck[i] = new PlayingCard(s,n);
        i++;
      }
    }
    Console.WriteLine( afs.to_s(deck) );
    afs.shuffle(deck);
    Console.WriteLine( afs.to_s(deck) );
  }
  // この勢いで black jack を書く？

  //static void Main() {
  //  Array suits = Enum.GetValues(typeof(Suit));
  //  int [] nums  = { 1,2,3 };
  //  foreach(int n in nums) {
  //    Console.WriteLine(n);                               // testcode
  //  }
  //  foreach(Suit s in suits) {
  //    Console.WriteLine((int)s + " " + s.ToString());     // testcode
  //  }
  //}
}

