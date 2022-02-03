using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System;

namespace Tanks_2D_CSharp
{
	class Bullet
	{
		private int height, width;
		private string fileBullet;
		private Image image;
		private Texture texture;
		private Sprite sprite;
		private float x, y;
		public float X { set { this.x = value; } }
		public float Y { set { this.y = value; } }
		private float dx, dy;
		private float speed;
		private int direction;
		public int Direction { set { this.direction = value; } }
		private int damage;
		private bool live;
		public bool Live { get { return this.live; } set { this.live = value; } }

		public Bullet() // конструктор без параметров класса Bullet
		{
			this.height = 0;
			this.width = 0;
			this.x = 0;
			this.y = 0;
			this.dx = 0;
			this.dy = 0;
			this.speed = 0;
			this.direction = 0;
			this.damage = 0;
			this.live = false;
			this.fileBullet = "";
		}

		public Bullet(String fileBullet, int height, int width, int x, int y, int direction, int damage) // конструктор с параметрами класса Bullet
		{
			this.height = height;
			this.width = width;
			this.x = x;
			this.y = y;
			this.dx = 0;
			this.dy = 0;
			this.speed = (float)0.3;
			this.direction = direction;
			this.damage = damage;
			this.live = true;
			this.fileBullet = fileBullet;
			Image image = new Image("images/" + this.fileBullet);
			Color color1 = new Color(255, 255, 255);
			image.CreateMaskFromColor(color1);
			this.image = image;
			Texture texture = new Texture(image);
			this.texture = texture;
			IntRect intRect = new IntRect(0, 0, this.width, this.height);
			Sprite sprite = new Sprite(texture, intRect);
			Color color2 = new Color(0, 0, 0);
			sprite.Color = color2;
			this.sprite = sprite;
		}

		public void interactionBulletWithMap(Map map) // взаимодействие пули с картой
		{
			for (int i = (int)this.y / 60; i < (this.y + this.height) / 60; i++)
			{
				for (int j = (int)this.x / 60; j < (this.x + this.width) / 60; j++)
				{
					// столкновение со стенкой
					if (map.TileMap[i][j] == 'w')
					{
						this.live = false;
					}
				}
			}
		}

		public void update(float time, Map map, RenderWindow window, Tank tank_1, Tank tank_2) // обновление состояноия пули
		{
			if (this.live == true)
			{
				window.Draw(this.sprite);
				switch (this.direction)
				{
					case 1: // движение вверх
						this.dx = 0;
						this.dy = -this.speed;
						break;
					case 2: // движение вниз
						this.dx = 0;
						this.dy = this.speed;
						break;
					case 3: // движение влево
						this.dx = -this.speed;
						this.dy = 0;
						break;
					case 4: // движение вправо
						this.dx = this.speed;
						this.dy = 0;
						break;
				}

				this.x = this.x + this.dx * time;
				this.y = this.y + this.dy * time;

				Vector2f vector2F = new Vector2f(this.x + 15, this.y + 15);
				this.sprite.Position = vector2F;

				this.interactionBulletWithMap(map);
				this.interactionBulletWithTank(tank_2);
			}
			else
			{
				tank_1.IsShoot = false;
			}
		}

		public void interactionBulletWithTank(Tank tank) // взаимодействие пули с танком
		{
			if (this.getRect().Intersects(tank.getRect()))
			{
				this.live = false;
				tank.AmountOfHealth = tank.AmountOfHealth - this.damage + tank.Protection;
				if (tank.AmountOfHealth <= 0)
				{
					tank.Live = false;
				}
			}
		}

		public FloatRect getRect() // получить прямоугольник пули
		{
			FloatRect floatRect = new FloatRect(this.x, this.y, this.width, this.height);
			return floatRect;
		}
}
}
