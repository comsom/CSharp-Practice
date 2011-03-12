// desc: 券売機 ticket-vending machine シミュレータ ... delegate の練習。

// ticketvm tvm :abbrev: ticket-vending machine

// command : gmcs ticketvm.cs && mono ticketvm.exe

// 券売機の仕様を日本語で書くと：
// - コイン投入を待ち、投入されるたびに蓄積する。
//   コインには 1,5,10,50,100,500 円玉の6種類があり、1,5円玉の場合にはそれを返却する。
// - ボタンを押して券を買う。券は1枚だけ買える。買ったらおつりも返す。
//   券には 120,150,180,210,260 の5種類があり、それぞれに対応したボタンがある。
// - キャンセルボタンがある。押されたら投入されたお金を返して終了する。
//   キャンセルボタンが押された時に *だけ* 正常に END となる。

using System;
using System.Text.RegularExpressions;

// まず delegate を使わずに書いてみる。
class Simulator {
  static void Main() {
    TVM tvm = new TVM();
    while (true) {
      tvm.run();
      if (tvm.state == TVM.State.END) { break; }
    }
  }
}

class TVM {
  public enum State { START, WAIT, END };
  State private_state = State.START;
  public State state { get { return private_state; } }
  string [] buttons = { "A", "B", "C", "D", "E", "cancel", "exit" };
  int [] prices = { 120, 150, 180, 210, 260 };
  int coin_total = 0;
  public void run() {
    switch (private_state) {
    case State.START :
      Console.WriteLine("The ticket-vending machine started running.");
      Console.WriteLine("Drop the coins in the ticket-vending machine,");
      Console.WriteLine("and push the ticket button to buy a ticket.");
      Console.WriteLine("There are 7 buttons :" +
			" [{0}]:{1}, [{2}]:{3}," +
			" [{4}]:{5}, [{6}]:{7}," +
			" [{8}]:{9}," +
			" [{10}], or [{11}]",
		 	buttons[0],prices[0], buttons[1],prices[1],
		 	buttons[2],prices[2], buttons[3],prices[3],
		 	buttons[4],prices[4],
			buttons[5], buttons[6]);
      private_state = State.WAIT;
      break;
    case State.WAIT :
      string action;
      Regex r_coin = new Regex("^(1|5|10|50|100|500)$");
      Regex r_button = new Regex("^(A|B|C|D|E|cancel|exit)$");
      int i;
      Console.WriteLine("");
      // 表示：投入総額
      if (coin_total > 0) {
	Console.WriteLine("coin total : {0}", coin_total);
      }
      // 表示：押すことのできるボタン
      Console.Write("Buttons ... ");
      for (i=0; i<prices.Length && prices[i] <= coin_total; i++) {
	Console.Write("[{0}]:{1} ", buttons[i], prices[i]);
      }
      Console.WriteLine("[{0}] [{1}]", buttons[5], buttons[6]);
      // 表示：コインについて
      Console.WriteLine("You can use the 10, 50, 100, or 500 yen coin.");
      // 入力を受け付ける。
      Console.Write("> ");
      action = Console.ReadLine();
      if ( r_coin.IsMatch(action) ) {
	// (1) コインを投入した場合
	Match m_coin = r_coin.Match(action);
	int c = int.Parse(m_coin.Value);
	if (c==1 || c==5) {
	  Console.WriteLine("I can't deal {0} yen coins.", c);
	} else {
	  coin_total += c;
	}
      } else if ( r_button.IsMatch(action) ) {
	// (2) ボタンを押した場合
	Match m_button = r_button.Match(action);
	int j = Array.IndexOf(buttons, m_button.Value); // index of button
	if ( j < prices.Length ) {
	  // ticket を買う場合
	  int ticket_price = prices[j];
	  if (ticket_price <= coin_total) {
	    // 投入総額が足りている場合 ... 買える。
	    Console.WriteLine("Bought the ticket {0}:{1}",
			      action, ticket_price);
	    coin_total -= ticket_price;
	  } else {
	    // 投入総額が足りない場合 ... 買えない。
	    Console.WriteLine("You can't buy the ticket {0}:{1}.",
			      action, ticket_price);
	  }
	} else if ( String.Compare(action, buttons[5])==0 ) {
	  // cancel する場合
	  Console.WriteLine("Canceled. {0} yen is returned in change.",
			    coin_total);
	  coin_total = 0;
	} else {
	  // exit する場合
	  Console.WriteLine("Exit. {0} yen is returned in change.",
			    coin_total);
	  private_state = State.END;
	}
      } else {
	// (3) 不正な action であった場合
	Console.WriteLine("Illegal action.");
      }
      break;
    case State.END :
      Console.WriteLine("Bye.");
      break;
    }
  }
}
