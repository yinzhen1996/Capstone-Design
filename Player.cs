using System.Windows.Forms;
using System.Drawing;

//----------------------------------------------------------------
//     player 角色类
//
//----------------------------------------------------------------
public class Player
{
    //当前角色
    public static int current_player = 0;
    public enum Status
    {
        WALK = 1,
        PANEL = 2,
        TASK = 3,
        FIGHT = 4,

    }
    public static Status status = Status.WALK;
    //行走
    public int x = 0;
    public int y = 0;
    public int face = 1;
    public int anm_frame = 0;
    public long last_walk_time = 0;
    public long walk_interval = 60;
    public int speed = 30;

    public int x_offset = -120;
    public int y_offset = -220;


    //图像
    public Bitmap bitmap;
    //是否激活
    public int is_active = 0;
    //碰撞
    public int collision_ray = 80;

    public Player()
    {
        bitmap = new Bitmap(@"r1.png");
        bitmap.SetResolution(96, 96);
        move_flag = new Bitmap(@"move_flag.png");
        move_flag.SetResolution(96, 96);
    }

    //鼠标操作
    public static int target_x = -1;
    public static int target_y = -1;

    public static Bitmap move_flag;
    public static long FLAG_SHOW_TIME = 3000;
    public static long flag_start_time = 0;

    //----------------------------------------------------------------
    //     操控
    //----------------------------------------------------------------
    public static void key_ctrl(Player[] player, Map[] map, Npc[] npc, KeyEventArgs e)
    {
        if (Player.status != Status.WALK)
            return;

        Player p = player[current_player];
        //切换角色
        if (e.KeyCode == Keys.Tab) { key_change_player(player); }

        //行走
        if (e.KeyCode == Keys.Up)
        {
            walk(player, map, Comm.Direction.UP);
        }
        else if (e.KeyCode == Keys.Down)
        {
            walk(player, map, Comm.Direction.DOWN);
        }
        else if (e.KeyCode == Keys.Left)
        {
            walk(player, map, Comm.Direction.LEFT);
        }
        else if (e.KeyCode == Keys.Right)
        {
            walk(player, map, Comm.Direction.RIGHT);
        }
        //npc碰撞
        npc_collision(player, map, npc, e);

    }

    public static Point get_collision_point(Player[] player)
    {
        Player p = player[current_player];
        int collision_x = 0;
        int collision_y = 0;

        if (p.face == (int)Comm.Direction.UP)
        {
            collision_x = p.x;
            collision_y = p.y - p.collision_ray;
        }
        if (p.face == (int)Comm.Direction.DOWN)
        {
            collision_x = p.x;
            collision_y = p.y + p.collision_ray;
        }
        if (p.face == (int)Comm.Direction.LEFT)
        {
            collision_x = p.x - p.collision_ray;
            collision_y = p.y;
        }
        if (p.face == (int)Comm.Direction.RIGHT)
        {
            collision_x = p.x + p.collision_ray;
            collision_y = p.y;
        }
        return new Point(collision_x, collision_y);
    }

    public static void npc_collision(Player[] player, Map[] map, Npc[] npc, KeyEventArgs e)
    {
        Player p = player[current_player];

        Point p1 = new Point(p.x, p.y);
        Point p2 = get_collision_point(player);

        for (int i = 0; i < npc.Length; i++)
        {
            if (npc[i] == null)
                continue;

            if (npc[i].map != Map.current_map)
                continue;


            if (npc[i].is_line_collision(p1, p2))
            {
                if (npc[i].collision_type == Npc.Collosion_type.ENTER)
                {
                    Task.Talk(i);
                    break;
                }
                else if (npc[i].collision_type == Npc.Collosion_type.KEY)
                {
                    if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
                    {
                        Task.Talk(i);
                        break;
                    }
                }

            }
        }
    }

    public static void walk(Player[] player, Map[] map, Comm.Direction direction)
    {
        Player p = player[current_player];
        //转向
        p.face = (int)direction;
        //间隔判定
        if (Comm.Time() - p.last_walk_time <= p.walk_interval)
            return;
        //行走
        //up
        if (direction == Comm.Direction.UP && Map.can_through(map, p.x, p.y - p.speed))
        {
            p.y = p.y - p.speed;
        }
        //down
        else if (direction == Comm.Direction.DOWN && Map.can_through(map, p.x, p.y + p.speed))
        {
            p.y = p.y + p.speed;
        }
        //right
        else if (direction == Comm.Direction.LEFT && Map.can_through(map, p.x - p.speed, p.y))
        {
            p.x = p.x - p.speed;
        }
        //left
        else if (direction == Comm.Direction.RIGHT && Map.can_through(map, p.x + p.speed, p.y))
        {
            p.x = p.x + p.speed;
        }

        //动画帧
        p.anm_frame = p.anm_frame + 1;
        if (p.anm_frame >= int.MaxValue) p.anm_frame = 0;
        //时间
        p.last_walk_time = Comm.Time();
        return;
    }

    public static void key_ctrl_up(Player[] player, KeyEventArgs e)
    {

        stop_walk(player);

        //注意 抽出成函数了
        //Player p = player[current_player];
        //动画帧
        //p.anm_frame = 0;
        // p.last_walk_time = 0;
    }


    public static void stop_walk(Player[] player)
    {
        Player p = player[current_player];
        //动画帧
        p.anm_frame = 0;
        p.last_walk_time = 0;
        //目标位置
        target_x = -1;
        target_y = -1;
    }

    //----------------------------------------------------------------
    //     绘图
    //----------------------------------------------------------------
    public static void draw(Player[] player, Graphics g, int map_sx, int map_sy)
    {
        Player p = player[current_player];

        Rectangle crazycoderRgl = new Rectangle(p.bitmap.Width / 4 * (p.anm_frame % 4), p.bitmap.Height / 4 * (p.face - 1), p.bitmap.Width / 4, p.bitmap.Height / 4);//定义区域
        Bitmap bitmap0 = p.bitmap.Clone(crazycoderRgl, p.bitmap.PixelFormat);//复制小图
        g.DrawImage(bitmap0, map_sx + p.x + p.x_offset, map_sy + p.y + p.y_offset);
    }



    public static void draw_flag(Graphics g, int map_sx, int map_sy)
    {
        if (target_x < 0 || target_y < 0)
            return;

        if (move_flag == null)
            return;

        if (Comm.Time() - flag_start_time > FLAG_SHOW_TIME)
            return;

        g.DrawImage(move_flag, map_sx + target_x - 16, map_sy + target_y - 25);

    }


    //----------------------------------------------------------------
    //     切换角色
    //----------------------------------------------------------------
    public static void key_change_player(Player[] player)
    {
        for (int i = current_player + 1; i < player.Length; i++)
            if (player[i].is_active == 1)
            {
                set_player(player, current_player, i);
                return;
            }
        for (int i = 0; i < current_player - 1; i++)
            if (player[i].is_active == 1)
            {
                set_player(player, current_player, i);
                return;
            }
    }

    public static void set_player(Player[] player, int oldindex, int newindex)
    {
        current_player = newindex;
        player[newindex].x = player[oldindex].x;
        player[newindex].y = player[oldindex].y;
        player[newindex].face = player[oldindex].face;

    }

    //----------------------------------------------------------------
    //     位置
    //----------------------------------------------------------------

    public static int get_pos_x(Player[] player)
    {
        return player[current_player].x;
    }

    public static int get_pos_y(Player[] player)
    {
        return player[current_player].y;
    }

    public static int get_pos_f(Player[] player)
    {
        return player[current_player].face;
    }

    public static void set_pos(Player[] player, int x, int y, int face)
    {
        player[current_player].x = x;
        player[current_player].y = y;
        player[current_player].face = face;
    }

    //----------------------------------------------------------------
    //     鼠标操控
    //----------------------------------------------------------------

    public static void mouse_click(Map[] map, Player[] player, Rectangle stage, MouseEventArgs e)
    {
        if (Player.status != Status.WALK)
            return;

        if (e.Button == MouseButtons.Left)
        {

            target_x = e.X - Map.get_map_sx(map, player, stage);
            target_y = e.Y - Map.get_map_sy(map, player, stage);
            flag_start_time = Comm.Time();
        }
    }


    public static void timer_logic(Player[] player, Map[] map)
    {
        move_logic(player, map);
    }


    public static void move_logic(Player[] player, Map[] map)
    {
        if (target_x < 0 || target_y < 0)
            return;

        step_to(player, map, target_x, target_y);
    }


    public static void step_to(Player[] player, Map[] map, int target_x, int target_y)
    {


        if (is_reach_x(player, target_x) == 0
            && is_reach_y(player, target_y) == 0)
        {
            stop_walk(player);
            return;
        }



        Player p = player[current_player];

        if (is_reach_x(player, target_x) > 0
            && Map.can_through(map, p.x - p.speed, p.y))
        {
            walk(player, map, Comm.Direction.LEFT);
            return;
        }
        else if (is_reach_x(player, target_x) < 0
            && Map.can_through(map, p.x + p.speed, p.y))
        {
            walk(player, map, Comm.Direction.RIGHT);
            return;
        }


        if (is_reach_y(player, target_y) > 0
            && Map.can_through(map, p.x, p.y - p.speed))
        {
            walk(player, map, Comm.Direction.UP);
            return;
        }
        else if (is_reach_y(player, target_y) < 0
            && Map.can_through(map, p.x, p.y + p.speed))
        {
            walk(player, map, Comm.Direction.DOWN);
            return;
        }

        stop_walk(player);

    }



    public static int is_reach_x(Player[] player, int target_x)
    {
        Player p = player[current_player];

        if (p.x - target_x > p.speed / 2) return 1;
        if (p.x - target_x < -p.speed / 2) return -1;
        return 0;
    }

    public static int is_reach_y(Player[] player, int target_y)
    {
        Player p = player[current_player];

        if (p.y - target_y > p.speed / 2) return 1;
        if (p.y - target_y < -p.speed / 2) return -1;
        return 0;
    }

}
