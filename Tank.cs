using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;

namespace Tanks_2D_CSharp
{
    class Tank
    {
        protected Bullet bullet; // пуля
        protected bool isShoot; // стрельнул ли уже
		public bool IsShoot { set { this.isShoot = value; } }
        protected Clock clock; // часы
        protected int height, width; // высота и ширина картинки
        protected string fileTank; // имя файла с картинками танка
        protected Image image; // картинка
        protected Texture texture; // текстура
        protected Sprite sprite; // спрайт
		public Sprite SpriteTank { get { return this.sprite; } }
        protected float x, y; // текущие координаты танка;
		public float X { get { return this.x; } }
		public float Y { get { return this.y; } }
        protected float dx, dy; // dx - ускорение по x, dy - ускорение по y
        protected float speed; // скорость движения танка
        protected int direction; // направление движения танка (вверх, вниз, влево, вправо)
        protected int damage; // урон, который может нанести танк
        protected int protection; // защита, которой обладает танк
		public int Protection { get { return this.protection; } }
        protected int amountOfHealth; // количество здоровья
		public int AmountOfHealth { get { return this.amountOfHealth; } set { this.amountOfHealth = value; } }
        protected bool live; // жизнь
		public bool Live { set { this.live = value; } }

		// конструктор
		public Tank(string fileTank, int height, int width, int x, int y)
        {
			this.height = height;
			this.width = width;
			this.x = x;
			this.y = y;
			this.dx = 0;
			this.dy = 0;
			this.speed = 0;
			this.direction = 1;
			this.damage = 27;
			this.protection = 5;
			this.amountOfHealth = 100;
			this.live = true;
			this.fileTank = fileTank;
			Image image = new Image("images/" + this.fileTank);
			Color color = new Color(255, 255, 255);
			image.CreateMaskFromColor(color);
			this.image = image;
			Texture texture = new Texture(this.image);
			this.texture = texture;
			IntRect intRect = new IntRect(0, 0, this.width, this.height);
			Sprite sprite = new Sprite(this.texture, intRect);
			this.sprite = sprite;
			Bullet bullet = new Bullet("bullet.png", 10, 10, (int)this.x, (int)this.y, this.direction, this.damage);
			this.bullet = bullet;
			this.isShoot = false;
		}

		// получить прямоугольник объекта
		public FloatRect getRect()
		{
			FloatRect floatRect = new FloatRect(this.x, this.y, this.width + 20, this.height + 20);
			return floatRect;
		}

		// взаимодействие одноклассников
		public void interactionOfClassmates(Tank tank)
		{
			if (this.getRect().Intersects(tank.getRect()))
			{
				if (this.dx > 0)
				{
					this.x -= 1;
				}
				else if (this.dx < 0)
				{
					this.x += 1;
				}
				else if (this.dy > 0)
				{
					this.y -= 1;
				}
				else if (this.dy < 0)
				{
					this.y += 1;
				}
			}
		}

		// стрелять
		public void shoot()
		{
			this.isShoot = true;
			this.bullet.Live = true;
			this.bullet.Direction = this.direction;
			this.bullet.X = this.x;
			this.bullet.Y = this.y;
		}
}
}
