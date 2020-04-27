using System.Windows.Forms;
using RPG;
public class Task
{
    public static Player.Status player_last_status = Player.Status.WALK;
    public static void Talk(int i)
    {
        if (Player.status != Player.Status.TASK)
            player_last_status = Player.status;
        Player.status = Player.Status.TASK;
        
        if(i==0)
        {
            Message.show("인전","안녕하세요!", "yo.png", Message.Face.LEFT);
            block();
            Message.show("NPC", "게임월드에 오신걸 환영합니다!", "face3_2.png", Message.Face.RIGHT);
            block();
            Message.show("인전", "나의 첫번쨰 임무는 무엇인가요?", "yo.png", Message.Face.LEFT);
            block();
        }
        if(i==1)
        {
            Message.showtip("남자를 만나다");
            block();
        }
        if(i==2)
        {
            Map.change_map(Form1.map, Form1.player, Form1.npc, 1, 950, 500, 2, Form1.music_player);
        }
        if(i==3)
        {
            Map.change_map(Form1.map, Form1.player, Form1.npc, 0, 50, 500, 3, Form1.music_player);
        }
        Player.status = player_last_status;
    }
    public static void block()
    {
        while (Player.status == Player.Status.PANEL)
            Application.DoEvents();
    }
}