using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ItemManagerParser
{
    class Program
    {
        class Brick
        {
            public int Id { get; set; }
            public int Offset { get; set; }
            public int Type { get; set; }
            public int Count { get; set; }
        }
        class Morph
        {
            public string Type { get; set; }
            public int Offset { get; set; }
            public int Count { get; set; }
        }

        public enum Type
        {
            forgroundBricksBMD = 0,
            backgroundBricksBMD = 1,
            decorationsBMD = 2,
            specialBricksBMD = 3,
            effectBricksBMD = 4,
            doorsBMD = 5
        };

        static void Main(string[] undefned)
        {
            var file = File.ReadAllLines(@"..\..\..\..\EverybodyEdits\scripts\ItemManager.as");
            var fg_bitmap = Bitmap.FromFile(@"..\..\..\..\EverybodyEdits\images\1517_items.ItemManager_forgroundBricksBM.png");
            var bg_bitmap = Bitmap.FromFile(@"..\..\..\..\EverybodyEdits\images\1527_items.ItemManager_backgroundBricksBM.png");
            var dc_bitmap = Bitmap.FromFile(@"..\..\..\..\EverybodyEdits\images\1531_items.ItemManager_decorationsBM.png");
            var sp_bitmap = Bitmap.FromFile(@"..\..\..\..\EverybodyEdits\images\1532_items.ItemManager_specialBricksBM.png");
            var ef_bitmap = Bitmap.FromFile(@"..\..\..\..\EverybodyEdits\images\1550_items.ItemManager_effectBricksBM.png");
            var dr_bitmap = Bitmap.FromFile(@"..\..\..\..\\EverybodyEdits\images\1575_items.ItemManager_doorsBM.png");

            var bricks = new List<Brick>();
            var morphs = new List<Morph>();

            foreach (var line in file)
            {
                //:BlSprite = new BlSprite(specialBricksBMD,440,0,16,16,4);
                if (line.Contains(":BlSprite = new BlSprite("))
                {
                    var args = line.Split(',');
                    var type = args[0].Split('(')[1];

                    var offset = args[1];
                    var count = new string(args[5].Where(x => char.IsDigit(x)).ToArray());

                    if (Enum.GetNames(typeof(Type)).Any(x => x == type))
                        morphs.Add(new Morph() { Type = type, Offset = int.Parse(offset), Count = int.Parse(count) });
                }
            }

            foreach (var line in file)
            {
                if (line.Contains(".addBrick(createBrick("))
                {
                    var args = line.Split(',');
                    var id = args[0].Split('(')[2];
                    var offset = args[8];

                    int _id;
                    int _offset;

                    var name = Enum.GetNames(typeof(Type)).ToList().Where(x => args.Contains(x)).FirstOrDefault();

                    if (name == null)
                        continue;
                    else {
                        var type = Enum.Parse(typeof(Type), name);

                        if (type == null || (int)type == -1)
                            continue;

                        if (!int.TryParse(id, out _id))
                            _id = (int)Enum.Parse(typeof(ItemId), id.Replace("ItemId.", ""));

                        offset = Convert.ToString(new System.Data.DataTable().Compute(offset, null));

                        if (int.TryParse(offset, out _offset))
                        {
                            var count = morphs.Where(x => x.Offset == _offset - 1 && x.Type == name).FirstOrDefault()?.Count;

                            if (count == null)
                                count = 1;

                            bricks.Add(new Brick() { Id = _id, Type = (int)type, Offset = _offset, Count = (int)count });
                        }
                    }
                }
            }

            var main = new Bitmap(16 * bricks.OrderByDescending(x => x.Id).First().Id, 16 * bricks.OrderByDescending(x => x.Count).First().Count);
            var g = Graphics.FromImage(main);

            foreach (var brick in bricks)
            {
                Bitmap bitmap = null;

                switch (brick.Type)
                {
                    case 0:
                        bitmap = (Bitmap)fg_bitmap;
                        break;
                    case 1:
                        bitmap = (Bitmap)bg_bitmap;
                        break;
                    case 2:
                        bitmap = (Bitmap)dc_bitmap;
                        break;
                    case 3:
                        bitmap = (Bitmap)sp_bitmap;
                        break;
                    case 4:
                        bitmap = (Bitmap)ef_bitmap;
                        break;
                    case 5:
                        bitmap = (Bitmap)dc_bitmap;
                        break;
                }

                for (int y = 0; y < brick.Count; y++)
                    g.DrawImageUnscaled(CropImage(bitmap, new Rectangle((brick.Offset * 16) + 16 * y - 16, 0, 16, 16)), brick.Id * 16, y * 16, 16, 16);
            }

            CropImage(main, new Rectangle(16, 0, main.Width-16, main.Height)).Save(@"ItemManagerParser_output.png", System.Drawing.Imaging.ImageFormat.Png);
            Console.WriteLine("Done.");
            Console.ReadLine();
        }

        public static Bitmap CropImage(Bitmap source, Rectangle section)
        {
            Bitmap bmp = new Bitmap(section.Width, section.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);

            return bmp;
        }
    }

    public enum ItemId
    {
        SWITCH_PURPLE = 113,
        DOOR_PURPLE = 184,
        GATE_PURPLE = 185,
        DOOR_GOLD = 200,
        GATE_GOLD = 201,
        SPEED_LEFT = 114,
        SPEED_RIGHT = 115,
        SPEED_UP = 116,
        SPEED_DOWN = 117,
        CHAIN = 118,
        WATER = 119,
        NINJA_LADDER = 120,
        BRICK_COMPLETE = 121,
        RESET_POINT = 466,
        TIMEDOOR = 156,
        TIMEGATE = 157,
        COINDOOR = 43,
        COINGATE = 165,
        BLUECOINDOOR = 213,
        BLUECOINGATE = 214,
        VINE_V = 98,
        VINE_H = 99,
        ROPE = 424,
        DIAMOND = 241,
        WAVE = 300,
        CAKE = 337,
        HOLOGRAM = 397,
        CHECKPOINT = 360,
        SPIKE = 361,
        FIRE = 368,
        MUD = 369,
        MUD_BUBBLE = 370,
        PORTAL = 242,
        WORLD_PORTAL = 374,
        ZOMBIE_GATE = 206,
        ZOMBIE_DOOR = 207,
        GLOWYLINE_BLUE_SLOPE = 375,
        GLOWY_LINE_BLUE_STRAIGHT = 376,
        GLOWY_LINE_YELLOW_SLOPE = 377,
        GLOWY_LINE_YELLOW_STRAIGHT = 378,
        GLOWY_LINE_GREEN_SLOPE = 379,
        GLOWY_LINE_GREEN_STRAIGHT = 380,
        GLOWY_LINE_RED_SLOPE = 438,
        GLOWY_LINE_RED_STRAIGHT = 439,
        PORTAL_INVISIBLE = 381,
        TEXT_SIGN = 385,
        ONEWAY_CYAN = 1001,
        ONEWAY_ORANGE = 1002,
        ONEWAY_YELLOW = 1003,
        ONEWAY_PINK = 1004,
        ONEWAY_GRAY = 1052,
        ONEWAY_BLUE = 1053,
        ONEWAY_RED = 1054,
        ONEWAY_GREEN = 1055,
        ONEWAY_BLACK = 1056,
        ONEWAY_WHITE = 1092,
        DEATH_DOOR = 1011,
        DEATH_GATE = 1012,
        LAVA = 416,
        EFFECT_JUMP = 417,
        EFFECT_FLY = 418,
        EFFECT_RUN = 419,
        EFFECT_PROTECTION = 420,
        EFFECT_CURSE = 421,
        EFFECT_ZOMBIE = 422,
        EFFECT_TEAM = 423,
        EFFECT_LOW_GRAVITY = 453,
        EFFECT_MULTIJUMP = 461,
        TEAM_DOOR = 1027,
        TEAM_GATE = 1028,
        MEDIEVAL_SHIELD = 273,
        MEDIEVAL_AXE = 275,
        MEDIEVAL_BANNER = 327,
        MEDIEVAL_COATOFARMS = 328,
        MEDIEVAL_SWORD = 329,
        MEDIEVAL_TIMBER = 440,
        TOOTH_BIG = 338,
        TOOTH_SMALL = 339,
        TOOTH_TRIPLE = 340,
        DOJO_LIGHT_LEFT = 276,
        DOJO_LIGHT_RIGHT = 277,
        DOJO_DARK_LEFT = 279,
        DOJO_DARK_RIGHT = 280,
        DOMESTIC_LIGHT_BULB = 447,
        DOMESTIC_TAP = 448,
        DOMESTIC_PAINTING = 449,
        DOMESTIC_VASE = 450,
        DOMESTIC_TV = 451,
        DOMESTIC_WINDOW = 452,
        HALFBLOCK_DOMESTIC_YELLOW = 1041,
        HALFBLOCK_DOMESTIC_BROWN = 1042,
        HALFBLOCK_DOMESTIC_WHITE = 1043,
        HALLOWEEN_2015_ONEWAY = 1050,
        HALLOWEEN_2015_WINDOW_RECT = 456,
        HALLOWEEN_2015_WINDOW_CIRCLE = 457,
        HALLOWEEN_2015_LAMP = 458,
        ONEWAY_SCIFI_YELLOW = 1051,
        ICE = 1064,
        SLOW_DOT = 459,
        SLOW_DOT_INVISIBLE = 460,
        NEW_YEAR_2015_BALLOON = 464,
        NEW_YEAR_2015_STREAMER = 465,
        FAIRYTALE_FLOWERS = 471,
        FAIRYTALE_LADDER = 472,
        HALFBLOCK_FAIRYTALE_ORANGE = 1075,
        HALFBLOCK_FAIRYTALE_GREEN = 1076,
        HALFBLOCK_FAIRYTALE_BLUE = 1077,
        HALFBLOCK_FAIRYTALE_PINK = 1078,
        SWITCH_ORANGE = 467,
        DOOR_ORANGE = 1079,
        GATE_ORANGE = 1080,
        SPRING_DAISY = 475,
        SPRING_TULIP = 476,
        SPRING_DAFFODIL = 477,
        SUMMER_FLAG = 481,
        SUMMER_AWNING = 482,
        SUMMER_ICECREAM = 483,
    }

}
