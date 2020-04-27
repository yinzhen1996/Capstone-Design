using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPG
{
    public partial class Form1 : Form
  
    {
        public static Player[] player = new Player[3];
        public static Map[] map = new Map[2];
        public static Npc[] npc = new Npc[6];

        public static WMPLib.WindowsMediaPlayer music_player = new WMPLib.WindowsMediaPlayer();


        public Bitmap mc_nomal;
        public Bitmap mc_event;
        public int mc_mod = 0;//0-nomal 1-event


        public Form1()
        {
            InitializeComponent();
        }


        private void Draw()
        {
            //创建在pictureBox1上的图像g1
            Graphics g1 = stage.CreateGraphics();
            // 将图像画在内存上，并使g为pictureBox1上的图像
            BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
            BufferedGraphics myBuffer = currentContext.Allocate(g1, this.DisplayRectangle);
            Graphics g = myBuffer.Graphics;
            // 自定义绘图
            Map.draw(map, player, npc, g, new Rectangle(0, 0, stage.Width, stage.Height));
            if (Panel.panel != null)
                Panel.draw(g);


            draw_mouse(g);
            // 显示图像并释放资源
            myBuffer.Render();
            myBuffer.Dispose();
         
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Player.key_ctrl(player, map, npc, e);
            if (Panel.panel != null)
                Panel.key_ctrl(e);
            Draw();
        }
        public void tryevent()
        {
            MessageBox.Show("成功");
        }






        private void Form1_Load(object sender, EventArgs e)
        {

            //光标
            mc_nomal = new Bitmap(@"mc_1.png");
            mc_nomal.SetResolution(96, 96);
            mc_event = new Bitmap(@"mc_2.png");
            mc_nomal.SetResolution(96, 96);
            Title.init();
            Message.init();

            //player define
            player[0] = new Player();
            player[0].bitmap = new Bitmap(@"r1.png");
            player[0].bitmap.SetResolution(96, 96);
            player[0].is_active = 1;

            player[1] = new Player();
            player[1].bitmap = new Bitmap(@"r2.png");
            player[1].bitmap.SetResolution(96, 96);
            player[1].is_active = 1;

            player[2] = new Player();
            player[2].bitmap = new Bitmap(@"r3.png");
            player[2].bitmap.SetResolution(96, 96);
            player[2].is_active = 1;
            //npc define
            npc[0] = new Npc();
            npc[0].map = 0;
            npc[0].x = 700;
            npc[0].y = 300;
            npc[0].bitmap_path = "npc1.png";
            npc[0].collision_type = Npc.Collosion_type.KEY;

            npc[1] = new Npc();
            npc[1].map = 0;
            npc[1].x = 800;
            npc[1].y = 280;
            npc[1].bitmap_path = "npc2.png";
            npc[1].collision_type = Npc.Collosion_type.KEY;

            npc[2] = new Npc();
            npc[2].map = 0;
            npc[2].x = 20;
            npc[2].y = 600;
            npc[2].region_x = 40;
            npc[2].region_y = 400;
            npc[2].collision_type = Npc.Collosion_type.ENTER;

            npc[3] = new Npc();
            npc[3].map = 1;
            npc[3].x = 980;
            npc[3].y = 600;
            npc[3].region_x = 40;
            npc[3].region_y = 400;
            npc[3].collision_type = Npc.Collosion_type.ENTER;

           

           

            //map define
            map[0] = new Map();
            map[0].bitmap_path = "map1.png";
            map[0].shade_path = "map1_shade.png";
            map[0].block_path = "map1_block.png";
            map[0].music = "1.mp3";
            map[0].back_path = "map1_back.png";

            map[1] = new Map();
            map[1].bitmap_path = "map2.png";
            map[1].shade_path = "map2_shade.png";
            map[1].block_path = "map2_block.png";
            map[1].music = "2.mp3";





            Title.show();
          
            MessageBox.Show("먼저 CRTL키보드를 누러서 화면이 나타나다.질문이 있으면 thefrozensnow@naver.com로 연락해주십시오.");
        }

        private void stage_Click_1(object sender, EventArgs e)
        {
            
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Player.key_ctrl_up(player, e);
        }

        private void draw_mouse(Graphics g)
        {
            Point showpoint = stage.PointToClient(Cursor.Position);
            if (mc_mod == 0)
                g.DrawImage(mc_nomal, showpoint.X, showpoint.Y);
            else
                g.DrawImage(mc_event, showpoint.X, showpoint.Y);
        }

     
       

        
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void stage_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (Panel.panel != null)
                Panel.mouse_move(e);
            mc_mod = Npc.check_mouse_collision(map, player, npc, new Rectangle(0, 0, stage.Width, stage.Height), e);
        }

        private void stage_MouseClick(object sender, MouseEventArgs e)
        {
            Player.mouse_click(map, player, new Rectangle(0, 0, stage.Width, stage.Height), e);
            Npc.mouse_click(map, player, npc, new Rectangle(0, 0, stage.Width, stage.Height), e);
            if (Panel.panel != null)
                Panel.mouse_click(e);
        }

        private void stage_MouseLeave(object sender, EventArgs e)
        {
            Cursor.Show();
        }

        private void stage_MouseEnter(object sender, EventArgs e)
        {
            Cursor.Hide();
        }
    }
}